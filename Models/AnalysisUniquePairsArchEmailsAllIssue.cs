using System;
using System.Collections.Generic;

namespace databaseEditor.Models;

public partial class AnalysisUniquePairsArchEmailsAllIssue
{
    public int? Id { get; set; }

    public int? EmailId { get; set; }

    public string? IssueKey { get; set; }

    public decimal? Similarity { get; set; }

    public long? EmailWordCount { get; set; }

    public long? IssueDescriptionWordCount { get; set; }

    public long? SmallestWordCountas { get; set; }

    public int? CreationTimeDifference { get; set; }

    public int? EmailThreadId { get; set; }

    public string? IssueParentKey { get; set; }
}
