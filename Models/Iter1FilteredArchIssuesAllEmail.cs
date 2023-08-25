using System;
using System.Collections.Generic;

namespace databaseEditor.Models;

public partial class Iter1FilteredArchIssuesAllEmail
{
    public int? Id { get; set; }

    public int? EmailId { get; set; }

    public string? IssueKey { get; set; }

    public decimal? Similarity { get; set; }

    public DateTime? EmailDate { get; set; }

    public DateTime? IssueCreated { get; set; }

    public int? EmailWordCount { get; set; }

    public int? IssueDescriptionWordCount { get; set; }

    public int? SmallestWordCount { get; set; }

    public int? EmailThreadId { get; set; }

    public string? IssueParentKey { get; set; }

    public int? CreationTimeDifference { get; set; }

    public int? Pattern { get; set; }
}
