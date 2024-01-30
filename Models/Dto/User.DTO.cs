using System.ComponentModel.DataAnnotations;

namespace Users.Models.DTO
{
      class UserDTO
      {
            public int Id;
            public string FullNames = string.Empty;

            [Required]
            [MaxLength(50)]
            public string Email = String.Empty;

            [Required]
            public string Password = String.Empty;
      }
}
