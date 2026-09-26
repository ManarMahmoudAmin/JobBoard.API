using Google.Apis.Auth;
using JobBoard.Application.DTOs.AuthDTOs;
using JobBoard.Application.DTOs.ExternalLoginDTOs;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ResultDto> RegisterAsync(RegisterDto model);
        Task<ResultLoginDto> LoginAsync(LoginDto model);
        Task<ResultDto> ChangePasswordAsync(ChangePasswordDto model);
        Task<ResultDto> ResetPasswordAsync(ResetPassDto model);
        Task<ResultDto> ForgotPasswordAsync(ForgetPasswordDto model);
        Task<ResultDto> SendConfirmationEmailAsync(string email);
        Task<ResultDto> ConfirmEmailAsync(ConfirmEmailDto model);
        //Task<string> RefreshTokenAsync(string token, string refreshToken);
        //Task LogoutAsync();
        //Task<bool> IsEmailConfirmedAsync(string email);
        //Task<bool> ConfirmEmailAsync(string email, string token);
        public Task<GoogleJsonWebSignature.Payload?> VerifyGoogleTokenAsync(string idToken);
        public Task<ResultLoginDto> GenerateJwtTokenAsync(ApplicationUser user);
        public Task<ResultLoginDto> ExternalLoginAsync(ExternalLoginReceiverDto model);

    }
}
