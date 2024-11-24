using Deface.NET.Graphics.Models;

namespace Deface.NET.Graphics.Interfaces;

internal interface IFrameCreator
{
    Frame FromBgrArray(byte[] bgrData, int width, int height);
    Frame FromFile(string path);
}