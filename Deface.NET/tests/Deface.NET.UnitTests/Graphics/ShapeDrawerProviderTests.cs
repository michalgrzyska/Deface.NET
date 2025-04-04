﻿using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.Graphics;
using Deface.NET.Graphics.ShapeDrawers;
using Deface.NET.UnitTests._TestsConfig;

namespace Deface.NET.UnitTests.Graphics;

[Collection(nameof(SettingsCollection))]
public class ShapeDrawerProviderTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Fact]
    public void ShapeDrawer_ShouldBeGaussianBlurShapeDrawer()
    {
        var settingsProvider = GetSettingsProvider(AnonimizationMethod.GaussianBlur);

        ShapeDrawerProvider provider = new(settingsProvider);

        provider.ShapeDrawer.ShouldBeAssignableTo<GaussianBlurShapeDrawer>();
    }

    [Fact]
    public void ShapeDrawer_ShouldBeMosaicShapeDrawer()
    {
        var settingsProvider = GetSettingsProvider(AnonimizationMethod.Mosaic);

        ShapeDrawerProvider provider = new(settingsProvider);

        provider.ShapeDrawer.ShouldBeAssignableTo<MosaicShapeDrawer>();
    }

    [Fact]
    public void ShapeDrawer_ShouldBeColorShapeDrawer()
    {
        var settingsProvider = GetSettingsProvider(AnonimizationMethod.Color(0, 0, 0));

        ShapeDrawerProvider provider = new(settingsProvider);

        provider.ShapeDrawer.ShouldBeAssignableTo<ColorShapeDrawer>();
    }

    private IScopedSettingsProvider GetSettingsProvider(AnonimizationMethod method)
    {
        return _settingsFixture.GetScopedSettingsProvider(x =>
        {
            x.AnonimizationMethod = method;
        });
    }
}
