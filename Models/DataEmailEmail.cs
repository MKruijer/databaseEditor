using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace databaseEditor.Models;

public partial class DataEmailEmail
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public string? MessageId { get; set; }

    public string? Subject { get; set; }

    public string? InReplyTo { get; set; }

    public string? SentFrom { get; set; }

    public DateTime? Date { get; set; }

    public string? Body { get; set; }

    public bool? Hidden { get; set; }

    public NpgsqlTsVector? BodyVector { get; set; }

    public string? BodyParsed { get; set; }

    public int? ThreadId { get; set; }

    public int? WordCount { get; set; }

    public virtual ICollection<Iter4CosSimResultArchIssuesAllEmail> Iter4CosSimResultArchIssuesAllEmails { get; set; } = new List<Iter4CosSimResultArchIssuesAllEmail>();

    public virtual ICollection<Iter4SenSimResultArchIssuesAllEmail> Iter4SenSimResultArchIssuesAllEmails { get; set; } = new List<Iter4SenSimResultArchIssuesAllEmail>();

    public virtual ICollection<ResultArchEmailsAllIssue> ResultArchEmailsAllIssues { get; set; } = new List<ResultArchEmailsAllIssue>();

    public virtual ICollection<ResultArchIssuesAllEmail> ResultArchIssuesAllEmails { get; set; } = new List<ResultArchIssuesAllEmail>();

    public virtual ICollection<SimResultArchEmailsAllIssue> SimResultArchEmailsAllIssues { get; set; } = new List<SimResultArchEmailsAllIssue>();

    public virtual ICollection<SimResultArchIssuesAllEmail> SimResultArchIssuesAllEmails { get; set; } = new List<SimResultArchIssuesAllEmail>();

    public virtual ICollection<DataEmailTag> Tags { get; set; } = new List<DataEmailTag>();
}
