using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelX.Infrastructure.Database.Extensions;

internal static class MigrationBuilderExtensions
{
    public static MigrationBuilder RunFile(this MigrationBuilder builder,
        string filename)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        var sqlPath = Path.Combine(Directory.GetCurrentDirectory(), "SQLScripts",
            filename);

        if (File.Exists(sqlPath))
        {
            builder.Sql(File.ReadAllText(sqlPath));
        }
        else
        {
            throw new Exception(
                $"Migration .sql file not found: $={filename}. $path={sqlPath}");
        }

        return builder;
    }
}