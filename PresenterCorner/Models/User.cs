using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresenterCorner.Models
{
    [Index(nameof(Nickname))]
    public class User
    {
        public int UserId { get; set; } // Primary Key
        [Required(AllowEmptyStrings = false, ErrorMessage = " The nickname must not be empty")]
        public string Nickname { get; set; } // User's arbitrary nickname
        public string ? ConnectionId { get; set; } // SignalR connection ID
        public string Role { get; set; } // Roles: "Creator", "Editor", "Viewer"
        public int PresentationId { get; set; }
        [ForeignKey(nameof(PresentationId))]
        [ValidateNever]
        public Presentation Presentation { get; set; }
    }
}
