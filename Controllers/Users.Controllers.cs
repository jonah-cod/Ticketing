using Microsoft.AspNetCore.Mvc;
using Ticketing.Data;
using Ticketing.Logging;
using Users.Models;

namespace Users.Controllers
{
      [Route("/api/users")]
      [ApiController]
      public class UsersControllers : ControllerBase
      {

            private readonly ILogging _logger;
            private readonly ApplicationDbContext dbContext;
            public UsersControllers(ApplicationDbContext db, ILogging logger)
            {
                  _logger = logger;
                  dbContext = db;
            }


            [HttpGet(Name ="GetUsers")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public ActionResult GetUsers()
            {

                  var users = dbContext.Users;
                  
                  _logger.Log("Getting users", "infor");
                  return Ok(users);
            }

            [HttpGet("{id:int}", Name ="GetUser")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public ActionResult GetUser(int id){
                  if (id<=0)
                  {
                        ModelState.AddModelError("ModelError", "Id is needed");
                        return BadRequest(ModelState);
                  }
                  var User = dbContext.Users.FirstOrDefault(u=>u.Id==id);

                  if (User==null)
                  {
                        return NotFound();
                  }

                  return Ok(User);
            }
      }
}