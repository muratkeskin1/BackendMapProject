using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StajProje.Helper;
using StajProje.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StajProje.Controllers
{
    [ApiController]
    [Route("atm")]
    [EnableCors]
    public class ATMController : ControllerBase
    {
        private readonly ILogger<ATMController> _logger;
        private readonly DbHelper _db;
        public ATMController(ILogger<ATMController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _db = new DbHelper(context);
        }
        [HttpGet]
        public IActionResult Home()
        {
            _logger.LogWarning(HttpContext.Connection.RemoteIpAddress + " getAtmAll called");
            // _db.emailTest();
            return Ok(_db.GetFromRedis());
        }
        [HttpPost]
        public IActionResult Add([FromForm] ATM atm)
        {
            _logger.LogInformation(atm.ToString() + " is created by this ip" + HttpContext.Connection.RemoteIpAddress);
            _db.AddAtm(atm);
            return Ok();
        }
        [HttpPost("/excel")]
        public IActionResult ExcelPost()
        {
            _logger.LogInformation( "excel file is created by this ip" + HttpContext.Connection.RemoteIpAddress );
            _db.TestExcel() ;
            return Ok();
        }
        [HttpPost("/update/capacity")]
        public IActionResult UpdateCapacity([FromForm] Capacity capacity)
        {
            _logger.LogInformation(capacity.ToString()+ "updated by this ip" + HttpContext.Connection.RemoteIpAddress);
            _db.UpdateCapacity(capacity);
            return Ok();
        }
        [HttpPost("/simulate")]
        public IActionResult Simulate()
        {
            _db.RunAtmCapacitySimulation();
            return Ok();
        }
        [HttpPost("/postimage")]
        public IActionResult ImageTest()
        {
            _logger.LogInformation("image send to email by this ip" + HttpContext.Connection.RemoteIpAddress);
            _db.EmailTest(HttpContext.Request.Form.Files[0]);
            return Ok();
        }
        [HttpGet("/status")]
        public IActionResult Status()
        {

            return Ok(_db.GetByStatus());
        }
        [HttpPost("/updateatms")]
        public IActionResult UpdateAtms()
        {
            _logger.LogInformation("atms updated" + HttpContext.Connection.RemoteIpAddress );
            _db.SimulateDelivery();
            return Ok();
        }
        [HttpPost("delete/{id}")]
        public IActionResult DeleteAtm([FromRoute] int id)
        {
            _logger.LogInformation("silinecek atm id : " + id);
            _db.DeleteAtm(id);
            return Ok();
        }
        [HttpPost("/savedetails/{totalDistance}/{totalTime}/{routeCount}")]
        public IActionResult SaveDetail(double totalDistance, double totalTime, int routeCount)
        {

            _logger.LogInformation(totalDistance.ToString() + ' ' + totalTime.ToString()+ " routes" + (routeCount - 2));
            if (routeCount != 2)
            {
                _db.DeliveryDetailSave(totalDistance, totalTime, routeCount);
                _db.SimulateDelivery();
            }

            return Ok();
        }
        [HttpGet("/DeliverHistory")]
        public IActionResult DeliverHistory()
        {

            return Ok(_db.GetDeliveryHistory());
        }
        [HttpGet("/getchartdata")]
        public IActionResult ChartData()
        {

            return Ok(_db.GetChartData());
        }
        [HttpGet("/redis")]
        public IActionResult Redis()
        {

            return Ok(_db.GetATMs());
        }
        [HttpGet("/testdata")]
        public IActionResult TestEndpoint()
        {

            return Ok(DateTime.Today.Year +" "+DateTime.Today.Month );
        }
    }
}
