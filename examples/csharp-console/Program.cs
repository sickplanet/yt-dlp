using System.Diagnostics;

namespace YtDlpConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("yt-dlp C# Console Example");
            Console.WriteLine("==========================\n");

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: YtDlpConsole <URL> [options]");
                Console.WriteLine("\nExample:");
                Console.WriteLine("  YtDlpConsole https://www.youtube.com/watch?v=VIDEO_ID");
                Console.WriteLine("  YtDlpConsole https://www.youtube.com/watch?v=VIDEO_ID --audio");
                Console.WriteLine("  YtDlpConsole https://www.youtube.com/playlist?list=PLAYLIST_ID --audio --thumbnail");
                Console.WriteLine("\nOptions:");
                Console.WriteLine("  --audio             Extract audio only (mp3)");
                Console.WriteLine("  --thumbnail         Download and embed thumbnail");
                Console.WriteLine("  --format <format>   Output format (mp3, mp4, webm, etc.)");
                Console.WriteLine("  --output <path>     Output directory");
                return;
            }

            string url = args[0];
            bool audioOnly = args.Contains("--audio");
            bool thumbnail = args.Contains("--thumbnail");
            string format = GetArgValue(args, "--format") ?? (audioOnly ? "mp3" : "mp4");
            string outputPath = GetArgValue(args, "--output") ?? "./downloads";

            Console.WriteLine($"URL: {url}");
            Console.WriteLine($"Audio Only: {audioOnly}");
            Console.WriteLine($"Thumbnail: {thumbnail}");
            Console.WriteLine($"Format: {format}");
            Console.WriteLine($"Output: {outputPath}\n");

            try
            {
                Directory.CreateDirectory(outputPath);
                await DownloadVideo(url, outputPath, format, audioOnly, thumbnail);
                Console.WriteLine("\n✓ Download completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error: {ex.Message}");
                Environment.Exit(1);
            }
        }

        static async Task DownloadVideo(string url, string outputPath, string format, bool audioOnly, bool thumbnail)
        {
            // Build yt-dlp arguments
            List<string> arguments = new();
            
            // Output template
            arguments.Add("-o");
            arguments.Add(Path.Combine(outputPath, "%(title)s.%(ext)s"));

            if (audioOnly)
            {
                // Extract audio
                arguments.Add("-x");
                arguments.Add("--audio-format");
                arguments.Add(format);
            }
            else
            {
                // Video download
                arguments.Add("-f");
                arguments.Add($"bestvideo[ext={format}]+bestaudio/best[ext={format}]/best");
            }

            if (thumbnail)
            {
                arguments.Add("--write-thumbnail");
                arguments.Add("--embed-thumbnail");
            }

            arguments.Add(url);

            // Execute yt-dlp
            ProcessStartInfo startInfo = new()
            {
                FileName = "yt-dlp",
                Arguments = string.Join(" ", arguments.Select(a => a.Contains(" ") ? $"\"{a}\"" : a)),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using Process? process = Process.Start(startInfo);
            if (process == null)
            {
                throw new Exception("Failed to start yt-dlp process");
            }

            // Read output asynchronously
            Task outputTask = Task.Run(() =>
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    string? line = process.StandardOutput.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        Console.WriteLine(line);
                    }
                }
            });

            Task errorTask = Task.Run(() =>
            {
                while (!process.StandardError.EndOfStream)
                {
                    string? line = process.StandardError.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        Console.Error.WriteLine($"ERROR: {line}");
                    }
                }
            });

            await Task.WhenAll(outputTask, errorTask);
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception($"yt-dlp exited with code {process.ExitCode}");
            }
        }

        static string? GetArgValue(string[] args, string argName)
        {
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == argName)
                {
                    return args[i + 1];
                }
            }
            return null;
        }
    }
}
