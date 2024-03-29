﻿using System;
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

    public virtual DbSet<DataEmailEmail> DataEmailEmails { get; set; }

    public virtual DbSet<DataJiraJiraIssue> DataJiraJiraIssues { get; set; }

    public virtual DbSet<DataJiraJiraIssueComment> DataJiraJiraIssueComments { get; set; }

    public virtual DbSet<Iter0ExpandedArchEmailsAllIssue> Iter0ExpandedArchEmailsAllIssues { get; set; }

    public virtual DbSet<Iter0ExpandedArchIssuesAllEmail> Iter0ExpandedArchIssuesAllEmails { get; set; }

    public virtual DbSet<Iter1FilteredArchEmailsAllIssue> Iter1FilteredArchEmailsAllIssues { get; set; }

    public virtual DbSet<Iter1FilteredArchIssuesAllEmail> Iter1FilteredArchIssuesAllEmails { get; set; }

    public virtual DbSet<Iter2ExpandedArchEmailsAllIssue> Iter2ExpandedArchEmailsAllIssues { get; set; }

    public virtual DbSet<Iter2ExpandedArchIssuesAllEmail> Iter2ExpandedArchIssuesAllEmails { get; set; }

    public virtual DbSet<Iter2FilteredArchEmailsAllIssue> Iter2FilteredArchEmailsAllIssues { get; set; }

    public virtual DbSet<Iter2FilteredArchIssuesAllEmail> Iter2FilteredArchIssuesAllEmails { get; set; }

    public virtual DbSet<Iter3AverageSimilarityArchEmailsAllIssue> Iter3AverageSimilarityArchEmailsAllIssues { get; set; }

    public virtual DbSet<Iter3AverageSimilarityArchIssuesAllEmail> Iter3AverageSimilarityArchIssuesAllEmails { get; set; }

    public virtual DbSet<Iter4AverageSimilarityArchIssuesAllEmail> Iter4AverageSimilarityArchIssuesAllEmails { get; set; }

    public virtual DbSet<Iter4CosSimExpandedArchIssuesAllEmail> Iter4CosSimExpandedArchIssuesAllEmails { get; set; }

    public virtual DbSet<Iter4CosSimFilteredArchIssuesAllEmail> Iter4CosSimFilteredArchIssuesAllEmails { get; set; }

    public virtual DbSet<Iter4CosSimResultArchIssuesAllEmail> Iter4CosSimResultArchIssuesAllEmails { get; set; }

    public virtual DbSet<Iter4SenSimExpandedArchIssuesAllEmail> Iter4SenSimExpandedArchIssuesAllEmails { get; set; }

    public virtual DbSet<Iter4SenSimFilteredArchIssuesAllEmail> Iter4SenSimFilteredArchIssuesAllEmails { get; set; }

    public virtual DbSet<Iter4SenSimResultArchIssuesAllEmail> Iter4SenSimResultArchIssuesAllEmails { get; set; }

    public virtual DbSet<ResultArchEmailsAllIssue> ResultArchEmailsAllIssues { get; set; }

    public virtual DbSet<ResultArchIssuesAllEmail> ResultArchIssuesAllEmails { get; set; }

    public virtual DbSet<SimResultArchEmailsAllIssue> SimResultArchEmailsAllIssues { get; set; }

    public virtual DbSet<SimResultArchIssuesAllEmail> SimResultArchIssuesAllEmails { get; set; }

    public virtual DbSet<UniquePair> UniquePairs { get; set; }

    public virtual DbSet<UniquePairsUnionAdd> UniquePairsUnionAdds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=relationsDB;Username=postgres;Password=UnsavePassword");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("tablefunc");

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
            entity.Property(e => e.IsExecutive).HasColumnName("is_executive");
            entity.Property(e => e.IsExistence).HasColumnName("is_existence");
            entity.Property(e => e.IsProperty).HasColumnName("is_property");
            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.SentFrom).HasColumnName("sent_from");
            entity.Property(e => e.Subject).HasColumnName("subject");
            entity.Property(e => e.ThreadId).HasColumnName("thread_id");
            entity.Property(e => e.WordCount).HasColumnName("word_count");
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
            entity.Property(e => e.ParentKey).HasColumnName("parent_key");
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

        modelBuilder.Entity<Iter0ExpandedArchEmailsAllIssue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("iter0_expanded_arch_emails_all_issues_pkey");

            entity.ToTable("iter0_expanded_arch_emails_all_issues");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("email_date");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.EmailWordCount).HasColumnName("email_word_count");
            entity.Property(e => e.IssueCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_created");
            entity.Property(e => e.IssueDescriptionWordCount).HasColumnName("issue_description_word_count");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Pattern).HasColumnName("pattern");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter0ExpandedArchIssuesAllEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("iter0_expanded_arch_issues_all_emails_pkey");

            entity.ToTable("iter0_expanded_arch_issues_all_emails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("email_date");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.EmailWordCount).HasColumnName("email_word_count");
            entity.Property(e => e.IssueCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_created");
            entity.Property(e => e.IssueDescriptionWordCount).HasColumnName("issue_description_word_count");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Pattern).HasColumnName("pattern");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter1FilteredArchEmailsAllIssue>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("iter1_filtered_arch_emails_all_issues");

            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("email_date");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.EmailWordCount).HasColumnName("email_word_count");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IssueCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_created");
            entity.Property(e => e.IssueDescriptionWordCount).HasColumnName("issue_description_word_count");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Pattern).HasColumnName("pattern");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter1FilteredArchIssuesAllEmail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("iter1_filtered_arch_issues_all_emails");

            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("email_date");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.EmailWordCount).HasColumnName("email_word_count");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IssueCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_created");
            entity.Property(e => e.IssueDescriptionWordCount).HasColumnName("issue_description_word_count");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Pattern).HasColumnName("pattern");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter2ExpandedArchEmailsAllIssue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("iter2_expanded_arch_emails_all_issues_pkey");

            entity.ToTable("iter2_expanded_arch_emails_all_issues");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("email_date");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.EmailWordCount).HasColumnName("email_word_count");
            entity.Property(e => e.IssueCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_created");
            entity.Property(e => e.IssueDescriptionWordCount).HasColumnName("issue_description_word_count");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter2ExpandedArchIssuesAllEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("iter2_expanded_arch_issues_all_emails_pkey");

            entity.ToTable("iter2_expanded_arch_issues_all_emails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("email_date");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.EmailWordCount).HasColumnName("email_word_count");
            entity.Property(e => e.IssueCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_created");
            entity.Property(e => e.IssueDescriptionWordCount).HasColumnName("issue_description_word_count");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter2FilteredArchEmailsAllIssue>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("iter2_filtered_arch_emails_all_issues");

            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("email_date");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.EmailWordCount).HasColumnName("email_word_count");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IssueCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_created");
            entity.Property(e => e.IssueDescriptionWordCount).HasColumnName("issue_description_word_count");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter2FilteredArchIssuesAllEmail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("iter2_filtered_arch_issues_all_emails");

            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("email_date");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.EmailWordCount).HasColumnName("email_word_count");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IssueCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_created");
            entity.Property(e => e.IssueDescriptionWordCount).HasColumnName("issue_description_word_count");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter3AverageSimilarityArchEmailsAllIssue>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("iter3_average_similarity_arch_emails_all_issues");

            entity.Property(e => e.AverageSimilarity).HasColumnName("average_similarity");
            entity.Property(e => e.CosineSimilarity).HasColumnName("cosine_similarity");
            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Pattern).HasColumnName("pattern");
            entity.Property(e => e.SentenceSimilarity).HasColumnName("sentence_similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter3AverageSimilarityArchIssuesAllEmail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("iter3_average_similarity_arch_issues_all_emails");

            entity.Property(e => e.AverageSimilarity).HasColumnName("average_similarity");
            entity.Property(e => e.CosineSimilarity).HasColumnName("cosine_similarity");
            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Pattern).HasColumnName("pattern");
            entity.Property(e => e.SentenceSimilarity).HasColumnName("sentence_similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter4AverageSimilarityArchIssuesAllEmail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("iter4_average_similarity_arch_issues_all_emails");

            entity.Property(e => e.AverageSimilarity).HasColumnName("average_similarity");
            entity.Property(e => e.CosineSimilarity).HasColumnName("cosine_similarity");
            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Pattern).HasColumnName("pattern");
            entity.Property(e => e.SentenceSimilarity).HasColumnName("sentence_similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter4CosSimExpandedArchIssuesAllEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("iter4_cos_sim_expanded_arch_issues_all_emails_pkey");

            entity.ToTable("iter4_cos_sim_expanded_arch_issues_all_emails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("email_date");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.EmailWordCount).HasColumnName("email_word_count");
            entity.Property(e => e.IssueCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_created");
            entity.Property(e => e.IssueDescriptionWordCount).HasColumnName("issue_description_word_count");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter4CosSimFilteredArchIssuesAllEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("iter4_cos_sim_filtered_arch_issues_all_emails_pkey");

            entity.ToTable("iter4_cos_sim_filtered_arch_issues_all_emails");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter4CosSimResultArchIssuesAllEmail>(entity =>
        {
            entity.HasKey(e => new { e.IssueKey, e.EmailId }).HasName("iter4_RESULT_arch_issues_all_emails_pkey");

            entity.ToTable("iter4_cos_sim_result_arch_issues_all_emails");

            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.Similarity).HasColumnName("similarity");

            entity.HasOne(d => d.Email).WithMany(p => p.Iter4CosSimResultArchIssuesAllEmails)
                .HasForeignKey(d => d.EmailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("iter4_RESULT_arch_issues_all_emails_email_id_fkey");

            entity.HasOne(d => d.IssueKeyNavigation).WithMany(p => p.Iter4CosSimResultArchIssuesAllEmails)
                .HasForeignKey(d => d.IssueKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("iter4_RESULT_arch_issues_all_emails_issue_key_fkey");
        });

        modelBuilder.Entity<Iter4SenSimExpandedArchIssuesAllEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("iter4_sim_expanded_arch_issues_all_emails_pkey");

            entity.ToTable("iter4_sen_sim_expanded_arch_issues_all_emails");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('iter4_sim_expanded_arch_issues_all_emails_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("email_date");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.EmailWordCount).HasColumnName("email_word_count");
            entity.Property(e => e.IssueCreated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issue_created");
            entity.Property(e => e.IssueDescriptionWordCount).HasColumnName("issue_description_word_count");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter4SenSimFilteredArchIssuesAllEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("iter4_sen_sim_filtered_arch_issues_all_emails_pkey");

            entity.ToTable("iter4_sen_sim_filtered_arch_issues_all_emails");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTimeDifference).HasColumnName("creation_time_difference");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
            entity.Property(e => e.SmallestWordCount).HasColumnName("smallest_word_count");
        });

        modelBuilder.Entity<Iter4SenSimResultArchIssuesAllEmail>(entity =>
        {
            entity.HasKey(e => new { e.IssueKey, e.EmailId }).HasName("iter4_SIM_RESULT_arch_issues_all_emails_pkey");

            entity.ToTable("iter4_sen_sim_result_arch_issues_all_emails");

            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.Similarity).HasColumnName("similarity");

            entity.HasOne(d => d.Email).WithMany(p => p.Iter4SenSimResultArchIssuesAllEmails)
                .HasForeignKey(d => d.EmailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("iter4_SIM_RESULT_arch_issues_all_emails_email_id_fkey");

            entity.HasOne(d => d.IssueKeyNavigation).WithMany(p => p.Iter4SenSimResultArchIssuesAllEmails)
                .HasForeignKey(d => d.IssueKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("iter4_SIM_RESULT_arch_issues_all_emails_issue_key_fkey");
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

        modelBuilder.Entity<SimResultArchEmailsAllIssue>(entity =>
        {
            entity.HasKey(e => new { e.IssueKey, e.EmailId }).HasName("SIM_RESULT_arch_emails_all_issues_pkey");

            entity.ToTable("sim_result_arch_emails_all_issues");

            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.Similarity).HasColumnName("similarity");

            entity.HasOne(d => d.Email).WithMany(p => p.SimResultArchEmailsAllIssues)
                .HasForeignKey(d => d.EmailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SIM_RESULT_arch_emails_all_issues_email_id_fkey");

            entity.HasOne(d => d.IssueKeyNavigation).WithMany(p => p.SimResultArchEmailsAllIssues)
                .HasForeignKey(d => d.IssueKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SIM_RESULT_arch_emails_all_issues_issue_key_fkey");
        });

        modelBuilder.Entity<SimResultArchIssuesAllEmail>(entity =>
        {
            entity.HasKey(e => new { e.IssueKey, e.EmailId }).HasName("SIM_RESULT_arch_issues_all_emails_pkey");

            entity.ToTable("sim_result_arch_issues_all_emails");

            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.Similarity).HasColumnName("similarity");

            entity.HasOne(d => d.Email).WithMany(p => p.SimResultArchIssuesAllEmails)
                .HasForeignKey(d => d.EmailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SIM_RESULT_arch_issues_all_emails_email_id_fkey");

            entity.HasOne(d => d.IssueKeyNavigation).WithMany(p => p.SimResultArchIssuesAllEmails)
                .HasForeignKey(d => d.IssueKey)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SIM_RESULT_arch_issues_all_emails_issue_key_fkey");
        });

        modelBuilder.Entity<UniquePair>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("unique_pairs");

            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.IsCatExecutive).HasColumnName("is_cat_executive");
            entity.Property(e => e.IsCatExistence).HasColumnName("is_cat_existence");
            entity.Property(e => e.IsCatProperty).HasColumnName("is_cat_property");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Pattern).HasColumnName("pattern");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
        });

        modelBuilder.Entity<UniquePairsUnionAdd>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("unique_pairs_union_add");

            entity.Property(e => e.EmailId).HasColumnName("email_id");
            entity.Property(e => e.EmailThreadId).HasColumnName("email_thread_id");
            entity.Property(e => e.IsCatExecutive).HasColumnName("is_cat_executive");
            entity.Property(e => e.IsCatExistence).HasColumnName("is_cat_existence");
            entity.Property(e => e.IsCatProperty).HasColumnName("is_cat_property");
            entity.Property(e => e.IssueKey).HasColumnName("issue_key");
            entity.Property(e => e.IssueParentKey).HasColumnName("issue_parent_key");
            entity.Property(e => e.Pattern).HasColumnName("pattern");
            entity.Property(e => e.Similarity).HasColumnName("similarity");
        });
        modelBuilder.HasSequence<int>("expanded_arch_emails_all_issues_id_seq");
        modelBuilder.HasSequence<int>("expanded_arch_issues_all_emails_id_seq");
        modelBuilder.HasSequence<int>("iter0_expanded_arch_emails_all_issues_id_seq");
        modelBuilder.HasSequence<int>("iter0_expanded_arch_issues_all_emails_id_seq");
        modelBuilder.HasSequence<int>("iter2_expanded_arch_emails_all_issues_id_seq");
        modelBuilder.HasSequence<int>("iter2_expanded_arch_issues_all_emails_id_seq");
        modelBuilder.HasSequence<int>("iter4_cos_sim_expanded_arch_issues_all_emails_id_seq");
        modelBuilder.HasSequence<int>("iter4_sim_expanded_arch_issues_all_emails_id_seq");
        modelBuilder.HasSequence<int>("sim_expanded_arch_emails_all_issues_id_seq");
        modelBuilder.HasSequence<int>("sim_expanded_arch_issues_all_emails_id_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
