﻿using System.Text.RegularExpressions;

namespace QLThuVien.WebApi.Conventions;

public class KebabCaseTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value) => value != null
        ? Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLower()
        : null;
}
