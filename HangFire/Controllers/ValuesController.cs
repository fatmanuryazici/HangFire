using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HangFireApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() 
        {
            BackgroundJob.Enqueue(() => BackgroundTestServices.Test()); //Bu metot sayesinde tetikleme yapabiliyorum
            return Ok("Hangfire çalıştı");
        }
        
    }

    public class BackgroundTestServices
    {
        public static void Test()
        {
            Console.WriteLine("Hangfire calısıyor:" + DateTime.Now);
        }
    }
}

