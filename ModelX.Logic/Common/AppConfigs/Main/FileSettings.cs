namespace ModelX.Logic.Common.AppConfigs.Main;

public record FileSettings
{
    public List<string> MimeContentTypes { get; set; }

    public int MaxLengthBytes { get; set; }

    public int FileNameMaxLength { get; set; }
}