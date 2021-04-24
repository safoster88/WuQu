namespace WuQu.Subscribing
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using WuQu.HeartBeat;

    [ApiController]
    [Route("api/[controller]")]
    public class SubscribeRequestController : ControllerBase
    {
        private readonly IMediator mediator;

        public SubscribeRequestController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Post(
            [FromBody] SubscribeRequest request)
        {
            try
            {
                await mediator.Send(request);
            }
            catch (HeartBeatFailedException)
            {
                return BadRequest("Failed to reach heartbeat endpoint.");
            }
            return Ok();
        }
    }
}