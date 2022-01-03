using System.ComponentModel;

namespace ModelX.Domain.Enums;

public enum Roles
{
    [Description("admin")]
    Admin = 1,

    [Description("user")]
    User = 2
}