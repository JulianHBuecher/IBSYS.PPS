using System.Collections;
using System.IO;
using IBSYS.PPS.Models;
using IBSYS.PPS.Serializer;
using Microsoft.AspNetCore.Mvc;

namespace IBSYS.PPS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class XmlDataController : ControllerBase
    {
        // GET
        [HttpGet]
        public ActionResult<Input> GetAll()
        {
            DataSerializer serializer = new DataSerializer();

            var input = serializer.ReadDataAndDeserialize(@"/home/julian/RiderProjects/IBSYS.PPS/IBSYS.PPS/Assets/input.xml");

            if (input == null)
            {
                return NotFound();
            }

            return input;
        }
    }
}