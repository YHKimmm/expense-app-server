using expense_app_server.Dto;
using expense_app_server.Models;

namespace expense_app_server.Interfaces
{
    public interface IUserRepository
    {
        Task<AuthenticatedUser>SignUp(User user);
        Task<AuthenticatedUser>SignIn(User user);
        Task<AuthenticatedUser>ExternalSignIn(User user);
    }
}
