namespace Deface.NET.Configuration.Validation;

internal interface ISettingsPropertyValidator
{
    void Validate(Settings settings);
}