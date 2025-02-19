// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Docfx.YamlSerialization;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace Docfx.Build.RestApi.Swagger;

[Serializable]
public class TagItemObject
{
    /// <summary>
    /// Required. The name of the tag.
    /// </summary>
    [YamlMember(Alias = "name")]
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// A short description for the tag. GFM syntax can be used for rich text representation.
    /// </summary>
    [YamlMember(Alias = "description")]
    [JsonProperty("description")]
    public string Description { get; set; }

    /// <summary>
    /// Define the bookmark id for the tag. It's extensions to the Swagger Schema, which MUST begin with 'x-'.
    /// </summary>
    [YamlMember(Alias = "x-bookmark-id")]
    [JsonProperty("x-bookmark-id")]
    public string BookmarkId { get; set; }

    [ExtensibleMember]
    [JsonExtensionData]
    public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
}
