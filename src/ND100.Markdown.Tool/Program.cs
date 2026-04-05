using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NDGen.Core;
using NDGen.Core.Utils;

namespace ND100.Markdown.Tool;

internal static class Program
{
    private const string SolutionMarker = "ND100.Markdown.sln";

    public static int Main(string[] args)
    {
        if (args.Length == 1 && string.Equals(args[0], "help", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("ND100 Markdown documentation generator. Requires nd100-definitions submodule.");
            Console.WriteLine("Usage: dotnet run --project src/ND100.Markdown.Tool");
            Console.WriteLine("Output: ./docs");
            return 0;
        }

        try
        {
            var baseDir = FindRepoRoot();
            var docsDir = Path.Combine(baseDir, "docs");
            Directory.CreateDirectory(docsDir);

            var services = new ServiceCollection();
            services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Information));
            services.AddTransient<YamlLoader>();
            services.AddTransient<DefinitionLoader>();
            var sp = services.BuildServiceProvider();
            var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("nd100-markdown");

            var loader = sp.GetRequiredService<DefinitionLoader>();
            loader.Initialize(GetCpuYamlPath(baseDir));

            logger.LogInformation("Generating Markdown docs...");
            new MarkdownGenerator(docsDir, loader).Generate();
            logger.LogInformation("Done. Output: {Path}", docsDir);
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return 1;
        }
    }

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, SolutionMarker)))
                return dir.FullName;
            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException($"Could not find {SolutionMarker} (repo root).");
    }

    private static string GetCpuYamlPath(string baseDir)
    {
        var env = Environment.GetEnvironmentVariable("ND100_SPECS_CPU_YAML");
        if (!string.IsNullOrWhiteSpace(env) && File.Exists(env))
            return env;

        var path = Path.Combine(baseDir, "nd100-definitions", "specs", "cpu.yaml");
        if (!File.Exists(path))
            throw new FileNotFoundException(
                $"CPU definition not found at '{path}'. Run: git submodule update --init --recursive");
        return path;
    }
}
