using System;
using System.Collections.Generic;

namespace databaseEditor.Models;

public partial class Iter5SimResultArchEmailsAllIssue
{
    public int EmailId { get; set; }

    public string IssueKey { get; set; } = null!;

    public decimal? Similarity { get; set; }

    public virtual DataEmailEmail Email { get; set; } = null!;

    public virtual DataJiraJiraIssue IssueKeyNavigation { get; set; } = null!;
}
