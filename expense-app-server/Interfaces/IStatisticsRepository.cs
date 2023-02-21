namespace expense_app_server.Interfaces
{
    public interface IStatisticsRepository
    {
        IEnumerable<KeyValuePair<string, double>> GetExpenseAmountPerCategory();
    }
}
