namespace Ecommerce.Model
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string Status { get; set; } // Pending / Completed

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
