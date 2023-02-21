using expense_app_server.Interfaces;
using expense_app_server.Models;

namespace expense_app_server.Repository
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ExpenseContext _context;
        private readonly User _user;
        public StatisticsRepository(ExpenseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = _context.Users.First(u => u.Username == httpContextAccessor.HttpContext.User.Identity.Name);
        }
        public IEnumerable<KeyValuePair<string, double>> GetExpenseAmountPerCategory()
        {
            return _context.Expenses
                    .Where(e => e.User.Id == _user.Id)
                    .AsEnumerable()
                    .GroupBy(e => e.Description)
                    .ToDictionary(e => e.Key, e => e.Sum(x => x.Amount))
                    .Select(x => new KeyValuePair<string, double>(x.Key, x.Value));
        }
    }
}
