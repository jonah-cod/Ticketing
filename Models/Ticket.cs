using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Models

{public class Ticket{
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int Id{get; set;}
      public string Title{get; set;}
      public string Description{get; set;}
      public string? Attachment {get; set;}
      public DateTime CreatedDate{get; set;}
      public DateTime UpdatedDate{get; set;}

}}