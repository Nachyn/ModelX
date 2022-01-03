using System.Globalization;
using ModelX.Domain.Enums;

namespace ModelX.Domain.Helpers;

public static class CulturesHelper
{
    public static CultureInfo[] GetSupportedCulturesInfo()
    {
        var enumValues = Enum.GetValues(typeof(Cultures)).Cast<Cultures>();

        return enumValues
            .Select(@enum =>
            {
                var enumDescription = @enum.GetEnumDescription();

                return new CultureInfo(enumDescription);
            })
            .ToArray();
    }

    public static List<Cultures> GetSupportedCultures()
    {
        return Enum.GetValues(typeof(Cultures)).Cast<Cultures>().ToList();
    }
}