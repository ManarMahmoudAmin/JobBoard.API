using JobBoard.Application.Specifications;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Specifications
{
    // Admin Specifications

    /////////////////////////get all Candidates///////////////////////
    public class AllCandidatesSpecification : BaseSpecifications<CandidateProfile>
    {
        public AllCandidatesSpecification()
        {
            // Include User data
            AddIncludes(s => s.User);
            AddIncludes(s => s.Skills);
            AddOrderByDesc(e => e.Id);

        }

    }

    /////////////////////////get Candidate by id///////////////////////
    public class CandidateByUserIdSpecification : BaseSpecifications<CandidateProfile>
    {
        public CandidateByUserIdSpecification(string CandidateId) : base(s => s.UserId == CandidateId)
        {
            AddIncludes(s => s.User);
            AddIncludes(s => s.CandidateCertificates);
            AddIncludes(s => s.CandidateInterests);
            AddIncludes(s => s.Skills);
            AddIncludes(s => s.CandidateTraining);

        }

    }

    /////////////////////////get all Recruiters///////////////////////
    public class AllRecruitersSpecification : BaseSpecifications<RecruiterProfile>
    {
        public AllRecruitersSpecification()
        {
            AddIncludes(e => e.User);
            AddOrderByDesc(e => e.Id);
        }
    }


    /////////////////////////get Recruiter by id///////////////////////
    public class RecruiterByUserIdSpecification : BaseSpecifications<RecruiterProfile>
    {
        public RecruiterByUserIdSpecification(string RecruiterId) : base(e => e.UserId == RecruiterId)
        {
            AddIncludes(e => e.User);

        }
    }

    ////////////////////////get jobs related Recruiter//////////////////////////
    public class JobsByRecruiterIdSpecification : BaseSpecifications<Job>
    {
        public JobsByRecruiterIdSpecification(int RecruiterId) : base(j => j.RecruiterId == RecruiterId)
        {
            AddIncludes(j => j.JobApplications);
        }
    }
    /////////////////////////get all jobs///////////////////////

    public class AllJobsSpecification : BaseSpecifications<Job>
    {
        public AllJobsSpecification()
        {
            AddIncludes(j => j.Recruiter);
            AddIncludes(j => j.Categories);
            AddIncludes(j => j.Skills);
            AddOrderByDesc(j => !j.IsApproved);
            AddThenByDesc(j => j.PostedDate);
        }
    }

    /////////////////////////get pending jobs///////////////////////
    public class PendingJobsSpecification : BaseSpecifications<Job>
    {
        public PendingJobsSpecification() : base(j => !j.IsApproved)
        {
            AddIncludes(j => j.Recruiter.User);
            AddOrderByDesc(j => j.PostedDate);
        }
    }

    /////////////////////////get job by id///////////////////////
    public class JobByIdSpecification : BaseSpecifications<Job>
    {
        public JobByIdSpecification(int jobId) : base(j => j.Id == jobId)
        {
            AddIncludes(j => j.Recruiter);
            AddIncludes(j => j.Categories);
            AddIncludes(j => j.Skills);
        }
    }

    public class ApprovedJobsCountSpecification : BaseSpecifications<Job>
    {
        public ApprovedJobsCountSpecification() : base(j => j.IsApproved == true)
        {
        }
    }

    public class ActiveJobsCountSpecification : BaseSpecifications<Job>
    {
        public ActiveJobsCountSpecification() : base(j => j.IsActive == true)
        {
        }
    }
}

//////////////////job applications///////////////////////
public class JobByIdWithApplication : BaseSpecifications<Job>
{
    public JobByIdWithApplication(int jobId) : base(a => a.Id == jobId)
    {
        AddIncludes(a => a.JobApplications);
        AddIncludes(a => a.Recruiter.User);
    }
}


