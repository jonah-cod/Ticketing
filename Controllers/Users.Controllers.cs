using Microsoft.AspNetCore.Mvc;
using Ticketing.Data;
using Ticketing.Logging;
using Ticketing.Models;
using Ticketing.Models.Dto;

namespace Ticketing.Controllers
{
      [Route("/api/users")]
      [ApiController]
      public class UsersControllers : ControllerBase
      {

            private readonly ILogging _logger;
            private readonly ApplicationDbContext dbContext;
            private readonly IPasswordHasherService _PasswordHasher;
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<IEnumerable<UserDTO>> CreateUser([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                  return BadRequest();
            }

            if (dbContext.Users.FirstOrDefault(u=>u.Email==userDTO.Email)!=null)
            {
                  ModelState.AddModelError("Emailerror", "Email already exists");

                  return BadRequest(ModelState);
            }

            User model = new()
            {
                  Id=userDTO.Id,
                  FullNames=userDTO.FullNames,
                  Email=userDTO.Email,
                  Password=_PasswordHasher.HashPassword(userDTO.Password)
            };
            dbContext.Add(model);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}