namespace TodoListApp.Models
{
    public class TodoList
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime? Duedate { get; set; }
        public bool IsCompleted { get; set; }

    }
}
