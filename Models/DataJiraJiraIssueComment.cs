using System;
using System.Collections.Generic;

namespace databaseEditor.Models;

public partial class DataJiraJiraIssueComment
{
    public string IssueKey { get; set; } = null!;

    public string Body { get; set; } = null!;

    public int InternalId { get; set; }

    public virtual DataJiraJiraIssue IssueKeyNavigation { get; set; } = null!;
}
