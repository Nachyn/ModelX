namespace ModelX.Logic.Common.AppConfigs.Entities;

public record AppUserSettings
{
    public int EmailMaxLength { get; set; }

    public int UsernameMaxLength { get; set; }

    public int UsernameMinLength { get; set; }

    public string UsernameRegex { get; set; }
}