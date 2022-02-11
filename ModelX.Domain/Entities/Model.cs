namespace ModelX.Domain.Entities;

public class Model
{
    public int AttachmentId { get; set; }
    public Attachment Attachment { get; set; }

    public int Latitude { get; set; }

    public int Longitude { get; set; }
}