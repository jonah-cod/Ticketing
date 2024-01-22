using System.ComponentModel.DataAnnotations;

namespace Ticketing.Models.Dto

{
      public class TicketDTO{
      public int Id{get; set;}

      [Required]
      [MaxLength(50)]
      public string Title{get; set;}

      [Required]
      [MaxLength(100)]
      public string Description{get; set;}

}}