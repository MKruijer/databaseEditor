using System;
using System.Collections.Generic;

namespace databaseEditor.Models;

public partial class UniqueFilteredSimArchEmailsAllIssue
{
    public int Id { get; set; }

    public int EmailId { get; set; }

    public string IssueKey { get; set; } = null!;

    public decimal? Similarity { get; set; }

    public int? SmallestWordCount { get; set; }

    public int? CreationTimeDifference { get; set; }

    public int? EmailThreadId { get; set; }

    public string? IssueParentKey { get; set; }
}
