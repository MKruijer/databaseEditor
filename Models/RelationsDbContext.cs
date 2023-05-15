using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace databaseEditor.Models;

public partial class RelationsDbContext : DbContext
{
    public RelationsDbContext()
    {
    }

    public RelationsDbContext(DbContextOptions<RelationsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnalysisArchEmailsAllIssue> AnalysisArchEmailsAllIssues { get; set; }

    public virtual DbSet<AnalysisArchIssuesAllEmail> AnalysisArchIssuesAllEmails { get; set; }

    public virtual DbSet<AnalysisAxialCode> AnalysisAxialCodes { get; set; }

    public virtual DbSet<DataEmailEmail> DataEmailEmails { get; set; }

    public virtual DbSet<DataEmailTag> DataEmailTags { get; set; }

    public virtual DbSet<DataJiraJiraIssue> DataJiraJiraIssues { get; set; }

    public virtual DbSet<DataJiraJiraIssueComment> DataJiraJiraIssueComments { get; set; }

    public virtual DbSet<ModifiedArchEmailsAllIssue> ModifiedArchEmailsAllIssues { get; set; }

    public virtual DbSet<ModifiedArchIssuesAllEmail> ModifiedArchIssuesAllEmails { get; set; }

    public virtual DbSet<ResultArchEmailsAllIssue> ResultArchEmailsAllIssues { get; set; }

    public virtual DbSet<ResultArchIssuesAllEmail> ResultArchIssuesAllEmails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=relationsDB;Username=postgres;Password=UnsavePassword");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("tablefunc");

        modelBuilder.Entity<AnalysisArchEmailsAllIssue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("analysis_arch_emails_all_issues_pk");

            entity.ToTable("analysis_arch_emails_all_issues");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmailBody).HasColumnName("email_body");
            entity.Property(e => e.EmailDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("email_date");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.IssueCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_created");
            entity.Property(e => e.IssueDescription).HasColumnName("issue_description");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueModified)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_modified");
            entity.Property(e => e.IssueSummary).HasColumnName("issue_summary");
            entity.Property(e => e.OpenCoding).HasColumnName("open_coding");
            entity.Property(e => e.Similarity).HasColumnName("similarity");

            entity.HasMany(d => d.AxialCodes).WithMany(p => p.EmailIssues)
                .UsingEntity<Dictionary<string, object>>(
                    "AnalysisArchEmailsAllIssuesAxialCoding",
                    r => r.HasOne<AnalysisAxialCode>().WithMany()
                        .HasForeignKey("AxialCode")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("analysis_arch_emails_all_issues_axial_coding_axial_code_fkey"),
                    l => l.HasOne<AnalysisArchEmailsAllIssue>().WithMany()
                        .HasForeignKey("EmailIssueId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("analysis_arch_emails_all_issues_axial_codin_email_issue_id_fkey"),
                    j =>
                    {
                        j.HasKey("EmailIssueId", "AxialCode").HasName("analysis_arch_emails_all_issues_axial_coding_pkey");
                        j.ToTable("analysis_arch_emails_all_issues_axial_coding");
                        j.IndexerProperty<int>("EmailIssueId").HasColumnName("email_issue_id");
                        j.IndexerProperty<string>("AxialCode").HasColumnName("axial_code");
                    });
        });

        modelBuilder.Entity<AnalysisArchIssuesAllEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("analysis_arch_issues_all_emails_pk");

            entity.ToTable("analysis_arch_issues_all_emails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmailBody).HasColumnName("email_body");
            entity.Property(e => e.EmailDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("email_date");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.IssueCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_created");
            entity.Property(e => e.IssueDescription).HasColumnName("issue_description");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueModified)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_modified");
            entity.Property(e => e.IssueSummary).HasColumnName("issue_summary");
            entity.Property(e => e.OpenCoding).HasColumnName("open_coding");
            entity.Property(e => e.Similarity).HasColumnName("similarity");

            entity.HasMany(d => d.AxialCodes).WithMany(p => p.IssueEmails)
                .UsingEntity<Dictionary<string, object>>(
                    "AnalysisArchIssuesAllEmailsAxialCoding",
                    r => r.HasOne<AnalysisAxialCode>().WithMany()
                        .HasForeignKey("AxialCode")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("analysis_arch_issues_all_emails_axial_coding_axial_code_fkey"),
                    l => l.HasOne<AnalysisArchIssuesAllEmail>().WithMany()
                        .HasForeignKey("IssueEmailId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("analysis_arch_issues_all_emails_axial_codin_issue_email_id_fkey"),
                    j =>
                    {
                        j.HasKey("IssueEmailId", "AxialCode").HasName("analysis_arch_issues_all_emails_axial_coding_pkey");
                        j.ToTable("analysis_arch_issues_all_emails_axial_coding");
                        j.IndexerProperty<int>("IssueEmailId").HasColumnName("issue_email_id");
                        j.IndexerProperty<string>("AxialCode").HasColumnName("axial_code");
                    });
        });

        modelBuilder.Entity<AnalysisAxialCode>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("analysis_axial_codes_pk");

            entity.ToTable("analysis_axial_codes");

            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.MeaningfulRelation).HasColumnName("meaningful_relation");
        });

        modelBuilder.Entity<DataEmailEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("email_pkey");

            entity.ToTable("data_email_email");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('email_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Body).HasColumnName("body");
            entity.Property(e => e.BodyParsed).HasColumnName("body_parsed");
            entity.Property(e => e.BodyVector).HasColumnName("body_vector");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.Hidden).HasColumnName("hidden");
            entity.Property(e => e.InReplyTo).HasColumnName("in_reply_to");
            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.SentFrom).HasColumnName("sent_from");
            entity.Property(e => e.Subject).HasColumnName("subject");
            entity.Property(e => e.ThreadId).HasColumnName("thread_id");
            entity.Property(e => e.WordCount).HasColumnName("word_count");

            entity.HasMany(d => d.Tags).WithMany(p => p.Emails)
                .UsingEntity<Dictionary<string, object>>(
                    "DataEmailEmailTag",
                    r => r.HasOne<DataEmailTag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("email_tag_tag_id_fkey"),
                    l => l.HasOne<DataEmailEmail>().WithMany()
                        .HasForeignKey("EmailId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("email_tag_email_id_fkey"),
                    j =>
                    {
                        j.HasKey("EmailId", "TagId").HasName("email_tag_pkey");
                        j.ToTable("data_email_email_tag");
                        j.IndexerProperty<int>("EmailId").HasColumnName("email_id");
                        j.IndexerProperty<int>("TagId").HasColumnName("tag_id");
                    });
        });

        modelBuilder.Entity<DataEmailTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tag_pkey");

            entity.ToTable("data_email_tag");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('tag_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Architectural).HasColumnName("architectural");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<DataJiraJiraIssue>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("jira_issue_pkey");

            entity.ToTable("data_jira_jira_issue");

            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Created)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DescriptionSummaryVector).HasColumnName("description_summary_vector");
            entity.Property(e => e.DescriptionWordCount).HasColumnName("description_word_count");
            entity.Property(e => e.IsCatExecutive).HasColumnName("is_cat_executive");
            entity.Property(e => e.IsCatExistence).HasColumnName("is_cat_existence");
            entity.Property(e => e.IsCatProperty).HasColumnName("is_cat_property");
            entity.Property(e => e.IsDesign).HasColumnName("is_design");
            entity.Property(e => e.Modified)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified");
            entity.Property(e => e.Summary).HasColumnName("summary");
        });

        modelBuilder.Entity<DataJiraJiraIssueComment>(entity =>
        {
            entity.HasKey(e => e.InternalId).HasName("jira_issue_comment_pkey");

            entity.ToTable("data_jira_jira_issue_comment");

            entity.Property(e => e.InternalId)
                .HasDefaultValueSql("nextval('jira_issue_comment_internal_id_seq'::regclass)")
                .HasColumnName("internal_id");
            entity.Property(e => e.Body).HasColumnName("body");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");

            entity.HasOne(d => d.IssueKeyNavigation).WithMany(p => p.DataJiraJiraIssueComments)
                .HasForeignKey(d => d.IssueKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("jira_issue_comment_issue_key_fkey");
        });

        modelBuilder.Entity<ModifiedArchEmailsAllIssue>(entity =>
        {
            entity.HasKey(e => e.ModifyPairId).HasName("modify_pair_id");

            entity.ToTable("modified_arch_emails_all_issues");

            entity.Property(e => e.ModifyPairId)
                .HasDefaultValueSql("nextval('modify_arch_emails_all_issue_id_seq'::regclass)")
                .HasColumnName("modify_pair_id");
            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailBody).HasColumnName("email_body");
            entity.Property(e => e.EmailDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("email_date");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.EmailWordCount).HasColumnName("email_word_count");
            entity.Property(e => e.IssueCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_created");
            entity.Property(e => e.IssueDescription).HasColumnName("issue_description");
            entity.Property(e => e.IssueDescriptionWordCount).HasColumnName("issue_description_word_count");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueModified)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_modified");
            entity.Property(e => e.IssueSummary).HasColumnName("issue_summary");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
        });

        modelBuilder.Entity<ModifiedArchIssuesAllEmail>(entity =>
        {
            entity.HasKey(e => e.PairId).HasName("pair_id");

            entity.ToTable("modified_arch_issues_all_emails");

            entity.Property(e => e.PairId)
                .HasDefaultValueSql("nextval('modify_arch_issues_all_email_id_seq'::regclass)")
                .HasColumnName("pair_id");
            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailBody).HasColumnName("email_body");
            entity.Property(e => e.EmailDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("email_date");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.EmailWordCount).HasColumnName("email_word_count");
            entity.Property(e => e.IssueCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_created");
            entity.Property(e => e.IssueDescription).HasColumnName("issue_description");
            entity.Property(e => e.IssueDescriptionWordCount).HasColumnName("issue_description_word_count");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueModified)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_modified");
            entity.Property(e => e.IssueSummary).HasColumnName("issue_summary");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
        });

        modelBuilder.Entity<ResultArchEmailsAllIssue>(entity =>
        {
            entity.HasKey(e => new { e.IssueKey, e.EmailId }).HasName("RESULT_arch_emails_all_issues_pkey");

            entity.ToTable("result_arch_emails_all_issues");

            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.Similarity).HasColumnName("similarity");

            entity.HasOne(d => d.Email).WithMany(p => p.ResultArchEmailsAllIssues)
                .HasForeignKey(d => d.EmailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RESULT_arch_emails_all_issues_email_id_fkey");

            entity.HasOne(d => d.IssueKeyNavigation).WithMany(p => p.ResultArchEmailsAllIssues)
                .HasForeignKey(d => d.IssueKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RESULT_arch_emails_all_issues_issue_key_fkey");
        });

        modelBuilder.Entity<ResultArchIssuesAllEmail>(entity =>
        {
            entity.HasKey(e => new { e.IssueKey, e.EmailId }).HasName("RESULT_arch_issues_all_emails_pkey");

            entity.ToTable("result_arch_issues_all_emails");

            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.Similarity).HasColumnName("similarity");

            entity.HasOne(d => d.Email).WithMany(p => p.ResultArchIssuesAllEmails)
                .HasForeignKey(d => d.EmailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RESULT_arch_issues_all_emails_email_id_fkey");

            entity.HasOne(d => d.IssueKeyNavigation).WithMany(p => p.ResultArchIssuesAllEmails)
                .HasForeignKey(d => d.IssueKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RESULT_arch_issues_all_emails_issue_key_fkey");
        });
        modelBuilder.HasSequence<int>("modify_arch_emails_all_issue_id_seq");
        modelBuilder.HasSequence<int>("modify_arch_issues_all_email_id_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
