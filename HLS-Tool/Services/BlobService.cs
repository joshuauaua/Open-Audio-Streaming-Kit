using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace Hyper_Radio_API.Services.UploadServices;

public class BlobService
{
    private readonly BlobContainerClient _containerClient;

    public BlobService(IOptions<BlobSettings> options)
    {
        var settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        if (string.IsNullOrEmpty(settings.ConnectionString))
            throw new ArgumentNullException(nameof(settings.ConnectionString));

        var blobServiceClient = new BlobServiceClient(settings.ConnectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(settings.ContainerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true);
        return blobClient.Uri.ToString();
    }

    public async Task<string> DownloadFileAsync(string blobName, string localPath)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        await blobClient.DownloadToAsync(localPath);
        return localPath;
    }

    public async Task<List<string>> UploadDirectoryAsync(string directory, string trackFolder)
    {
        var urls = new List<string>();
        foreach (var file in Directory.GetFiles(directory))
        {
            await using var stream = File.OpenRead(file);
            var fileName = Path.GetFileName(file);
            var blobClient = _containerClient.GetBlobClient($"{trackFolder}/{fileName}");
            await blobClient.UploadAsync(stream, overwrite: true);
            urls.Add(blobClient.Uri.ToString());
        }
        return urls;
    }

    /// <summary>
    /// Downloads a text blob. Accepts either full URL or relative blob name.
    /// Automatically appends 'playlist.m3u8' if a folder is provided.
    /// </summary>
    public async Task<string> DownloadTextAsync(string blobUrlOrName)
    {
        if (string.IsNullOrWhiteSpace(blobUrlOrName))
            throw new ArgumentException("Blob URL or name cannot be empty.", nameof(blobUrlOrName));

        // Extract relative blob name if full URL is passed
        var blobName = blobUrlOrName.Contains(".core.windows.net/")
            ? blobUrlOrName[(blobUrlOrName.IndexOf(_containerClient.Name) + _containerClient.Name.Length + 1)..]
            : blobUrlOrName;

        // If the blobName is a folder (no extension), append 'playlist.m3u8'
        if (!Path.HasExtension(blobName))
        {
            blobName = blobName.TrimEnd('/') + "/playlist.m3u8";
        }

        var blobClient = _containerClient.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync())
            throw new FileNotFoundException($"Blob '{blobName}' does not exist in container '{_containerClient.Name}'.");

        using var stream = new MemoryStream();
        await blobClient.DownloadToAsync(stream);
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}

public class BlobSettings
{
    public string ConnectionString { get; set; } = default!;
    public string ContainerName { get; set; } = default!;
}