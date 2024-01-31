
using Ticketing.Models;
using Ticketing.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Data;
using Microsoft.AspNetCore.JsonPatch;
using Ticketing.Logging;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{



      [Route("api/tickets")]
      [ApiController]
      public class TicketController : ControllerBase
      {
            // Logging
            private readonly ILogging _logger;
            private readonly ApplicationDbContext _db;
            public TicketController(ILogging logger, ApplicationDbContext db)
            {
                  _logger = logger;
                  _db = db;
            }

            // Get all tickets
            [HttpGet]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public ActionResult<IEnumerable<TicketDTO>> GetTickets()
            {
                  _logger.Log("Getting all tickets", "Info");
                  return Ok(_db.Tickets);
            }

            // Getting a single ticket
            [HttpGet("{id:int}", Name = "GetTicket")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public ActionResult<TicketDTO> GetTicket(int id)
            {
                  if (id == 0)
                  {
                        return BadRequest();
                  }
                  var ticket = _db.Tickets.FirstOrDefault(u => u.Id == id);

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
        public ActionResult<TicketDTO> CreateTicket([FromBody] TicketDTO ticketDTO)
        {
            // if (!ModelState.IsValid){
            //       return BadRequest(ModelState);
            // }
            if (_db.Tickets.FirstOrDefault(u => u.Title == ticketDTO.Title) != null)
            {
                ModelState.AddModelError("customError", "Ticket with the same title already exists");
                return BadRequest(ModelState);
            }
            if (ticketDTO == null)
            {
                return BadRequest(ticketDTO);
            }

            if (ticketDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Ticket model = new()
            {
                Id = ticketDTO.Id,
                Title = ticketDTO.Title,
                Description = ticketDTO.Description,
                Attachment = ticketDTO.Attachment

            };

            _db.Tickets.Add(model);
            _db.SaveChanges();

            return CreatedAtRoute("GetTicket", new { id = ticketDTO.Id }, ticketDTO);
        }


        // Editing a ticket
        [HttpPut("{id:int}", Name = "UpdateTicket")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]

            public IActionResult UpdateTicket(int id, [FromBody] TicketDTO ticketDTo)
            {

                  if (ticketDTo == null || id != ticketDTo.Id)
                  {
                        return BadRequest();
                  }

                  Ticket model = new()
                  {
                        Id = ticketDTo.Id,
                        Title = ticketDTo.Title,
                        Description = ticketDTo.Description,
                        Attachment = ticketDTo.Attachment

                  };

                  _db.Tickets.Update(model);
                  _db.SaveChanges();
                  return NoContent();
            }


            // Deleting a Ticket

            [HttpDelete("{id:int}", Name = "DeleteTicket")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public IActionResult DeleteTicket(int id)
            {

                  if (id <= 0)
                  {
                        return BadRequest();
                  }
                  var ticketToDelete = _db.Tickets.FirstOrDefault(u => u.Id == id);
                  if (ticketToDelete == null)
                  {
                        return NotFound();
                  }
                  _logger.Log("Deleting a ticket cannot be reverted, Ticket deleted: " + id, "warning");
                  _db.Tickets.Remove(ticketToDelete);
                  _db.SaveChanges();
                  return NoContent();
            }

            // partially update tickets 
            [HttpPatch("id:int", Name = "PartialyUpdate")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]

            public IActionResult PartislyUpdateTicket(int id, JsonPatchDocument<TicketDTO> patchTicketDTO)
            {
                  if (patchTicketDTO == null || id == 0)
                  {
                        return BadRequest();
                  }

                  var ticket = _db.Tickets.AsNoTracking().FirstOrDefault(u => u.Id == id);

                  if (ticket == null)
                  {
                        return NotFound();
                  }

                  TicketDTO ticketDTO = new()
                  {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        Description = ticket.Description,
                        Attachment = ticket.Attachment

                  };

                  patchTicketDTO.ApplyTo(ticketDTO, ModelState);
                  if (!ModelState.IsValid)
                  {
                        return BadRequest(ModelState);
                  }


                  Ticket model = new Ticket()
                  {
                        Id = ticketDTO.Id,
                        Title = ticketDTO.Title,
                        Description = ticketDTO.Description,
                        Attachment = ticketDTO.Attachment

                  };

                  _db.Tickets.Update(model);
                  _db.SaveChanges();
                  return NoContent();

            }
      }
}