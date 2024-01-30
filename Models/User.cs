using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Users.Models
{
      public class User
      {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id{get; set;}
            public string FullNames{get; set;} = string.Empty;
            public string Email{get; set;} = String.Empty;
            public string Password{get; set;} = String.Empty;
            public DateTime JoinedAt{get; set;}
            public DateTime UpdatedOn{get; set;}
      }
}
