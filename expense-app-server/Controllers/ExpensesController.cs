using expense_app_server.Interfaces;
using expense_app_server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace expense_app_server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : Controller
    {
        private readonly IExpenseRepository _expenseRepository;
        public ExpensesController(IExpenseRepository expenseRepository)
        { 
            _expenseRepository= expenseRepository;
        }

        [HttpGet]
        public IActionResult GetExpense()
        {
            return Ok(_expenseRepository.GetExpenses());
        }

        [HttpGet("totalexpense")]
        public async Task<IActionResult> GetTotalExpenseAmount()
        {
            var totalExpenses = await _expenseRepository.GetTotalExpensesAsync();
            return Ok(totalExpenses);
        }

        [HttpGet("{id}", Name = "GetExpense")]
        public IActionResult GetExpense(int id)
        {
            return Ok(_expenseRepository.GetExpense(id));
        }

        [HttpPost]
        public IActionResult CreateExpense(Expense expense)
        {
            var newExpense = _expenseRepository.CreateExpense(expense);
            return CreatedAtRoute("GetExpense", new {newExpense.Id}, newExpense);
        }

        [HttpDelete]
        public IActionResult DeleteExpense(Expense expense)
        {
            _expenseRepository.DeleteExpense(expense);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateExpense(Expense expense)
        {
            return Ok(_expenseRepository.UpdateExpense(expense));

        }
    }
}
