namespace Domain.Repository;

using System.Text.Json;
using StackExchange.Redis;
using Domain.Model;

public class TextRepository : ITextRepository
{
    private const string IDS_KEY = "IDS";
    private const string TEXT_KEY = "TEXT";
    private const string RANK_KEY = "RANK";
    private const string SIMILARITY_KEY = "SIMILARITY";
    private readonly IDatabase _database;

    public TextRepository(IDatabase database)
    {
        _database = database;
    }

    public Text Store(Text text) {
        text.Id ??= Guid.NewGuid().ToString();
        
        _database.StringSet($"{TEXT_KEY}-{text.Id}", text.Content);
        _database.StringSet($"{RANK_KEY}-{text.Id}", text.Rank);
        _database.StringSet($"{SIMILARITY_KEY}-{text.Id}", text.Similarity);

        var idsList = GetIds();
        idsList.Add(text.Id);
        _database.StringSet(IDS_KEY, JsonSerializer.Serialize(idsList));

        return text;
    }

    public Text? Get(string id) {
        var content = _database.StringGet($"{TEXT_KEY}-{id}");
        if (content.IsNull) {
            return null;
        }

        return new Text {
            Id = id,
            Rank = (double) _database.StringGet($"{RANK_KEY}-{id}"),
            Similarity = (int) _database.StringGet($"{SIMILARITY_KEY}-{id}"),
            Content = content.ToString(),
        };
    }

    public List<Text> GetAll() 
    {
        List<Text> res = [];
        foreach (string id in GetIds())
        {
            res.Add(Get(id));
        }

        return res;
    }

    private List<string> GetIds()
    {
        var ids = _database.StringGet(IDS_KEY);
        return ids.IsNullOrEmpty 
            ? []
            : JsonSerializer.Deserialize<List<string>>(_database.StringGet(IDS_KEY));
    }
}