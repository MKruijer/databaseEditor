using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace databaseEditor.Models;

public partial class DataEmailEmail
{
    public long Id { get; set; }

    public long? ParentId { get; set; }

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

    public virtual ICollection<ResultArchEmailsAllIssue> ResultArchEmailsAllIssues { get; set; } = new List<ResultArchEmailsAllIssue>();

    public virtual ICollection<ResultArchIssuesAllEmail> ResultArchIssuesAllEmails { get; set; } = new List<ResultArchIssuesAllEmail>();

    public virtual ICollection<DataEmailTag> Tags { get; set; } = new List<DataEmailTag>();
}
