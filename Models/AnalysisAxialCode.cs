using System;
using System.Collections.Generic;

namespace databaseEditor.Models;

public partial class AnalysisAxialCode
{
    public string Name { get; set; } = null!;

    public bool MeaningfulRelation { get; set; }

    public virtual ICollection<AnalysisArchEmailsAllIssue> EmailIssues { get; set; } = new List<AnalysisArchEmailsAllIssue>();

    public virtual ICollection<AnalysisArchIssuesAllEmail> IssueEmails { get; set; } = new List<AnalysisArchIssuesAllEmail>();
}
