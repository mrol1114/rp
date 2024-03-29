using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Repository;
using Domain.Model;

namespace Valuator.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    readonly ITextRepository _textRepository;

    public IndexModel(ILogger<IndexModel> logger, ITextRepository textRepository)
    {
        _logger = logger;
        _textRepository = textRepository;
    }

    public void OnGet()
    {

    }

    public IActionResult OnPost(string text)
    {
        _logger.LogDebug(text);

        Text textModel = new()
        {
            Rank = CalculateRank(text),
            Similarity = CalculateSimilarity(text),
            Content = text
        };
        textModel = _textRepository.Store(textModel);

        return Redirect($"summary?id={textModel.Id}");
    }

    private double CalculateRank(string text)
    {
        int alphabetSymbolsCount = 0;
        foreach (char ch in text) alphabetSymbolsCount += char.IsLetter(ch) ? 1 : 0; 

        return alphabetSymbolsCount / text.Length;
    }

    private int CalculateSimilarity(string text)
    {
        return _textRepository.GetAll().Find(textModel => textModel.Content == text) != null ? 1 : 0;
    }
}
