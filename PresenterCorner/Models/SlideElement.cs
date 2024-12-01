using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresenterCorner.Models
{
    public class SlideElement
    {
        [Key]
        public int ElementId { get; set; } // Primary Key
        public int SlideId { get; set; } // FK to Slide
        public string Type { get; set; } // Type of element (e.g., "text", "rectangle", "circle")
        public string Content { get; set; } // Content (e.g., text for text blocks)
        public string Position { get; set; } // Position JSON (e.g., "{x:10, y:20}")
        public string Style { get; set; } // Style JSON (e.g., "{color:'#FF0000', fontSize:'14px'}")
        public bool IsMovable { get; set; } // If the element can be moved
    }

}
//todo Datatype field
