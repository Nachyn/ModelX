namespace ModelX.Domain.Entities;

public class Token
{
    public const string TypePassword = "password";

    public const string TypeRefresh = "refresh";

    public int Id { get; set; }

    public int UserId { get; set; }
    public AppUser User { get; set; }

    public string Client { get; set; }

    public string Value { get; set; }

    public DateTime ExpiryTimeUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }
}