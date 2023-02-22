using expense_app_server.Dto;
using expense_app_server.Interfaces;
using expense_app_server.Models;
using Microsoft.EntityFrameworkCore;
using expense_app_server.CustomException;
using Microsoft.AspNet.Identity;
using expense_app_server.Utilities;
using System.Text.RegularExpressions;

namespace expense_app_server.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ExpenseContext _context;
        private readonly IPasswordHasher _passwordHasher;
        public UserRepository(ExpenseContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;

        }

        public async Task<AuthenticatedUser> ExternalSignIn(User user)
        {
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.ExternalId.Equals(user.ExternalId) && u.ExternalType.Equals(user.ExternalType));

            if (dbUser == null)
            {
                user.Password = GenerateRandomPassword(8);
                user.Username = CreateUniqueUsernameFromEmail(user.Email);
                return await SignUp(user);
            }

            return new AuthenticatedUser
            {
                UserName = dbUser.Username,
                Token = JWTGenerator.GenerateUserToken(dbUser.Username)
            };
        }

        public async Task<AuthenticatedUser> SignIn(User user)
        {
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username);

            if (dbUser == null || dbUser.Password == null || _passwordHasher.VerifyHashedPassword(dbUser.Password, user.Password) == PasswordVerificationResult.Failed)
            {
                throw new InvalidUsernamePasswordException("Invalid username or password");
            }

            return new AuthenticatedUser
            {
                UserName = user.Username,
                Token = JWTGenerator.GenerateUserToken(user.Username)
            };
        }

        public async Task<AuthenticatedUser> SignUp(User user)
        {
            var checkUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username);

            string upperCasePattern = @"[A-Z]";

            if (checkUser != null)
            {
                throw new UsernameAlreadyExistsException("Username aleardy exists");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentNullException(nameof(user.Password), "Password should not be empty");
            }

            if (user.Password.Length < 8)
            {
                throw new PasswordException("Password should be at least 8 characters long");
            }

            if (!Regex.IsMatch(user.Password, upperCasePattern))
            {
                throw new PasswordException("Password should contain at least one uppercase letter");
            }


            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = _passwordHasher.HashPassword(user.Password);
            }

            if(user.ExternalId == null || user.ExternalType == null)
            {
                user.ExternalId = GenerateRandomExternalId(21);
                user.ExternalType = "EXPENSETRACKER";
            }

            
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return new AuthenticatedUser
            {
                UserName = user.Username,
                Token = JWTGenerator.GenerateUserToken(user.Username)
            };
        }

        private string CreateUniqueUsernameFromEmail(string email)
        {
            var emailSplit = email.Split("@").First();
            var random = new Random();
            var username = emailSplit;

            while(_context.Users.Any(u => u.Username.Equals(username)))
            {
                username = emailSplit + random.Next(100000000);
            }

            return username;
        }

        private static string GenerateRandomPassword(int length)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            var random = new Random();

            var chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }

            return new string(chars);
        }

        private static string GenerateRandomExternalId(int length)
        {
            int minValue = (int)Math.Pow(10, length - 1);
            int maxValue = (int)Math.Pow(10, length) - 1;

            Random random = new Random();
            int randomNumber = random.Next(minValue, maxValue);

            string randomString = randomNumber.ToString().PadLeft(length, '0');

            return randomString;
        }

    }
}
