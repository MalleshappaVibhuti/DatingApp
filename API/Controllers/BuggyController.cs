using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;            
        }
      
        [Authorize]
        [HttpGet("Auth")]
        public ActionResult<string> GetSecret(){
            return "Secrete Data";
        }

        [HttpGet("Not-Found")]
        public ActionResult<string> GetNoltFound(){
            var thing = _context.Users.Find(-1);
            if(thing==null) return NotFound();
            return Ok(thing);
        }
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError(){
           try{
           var thing=_context.Users.Find(-1);
            var dhold=thing.ToString();
            return dhold;
           }
           catch(Exception ex){
            return StatusCode(500,ex.Message);
           }
            
        }
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest(){
            return BadRequest("this is not a good request");
        }        

    }
}