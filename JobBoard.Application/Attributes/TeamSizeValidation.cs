using JobBoard.Application.DTOs.JobDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JobBoard.Application.Attributes
{
    public class TeamSizeValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is CreateUpdateJobDto dto)
            {
                if (dto.MinTeamSize.HasValue && dto.MinTeamSize.HasValue)
                    return dto.MinTeamSize < dto.MaxTeamSize;
            }
            return true;
        }
        public override string FormatErrorMessage(string name)
        {
            return "Maximum team size must be greater minimum team size";
        }
    }
}
