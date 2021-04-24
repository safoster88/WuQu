namespace WuQu.Subscribing
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using WuQu.HeartBeat;

    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IMediator mediator;

        public SubscriptionsController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut]
        public async Task<ActionResult> Put(
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