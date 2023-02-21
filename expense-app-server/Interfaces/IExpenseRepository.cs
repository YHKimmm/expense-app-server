using expense_app_server.Models;

namespace expense_app_server.Interfaces
{
    public interface IExpenseRepository
    {
        ICollection<Expense>GetExpenses();
        Expense GetExpense(int id);
        Expense CreateExpense(Expense expense);
        void DeleteExpense(Expense expense);
        Expense UpdateExpense(Expense expense);
    }
}
