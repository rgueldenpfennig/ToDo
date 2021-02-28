using System.ComponentModel.DataAnnotations;

namespace Todo.Application.Models
{
    public class CreateTodoRecord
    {
        [Required]
        public string Title { get; set; } = null!;
    }
}
