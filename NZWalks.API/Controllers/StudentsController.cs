using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        // GET : https://localhost:portnumber/api/students
        [HttpGet]
        [ActionName("Get")]
        public IActionResult GetAllStudents()
        {
            string[] studentName = new string[] { "Teju", "Sri", "Ram", "Suche", "Ananya", "bhaaai", "JP", "Adarsh" };

            return Ok(studentName); // gets the 200 
        }
    }
}
