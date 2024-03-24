﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Domain.Repository;
using Domain.Model;

namespace Valuator.Pages;
public class SummaryModel : PageModel
{
    private readonly ILogger<SummaryModel> _logger;
    readonly ITextRepository _textRepository;

    public SummaryModel(ILogger<SummaryModel> logger, ITextRepository textRepository)
    {
        _logger = logger;
        _textRepository = textRepository;
    }

    public double Rank { get; set; }
    public double Similarity { get; set; }

    public void OnGet(string id)
    {
        _logger.LogDebug(id);
        Text? text = _textRepository.Get(id);

        Rank = text != null ? Math.Round(text.Rank, 2) : 0;
        Similarity = text != null ? text.Similarity : 0;
    }
}
