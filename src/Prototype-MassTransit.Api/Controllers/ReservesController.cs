using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Prototype_MassTransit.Contracts.Reserves;
using Swashbuckle.AspNetCore.Annotations;

namespace Prototype_MassTransit.Api.Controllers;

[ApiController]
[Route("api/reserves")]
public class ReservesController : ControllerBase
{
    private readonly ILogger<ReservesController> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public ReservesController(ILogger<ReservesController> logger,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status202Accepted)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [Route("")]
    public async Task<IActionResult> PostAsync([FromBody] Reserve reserve)
    {
        await _publishEndpoint.Publish<ISubmitReserve>(new
            { reserve.Id, reserve.Effort, reserve.Duration, reserve.PlanningCode, reserve.ResourceCode });


        return Accepted(new { reserve.Id, Status = nameof(Microsoft.AspNetCore.Http.HttpResults.Accepted) });
    }
}