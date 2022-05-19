using Azure.Storage.Blobs;

namespace vizion_test.Services;

public class AzureStorageService
{
    private readonly IConfiguration _configuration;
    private readonly BlobContainerClient _container;
    public AzureStorageService(IConfiguration configuration)
    {
        _configuration = configuration;
        _container = new BlobContainerClient(
            _configuration["BlobStorage:ConnectionString"],
            _configuration["BlobStorage:ContainerName"]);
    }

    public async Task UploadFileToBlobStorage(string applicantId, string path)
    {
        var stream = File.OpenRead(path);
        var blob = _container.GetBlobClient(applicantId + '/' + stream.Name.Split("\\").Last());

        var blodsExist = _container.GetBlobs(prefix: applicantId);
        if (blodsExist.Any())
        {
            foreach (var file in blodsExist)
            {
                await _container.GetBlobClient(file.Name).DeleteIfExistsAsync();
            }
        }
        await blob.UploadAsync(stream, true);
    }
}
