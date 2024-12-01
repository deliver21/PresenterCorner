namespace PresenterCorner.Models
{
    public class Slide
    {
        public int SlideId { get; set; } // Primary Key
        public int PresentationId { get; set; } // FK to Presentation
        public int Order { get; set; } // Slide order in the presentation
        public ICollection<SlideElement> Elements { get; set; } // Associated elements
    }

}
