using System;
using System.Collections.Generic;

namespace databaseEditor.Models;

public partial class AnalysisUniquePairsArchEmailsAllIssue
{
    public int? Id { get; set; }

    public int? EmailId { get; set; }

    public string? IssueKey { get; set; }

    public decimal? Similarity { get; set; }

    public int? EmailWordCount { get; set; }

    public int? IssueDescriptionWordCount { get; set; }

    public int? SmallestWordCountas { get; set; }

    public int? CreationTimeDifference { get; set; }

    public int? EmailThreadId { get; set; }

    public string? IssueParentKey { get; set; }
}
