namespace Deface.NET.Configuration.Validation
{
    internal interface ISettingsValidator
    {
        void Validate(Settings settings, ProcessingType processingType);
    }
}