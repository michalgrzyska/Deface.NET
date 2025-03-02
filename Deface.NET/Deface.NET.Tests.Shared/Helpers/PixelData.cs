using Deface.NET.Graphics.Interfaces;

namespace Deface.NET.Tests.Shared.Helpers;
internal record PixelData(int X, int Y, byte R, byte G, byte B) : IRgb, ICoordinates;