using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection;

namespace Deface.NET.Graphics.Interfaces;

internal interface IEffectShape
{
    Frame Draw(Frame bitmap, List<DetectedObject> objects);
}
