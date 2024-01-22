using Ticketing.Models.Dto;

namespace Ticketing.Data
{
      public class TicketStore{
            public static  List<TicketDTO> TicketList = new List<TicketDTO>{
                        new TicketDTO{Id=1, Title= "No projects assigned", Description="Assigned projects don't apper"},
                        new TicketDTO{Id=2, Title= "Non assigned projects appear", Description="Previously Assigned projects apper"}

                  };
      }
}