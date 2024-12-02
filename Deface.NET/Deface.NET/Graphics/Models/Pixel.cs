using Deface.NET.Graphics.Interfaces;

namespace Deface.NET.Graphics.Models;

internal record Pixel(byte R, byte G, byte B) : IRgb;