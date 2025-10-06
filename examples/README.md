# yt-dlp Integration Examples

This directory contains examples demonstrating how to integrate yt-dlp into applications written in various programming languages.

## Available Examples

### [C# Console Application](csharp-console/)

A cross-platform command-line application that demonstrates basic yt-dlp integration in C#.

**Features:**
- Download videos and audio
- Extract audio to various formats (mp3, m4a, opus, etc.)
- Thumbnail download and embedding
- Playlist support
- Works on Windows, Linux, and macOS

**Requirements:**
- .NET 8.0 or later
- yt-dlp installed on the system
- FFmpeg (optional, for audio extraction)

See [csharp-console/README.md](csharp-console/README.md) for detailed instructions.

### [C# GUI Application](csharp-gui/)

A Windows Forms application that provides a graphical user interface for downloading videos and audio using yt-dlp.

**Features:**
- Download from single videos, multiple videos, or playlists
- Audio extraction (mp3, m4a, opus, etc.)
- Video downloads with format selection
- Thumbnail download and embedding
- Real-time download progress

**Requirements:**
- .NET 8.0 or later (Windows only)
- yt-dlp installed on the system
- FFmpeg (optional, for audio extraction)

See [csharp-gui/README.md](csharp-gui/README.md) for detailed instructions.

## Other Integration Methods

While this directory currently contains examples in specific languages, yt-dlp can be integrated with any programming language that can execute command-line programs or make HTTP requests.

### Python

For Python integration, refer to the [EMBEDDING YT-DLP](../README.md#embedding-yt-dlp) section in the main README. Python users can directly import and use yt-dlp as a library:

```python
from yt_dlp import YoutubeDL

ydl_opts = {
    'format': 'bestaudio/best',
    'postprocessors': [{
        'key': 'FFmpegExtractAudio',
        'preferredcodec': 'mp3',
    }]
}

with YoutubeDL(ydl_opts) as ydl:
    ydl.download(['https://www.youtube.com/watch?v=VIDEO_ID'])
```

### JavaScript/Node.js

For Node.js, you can use child_process to execute yt-dlp:

```javascript
const { exec } = require('child_process');

exec('yt-dlp -f bestaudio -x --audio-format mp3 "URL"', (error, stdout, stderr) => {
    if (error) {
        console.error(`Error: ${error}`);
        return;
    }
    console.log(stdout);
});
```

### Java

Java applications can use ProcessBuilder:

```java
ProcessBuilder pb = new ProcessBuilder("yt-dlp", "-f", "bestaudio", "-x", "--audio-format", "mp3", "URL");
Process process = pb.start();
```

### PHP

PHP can use exec() or shell_exec():

```php
$output = shell_exec('yt-dlp -f bestaudio -x --audio-format mp3 "URL"');
echo $output;
```

## Contributing Examples

If you've created an integration example in another language and would like to contribute it, please:

1. Create a new directory under `examples/` with a descriptive name
2. Include a comprehensive README.md with setup and usage instructions
3. Add your example to the list above
4. Submit a pull request

Examples should be:
- Well-documented
- Easy to understand and modify
- Include all necessary dependencies and setup instructions
- Follow best practices for the respective language

## Resources

- [yt-dlp README](../README.md)
- [yt-dlp Wiki](https://github.com/yt-dlp/yt-dlp/wiki)
- [Format Selection](../README.md#format-selection)
- [Output Templates](../README.md#output-template)
