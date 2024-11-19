using Deface.NET.Graphics.Models;

namespace Deface.NET.Graphics.Interfaces;

internal interface IFrameCreator
{
    Frame FromFile(string path);
}