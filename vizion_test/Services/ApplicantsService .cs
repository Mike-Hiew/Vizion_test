using Microsoft.Extensions.Options;
using MongoDB.Driver;
using vizion_test.Model;

namespace vizion_test.Services;

public class ApplicantsService
{
    private readonly IMongoCollection<Applicants> _applicantsCollection;

    public ApplicantsService(
        IOptions<VizionDbSettings> myStoreDbSettings)
    {
        var mongoClient = new MongoClient(
            myStoreDbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            myStoreDbSettings.Value.DatabaseName);

        _applicantsCollection = mongoDatabase.GetCollection<Applicants>(
            myStoreDbSettings.Value.ApplicantsCollectionName);
    }

    public async Task<List<Applicants>> GetAsync() =>
        await _applicantsCollection.Find(_ => true).ToListAsync();

    public async Task<Applicants?> GetAsync(string id) =>
        await _applicantsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Applicants newApplicant) =>
        await _applicantsCollection.InsertOneAsync(newApplicant);

    public async Task UpdateAsync(string id, Applicants updatedApplicant) =>
        await _applicantsCollection.ReplaceOneAsync(x => x.Id == id, updatedApplicant);

    public async Task RemoveAsync(string id) =>
        await _applicantsCollection.DeleteOneAsync(x => x.Id == id);
}
