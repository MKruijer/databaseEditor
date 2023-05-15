﻿using System;
using System.Collections.Generic;

namespace databaseEditor.Models;

public partial class AnalysisArchEmailsAllIssue
{
    public int EmailId { get; set; }

    public string IssueKey { get; set; } = null!;

    public decimal? Similarity { get; set; }

    public string? EmailBody { get; set; }

    public DateTime? EmailDate { get; set; }

    public string? IssueSummary { get; set; }

    public string? IssueDescription { get; set; }

    public DateTime? IssueCreated { get; set; }

    public DateTime? IssueModified { get; set; }

    public string? OpenCoding { get; set; }

    public int Id { get; set; }

    public virtual ICollection<AnalysisAxialCode> AxialCodes { get; set; } = new List<AnalysisAxialCode>();
}
