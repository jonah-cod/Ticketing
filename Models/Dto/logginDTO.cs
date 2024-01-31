using System.ComponentModel.DataAnnotations;

namespace Ticketing.Models.Dto
{
      public class LogginDTO
      {
            [Required]
            public string Email { get; set; } = String.Empty;
            [Required]
            public string Password { get; set; } = String.Empty;
      }
}