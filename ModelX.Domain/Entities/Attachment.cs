namespace ModelX.Domain.Entities;

public class Attachment
{
    public int Id { get; set; }

    public int OwnerId { get; set; }
    public AppUser Owner { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public DateTime LoadedUtc { get; set; }


    public Model Model { get; set; }
}