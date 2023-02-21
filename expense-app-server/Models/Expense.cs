using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace expense_app_server.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User? User { get; set; }

    }
}
