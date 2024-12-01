namespace PresenterCorner.Models
{
    public class Presentation
    {
        public int PresentationId { get; set; } // Primary Key
        public string Title { get; set; } // Title of the presentation
        public int CreatorId { get; set; } // FK to User
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Slide> Slides { get; set; } // Associated slides
        public ICollection<User> Users { get; set; } = new List<User>(); // List of participants
    }

}
