﻿using System;
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

    public int? DescriptionWordCount { get; set; }

    public string? ParentKey { get; set; }

    public virtual ICollection<DataJiraJiraIssueComment> DataJiraJiraIssueComments { get; set; } = new List<DataJiraJiraIssueComment>();

    public virtual ICollection<Iter4CosSimResultArchIssuesAllEmail> Iter4CosSimResultArchIssuesAllEmails { get; set; } = new List<Iter4CosSimResultArchIssuesAllEmail>();

    public virtual ICollection<Iter4SenSimResultArchIssuesAllEmail> Iter4SenSimResultArchIssuesAllEmails { get; set; } = new List<Iter4SenSimResultArchIssuesAllEmail>();

    public virtual ICollection<ResultArchEmailsAllIssue> ResultArchEmailsAllIssues { get; set; } = new List<ResultArchEmailsAllIssue>();

    public virtual ICollection<ResultArchIssuesAllEmail> ResultArchIssuesAllEmails { get; set; } = new List<ResultArchIssuesAllEmail>();

    public virtual ICollection<SimResultArchEmailsAllIssue> SimResultArchEmailsAllIssues { get; set; } = new List<SimResultArchEmailsAllIssue>();

    public virtual ICollection<SimResultArchIssuesAllEmail> SimResultArchIssuesAllEmails { get; set; } = new List<SimResultArchIssuesAllEmail>();
}
