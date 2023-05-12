using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace databaseEditor.Models;

public partial class DataJiraJiraIssue
{
    public string Key { get; set; } = null!;

    public string? Description { get; set; }

    public string? Summary { get; set; }

    public bool? IsDesign { get; set; }

    public bool? IsCatExistence { get; set; }

    public bool? IsCatExecutive { get; set; }

    public bool? IsCatProperty { get; set; }

    public NpgsqlTsVector? DescriptionSummaryVector { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public long? DescriptionWordCount { get; set; }

    public virtual ICollection<DataJiraJiraIssueComment> DataJiraJiraIssueComments { get; set; } = new List<DataJiraJiraIssueComment>();

    public virtual ICollection<ResultArchEmailsAllIssue> ResultArchEmailsAllIssues { get; set; } = new List<ResultArchEmailsAllIssue>();

    public virtual ICollection<ResultArchIssuesAllEmail> ResultArchIssuesAllEmails { get; set; } = new List<ResultArchIssuesAllEmail>();
}
