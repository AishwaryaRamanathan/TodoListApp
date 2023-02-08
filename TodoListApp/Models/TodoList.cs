using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models
{
    public class TodoList
    {
        [Key]
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime? Duedate { get; set; }
        public bool IsCompleted { get; set; }

    }
}
