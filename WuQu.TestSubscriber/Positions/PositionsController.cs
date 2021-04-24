namespace WuQu.TestSubscriber.Positions
{
    using System.Numerics;
    using Microsoft.AspNetCore.Mvc;
    using Serilog;

    [ApiController]
    [Route("wuqu/[controller]")]
    public class PositionsController : ControllerBase
    {
        private readonly ILogger logger;

        public PositionsController(ILogger logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Position position)
        {
            logger.Debug("Received position {@Position}", position);
            return Ok();
        }
    }
}