namespace ModelX.Domain.Entities;

public class Model
{
    public int AttachmentId { get; set; }
    public Attachment Attachment { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}