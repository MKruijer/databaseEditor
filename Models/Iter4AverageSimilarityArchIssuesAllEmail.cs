using System;
using System.Collections.Generic;

namespace databaseEditor.Models;

public partial class Iter4AverageSimilarityArchIssuesAllEmail
{
    public int? Id { get; set; }

    public int? EmailId { get; set; }

    public string? IssueKey { get; set; }

    public int? SmallestWordCount { get; set; }

    public int? CreationTimeDifference { get; set; }

    public int? EmailThreadId { get; set; }

    public string? IssueParentKey { get; set; }

    public decimal? SentenceSimilarity { get; set; }

    public decimal? CosineSimilarity { get; set; }

    public decimal? AverageSimilarity { get; set; }
}
