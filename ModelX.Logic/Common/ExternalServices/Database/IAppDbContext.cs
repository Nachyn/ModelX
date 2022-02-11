using Microsoft.EntityFrameworkCore;
using ModelX.Domain.Entities;

namespace ModelX.Logic.Common.ExternalServices.Database;

public interface IAppDbContext
{
    DbSet<Attachment> Attachments { get; set; }

    DbSet<AppUser> Users { get; set; }

    DbSet<Token> Tokens { get; set; }

    DbSet<Model> Models { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}