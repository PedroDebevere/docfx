// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Runtime.InteropServices;
using Docfx.Common;
using Docfx.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Docfx;

internal static class RunServe
{
    public static void Exec(string folder, string host, int? port, bool openBrowser, string openFile)
    {
        if (string.IsNullOrEmpty(folder))
            folder = Directory.GetCurrentDirectory();

        folder = Path.GetFullPath(folder);

        var url = $"http://{host ?? "localhost"}:{port ?? 8080}";

        if (!Directory.Exists(folder))
        {
            throw new ArgumentException("Site folder does not exist. You may need to build it first. Example: \"docfx docfx_project/docfx.json\"", nameof(folder));
        }

        var fileServerOptions = new FileServerOptions
        {
            EnableDirectoryBrowsing = true,
            FileProvider = new PhysicalFileProvider(folder),
        };

        // Fix the issue that .JSON file is 404 when running docfx serve
        fileServerOptions.StaticFileOptions.ServeUnknownFileTypes = true;

        try
        {
            var builder = WebApplication.CreateBuilder();
            builder.Logging.ClearProviders();
            builder.WebHost.UseUrls(url);

            Console.WriteLine($"Serving \"{folder}\" on {url}. Press Ctrl+C to shut down.");
            using var app = builder.Build();
            app.UseFileServer(fileServerOptions);

            if (openBrowser || !string.IsNullOrEmpty(openFile))
            {
                string relativePath = openFile;
                var launchUrl = string.IsNullOrEmpty(relativePath)
                    ? url
                    : ResolveOutputHtmlRelativePath(baseUrl: url, folder, relativePath);

                // Start web server.
                app.Start();

                // Launch browser process.
                Console.WriteLine($"Launching browser with url: {launchUrl}.");
                LaunchBrowser(launchUrl);

                // Wait until server exited.
                app.WaitForShutdown();
            }
            else
            {
                app.Run();
            }
        }
        catch (System.Reflection.TargetInvocationException)
        {
            Logger.LogError($"Error serving \"{folder}\" on {url}, check if the port is already being in use.");
        }
    }

    /// <summary>
    /// Resolve output HTML file path by `manifest.json` file.
    /// If failed to resolve path. return baseUrl.
    /// </summary>
    private static string ResolveOutputHtmlRelativePath(string baseUrl, string folder, string relativePath)
    {
        var manifestPath = Path.GetFullPath(Path.Combine(folder, "manifest.json"));
        if (!File.Exists(manifestPath))
            return baseUrl;

        try
        {
            Manifest manifest = JsonUtility.Deserialize<Manifest>(manifestPath);

            // Try to find output html file (html->html)
            OutputFileInfo outputFileInfo = manifest.FindOutputFileInfo(relativePath);
            if (outputFileInfo != null)
                return outputFileInfo.RelativePath;

            // Try to resolve output HTML file. (md->html)
            relativePath = relativePath.Replace('\\', '/'); // Normalize path.
            var manifestFile = manifest.Files
                                       .Where(x => FilePathComparer.OSPlatformSensitiveRelativePathComparer.Equals(x.SourceRelativePath, relativePath))
                                       .FirstOrDefault(x => x.OutputFiles.TryGetValue(".html", out outputFileInfo));

            if (outputFileInfo != null)
            {
                var baseUri = new Uri(baseUrl);
                return new Uri(baseUri, relativeUri: outputFileInfo.RelativePath).ToString();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to resolve output HTML file by exception. file - {relativePath} with error - {ex.Message}");
            return baseUrl;
        }

        // Failed to resolve output HTML file.
        Logger.LogError($"Failed to resolve output HTML file. file - {relativePath}");
        return baseUrl;
    }

    private static void LaunchBrowser(string url)
    {
        try
        {
            // Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start("cmd", new[] { "/C", "start", url });
                return;
            }

            // Linux
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
                return;
            }

            // OSX
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
                return;
            }

            Logger.LogError($"Could not launch the browser process. Unknown OS platform: {RuntimeInformation.OSDescription}");
        }
        catch (Exception ex)
        {
            Logger.LogError($"Could not launch the browser process. with error - {ex.Message}");
        }
    }
}
