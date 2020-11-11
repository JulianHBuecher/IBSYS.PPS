using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Disposition;
using IBSYS.PPS.Models.Capacity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Loader;

namespace IBSYS.PPS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapacityController : ControllerBase
    {
        private readonly ILogger<CapacityController> _logger;

        private readonly IbsysDatabaseContext _db;

        public CapacityController(ILogger<CapacityController> logger, IbsysDatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        
    }
}
