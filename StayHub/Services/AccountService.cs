using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StayHub.Data.DBModels;
using StayHub.Data.ResponseModel;
using StayHub.Data.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StayHub.Services
{
    public class AccountService
    {
        private StayHubContext db;
        public AccountService(StayHubContext _db)
        {
            db = _db;
        }
        public bool CreateAdmin()
        {

            TblUser tblAdmin = new TblUser()
            {
                FirstName = "StayHub",
                LastName = "Admin",
                Email = "admin@gmail.com",
                PhoneNumber = "128398090",
                Password = "Admin123",
                IsActive = true,
                Role = "ADMIN",
            };
            db.Add(tblAdmin);
            bool isCreated= db.SaveChanges()>0;
            return isCreated;
            
        }

        public ResponseModel AddPassword(TblUser user, string newPassword)
        {
            ResponseModel response = new ResponseModel();

            if (user != null)
            {

                string hashPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                response.Success = true;
                user.Password = hashPassword;
                db.Update(user);
                response.Success = db.SaveChanges() > 0;
            }
            else
            {
                response.Success = false;
            }

            return response;
        }

        public ResponseModel CheckEmailAlreadyExist(string Email, long Id)
        {
            ResponseModel response = new ResponseModel();
            if (Email != null)
            {
                if (Id == 0)
                {
                    response.Success = db.tblUsers.Where(s => s.Email.ToLower() == Email.ToLower()).Count() > 0;
                }
                else
                {
                    response.Success = db.tblUsers.Where(s => s.Email.ToLower() == Email.ToLower() && s.Id!= Id).Count() > 0;

                }
            }
            return response;
        }
        public ResponseModel ChangePassword(TblUser user, string currentPassword, string newPassword)
        {
            ResponseModel response = new ResponseModel();

            if (user != null)
            {
                bool isPasswordMatch = BCrypt.Net.BCrypt.Verify(currentPassword, user.Password);

                if (isPasswordMatch)
                {
                    response.Success = true;
                    string hashPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    user.Password = hashPassword;
                    db.Update(user);
                    db.SaveChanges();
                }
                else
                {
                    response.Success = false;
                }
            }
            else
            {
                response.Success = false;
            }
            return response;
        }

        public ResponseModel<LoginClaimsModel> CheckAccountAndGenerateToken(LoginModel loginModel)
        {
            ResponseModel<LoginClaimsModel> response = new ResponseModel<LoginClaimsModel>();
           

            var user = db.tblUsers
                .Where(u => u.Email.ToUpper() == loginModel.Email.ToUpper() && u.IsActive == true
                && u.Password == loginModel.Password)
                .FirstOrDefault();

            // Check if the user exists
            if (user != null)
            {
             
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes("StayHubKey25102023139");

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim("id", user.Id.ToString()),
                            new Claim(ClaimTypes.Role, user.Role.Trim()),
                            new Claim("email", user.Email),
                            new Claim("name", user.FirstName+" "+user.LastName),
                        }),
                        Expires = DateTime.Now.AddDays(30),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    LoginClaimsModel res = new LoginClaimsModel();
                    res.id = user.Id.ToString();
                    res.token = tokenHandler.WriteToken(token);
                    res.email = user.Email;
                    res.role = user.Role.Trim();
                    res.name = user.FirstName + " " + user.LastName;
                    res.expiry = tokenDescriptor.Expires.ToString()?? DateTime.Now.AddDays(30).ToString();
                  response.Data = res;
            }

            return response;
        }

        public string GeneratePasswordResetToken(TblUser user)
        {

            if (user != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("StayHubResetKey25102023139");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                       new Claim("UserEmail", user.Email),

                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            else
            {

                return string.Empty;
            }


        }

        public ResponseModel ResetPassword(string resetToken, string newPassword)
        {

            var response = new ResponseModel();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("StayHubResetKey25102023139");

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(resetToken, tokenValidationParameters, out validatedToken);

                // Check if the token is valid and not expired
                var emailClaim = principal.Claims.FirstOrDefault(claim => claim.Type == "UserEmail");
                var tokenExpiration = validatedToken.ValidTo;

                if (emailClaim != null && DateTime.UtcNow <= tokenExpiration)
                {
                    string userEmail = emailClaim.Value;

                    // Reset the password for the user with userEmail
                    // Update the user's password as needed
                    TblUser user = FindByEmail(userEmail);
                    if (user != null)
                    {
                       
                            string hashPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                            user.Password = hashPassword;
                           
                           db.Update(user);
                           response.Success = db.SaveChanges() > 0;

                    }

                }
                else
                {
                    response.Message = "Invalid or expired token.";
                }
            }
            catch (SecurityTokenException)
            {
                response.Message = "Invalid token format.";
            }

            return response;





        }

        public TblUser FindByEmail(string email)
        {

            var user = db.tblUsers
                .Where(u => u.Email.ToUpper() == email.ToUpper())
                .FirstOrDefault();

            return user;


        }

        public TblUser FindById(string Id)
        {

            var user =  db.tblUsers
                .Where(u => u.Id.ToString() == Id)
                .FirstOrDefault();

            return user;


        }


    }
}
