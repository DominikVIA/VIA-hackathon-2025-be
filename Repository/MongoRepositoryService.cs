using Dtos;
using Entities;
using MongoDB.Driver;

namespace Repository;

public class MongoRepositoryService : IRepositoryService
{
    private readonly IMongoCollection<Thought> _thoughtsCollection;

    public MongoRepositoryService(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _thoughtsCollection = database.GetCollection<Thought>("Thoughts");
    }

    public async Task<List<Thought>> GetAllAsync()
    {
        return await _thoughtsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Thought> GetByIdAsync(string id)
    {
        return await _thoughtsCollection.Find(t => t.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Thought> AddAsync(CreateThoughtDto thoughtToAdd)
    {
        Thought thought = new Thought
        {
            Title = thoughtToAdd.Title,
            Content = thoughtToAdd.Content,
        };
        
        await _thoughtsCollection.InsertOneAsync(thought);
        return await _thoughtsCollection.Find(t => t.Id == thought.Id).FirstOrDefaultAsync();
    }

    public async Task UpdateThoughtAsync(string id, UpdateThoughtDto thoughtToUpdate)
    {
        Thought updatedThought = new Thought
        {
            Id = id,
            Title = thoughtToUpdate.Title,
            Content = thoughtToUpdate.Content,
        };
        
        await _thoughtsCollection.ReplaceOneAsync(t => t.Id == id,
            updatedThought);
    }

    public async Task DeleteThoughtAsync(string id)
    {
        await _thoughtsCollection.DeleteOneAsync(t => t.Id == id);
    }
}