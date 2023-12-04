using Formatter;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Runtime.InteropServices;

namespace NiceSon;

internal static class Program
{
    static void Main(string[] args)
    {
        var inputParams = new RootCommand
        {
            new Option<bool?>(new[] { "--no-clipboard", "-nc" }, "Disable storing the formatted JSON in the clipboard"),
            new Option<bool?>(new[] { "--console", "--terminal", "-t" }, "Output the formatted JSON to the console"),
            new Option<string?>(new[] { "--file", "-f" }, "Directory to output a text file containing the formatted JSON"),
            new Argument<string>("json", "The JSON to format")
        };

        inputParams.Handler = CommandHandler.Create<bool?, bool?, string?, string>(HandleCommand);
        inputParams.Invoke(args);
    }

    private static void HandleCommand(bool? noClipboard, bool? terminal, string? file, string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("No JSON entered");
            Console.ResetColor();
            return;
        }

        var cleanedJson = json
            .Replace("{", "{\"")
            .Replace(":", "\":\"")
            .Replace(",", "\",\"")
            .Replace("}", "\"}");

        var formattedJson = JsonFormatter.Format(cleanedJson);

        if (terminal == true)
            Console.WriteLine(formattedJson);

        if (noClipboard != true)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Formatting directly to the clipboard is currently only supported in Windows. Please consider running with the --no-clipboard option.");
                Console.ResetColor();
            }

            WindowsClipboard.SetClipboardData(formattedJson);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Formatted JSON copied to clipboard");
            Console.ResetColor();
        }

        if (!string.IsNullOrWhiteSpace(file))
        {
            var outputFilePath = string.Empty;

            try
            {
                outputFilePath = Path.Combine(file, $"json-{DateTime.Now.Ticks}.txt");
                File.WriteAllText(outputFilePath, formattedJson);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Error saving the JSON file to \"{outputFilePath}\"");
                Console.ResetColor();
            }
        }
    }
}