using Deface.NET.Graphics.Interfaces;

namespace Deface.NET.Configuration;

internal record Color(byte R, byte G, byte B) : IRgb;
