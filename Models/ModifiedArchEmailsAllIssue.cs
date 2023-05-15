﻿using System;
using System.Collections.Generic;

namespace databaseEditor.Models;

public partial class ModifiedArchEmailsAllIssue
{
    public int ModifyPairId { get; set; }

    public int EmailId { get; set; }

    public string IssueKey { get; set; } = null!;

    public decimal? Similarity { get; set; }

    public string? EmailBody { get; set; }

    public DateTime? EmailDate { get; set; }

    public string? IssueSummary { get; set; }

    public string? IssueDescription { get; set; }

    public DateTime? IssueCreated { get; set; }

    public DateTime? IssueModified { get; set; }

    public int? EmailThreadId { get; set; }

    public long? EmailWordCount { get; set; }

    public long? IssueDescriptionWordCount { get; set; }

    public int? CreationTimeDifference { get; set; }
}
