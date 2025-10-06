# yt-dlp C# Console Example

A simple cross-platform console application that demonstrates how to integrate yt-dlp into a C# application. This example works on Windows, Linux, and macOS.

## Features

- Download videos from YouTube and other supported sites
- Extract audio from videos
- Download and embed thumbnails
- Support for playlists
- Cross-platform (works on Windows, Linux, macOS)
- Simple command-line interface

## Prerequisites

1. **.NET 8.0 SDK or later**: Download from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
2. **yt-dlp**: Must be installed and accessible in your system PATH
   - Download from [https://github.com/yt-dlp/yt-dlp/releases](https://github.com/yt-dlp/yt-dlp/releases)
   - Or install via package manager (see main repository README)

3. **FFmpeg** (Optional but recommended): Required for audio extraction and merging formats
   - Download from [https://ffmpeg.org/download.html](https://ffmpeg.org/download.html)
   - Or install via package manager

## Building and Running

1. Navigate to the example directory:
   ```bash
   cd examples/csharp-console
   ```

2. Build the application:
   ```bash
   dotnet build
   ```

3. Run the application:
   ```bash
   dotnet run -- <URL> [options]
   ```

   Or build and run a release version:
   ```bash
   dotnet run -c Release -- <URL> [options]
   ```

## Usage Examples

### Download a video (best quality MP4)
```bash
dotnet run -- "https://www.youtube.com/watch?v=VIDEO_ID"
```

### Extract audio to MP3
```bash
dotnet run -- "https://www.youtube.com/watch?v=VIDEO_ID" --audio
```

### Download with thumbnail
```bash
dotnet run -- "https://www.youtube.com/watch?v=VIDEO_ID" --audio --thumbnail
```

### Download to specific directory with custom format
```bash
dotnet run -- "https://www.youtube.com/watch?v=VIDEO_ID" --audio --format m4a --output "./my-music"
```

### Download entire playlist
```bash
dotnet run -- "https://www.youtube.com/playlist?list=PLAYLIST_ID" --audio
```

## Command-Line Options

- `--audio` - Extract audio only (default format: mp3)
- `--thumbnail` - Download and embed thumbnail image
- `--format <format>` - Specify output format (mp3, mp4, webm, m4a, etc.)
- `--output <path>` - Specify output directory (default: ./downloads)

## How It Works

This example demonstrates:

1. **Process Execution**: Spawning yt-dlp as a child process
2. **Argument Building**: Constructing command-line arguments based on user options
3. **Output Redirection**: Capturing and displaying yt-dlp output in real-time
4. **Error Handling**: Detecting and reporting errors from yt-dlp
5. **Async/Await**: Using asynchronous programming for non-blocking I/O

### Code Structure

```csharp
// Build arguments
List<string> arguments = new();
arguments.Add("-o");
arguments.Add(Path.Combine(outputPath, "%(title)s.%(ext)s"));

if (audioOnly)
{
    arguments.Add("-x");
    arguments.Add("--audio-format");
    arguments.Add(format);
}

// Execute yt-dlp
ProcessStartInfo startInfo = new()
{
    FileName = "yt-dlp",
    Arguments = string.Join(" ", arguments),
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    CreateNoWindow = true
};

using Process process = Process.Start(startInfo);
// ... handle output and errors
```

## Creating a Standalone Executable

You can create a self-contained executable that doesn't require .NET to be installed:

### Windows
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

### Linux
```bash
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
```

### macOS
```bash
dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true
```

The executable will be in `bin/Release/net8.0/<runtime>/publish/`

## Extending the Example

This example can be extended to:

1. **Parse yt-dlp JSON output** for metadata
2. **Implement progress tracking** by parsing download progress
3. **Add custom post-processing** after download
4. **Support batch downloads** from a file
5. **Add GUI** (see [../csharp-gui](../csharp-gui) for Windows Forms example)

## Troubleshooting

### "yt-dlp is not recognized..."

- Ensure yt-dlp is installed and in your PATH
- On Windows, you may need to restart your terminal after installation
- Try using the full path to yt-dlp.exe in the code

### Permission errors

- On Linux/macOS, make sure yt-dlp is executable: `chmod +x /usr/local/bin/yt-dlp`

## See Also

- [C# GUI Example](../csharp-gui) - Windows Forms application with graphical interface
- [Main yt-dlp README](../../README.md) - Full yt-dlp documentation
- [Format Selection](../../README.md#format-selection) - Advanced format selection options
