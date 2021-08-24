using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAPI.Database;


namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HalconController : ControllerBase
    {

        public HalconController(ILogger<HalconController> logger, DatabaseContext databaseContext)
        {
            this.Logger = logger;

            this.DatabaseContext = databaseContext;
        }

        private ILogger<HalconController> Logger { get; }
        private DatabaseContext DatabaseContext { get; }

        [HttpGet]
        public IActionResult GetVersion()
        {
            try
            {
                //* DatabaseContext. name_table
                
                String result =  "SpVision";
                Logger.LogInformation($"Halcon Version  {result}");              
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Halcon ReadImge {ex.Message}");
                return BadRequest();
            }
        }
    }
}