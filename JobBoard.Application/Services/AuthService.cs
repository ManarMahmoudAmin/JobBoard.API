using Google.Apis.Auth;
using JobBoard.Application.DTOs.AuthDTOs;
using JobBoard.Application.DTOs.ExternalLoginDTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace JobBoard.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public AuthService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        /*------------------------Login Service--------------------------*/
        public async Task<ResultLoginDto> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            // Check if the user exists and if the password is correct
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new ResultLoginDto()
                {
                    Succeeded = false,
                    Message = "Invalid email or password."
                };
            }

            // Check if the user has confirmed their email
            if (!user.EmailConfirmed)
            {
                return new ResultLoginDto()
                {
                    Succeeded = false,
                    Message = "Please confirm your email before logging in."
                };
            }
            var userRole = await _userManager.GetRolesAsync(user);  // Get the roles of the user (list of roles)

            List<Claim> userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            userClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); // Unique identifier for the token
            userClaims.Add(new Claim(ClaimTypes.Role, userRole.FirstOrDefault() ?? ""));


            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"])); // Secret key for signing the token
            var expiration = model.RememberMe ? DateTime.Now.AddDays(7) : DateTime.Now.AddDays(1); // Token expiration time
            var signingCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);  // Signing credentials for the token
                                                                                                  // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                expires: expiration,
                claims: userClaims,
                signingCredentials: signingCredentials
            );
            return new ResultLoginDto()
            {
                Succeeded = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,   //token.ValidTo,
                Role = userRole.FirstOrDefault() ?? "",
                Message = "Login successful.",
                UserId = user.Id

            };


        }

        /*------------------------Register Service--------------------------*/
        public async Task<ResultDto> RegisterAsync(RegisterDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
                return new ResultDto(
                    succeeded: false,
                    message: "User already exists."
                    );
            if (!Enum.IsDefined(typeof(UserType), model.user_type))
                return new ResultDto(
                    succeeded: false,
                    message: "Invalid user type."
                    );

            ApplicationUser newUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = false, // Set to false initially, will be confirmed later
                User_Type = model.user_type,

            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            // Check if the user creation was successful
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResultDto(
                    succeeded: false,
                    message: $"Registration failed: {errors}"
                    );
            }

            var roleName = model.user_type.ToString(); // Candidate or Recruiter
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole(roleName));

            await _userManager.AddToRoleAsync(newUser, roleName);

            // If the user is an Recruiter, set additional properties
            if (model.user_type == UserType.Recruiter)
            {
                var RecruiterProfile = new RecruiterProfile
                {
                    CompanyName = model.CompanyName,
                    CompanyLocation = model.CompanyLocation,
                    CompanyImage = $"{_config["ApiBaseUrl"]}/images/companies/logo.jpg",
                    UserId = newUser.Id
                };
                await _unitOfWork.Repository<RecruiterProfile>().AddAsync(RecruiterProfile);
                await _unitOfWork.CompleteAsync();
            }

            // If the user is a Candidate, you can set additional properties here if needed
            if (model.user_type == UserType.Candidate)
            {
                var CandidateProfile = new CandidateProfile
                {
                    UserId = newUser.Id,
                    ProfileImageUrl = $"{_config["ApiBaseUrl"]}/images/profilepic/user.jpg"

                };
                await _unitOfWork.Repository<CandidateProfile>().AddAsync(CandidateProfile);
                await _unitOfWork.CompleteAsync();
            }

            // Send confirmation email directly after register
            try
            {
                await SendConfirmationEmailAsync(newUser.Email);
            }
            catch (Exception ex)
            {
                return new ResultDto(
                    succeeded: false,
                    message: $"Failed to send confirmation email: {ex.Message}"
                    );
            }
            return new ResultDto(
                    succeeded: true,
                    message: "User registered successfully. Please confirm your email."
                    );
        }


        /*------------------------Change Password Service--------------------------*/
        public async Task<ResultDto> ChangePasswordAsync(ChangePasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ResultDto(
                    succeeded: false,
                    message: "User not found."
                    );
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResultDto(
                    succeeded: false,
                    message: $"Password change failed: {errors}"
                    );
            }
            return new ResultDto(
                succeeded: true,
                message: "Password changed successfully."
                );

        }



        /*----------------------Reset Password Service-----------------------*/
        public async Task<ResultDto> ResetPasswordAsync(ResetPassDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ResultDto(
                    succeeded: false,
                    message: "User not found."
                    );
            }
            //var decodedToken = WebUtility.UrlDecode(model.Token);

            var decodedBytes = WebEncoders.Base64UrlDecode(model.Token);
            var normalToken = Encoding.UTF8.GetString(decodedBytes);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);


            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResultDto(
                    succeeded: false,
                    message: $"Password reset failed: {errors}"
                    );
            }

            return new ResultDto(
                succeeded: true,
                message: "success");
        }


        /*----------------------Forget Password Service-----------------------*/
        public async Task<ResultDto> ForgotPasswordAsync(ForgetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ResultDto(
                    succeeded: false,
                    message: "User not found."
                    );
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);


            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var clientUrl = _config["AppSettings:ClientAppUrl"];

            var resetLink = $"{clientUrl}/reset-password?email={user.Email}&token={encodedToken}";

            var subject = "Reset Your Password";

            var body = $@"
                 <p>Hi {user.UserName},</p>
                 <p>We received a request to reset your password. Click the button below to choose a new password:</p>

               <div style='text-align:center; margin-top:20px;'>
                    <a href='{resetLink}' 
                   style='background-color:#4CAF50; color:white; padding:10px 20px;
                  text-decoration:none; border-radius:5px; display:inline-block;'>
                       Reset Password
                   </a>
              </div>

             <p style='margin-top: 30px; font-size: 14px; color: #999;'>
                    If you didn't request this, please ignore this email.
             </p>";


            await _emailService.SendEmailAsync(model.Email, subject, body);
            return new ResultDto(
                succeeded: true,
                message: "If the email exists in our system, a reset link has been sent"
                );
        }


        /*---------------------send Confirm Email Service------------------------*/
        public async Task<ResultDto> SendConfirmationEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ResultDto(false, "User not found.");
            }

            if (user.EmailConfirmed)
            {
                return new ResultDto(false, "Email already confirmed.");
            }

            // Generate confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var encodedToken = WebUtility.UrlEncode(token);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));


            // Confirmation link
            var confirmationLink = $"{_config["AppSettings:ClientAppUrl"]}/confirm-email?email={user.Email}&token={encodedToken}";

            // Email body
            var body = $@"
        <p>Hi {user.UserName},</p>
        <p>Please click the button below to confirm your email:</p>
        <div style='text-align:center;'>
            <a href='{confirmationLink}'
               style='background-color:#4CAF50;color:white;padding:10px 20px;
                      text-decoration:none;border-radius:5px;'>
                Confirm Email
            </a>
        </div>
        <p style='margin-top: 30px; font-size: 14px; color: #999;'>
            If you didn't create an account, please ignore this email.
        </p>";

            await _emailService.SendEmailAsync(user.Email, "Confirm Your Email", body);

            return new ResultDto(true, "Confirmation email sent.");
        }


        /*------------------------Confirm Email Service--------------------------*/
        public async Task<ResultDto> ConfirmEmailAsync(ConfirmEmailDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new ResultDto(false, "User not found.");

            //var decodedToken = WebUtility.UrlDecode(model.Token);
            var decodedBytes = WebEncoders.Base64UrlDecode(model.Token);
            var normalToken = Encoding.UTF8.GetString(decodedBytes);

            var result = await _userManager.ConfirmEmailAsync(user, normalToken);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResultDto(false, $"Email confirmation failed: {errors}");
            }

            return new ResultDto(true, "Email confirmed successfully.");
        }



        /* -------------------------------External login with google------------------- */

        public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleTokenAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { _config["GoogleAuthSettings:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                throw new Exception("Google token validation failed: " + ex.Message);
            }
        }
        public async Task<ResultLoginDto> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var userRole = await _userManager.GetRolesAsync(user);

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userRole.FirstOrDefault() ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: userClaims,
                signingCredentials: signingCredentials
            );

            return new ResultLoginDto
            {
                Succeeded = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = DateTime.Now.AddDays(1),
                Role = userRole.FirstOrDefault() ?? ""
            };
        }

        public async Task<ResultLoginDto> ExternalLoginAsync(ExternalLoginReceiverDto model)
        {
            // check google token
            var payload = await VerifyGoogleTokenAsync(model.IdToken);
            if (payload == null)
            {
                return new ResultLoginDto
                {
                    Succeeded = false,
                    Message = "Invalid Google token."
                };
            }

            // 2. search about user
            var user = await _userManager.FindByEmailAsync(payload.Email);

            // 3. if user not found --> generate user with role came from client
            if (user == null)
            {
                // Check if the role came from client is valid
                if (!Enum.TryParse<UserType>(model.RoleFromClient, true, out var userType))
                {
                    return new ResultLoginDto
                    {
                        Succeeded = false,
                        Message = "Invalid user type."
                    };
                }

                user = new ApplicationUser
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    User_Type = userType,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    return new ResultLoginDto
                    {
                        Succeeded = false,
                        Message = "Failed to create user from Google account."
                    };
                }

                // register the user with the role came from client
                await _userManager.AddToRoleAsync(user, userType.ToString());

                // If the user is an Recruiter, set additional properties
                if (userType == UserType.Recruiter)
                {
                    var RecruiterProfile = new RecruiterProfile
                    {
                        CompanyName = model.CompanyName,
                        CompanyLocation = model.CompanyLocation,
                        UserId = user.Id
                    };
                    await _unitOfWork.Repository<RecruiterProfile>().AddAsync(RecruiterProfile);
                }
                else if (userType == UserType.Candidate)
                {
                    var CandidateProfile = new CandidateProfile
                    {
                        UserId = user.Id
                    };
                    await _unitOfWork.Repository<CandidateProfile>().AddAsync(CandidateProfile);
                }
                await _unitOfWork.CompleteAsync();
            }
            return await GenerateJwtTokenAsync(user);
        }
    }
}
