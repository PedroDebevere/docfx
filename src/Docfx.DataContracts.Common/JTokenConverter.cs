﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json.Linq;

namespace Docfx.DataContracts.Common;

public static class JTokenConverter
{
    public static T Convert<T>(object obj)
    {
        if (obj is T)
        {
            return (T)obj;
        }

        if (obj is JToken jToken)
        {
            return jToken.ToObject<T>();
        }
        throw new InvalidCastException();
    }
}
