# 🎧 HLS-Tool

**HLS-Tool** is an open-source utility for developers working with media applications — especially audio streaming.  
It provides two key under-the-hood processes commonly required in audio streaming services:

1. **HLS Conversion using FFmpeg**  
   Upload an audio file (e.g. `.mp3`), convert it into segmented `.ts` chunks and generate an `.m3u8` playlist for HTTP Live Streaming.

2. **Upload Converted Output to Blob Storage**  
   After conversion, the generated files are uploaded to Blob storage and an access link to the `.m3u8` file is returned — ready for streaming players.

The project includes:
- A demo **MVC UI** for testing
- **EF Core (SQLite) sample database**
- **Service-layer architecture** for easy modification
- Extensible and open for contributions

---

## 🚀 Why HLS and Blob Storage?

### HLS (HTTP Live Streaming)
HLS breaks media (like mp3) into small chunks and provides a playlist `.m3u8` file.  
This enables:
- Adaptive bitrate streaming
- Efficient buffering for slow networks
- Mobile + browser playback compatibility
- Industry standard media delivery format

### Blob Storage
Cloud storage (Azure Blob in example) is ideal for hosting HLS output because:
- Public URLs can be served to media players
- Scalability & global content distribution
- Separation of processing and distribution
- Can integrate into CDN or streaming layers

Local conversion happens first (CPU heavy), then results are uploaded.

---

## 🛠️ Getting Started

### Requirements
- **.NET 9**
- **FFmpeg installed locally**
- **Azure Blob storage (or modify to your provider)**
- EF Core SQLite already configured by default

### Setup
```bash
git clone https://github.com/yourname/HLS-Tool.git
cd HLS-Tool
dotnet restore
```

Update configuration

Edit appsettings.json:

```
"AzureBlob": {
  "ConnectionString": "...",
  "ContainerName": "..."
}

"FFmpegSettings": {
  "LocalWorkDirectory": "/path/to/processing"
}
```


Run project:

```
dotnet run
```

Upload an audio file → Converts → Uploads → Returns .m3u8 streaming URL.


🧩 Project Architecture

```

HLS-Tool/
 ├── Services/       // Conversion, upload & processing logic
 ├── Controllers/    // MVC UI + API endpoints
 ├── Views/          // Simple demo UI for interaction
 ├── Data/           // EF Core + SQLite context
 └── wwwroot/        // Static files
```


🤝 Contribution & Issues

This project is open-source and contributions are welcome!

Submit Contributions
	1.	Fork repo
	2.	Create feature branch
git checkout -b feature/my-improvement
	3.	Commit changes with meaningful messages
	4.	Submit a Pull Request

Reporting Issues
	•	Use GitHub Issues for bugs, enhancements, questions
	•	Include logs, steps to reproduce, and environment info where possible

Ideas to contribute
	•	Support additional storage providers (S3, GCP, MinIO)
	•	UI upgrades for upload + playback preview
	•	Video support & bitrate ladder configuration
	•	Docker-based FFmpeg automation


License

MIT — free to use, modify, and distribute.


Enjoy building streaming experiences.

If you create something using this tool, share it! Dont forget to start this repository as well!
