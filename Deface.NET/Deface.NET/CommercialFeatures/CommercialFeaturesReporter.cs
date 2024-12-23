using Deface.NET.CommercialFeatures.Interfaces;
using Deface.NET.Configuration.Provider;
using Deface.NET.Logging;
using System.Text;

namespace Deface.NET.CommercialFeatures;

internal class CommercialFeaturesReporter(IDLogger<IDefaceService> logger, IScopedSettingsProvider scopedSettingsProvider) : ICommercialFeaturesReporter
{
    private readonly IDLogger<IDefaceService> _logger = logger;
    private readonly Settings _settings = scopedSettingsProvider.Settings;

    public void ReportCommercialFeatures()
    {
        if (_settings.HideCommercialFeaturesInfo)
        {
            return;
        }

        var enabledFeatures = CommercialFeatures.Features.Where(f => f.IsEnabled(_settings)).ToList();

        if (enabledFeatures.Count == 0)
        {
            return;
        }

        var message = GetFullMessage(enabledFeatures);

        _logger.LogBasic(message);
    }

    private static string GetFullMessage(List<ICommercialFeature> enabledFeatures)
    {
        var message = new StringBuilder();

        message.AppendLine($"There are {enabledFeatures.Count} commercial feature(s) enabled. Make sure you have a proper license to use them:");

        for (var i = 0; i < enabledFeatures.Count; i++)
        {
            var feature = enabledFeatures[i];

            message.AppendLine();
            message.AppendLine($"{i + 1}. {feature.Name}");
            message.AppendLine($"More info at:");

            foreach (var url in feature.Urls)
            {
                message.AppendLine($"- {url}");
            }
        }

        return message.ToString();
    }
}