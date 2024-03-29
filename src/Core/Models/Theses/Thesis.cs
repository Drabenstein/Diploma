﻿using System.Diagnostics.CodeAnalysis;
using Core.Models.Reviews;
using Core.Models.Theses.ValueObjects;
using Core.Models.Topics;
using Core.Models.Users;
using Core.SeedWork;

namespace Core.Models.Theses;

public record Thesis : EntityBase
{
    private readonly List<Declaration> _declarations = new List<Declaration>();
    private readonly List<Review> _reviews = new List<Review>();

    // EF Core only
    [ExcludeFromCodeCoverage]
    private Thesis() { }

    public Thesis(Topic topic, Student realizerStudent)
    {
        Topic = topic;
        RealizerStudent = realizerStudent;
        Status = ThesisStatus.InProgress;
    }
    
    public Topic Topic { get; set; }
    public ThesisStatus Status { get; private set; }
    public byte[]? Content { get; set; }
    public ThesisFileFormat? FileFormat { get; set; }
    public ThesisLanguage? Language { get; set; }
    public bool? HasConsentToChangeLanguage { get; set; }
    public Student RealizerStudent { get; set; }
    public virtual IReadOnlyCollection<Declaration> Declarations => _declarations.AsReadOnly();
    public virtual IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();
    public string? CloudBucket { get; set; }
    public string? CloudKey { get; set; }

    public void ChangeReviewer(Tutor reviewer)
    {
        if (this.Status == ThesisStatus.Reviewed)
        {
            throw new InvalidOperationException("Cannot change reviewer of already reviewed thesis");
        }
        
        var existingReviewerReview = Reviews.FirstOrDefault(x => !x.Reviewer.Equals(Topic.Supervisor));
        var isReviewerSupervisor = reviewer.Equals(Topic.Supervisor);

        if (existingReviewerReview is null)
        {
            if (!Reviews.Any() || !isReviewerSupervisor)
            {
                var review = new Review(reviewer);
                _reviews.Add(review);
            }
        }
        else if (!existingReviewerReview.Reviewer.Equals(reviewer))
        {
            existingReviewerReview.Reviewer = null!;
            _reviews.Remove(existingReviewerReview);
            var review = new Review(reviewer);
            _reviews.Add(review);
        }
    }

    public void DeclareAsReadyForReview()
    {
        if (Status == ThesisStatus.InProgress)
        {
            Status = ThesisStatus.ReadyToReview;
        }
        else
        {
            throw new InvalidOperationException("Thesis can only be declared as ready for review when it is in progress");
        }
    }

    public void ReviewThesis(string grade, long reviewId)
    {
        if (Status == ThesisStatus.ReadyToReview || Status == ThesisStatus.Reviewed)
        {
            var review = Reviews.FirstOrDefault(x => x.Id == reviewId);

            if (review is null) throw new InvalidOperationException("Submitted review does not correspond to this thesis");

            review.SubmitGrade(grade);
            Status = ThesisStatus.Reviewed;
        }
        else
        {
            throw new InvalidOperationException("Thesis can only be reviewed when it is ready to be reviewed or already reviewed by another reviewer");
        }
    }

    internal void AddReview(Tutor reviewer)
    {
        if (_reviews.Any(x => x.Reviewer == reviewer))
        {
            throw new InvalidOperationException("This tutor already is assigned as reviewer");
        }
        
        var review = new Review(reviewer);
        _reviews.Add(review);
    }
}