namespace Deface.NET.ObjectDetection;

internal static class PostProcessingHelper
{
    public static List<DetectedObject> NMS(List<DetectedObject> faces, float iouThreshold)
    {
        var sortedFaces = faces.OrderByDescending(f => f.Confidence).ToList();
        List<DetectedObject> selectedFaces = [];

        while (sortedFaces.Count != 0)
        {
            var bestFace = sortedFaces.First();

            selectedFaces.Add(bestFace);
            sortedFaces.RemoveAt(0);
            sortedFaces.RemoveAll(f => IOU(bestFace, f) > iouThreshold);
        }

        return selectedFaces;
    }

    private static float IOU(DetectedObject box1, DetectedObject box2)
    {
        var intersectionX1 = Math.Max(box1.X1, box2.X1);
        var intersectionY1 = Math.Max(box1.Y1, box2.Y1);
        var intersectionX2 = Math.Min(box1.X2, box2.X2);
        var intersectionY2 = Math.Min(box1.Y2, box2.Y2);

        var intersectionArea = Math.Max(0, intersectionX2 - intersectionX1 + 1) * Math.Max(0, intersectionY2 - intersectionY1 + 1);

        var box1Area = (box1.X2 - box1.X1 + 1) * (box1.Y2 - box1.Y1 + 1);
        var box2Area = (box2.X2 - box2.X1 + 1) * (box2.Y2 - box2.Y1 + 1);
        var unionArea = box1Area + box2Area - intersectionArea;

        return (float)intersectionArea / unionArea;
    }

    public static List<DetectedObject> RescaleBoundingBoxes(List<DetectedObject> scaledBoxes, int originalWidth, int originalHeight, int width, int height)
    {
        var scale = Math.Min((float)width / originalWidth, (float)height / originalHeight);

        var offsetX = (width - (int)(originalWidth * scale)) / 2;
        var offsetY = (height - (int)(originalHeight * scale)) / 2;

        List<DetectedObject> originalBoxes = [];

        foreach (var box in scaledBoxes)
        {
            var x1 = (box.X1 - offsetX) / scale;
            var y1 = (box.Y1 - offsetY) / scale;
            var x2 = (box.X2 - offsetX) / scale;
            var y2 = (box.Y2 - offsetY) / scale;

            originalBoxes.Add(new((int)x1, (int)y1, (int)x2, (int)y2, box.Confidence));
        }

        return originalBoxes;
    }
}
