
using Ticketing.Models;
using Ticketing.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Data;
using Microsoft.AspNetCore.JsonPatch;
using Ticketing.Logging;

namespace Ticketing.Controllers
{
      [Route("api/tickets")]
      [ApiController]
      public class TicketController : ControllerBase
      {
            // Logging
            private readonly ILogging _logger;
            public TicketController(ILogging logger)
            {
                  _logger = logger;
            }

            // Get all tickets
            [HttpGet]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public ActionResult<IEnumerable<TicketDTO>> GetTickets()
            {
                  _logger.Log("Getting all tickets", "Info");
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
                        _logger.Log("Didn't find ticket with ID: " + id, "Error");
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
                        ModelState.AddModelError("customError", "Ticket with the same title already exists");
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


            // Deleting a Ticket

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
                  _logger.Log("Deleting a ticket cannot be reverted, Ticket deleted: "+id, "warning");
                  TicketStore.TicketList.Remove(ticketToDelete);
                  return NoContent();
            }

            // partially update tickets 
            [HttpPatch("id:int", Name ="PartialyUpdate")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]

            public IActionResult PartislyUpdateTicket(int id, JsonPatchDocument<TicketDTO> patchTicketDTO){
                  if (patchTicketDTO==null || id ==0)
                  {
                        return BadRequest();
                  }

                  var ticket = TicketStore.TicketList.FirstOrDefault(u=>u.Id==id);

                  if (ticket == null)
                  {
                        return NotFound();
                  }

                  patchTicketDTO.ApplyTo(ticket, ModelState);

                  if (!ModelState.IsValid)
                  {
                        return BadRequest(ModelState);
                  }
                  return NoContent();
            }
      }
}