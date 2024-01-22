
using Ticketing.Models;
using Ticketing.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Data;

namespace Ticketing.Controllers
{
      [Route("api/tickets")]
      [ApiController]
      public class TicketController : ControllerBase
      {

            // Get all tickets
            [HttpGet]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public ActionResult<IEnumerable<TicketDTO>> GetTickets()
            {
                  return Ok(TicketStore.TicketList);
            }

            // Getting a single ticket
            [HttpGet("{id:int}", Name ="GetTicket")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public ActionResult<TicketDTO> GetTicket(int id)
            {
                  if (id == 0)
                  {
                        return BadRequest();
                  }
                  var ticket = TicketStore.TicketList.FirstOrDefault(u => u.Id == id);

                  if (ticket == null)
                  {
                        return NotFound();
                  }
                  return Ok(ticket);
            }

            // Creating a new ticket
            [HttpPost]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public ActionResult<TicketDTO> CreateTicket([FromBody]TicketDTO ticketDTO){
                  // if (!ModelState.IsValid){
                  //       return BadRequest(ModelState);
                  // }
                  if (TicketStore.TicketList.FirstOrDefault(u=>u.Title == ticketDTO.Title) != null)
                  {
                        ModelState.AddModelError("customError", "Ticket with the same title slready exists");
                        return BadRequest(ModelState);
                  }
                  if(ticketDTO == null){
                        return BadRequest(ticketDTO);
                  }

                  if(ticketDTO.Id >0){
                        return StatusCode(StatusCodes.Status500InternalServerError);
                  }
                  ticketDTO.Id = TicketStore.TicketList.OrderByDescending(u=>u.Id).FirstOrDefault().Id+1;
                  TicketStore.TicketList.Add(ticketDTO);

                  return CreatedAtRoute("GetTicket", new {id=ticketDTO.Id}, ticketDTO);
            }


            // Editing a ticket
            [HttpPut("{id:int}", Name ="UpdateTicket")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]

            public IActionResult UpdateTicket(int id, [FromBody]TicketDTO ticketDTo){
                  
                  if(ticketDTo == null || id!=ticketDTo.Id){
                        return BadRequest();
                  }

                  var ticket = TicketStore.TicketList.FirstOrDefault(u=>u.Id==id);

                  ticket.Title = ticketDTo.Title;
                  ticket.Description = ticketDTo.Description;
                  return NoContent();
            }


            // Deletin a Ticket

            [HttpDelete("{id:int}", Name ="DeleteTicket")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public IActionResult DeleteTicket(int id){
                  if(id<=0){
                        return BadRequest();
                  }
                  var ticketToDelete = TicketStore.TicketList.FirstOrDefault(u=>u.Id == id);
                  if (ticketToDelete == null)
                  {
                        return NotFound();
                  }

                  TicketStore.TicketList.Remove(ticketToDelete);
                  return NoContent();
            }
      }
}