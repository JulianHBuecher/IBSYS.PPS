using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBSYS.PPS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IBSYS.PPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly ILogger<ResultsController> _logger;
        private readonly IbsysDatabaseContext _db;

        public ResultsController(ILogger<ResultsController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult> GetInputFile()
        {
            // TODO: Logik for Input-File
            return Ok("Input File");
        }
    }
}
