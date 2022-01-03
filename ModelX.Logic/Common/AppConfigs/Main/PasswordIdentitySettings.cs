﻿namespace ModelX.Logic.Common.AppConfigs.Main;

public record PasswordIdentitySettings
{
    public int RequiredLength { get; set; }

    public bool RequireNonAlphanumeric { get; set; }

    public bool RequireLowercase { get; set; }

    public bool RequireUppercase { get; set; }

    public bool RequireDigit { get; set; }
}