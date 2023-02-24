using expense_app_server.Interfaces;
using expense_app_server.Models;
using Microsoft.EntityFrameworkCore;

namespace expense_app_server.Repository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseContext _context;
        private readonly User _user;
        public ExpenseRepository(ExpenseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            // compare user from db & user from token
            _user = _context.Users.First(u => u.Username == httpContextAccessor.HttpContext.User.Identity.Name);
        }
        public async Task<double> GetTotalExpensesAsync()
        {
            var expenses = await _context.Expenses
                .Where(e => e.User.Id == _user.Id)
                .ToListAsync();

            double totalExpenses = expenses.Sum(e => e.Amount);

            return totalExpenses;
        }

        public Expense CreateExpense(Expense expense)
        {
            expense.User= _user;
            _context.Add(expense);
            _context.SaveChanges();


            return expense;
        }

        public void DeleteExpense(Expense expense)
        {
            var dbExpense = _context.Expenses.FirstOrDefault(e=> e.User.Id == _user.Id && e.Id == expense.Id);
            _context.Expenses.Remove(dbExpense);
            _context.SaveChanges();
        }

        public Expense GetExpense(int id)
        {
            return _context.Expenses
                    .Where(e => e.User.Id == _user.Id && e.Id == id)
                    .First();
        }

        public ICollection<Expense> GetExpenses()
        {
            return _context.Expenses
                    .Where(e => e.User.Id == _user.Id)
                    .ToList();
        }

        public Expense UpdateExpense(Expense expense)
        {
            var dbExepnse = _context.Expenses.FirstOrDefault(e => e.User.Id == _user.Id && e.Id == expense.Id);
            dbExepnse.Description = expense.Description;
            dbExepnse.Amount = expense.Amount;
            _context.SaveChanges();

            return expense;
        }
    }
}
