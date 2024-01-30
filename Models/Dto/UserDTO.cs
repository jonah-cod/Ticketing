using System.ComponentModel.DataAnnotations;

namespace Ticketing.Models.Dto
{
      public class UserDTO
      {
            public int Id;
            public string FullNames;

            [Required]
            [MaxLength(50)]
            public string Email;

            [Required]
            public string Password;
      }
}
