using System;
using System.Collections.Generic;

namespace databaseEditor.Models;

public partial class UniquePair
{
    public int? EmailId { get; set; }

    public string? IssueKey { get; set; }

    public int? EmailThreadId { get; set; }

    public string? IssueParentKey { get; set; }

    public int? Pattern { get; set; }

    public decimal? Similarity { get; set; }
}
