using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using TasklistAPI.Helper;
using TasklistAPI.Interface;
using TasklistAPI.Model;
using TasklistAPI.Model.Entity;
using TasklistAPI.Model.Enum;
using TasklistAPI.Model.Request;
using TasklistAPI.Model.Response;

namespace TasklistAPI.Services
{
    public class UserServices : IUserServices
    {
        private readonly AppDbContext _context;

        public UserServices(AppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<GlobalResponse> Create(CreateUserRequest input)
        {
            GlobalResponse globalres = new GlobalResponse();
            ErrorFieldSet errorFieldSet = new ErrorFieldSet();
            bool isValid = true;
            try
            {
                if (string.IsNullOrWhiteSpace(input.Email))
                {
                    errorFieldSet.AddError("Email", "Email is required");
                    isValid = false;
                }

                if (_context.UserAccounts.Any(a=> a.UserName == input.Email))
                {
                    errorFieldSet.AddError("Email", "Email has been registered");
                }

                if (string.IsNullOrWhiteSpace(input.Password))
                {
                    errorFieldSet.AddError("Password", "Password is required");
                }

                if (string.IsNullOrWhiteSpace(input.RepeatPassword))
                {
                    errorFieldSet.AddError("RepeatPassword", "RepeatPassword is required");
                }

                if (!(string.IsNullOrWhiteSpace(input.RepeatPassword)) && (string.IsNullOrWhiteSpace(input.Password)) && input.Password != input.RepeatPassword)
                {
                    errorFieldSet.AddError("RepeatPassword", "RepeatPassword must be same with Password");
                }

                if (errorFieldSet.IsValid)
                {
                    HashSalt hashSalt = HashSalt.GenerateSaltedHash(64, input.Password);
                    var Pwd = hashSalt.Hash;
                    var Salt = hashSalt.Salt;
                    UserAccount userAccount = new UserAccount();
                    userAccount.UserName = input.Email;
                    userAccount.Password = Pwd;
                    userAccount.PasswordSalt = Salt;
                    await _context.UserAccounts.AddAsync(userAccount);
                    await _context.SaveChangesAsync();
                    globalres.status_code = HttpResponseCode.ResponseOK;
                    globalres.message = "Success";
                    globalres.data = userAccount;
                }
               else
                {
                    globalres.status_code = HttpResponseCode.ResponseError;
                    globalres.data = errorFieldSet;
                }

            }
            catch (Exception ex)
            {
                globalres.status_code = HttpResponseCode.ResponseError;
                globalres.message = ex.Message;
            }
            return globalres;
        }

        public async Task<GlobalResponse> Login(LoginRequest input)
        {
            GlobalResponse globalres = new GlobalResponse();
            bool IsValid = true;
            try
            {
                UserAccount? user = await _context.UserAccounts.Where(a => a.UserName == input.Email).FirstOrDefaultAsync();

                if (user == null)
                {
                    globalres.status_code = HttpResponseCode.ResponseError;
                    IsValid = false;
                    globalres.message = "Email is not found";
                }
                else
                {
                    HashSalt hashSalt = HashSalt.GenerateSaltedHash(64, input.Password);
                    var Pwd = hashSalt.Hash;
                    var Salt = hashSalt.Salt;

                    var isVerifyPassword = HashSalt.VerifyPassword(input.Password, user.Password, user.PasswordSalt);

                    if (!isVerifyPassword)
                    {
                        globalres.status_code = HttpResponseCode.ResponseError;
                        IsValid = false;
                        globalres.message = "Invalid password";
                    }

                    if (IsValid)
                    {

                     var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, ConfigJwt.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Email", user.UserName)
                   };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigJwt.Key));

                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                           ConfigJwt.Issuer,
                           ConfigJwt.Audience,
                            claims,
                            expires: DateTime.UtcNow.AddDays(1),
                            signingCredentials: signIn);

                        LoginResult loginResult = new LoginResult();
                        loginResult.Email = user.UserName;
                        loginResult.token = new JwtSecurityTokenHandler().WriteToken(token);
                        globalres.data = loginResult;
                        globalres.status_code = HttpResponseCode.ResponseOK;
                    }
                }

            }
            catch (Exception ex)
            {
                globalres.status_code = HttpResponseCode.ResponseError;
                globalres.message = ex.Message;
            }
            return globalres;
        }
    }
}
