using System.ComponentModel;

namespace ModelX.Domain.Helpers;

public static class EnumHelper
{
    public static TAttribute GetAttributeFromEnum<TEnum, TAttribute>(TEnum enumValue)
        where TAttribute : Attribute
    {
        var type = typeof(TEnum);
        var memInfo = type.GetMember(type.GetEnumName(enumValue));
        var attribute = memInfo[0]
            .GetCustomAttributes(typeof(TAttribute), false)
            .FirstOrDefault() as TAttribute;

        return attribute;
    }

    public static string GetEnumDescription<TEnum>(this TEnum enumValue)
    {
        var descriptionAttribute =
            GetAttributeFromEnum<TEnum, DescriptionAttribute>(enumValue);

        return descriptionAttribute.Description;
    }

    public static List<TEnum> GetAllMembers<TEnum>()
    {
        var type = typeof(TEnum);
        var members = Enum.GetValues(type)
            .Cast<TEnum>()
            .ToList();

        return members;
    }
}