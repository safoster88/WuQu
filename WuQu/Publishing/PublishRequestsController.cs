namespace WuQu.Publishing
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class PublishRequestsController : ControllerBase
    {
        private readonly IMediator mediator;

        public PublishRequestsController(
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