using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SSEEvent.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("NotifyPolicy")]
    public class NotificationController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<NotificationController> _logger;

        public  NotificationController(ILogger<NotificationController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task Get()
        {
            //1. Set content type
            Response.ContentType = "text/event-stream";
            Response.StatusCode = 200;

            StreamWriter streamWriter = new StreamWriter(Response.Body);
            Cordinates.YPoint = 10;
            Cordinates.XPoint = 20;
            while (!HttpContext.RequestAborted.IsCancellationRequested)
            {
                //2. Await something that generates messages
                await Task.Delay(5000, HttpContext.RequestAborted);

                //3. Write to the Response.Body stream
                // await streamWriter.WriteLineAsync($"{DateTime.Now } Looping");
                Cordinates.YPoint+=10;
                Cordinates.XPoint += 10 ;
                string message  =  "event: message\n"+"[{x:"+Cordinates.XPoint+","+"y:"+Cordinates.YPoint+"}]\n";
                await streamWriter.WriteAsync(message);
                await streamWriter.FlushAsync();

            }
        }
    }
}
