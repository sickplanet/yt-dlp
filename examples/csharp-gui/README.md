# yt-dlp C# GUI Example

This is a simple Windows Forms application that demonstrates how to integrate yt-dlp into a C# application. The application provides a graphical user interface for downloading videos and audio from YouTube (and other supported sites).

## Features

- **Multiple URL Support**: Download from single videos, multiple videos (one URL per line), or entire playlists
- **Download Type Options**:
  - Video (Best quality)
  - Audio Only (extracts audio from video)
  - Video + Audio (Best quality, merged)
  - Custom Format
- **Format Selection**: Choose output format (mp4, webm, mkv, mp3, m4a, etc.)
- **Thumbnail Options**:
  - Download thumbnail image alongside video/audio file
  - Embed thumbnail into audio/video file
- **Real-time Output**: View yt-dlp output in real-time
- **Custom Output Path**: Choose where to save downloaded files

## Prerequisites

1. **.NET 8.0 SDK or later** (Windows): Download from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
   - Note: This is a Windows Forms application and requires Windows to build and run
2. **yt-dlp**: Must be installed and accessible in your system PATH
   - Download from [https://github.com/yt-dlp/yt-dlp/releases](https://github.com/yt-dlp/yt-dlp/releases)
   - Or install via package manager:
     ```bash
     # Windows (using winget)
     winget install yt-dlp
     
     # Windows (using chocolatey)
     choco install yt-dlp
     
     # Or place yt-dlp.exe in the same folder as the application
     ```

3. **FFmpeg** (Optional but recommended): Required for audio extraction and merging formats
   - Download from [https://ffmpeg.org/download.html](https://ffmpeg.org/download.html)
   - Or install via package manager:
     ```bash
     # Windows (using winget)
     winget install FFmpeg
     
     # Windows (using chocolatey)
     choco install ffmpeg
     ```

## Building the Application

1. Navigate to the `examples/csharp-gui` directory:
   ```bash
   cd examples/csharp-gui
   ```

2. Build the application:
   ```bash
   dotnet build
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

   Or build a release version:
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained false
   ```

## Usage

1. **Enter URLs**: Paste one or more YouTube URLs in the text box (one per line)
   - Single video: `https://www.youtube.com/watch?v=VIDEO_ID`
   - Playlist: `https://www.youtube.com/playlist?list=PLAYLIST_ID`
   
2. **Select Output Path**: Choose where you want to save the downloaded files (default: My Videos/yt-dlp)

3. **Choose Download Type**:
   - **Video (Best)**: Downloads best quality video with audio
   - **Audio Only**: Extracts audio from the video
   - **Video + Audio (Best)**: Downloads and merges best video and audio streams
   - **Custom Format**: Use default format selection

4. **Select Format**: Choose the output file format
   - Video formats: mp4, webm, mkv, avi, flv
   - Audio formats: mp3, m4a, opus, wav, flac

5. **Thumbnail Options** (optional):
   - **Download Thumbnail**: Saves thumbnail image next to the media file
   - **Embed Thumbnail**: Embeds thumbnail into the media file (requires FFmpeg)

6. **Click Download**: The application will start downloading and show progress in the output log

## How It Works

The application is a Windows Forms wrapper around the yt-dlp command-line tool. It:

1. Constructs yt-dlp command-line arguments based on user selections
2. Spawns a yt-dlp process with the appropriate arguments
3. Captures and displays the output in real-time
4. Handles errors and provides user feedback

### Code Structure

- **Program.cs**: Application entry point
- **MainForm.cs**: Main form with UI and yt-dlp integration logic
- **YtDlpGui.csproj**: Project file with dependencies

## Integrating yt-dlp in Your C# Application

This example demonstrates the key concepts for integrating yt-dlp:

### 1. Execute yt-dlp as a Process

```csharp
ProcessStartInfo startInfo = new ProcessStartInfo
{
    FileName = "yt-dlp",
    Arguments = arguments,
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    CreateNoWindow = true
};

using (Process process = Process.Start(startInfo))
{
    // Read output
    process.OutputDataReceived += (sender, e) => {
        // Handle output
    };
    
    process.BeginOutputReadLine();
    process.WaitForExit();
}
```

### 2. Common yt-dlp Arguments

```csharp
// Download best video
"-f bestvideo+bestaudio/best"

// Extract audio to MP3
"-x --audio-format mp3"

// Write thumbnail
"--write-thumbnail"

// Embed thumbnail
"--embed-thumbnail"

// Set output path
"-o \"path/to/output/%(title)s.%(ext)s\""
```

### 3. Handle Playlists

yt-dlp automatically handles playlists when you provide a playlist URL. Just pass the URL and yt-dlp will download all videos in the playlist.

## Example Commands Generated

Here are examples of the yt-dlp commands this application generates:

**Video Download (MP4)**:
```bash
yt-dlp -o "C:\Videos\%(title)s.%(ext)s" -f "bestvideo[ext=mp4]+bestaudio/best[ext=mp4]/best" "https://www.youtube.com/watch?v=VIDEO_ID"
```

**Audio Extraction (MP3) with Thumbnail**:
```bash
yt-dlp -o "C:\Music\%(title)s.%(ext)s" -x --audio-format mp3 --write-thumbnail --embed-thumbnail "https://www.youtube.com/watch?v=VIDEO_ID"
```

**Playlist Download**:
```bash
yt-dlp -o "C:\Videos\%(title)s.%(ext)s" -f "bestvideo+bestaudio/best" "https://www.youtube.com/playlist?list=PLAYLIST_ID"
```

## Troubleshooting

### "yt-dlp is not recognized as an internal or external command"

- Make sure yt-dlp is installed and in your system PATH
- Alternatively, place `yt-dlp.exe` in the same folder as the application
- Or modify the code to use the full path to yt-dlp.exe

### "FFmpeg not found" errors when extracting audio

- Install FFmpeg and add it to your system PATH
- Or place `ffmpeg.exe` in the same folder as yt-dlp

### Downloads fail with HTTP errors

- Check your internet connection
- Some videos may be region-restricted or require authentication
- Try updating yt-dlp to the latest version: `yt-dlp -U`

## Advanced Integration

For more advanced scenarios, you can:

1. **Parse JSON output**: Use `yt-dlp -j URL` to get video metadata as JSON
2. **Use progress hooks**: Parse the output to extract download progress
3. **Custom formats**: Allow users to specify custom format strings
4. **Cookie files**: Pass cookies for authenticated downloads with `--cookies`
5. **Proxy support**: Add proxy configuration with `--proxy`

## License

This example code is provided as-is for educational purposes. yt-dlp itself is licensed under the Unlicense. See the main repository LICENSE file for details.

## Further Reading

- [yt-dlp Documentation](https://github.com/yt-dlp/yt-dlp#readme)
- [yt-dlp Format Selection](https://github.com/yt-dlp/yt-dlp#format-selection)
- [yt-dlp Output Template](https://github.com/yt-dlp/yt-dlp#output-template)
- [Embedding yt-dlp](https://github.com/yt-dlp/yt-dlp#embedding-yt-dlp)
