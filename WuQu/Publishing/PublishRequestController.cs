namespace WuQu.Publishing
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class PublishRequestController : ControllerBase
    {
        private readonly IMediator mediator;

        public PublishRequestController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Post(
            [FromBody] PublishRequest request)
        {
            await mediator.Send(request);
            return Ok();
        }
    }
}