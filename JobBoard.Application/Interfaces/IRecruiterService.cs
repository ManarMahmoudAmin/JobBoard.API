using JobBoard.Application.DTOs;
using JobBoard.Application.DTOs.RecruiterDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface IRecruiterService
    {
        Task<IEnumerable<RecruiterProfileDto>> GetAll();
        Task<RecruiterProfileDto> GetByUserId(string userId);
        //Task<string> Create(RecruiterProfileUpdateDto RecruiterProfile);
        Task<bool> Update(int id, RecruiterProfileUpdateDto RecruiterProfile);
        Task<bool> DeleteById(int id);
        
    }
}
