using System;
using System.Collections.Generic;

namespace databaseEditor.Models;

public partial class ResultArchIssuesAllEmail
{
    public string IssueKey { get; set; } = null!;

    public long EmailId { get; set; }

    public decimal? Similarity { get; set; }

    public virtual DataEmailEmail Email { get; set; } = null!;

    public virtual DataJiraJiraIssue IssueKeyNavigation { get; set; } = null!;
}
