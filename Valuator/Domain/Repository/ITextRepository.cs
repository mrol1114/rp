namespace Domain.Repository;

using Domain.Model;

public interface ITextRepository
{
    public Text Store(Text text);
    public Text? Get(string id);
    public List<Text> GetAll();
}