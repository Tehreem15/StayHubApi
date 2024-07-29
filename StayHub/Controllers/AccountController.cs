using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Logging;
using StayHub.Data.ResponseModel;
using StayHub.Data.ViewModels;
using StayHub.Services;
using System.Text.Encodings.Web;
using System.Text;
using StayHub.Data.DBModels;

namespace StayHub.Controllers
{
  
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
  
    {

        private AccountService accountService;
        private EmailService emailService;
        public AccountController(AccountService _accountService, EmailService _emailService) { 
            accountService = _accountService; emailService=_emailService;
        }

        [HttpGet("CreateAdmin")]
        [AllowAnonymous]
        public IActionResult CreateAdminAccount()
        {
           bool created = accountService.CreateAdmin();
           return Ok(created);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login([FromForm] LoginModel model)
        {
            var response = new ApiResponse<LoginClaimsModel>();
            try
            {
                response = accountService.CheckAccountAndGenerateToken(model);           
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

            }
            return Ok(response);
        }

        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword([FromBody] EmailModel model)
        {
            var response = new ApiBaseResponse();
            try
            {
                if (model.Email?.Trim() is not null and not "")
                {
                    TblUser user = accountService.FindByEmail(model.Email);
                    if (user == null)
                    {
                        response.Message = "Account does not exist";
                        response.Success = false;

                        return Ok(response);
                    }
                    else
                    {
                        string code = accountService.GeneratePasswordResetToken(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                        string logo = string.Empty;
                        string body = @$"<!DOCTYPE html>
                                        <html>
                                        <head>
                                            <meta charset=""utf-8"" />
                                            <title></title>
                                            
                                        </head>
                                        <body>
                                            <p>Hello {user.FirstName + " " + user.LastName},</p>
                                            <p>You have been requested to reset your password for your account.</p>
                                            <p>This is your reset password code.<p>
                                            <p id=""code"" style=""cursor: pointer; background-color: #f0f0f0; padding: 5px;"" >{code}</p>
                                            <p>Please enter this code on your account to reset your password.<p>
                                            <p>
                                               If you'any problems, please contact your manager.    
                                            </p>
                                            <p>StayHub Hotel</p>
                                            <p>Many thanks,</p>
                                        </body>
                                        </html>";

                        response.Success =  emailService.SendEmail(body, user.Email, "Reset Your Password", null, "");
                        response.Message = "Please check your inbox.";

                    }

                }
                else
                {
                    response.Success = false;
                    response.Message = "Invalid email.";
                }
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Message = "Unable to send.";
            }
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword([FromForm] ResetPasswordModel model)
        {
            var response = new ApiBaseResponse();
            try
            {
                //Decode Password Reset Token
                byte[] byteArray = WebEncoders.Base64UrlDecode(model.Code);
                string decodedCode = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
                //  var user = await _userService.FindByEmailAsync(model.Email);
                var result = accountService.ResetPassword(decodedCode, model.NewPassword);
                if (result.Success)
                {
                    response.Success = true;
                    response.Message = "Password reset successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Password must match.";
                }
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "Unable to reset.";

            }
            return Ok(response);
        }

        //manage account/update password
        [HttpPost("ChangePassword")]
        [Authorize]
        public IActionResult ChangrPassword([FromForm] ChangePasswordModel model)
        {
            var response = new ApiResponse<string>();
            if (ModelState.IsValid)
            {
                var user =  accountService.FindById(model.Id);

                var result = accountService.ChangePassword(user, model.CurrentPassword, model.NewPassword);
                if (result.Success)
                {

                    response.Success = true;
                    response.Message = "Password changed";


                }
                else
                {
                    response.Success = false;
                    response.Message = "Password must match";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Unable to change";
            }
            return Ok(response);
        }


    }
}
