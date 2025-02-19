// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;

namespace Docfx;

public enum MemberLayout
{
    /// <summary>
    /// Place members in the same page as their containing type
    /// </summary>
    SamePage,

    /// <summary>
    /// Place members in separate pages
    /// </summary>
    SeparatePages,
}

internal enum NamespaceLayout
{
    /// <summary>
    /// Renders the namespaces as a single flat list
    /// </summary>
    Flattened,

    /// <summary>
    /// Renders the namespaces in a nested tree form
    /// </summary>
    Nested,
}

/// <summary>
/// MetadataJsonItemConfig.
/// </summary>
/// <see href="https://dotnet.github.io/docfx/reference/docfx-json-reference.html#11-properties-for-metadata"/>
internal class MetadataJsonItemConfig
{
    /// <summary>
    /// Defines the source projects to have metadata generated.
    /// </summary>
    [JsonProperty("src")]
    public FileMapping Source { get; set; }

    /// <summary>
    /// Defines the output folder of the generated metadata files.
    /// </summary>
    [JsonProperty("dest")]
    public string Destination { get; set; }

    /// <summary>
    /// If set to true, DocFX would not render triple-slash-comments in source code as markdown.
    /// </summary>
    [JsonProperty("shouldSkipMarkup")]
    public bool? ShouldSkipMarkup { get; set; }

    [JsonProperty("raw")]
    public bool? Raw { get; set; }

    [JsonProperty("references")]
    public FileMapping References { get; set; }

    /// <summary>
    /// Defines the filter configuration file.
    /// </summary>
    [JsonProperty("filter")]
    public string FilterConfigFile { get; set; }


    /// <summary>
    /// Include private or internal APIs.
    /// The default is false.
    /// </summary>
    [JsonProperty("includePrivateMembers")]
    public bool IncludePrivateMembers { get; set; }

    /// <summary>
    /// Specify the name to use for the global namespace.
    /// </summary>
    [JsonProperty("globalNamespaceId")]
    public string GlobalNamespaceId { get; set; }

    /// <summary>
    /// An optional set of MSBuild properties used when interpreting project files. These
    ///  are the same properties that are passed to MSBuild via the /property:&lt;n&gt;=&lt;v&gt;
    ///  command line argument.
    /// </summary>
    [JsonProperty("properties")]
    public Dictionary<string, string> MSBuildProperties { get; set; }

    /// <summary>
    /// Disables generation of view source links.
    /// </summary>
    [JsonProperty("disableGitFeatures")]
    public bool DisableGitFeatures { get; set; }

    /// <summary>
    /// Specify the base directory that is used to resolve code source (e.g. `&lt;code source="Example.cs"&gt;`).
    /// </summary>
    [JsonProperty("codeSourceBasePath")]
    public string CodeSourceBasePath { get; set; }

    /// <summary>
    /// Disables the default filter configuration file.
    /// </summary>
    [JsonProperty("disableDefaultFilter")]
    public bool DisableDefaultFilter { get; set; }

    /// <summary>
    /// Do not run dotnet restore before building the projects.
    /// </summary>
    [JsonProperty("noRestore")]
    public bool NoRestore { get; set; }

    /// <summary>
    /// Defines how namespaces in TOC are organized.
    /// When set to flattened, renders namespaces as a single flat list.
    /// When set to nested, renders namespaces in a nested tree form.
    /// The default is flattened.
    /// </summary>
    [JsonProperty("namespaceLayout")]
    public NamespaceLayout NamespaceLayout { get; set; }

    /// <summary>
    /// Defines how member pages are organized.
    /// When set to samePage, places members in the same page as their containing type.
    /// When set to separatePages, places members in separate pages.
    /// The default is samePage.
    /// </summary>
    [JsonProperty("memberLayout")]
    public MemberLayout MemberLayout { get; set; }

    /// <summary>
    /// When enabled, continues documentation generation in case of compilation errors.
    /// </summary>
    [JsonProperty("allowCompilationErrors")]
    public bool AllowCompilationErrors { get; set; }
}

/// <summary>
/// MetadataJsonItemConfig
/// </summary>
internal class MetadataJsonConfig : List<MetadataJsonItemConfig>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MetadataJsonConfig"/> class.
    /// </summary>
    public MetadataJsonConfig(IEnumerable<MetadataJsonItemConfig> configs) : base(configs) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MetadataJsonConfig"/> class.
    /// </summary>
    public MetadataJsonConfig(params MetadataJsonItemConfig[] configs) : base(configs)
    {
    }
}
