using IBSYS.PPS.Models.Generated;
using IBSYS.PPS.Models;
using IBSYS.PPS.Serializer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBSYS.PPS.Models.Input;

namespace IBSYS.PPS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class XmlDataController : ControllerBase
    {
        private readonly ILogger<XmlDataController> _logger;

        private readonly IbsysDatabaseContext _db;

        public XmlDataController(ILogger<XmlDataController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        // GET 
        [HttpGet("input")]
        public ActionResult<Input> GetInput()
        {
            DataSerializer serializer = new DataSerializer();

            var input = serializer.ReadDataAndDeserialize(@"./Assets/input.xml");

            if (input == null)
            {
                return NotFound();
            }

            return input;
        }

        [HttpGet("inputfirstperiod")]
        public ActionResult<Input> GetInputFirstPeriod()
        {
            DataSerializer serializer = new DataSerializer();

            var input = serializer.ReadDataAndDeserialize(@"./Assets/inputFirstPeriod.xml");

            if (input == null)
            {
                return NotFound();
            }

            return input;
        }

        // GET 
        [HttpGet("result")]
        public ActionResult<Results> GetResult()
        {
            DataSerializer serializer = new DataSerializer();

            var input = serializer.ReadDataAndDeserializePeriodResults(@"./Assets/resultPeriodFour.xml");

            if (input == null)
            {
                return NotFound();
            }

            return input;
        }
    }
}