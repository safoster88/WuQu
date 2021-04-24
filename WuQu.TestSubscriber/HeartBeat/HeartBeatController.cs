namespace WuQu.TestSubscriber.HeartBeat
{
    using Microsoft.AspNetCore.Mvc;
    using Serilog;

    [ApiController]
    [Route("wuqu/[controller]")]
    public class HeartBeatController : ControllerBase
    {
        private readonly ILogger logger;

        public HeartBeatController(ILogger logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            logger.Information("Received HeartBeat");
            return Ok();
        }
    }
}