using OpenCvSharp;

namespace Deface.NET.CenterFace;

internal class CenterFaceDecoder(Mat heatMap, Mat scale, Mat offset, Mat landmarks, CenterFaceParams cfp)
{
    private readonly Mat heatMap = heatMap;
    private readonly Mat scale = scale;
    private readonly Mat offset = offset;
    private readonly Mat landmarks = landmarks;
    private readonly CenterFaceParams cfp = cfp;

    public List<FaceInfo> GetOutput()
    {
        var featH = heatMap.Size(2); // heatMap.Height;
        var featW = heatMap.Size(3); // heatMap.Width;
        var spacialSize = featW * featH;

        List<FaceInfo> faces = [];

        unsafe
        {
            var heatMapData = (float*)(heatMap.Data);

            var scale0 = (float*)(scale.Data);
            var scale1 = scale0 + spacialSize;

            var offset0 = (float*)(offset.Data);
            var offset1 = offset0 + spacialSize;

            var ids = GenIds(heatMapData, featH, featW, cfp.ScoreThreshold);

            List<FaceInfo> facesTmp = [];

            for (var i = 0; i < ids.Count / 2; i++)
            {
                var idH = ids[2 * i];
                var idW = ids[2 * i + 1];
                var index = idH * featW + idW;

                var s0 = (float)Math.Exp(scale0[index]) * 4;
                var s1 = (float)Math.Exp(scale1[index]) * 4;
                var o0 = offset0[index];
                var o1 = offset1[index];

                var x1 = (float)((idW + o1 + 0.5) * 4 - s1 / 2 > 0.0f ? (idW + o1 + 0.5) * 4 - s1 / 2 : 0);
                var y1 = (float)((idH + o0 + 0.5) * 4 - s0 / 2 > 0 ? (idH + o0 + 0.5) * 4 - s0 / 2 : 0);

                x1 = x1 < cfp.DW ? x1 : cfp.DW;
                y1 = y1 < cfp.DH ? y1 : cfp.DH;

                var x2 = x1 + s1 < (float)cfp.DW ? x1 + s1 : cfp.DW;
                var y2 = y1 + s0 < (float)cfp.DH ? y1 + s0 : cfp.DH;

                FaceInfo faceBox = new()
                {
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2,
                    Score = heatMapData[index],
                };

                faceBox.Area = (faceBox.X2 - faceBox.X1) * (faceBox.Y2 - faceBox.Y1);


                var boxW = x2 - x1; //=s1?
                var boxH = y2 - y1; //=s0?

                for (var j = 0; j < 5; j++)
                {
                    var xMap = (float*)landmarks.Data + (2 * j + 1) * spacialSize;
                    var yMap = (float*)landmarks.Data + (2 * j) * spacialSize;
                    faceBox.Landmarks[2 * j] = x1 + xMap[index] * s1; //box_w;
                    faceBox.Landmarks[2 * j + 1] = y1 + yMap[index] * s0; // box_h;
                }

                facesTmp.Add(faceBox);
            }

            NonMaximumSuppression(facesTmp, faces, cfp.NmsThreshold);
        }

        for (var k = 0; k < faces.Count; k++)
        {
            faces[k].X1 *= cfp.DScaleW * cfp.ScaleW;
            faces[k].Y1 *= cfp.DScaleH * cfp.ScaleH;
            faces[k].X2 *= cfp.DScaleW * cfp.ScaleW;
            faces[k].Y2 *= cfp.DScaleH * cfp.ScaleH;

            for (var kk = 0; kk < 5; kk++)
            {
                faces[k].Landmarks[2 * kk] *= cfp.DScaleW * cfp.ScaleW;
                faces[k].Landmarks[2 * kk + 1] *= cfp.DScaleH * cfp.ScaleH;
            }
        }

        return faces;
    }

    private unsafe List<int> GenIds(float* heatMap, int h, int w, float thresh)
    {
        List<int> ids = [];

        if (heatMap == null)
        {
            throw new Exception($"{nameof(heatMap)} is nullptr, please check! ");
        }

        for (var i = 0; i < h; i++)
        {
            for (var j = 0; j < w; j++)
            {
                if (heatMap[i * w + j] > thresh)
                {
                    ids.Add(i);
                    ids.Add(j);
                }
            }
        }

        return ids;
    }

    private static void NonMaximumSuppression(
        List<FaceInfo> input,
        List<FaceInfo> output,
        float nmsThreshold = 0.3f,
        NonMaximumSuppressionMode type = NonMaximumSuppressionMode.Minimum)
    {
        if (input.Count == 0)
        {
            return;
        }

        input.Sort((f1, f2) => f1.Score.CompareTo(f2.Score));

        var nPick = 0;
        List<Tuple<float, int>> vScores = [];
        var numBoxes = input.Count;
        var vPick = new int[numBoxes];

        for (int i = 0; i < numBoxes; ++i)
        {
            vScores.Add(new Tuple<float, int>(input[i].Score, i));
        }

        while (vScores.Count > 0)
        {
            var last = vScores.Last().Item2;
            vPick[nPick] = last;
            nPick += 1;

            for (var index = 0; index < vScores.Count;)
            {
                var it = vScores[index];
                var itemIndex = it.Item2;

                var maxX = Math.Max(input[itemIndex].X1, input[last].X1);
                var maxY = Math.Max(input[itemIndex].Y1, input[last].Y1);
                var minX = Math.Min(input[itemIndex].X2, input[last].X2);
                var minY = Math.Min(input[itemIndex].Y2, input[last].Y2);

                //maxX1 and maxY1 reuse 
                maxX = ((minX - maxX + 1) > 0) ? (minX - maxX + 1) : 0;
                maxY = ((minY - maxY + 1) > 0) ? (minY - maxY + 1) : 0;

                //IOU reuse for the area of two bbox
                var iou = maxX * maxY;
                switch (type)
                {
                    case NonMaximumSuppressionMode.Union:
                        iou = iou / (input[itemIndex].Area + input[last].Area - iou);
                        break;
                    case NonMaximumSuppressionMode.Minimum:
                        iou = iou / ((input[itemIndex].Area < input[last].Area) ? input[itemIndex].Area : input[last].Area);
                        break;
                }

                if (iou > nmsThreshold)
                {
                    vScores.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
        }

        Array.Resize(ref vPick, nPick);
        Resize(output, nPick);

        for (var i = 0; i < nPick; i++)
        {
            output[i] = input[vPick[i]];
        }
    }

    private static void Resize(List<FaceInfo> list, int size)
    {
        var count = list.Count;

        if (size < count)
        {
            list.RemoveRange(size, count - size);
        }
        else if (size > count)
        {
            if (size > list.Capacity)
            {
                list.Capacity = size;
            }

            list.AddRange(Enumerable.Repeat(new FaceInfo(), size - count));
        }
    }
}
