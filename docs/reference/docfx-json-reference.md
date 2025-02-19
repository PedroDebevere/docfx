---
uid: docfx_json_refence
title: docfx.json reference
---
`docfx.json` Configuration Reference
====================================

## Introduction
`docfx.json` is the main configuration file for Docfx.  The following is a description of the format of that file.

## 1. `docfx.json` Format

Top level `docfx.json` structure is key-value pair. `key` is the name of the subcommand, current supported subcommands are `metadata` and `build`.

### 1.1 Properties for `metadata`

`Metadata` section defines an array of source projects and their output folder. Each item has `src` and `dest` property. `src` defines the source projects to have metadata generated, which is in `File Mapping Format`. Detailed syntax is described in **4. Supported `name-files` File Mapping Format** below. `dest` defines the output folder of the generated metadata files.

Key                      | Description
-------------------------|-----------------------------
src                      | Defines the source projects to have metadata generated, which is in `File Mapping Format`. Relative paths are relative to the docfx.json file being used. To go up a folder use `../`.
dest                     | Defines the output folder of the generated metadata files. Relative paths are relative to the docfx.json file being used. To go up a folder use `../`.
shouldSkipMarkup         | If set to true, DocFX would not render triple-slash-comments in source code as markdown.
filter                   | Defines the filter configuration file, please go to [How to filter out unwanted apis attributes](../tutorial/howto_filter_out_unwanted_apis_attributes.md) for more details.
disableDefaultFilter     | Disables the default filter configuration file.
disableGitFeatures       | Disables generation of view source links.
properties               | Defines an optional set of MSBuild properties used when interpreting project files. These are the same properties that are passed to msbuild via the `/property:name=value` command line argument.
noRestore                | Do not run `dotnet restore` before building the projects.
namespaceLayout          | Defines how namespaces in TOC are organized. When set to *flattened*, renders namespaces as a single flat list. When set to *nested*, renders namespaces in a nested tree form. The default is *flattened*.
memberLayout             | Defines how member pages are organized. When set to *samePage*, places members in the same page as their containing type. When set to *separatePages*, places members in separate pages. The default is *samePage*.
allowCompilationErrors   | When enabled, continues documentation generation in case of compilation errors.

**Sample**
```json
{
  "metadata": [
    {
      "src": [
        {
          "files": ["**/*.csproj"],
          "exclude": [ "**/bin/**", "**/obj/**" ],
          "src": "../src"
        }
      ],
      "dest": "obj/docfx/api/dotnet",
      "shouldSkipMarkup": true,
      "properties": {
          "TargetFramework": "netstandard1.3"
      }
    },
    {
      "src": [
        {
          "files": ["**/*.js"],
          "src": "../src"
        }
      ],
      "dest": "obj/docfx/api/js",
      "properties": {
          "TargetFramework": "net46"
      }
    }
  ]
}
```
> [!Note]
> Make sure to specify `"TargetFramework": <one of the frameworks>` in your docfx.json when the project is targeting for multiple platforms.

### 1.2 Properties for `build`

Key                      | Description
-------------------------|-----------------------------
content                  | Contains all the files to generate documentation, including metadata `yml` files and conceptual `md` files. `name-files` file mapping with several ways to define it, as to be described in **Section4**. The `files` contains all the project files to have API generated.
resource                 | Contains all the resource files that conceptual and metadata files depend on (e.g., image files). `name-files` file mapping with several ways to define it, as to be described in **Section4**.
overwrite                | Contains all the conceptual files that contain yaml headers with `uid` values and is intended to override the existing metadata `yml` files. `name-files` file mapping with several ways to define it, as to be described in **Section4**.
globalMetadata           | Contains metadata that will be applied to every file, in key-value pair format. For example, you can define `"_appTitle": "This is the title"` in this section, and when applying template `default`, it will be part of the page title as defined in the template.
fileMetadata             | Contains metadata that will be applied to specific files. `name-files` file mapping with several ways to define it, as to be described in **Section4**.
globalMetadataFiles      | Specifies a list of JSON file paths containing globalMetadata settings, as similar to `{"key":"value"}`. See **Section3.2.3** for more detail.
fileMetadataFiles        | Specifies a list of JSON file paths containing fileMetadata settings, as similar to `{"key":"value"}`. See **Section3.2.3** for more detail.
template                 | The templates applied to each file in the documentation. Specify a string or an array. The latter ones will override the former ones if the name of the file inside the template collides. If omitted, the embedded `default` template will be used.
theme                    | The themes applied to the documentation. Theme is used to customize the styles generated by `template`. It can be a string or an array. The latter ones will override the former ones if the name of the file inside the template collides. If omitted, no theme will be applied, the default theme inside the template will be used.
xref                     | Specifies the urls of xrefmap used by content files. Currently, it supports following scheme: http, https, file.
exportRawModel           | If set to true, data model to run template script will be extracted in `.raw.json` extension.
rawModelOutputFolder     | Specifies the output folder for the raw model. If not set, the raw model will appear in the same folder as the output documentation.
exportViewModel          | If set to true, data model to apply template will be extracted in `.view.json` extension.
viewModelOutputFolder    | Specifies the output folder for the view model. If not set, the view model will appear in the same folder as the output documentation.
dryRun                   | If set to true, the template will not be applied to the documents. This option is always used with `--exportRawModel` or `--exportViewModel` so that only raw model files or view model files will be generated.
maxParallelism           | Sets the max parallelism. Setting 0 (default) is the same as setting to the count of CPU cores.
markdownEngineProperties | Sets the parameters for the markdown engine, value is a JSON object.
keepFileLink             | If set to true, docfx does not dereference (i.e., copy) the file to the output folder, instead, it saves a `link_to_path` property inside `manifest.json` to indicate the physical location of that file. A file link will be created by incremental build and copy resource file.
sitemap                  | In format [SitemapOptions](#125-sitemapoptions). Specifies the options for the sitemap.xml file.
disableGitFeatures       | Disable fetching Git related information for articles. Set to `true` if fetching Git related information is slow for huge Git repositories. Default value is `false`.

#### 1.2.1 `Template`s and `Theme`s

*Template*s are used to transform *YAML* files generated by `docfx` to human-readable *page*s. A *page* can be a markdown file, an html file or even a plain text file. Each *YAML* file will be transformed to ONE *page* and be exported to the output folder preserving its relative path to `src`. For example, if *page*s are in *HTML* format, a static website will be generated in the output folder.

*Theme* is to provide general styles for all the generated *page*s. Files inside a *theme* will be generally **COPIED** to the output folder. A typical usage is, after *YAML* files are transformed to *HTML* pages, well-designed *CSS* style files in a *Theme* can then overwrite the default styles defined in *template*, e.g. *main.css*.

There are two ways to use custom templates and themes.

To use a custom template, one way is to specify template path with `--template` (or `-t`) command option, multiple templates must be separated by `,` with no spaces. The other way is to set key-value mapping in `docfx.json`:

```json
{
  ...
  {
    "build" :
    {
      ...
      "template": "custom",
      ...
    }
  ...
}
```
```json
{
  ...
  {
    "build" :
    {
      ...
      "template": ["default", "X:/template/custom"],
      ...
    }
  ...
}
```

> [!Note]
> The template path could either be a zip file called `<template>.zip` or a folder called `<template>`.
>
> [!Warning]
> DocFX has embedded templates: `default`, `default(zh-cn)`, `pdf.default`, `statictoc` and `common`.
> Please avoid using these as template folder name.

One way to set a custom theme is to specify the theme name with the `--theme` command option. Multiple themes must be separated by `,` with no spaces. The other way is to set a key-value mapping in `docfx.json` similar to the way templates are defined. Also, either a `.zip` file or a folder can be used.

Please refer to [How to Create Custom Templates](../tutorial/howto_create_custom_template.md) to create custom templates.

**Sample**
```json
{
  "build": {
    "content":
      [
        {
          "files": ["**/*.yml"],
          "src": "obj/docfx"
        },
        {
          "files": ["tutorial/**/*.md", "spec/**/*.md", "spec/**/toc.yml"]
        },
        {
          "files": ["toc.yml"]
        }
      ],
    "resource": [
        {
          "files": ["spec/images/**"]
        }
    ],
    "overwrite": "apispec/*.md",
    "externalReference": [
    ],
    "globalMetadata": {
      "_appTitle": "DocFX website",
      "_gitContribute": {
        "repo": "https://github.com/org/repo",
        "branch": "dev",
        "apiSpecFolder": "docs-ref-overwrite"
      }
    },
    "dest": "_site",
    "template": "default"
  }
}
```

#### 1.2.2 Reserved Metadata

After passing values through global metadata or file metadata, DocFX can use these metadata in templates to control the output html.
Reserved metadatas:

Metadata Name         | Type    | Description
----------------------|---------|---------------------------
_appTitle             | string  | Will be appended to each output page's head title.
_appFooter            | string  | The footer text. Will show DocFX's Copyright text if not specified.
_appLogoPath          | string  | Logo file's path from output root. Will show DocFX's logo if not specified. Remember to add file to resource.
_appFaviconPath       | string  | Favicon file's path from output root. Will show DocFX's favicon if not specified. Remember to add file to resource.
_enableSearch         | bool    | Indicate whether to show the search box on the top of page.
_enableNewTab         | bool    | Indicate whether to open a new tab when clicking an external link. (internal link always shows within the current tab)
_disableNavbar        | bool    | Indicate whether to show the navigation bar on the top of page.
_disableBreadcrumb    | bool    | Indicate whether to show breadcrumb on the top of page.
_disableToc           | bool    | Indicate whether to show table of contents on the left of page.
_disableAffix         | bool    | Indicate whether to show the affix bar on the right of page.
_disableContribution  | bool    | Indicate whether to show the `View Source` and `Improve this Doc` buttons.
_gitContribute        | object  | Customize the `Improve this Doc` URL button for public contributors. Use `repo` to specify the contribution repository URL. Use `branch` to specify the contribution branch. Use `apiSpecFolder` to specify the folder for new overwrite files. If not set, the git URL and branch of the current git repository will be used.
_gitUrlPattern        | string  | Choose the URL pattern of the generated link for `View Source` and `Improve this Doc`. Supports `github` and `vso` currently. If not set, DocFX will try speculating the pattern from domain name of the git URL.
_noindex              | bool  | File(s) specified are not returned in search results

#### 1.2.3 Separated metadata files for global metadata and file metadata

There are three ways to set metadata for a file in DocFX:

1. Using global metadata, which will set metadata for every file.
2. Using file metadata, which will set metadata for files that match the pattern you specify.
3. Using YAML header, which will set metadata for the current file.

In the list above, the later method way will always overwrite the former if the same key of metadata is set.

Here we will show you how to set global metadata and file metadata using separated metadata files. Take global metadata for example, you can set `globalMetadataFiles` in `docfx.json` or `--globalMetadataFiles` in build command line. The usage of `fileMetadataFiles` is the same as `globalMetadataFiles`.

There're some metadata file examples:

+ globalMetadata file example
    ```json
    {
        "_appTitle": "DocFX website",
        "_enableSearch": "true"
    }
    ```

+ fileMetadata file example
    ```json
    {
        "priority": {
            "**.md": 2.5,
            "spec/**.md": 3
        },
        "keywords": {
            "obj/docfx/**": ["API", "Reference"],
            "spec/**.md": ["Spec", "Conceptual"]
        },
        _noindex: {
            "articles/**/article.md": true
        }
    }
    ```

There're some examples about how to use separated metadata files.

+ use `globalMetadataFiles` in `docfx.json`
    ```json
    ...
    "globalMetadataFiles": ["global1.json", "global2.json"],
    ...
    ```

+ use `--globalMetadataFiles` in build command line
    ```
    docfx build --globalMetadataFiles global1.json,global2.json
    ```

+ use `fileMetadataFiles` in `docfx.json`
    ```json
    ...
    "fileMetadataFiles": ["file1.json", "file2.json"],
    ...
    ```

+ use `--fileMetadataFiles` in build command line
    ```
    docfx build --fileMetadataFiles file1.json,file2.json
    ```

Note that, metadata set in command line will merge with metadata set in `docfx.json`.

+ If same key for global metadata was set, the order to be overwritten would be(the later one will overwrite the former one):
    1. global metadata from docfx config file
    2. global metadata from global metadata files
    3. global metadata from command line

+ If same file pattern for file metadata was set, the order to be overwritten would be(the later one will overwrite the former one):
    1. file metadata from docfx config file
    2. file metadata from file metadata files

Given multiple metadata files, the behavior would be **undetermined**, if same key is set in these files.

#### 1.2.5 SitemapOptions

The SitemapOptions is to configure the values for generating [sitemap.xml](https://www.sitemaps.org/protocol.html) file.

Property Name         | Type    | Description
----------------------|---------|---------------------------
`baseUrl`             | string  | Specifies the base url for the website to be published. It MUST begin with the protocol (such as http) and end with a trailing slash. For example, `https://dotnet.github.io/docfx/`. If the value is not specified, sitemap.xml will NOT be generated.
`lastmod`             | DateTime| Specifies the date of last modification of the file. If not specified, docfx automatically set the value to the time the file is built.
`changefreq`          | enum    | Specifies the value of [changefreq](https://www.sitemaps.org/protocol.html#changefreqdef) in sitemap.xml. Valid values are `always`, `hourly`, `daily`, `weekly`, `monthly`, `yearly`, `never`. If not specified, the default value is `daily`
`priority`            | double  | Specifies the value of [priority](https://www.sitemaps.org/protocol.html#prioritydef) in sitemap.xml. Valid values between `0.0` and `1.0`. If not specified, the default value is `0.5`
`fileOptions`         | SitemapOptions | Optional. This property can be used when some specific files have different sitemap settings. It is a set of key-value pairs, where key is the [*glob* pattern](#23-glob-pattern) for input files, and value is the sitemap options. Order matters and the latter matching option overwrites the former ones.

In the following sample settings, the yml files inside `api` folder are with priority 0.3 while Markdown files are with priority 0.8 and with a different baseUrl.

Sample settings:
```json
"build": {
    "sitemap":{
        "baseUrl": "https://dotnet.github.io/docfx",
        "priority": 0.1,
        "changefreq": "monthly",
        "fileOptions":{
            "**/api/**.yml": {
                "priority": 0.3,
                "lastmod": "2001-01-01",
            },
            "**/GettingStarted.md": {
                "baseUrl": "https://dotnet.github.io/docfx/conceptual",
                "priority": 0.8,
                "changefreq": "daily"
            }
        }
    }
}
```

Possible generated sitemap.xml:
```xml
<?xml version="1.0" encoding="utf-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url>
    <loc>https://dotnet.github.io/docfx/api/System.String.html</loc>
    <lastmod>2001-01-01T00:00:00.00+08:00</lastmod>
    <changefreq>monthly</changefreq>
    <priority>0.3</priority>
  </url>
  <url>
    <loc>https://dotnet.github.io/docfx/conceptual/GettingStarted.html</loc>
    <lastmod>2017-09-21T10:00:00.00+08:00</lastmod>
    <changefreq>daily</changefreq>
    <priority>0.3</priority>
  </url>
  <url>
    <loc>https://dotnet.github.io/docfx/ReadMe.html</loc>
    <lastmod>2017-09-21T10:00:00.00+08:00</lastmod>
    <changefreq>monthly</changefreq>
    <priority>0.1</priority>
  </url>
</urlset>
```


### 1.3 Properties for `pdf`

`pdf` supports **ALL** the [properties for `build`](#12-properties-for-build), besides that, the following table lists additional properties specified for `pdf` only.

Key                      | Description
-------------------------|-----------------------------
name                     | Specifies the prefix of the generated PDF files, e.g. PDF generated from `testproject\toc.yml` is named as `{name}.pdf`, `testproject\api\toc.yml` is named as `{name}_api.pdf`. If not specified, the value of `name` is the folder name `testproject`.
generatesAppendices      | If specified, an `appendices.pdf` file is generated containing all the not-in-TOC articles.
keepRawFiles             | If specified, the intermediate html files used to generate the PDF are not deleted after the PDF has been generated.
wkhtmltopdf              | Contains additional options specific to wkhtmltopdf which is used internally to generate the PDF files.
coverTitle               | The name of the bookmark to use for the cover page. If omitted, "Cover Page" will be used.
tocTitle                 | The name of the bookmark to use for the "Table of Contents". If omitted, "Table of Contents" will be used.
outline                  | The type of outline to use. Valid values are `NoOutline`, `DefaultOutline`, `WkDefaultOutline`. If not specified, the default value is `DefaultOutline`. If `WkDefaultOutline` is specified, `--outline` is passed to wkhtmltopdf; otherwise `--no-outline` is passed to wkhtmltopdf.
noStdin                  | Do not use `--read-args-from-stdin` for the wkhtmltopdf. Html input file names are set using the command line. It has been introduced to use in the Azure pipeline build. Can cause maximum allowed arguments length overflow if too many input parts (like Appendices, TocTitle, CoverPageTitle) were set for certain html source file.
excludeDefaultToc        | If true, excludes the table of contents (generated by DocFX) in the PDF file.

#### 1.3.1 Properties for the `wkhtmltopdf` Key

Key                      | Description
-------------------------|-----------------------------
filePath                 | The path and file name of a wkhtmltopdf.exe compatible executable.
additionalArguments      | Additional arguments that should be passed to the wkhtmltopdf executable. For example, pass `--enable-local-file-access` if you are building on a local file system. This will ensure that the supporting *.js and *.css files are loaded when rendering the HTML being converted to PDF.

## 2. Supported File Mapping Format

There are several ways to define file mapping.

### 2.1 Array Format

This form supports multiple file mappings, and also allows additional properties per mapping.
Supported properties:

Property Name      | Description
-------------------|-----------------------------
files              | **REQUIRED**. The file or file array, `glob` pattern is supported.
~~name~~           | **Obsoleted**, please use `dest`.
exclude            | The files to be excluded, `glob` pattern is supported.
~~cwd~~            | **Obsoleted**, please use `src`.
src                | Specifies the source directory. If omitted, the directory of the config file will be used. It is possible to set this path relative or absolute. Use the relative path defintion when you want to refer to files in relative folders while want to keep folder structure. e.g. set `src` to `..`. When you prefere absolut path, maybe it is more meaningful to use System Enviroment variables.
dest               | The folder name for the generated files.
version            | Version name for the current file mapping. If not set, treat the current file-mapping item as in default version. Mappings with the same version name will be built together. Cross reference doesn't support cross different versions.
caseSensitive      | **TOBEIMPLEMENTED**. Default value is `false`. If set to `true`, the glob pattern is case sensitive. e.g. `*.txt` will not match `1.TXT`. For OS Windows, file path is case insensitive while for Linux/Unix, file path is case sensitive. This option offers user the flexibility to determine how to search files.
supportBackslash   | **TOBEIMPLEMENTED**. Default value is `true`. If set to `true`, `\` will be considered as file path separator. Otherwise, `\` will be considered as normal character if `escape` is set to `true` and as escape character if `escape` is set to `false`. If `escape` is set to `true`, `\\` should be used to represent file path separator.
escape             | **TOBEIMPLEMENTED**. Default value is `false`. If set to `true`, `\` character is used as escape character, e.g. `\{\}.txt` will match `{}.txt`.

```json
"key": [
  {"files": ["file1", "file2"], "dest": "dest1"},
  {"files": "file3", "dest": "dest2"},
  {"files": ["file4", "file5"], "exclude": ["file5"], "src": "folder1"},
  {"files": "Example.yml", "src": "v1.0", "dest":"v1.0/api", "version": "v1.0"},
  {"files": "Example.yml", "src": "v2.0", "dest":"v2.0/api", "version": "v2.0"}
]
```

### 2.2 Compact Format

```json
"key": ["file1", "file2"]
```



### 2.3 Glob Pattern

`DocFX` uses [Glob](https://github.com/vicancy/Glob) to support *glob* pattern in file path.
It offers several options to determine how to parse the Glob pattern:
  * `caseSensitive`: Default value is `false`. If set to `true`, the glob pattern is case sensitive. e.g. `*.txt` will not match `1.TXT`. For OS Windows, file path is case insensitive while for Linux/Unix, file path is case sensitive. This option offers user the flexibility to determine how to search files.
  * `supportBackslash`: Default value is `true`. If set to `true`, `\` will be considered as file path separator. Otherwise, `\` will be considered as normal character if `escape` is set to `true` and as escape character if `escape` is set to `false`. If `escape` is set to `true`, `\\` should be used to represent file path separator.
  * `escape`: Default value is `false`. If set to `true`, `\` character is used as escape character, e.g. `\{\}.txt` will match `{}.txt`.

In general, the *glob* pattern contains the following rules:
1. `*` matches any number of characters, but not `/`
2. `?` matches a single character, but not `/`
3. `**` matches any number of characters, including `/`, as long as it's the only thing in a path part
4. `{}` allows for a comma-separated list of **OR** expressions

**SAMPLES**


## 3. Q & A

1. Do we support files outside current project folder (the folder where `docfx.json` exists)?  
   A: YES. DO specify `src` and files outside of current folder will be copied to output folder keeping the same relative path to `src`.
2. Do we support output folder outside current project folder (the folder where `docfx.json` exists)?  
   A: YES.
3. Do we support **referencing** files outside of current project folder (the folder where `docfx.json` exists)?  
   A: NO.

[1]: http://yaml.org/
