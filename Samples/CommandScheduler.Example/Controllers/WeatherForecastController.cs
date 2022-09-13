using CommandScheduler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CommandScheduler.Example.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private readonly IMediator _mediator;

    public WeatherForecastController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IActionResult Get()
    {
        // Executed after a certain time interval
        _mediator.Schedule(new Command { Id = 1 }, DateTimeOffset.Now.AddSeconds(5));

        //// Fire many times on the specified CRON schedule
        //_mediator.ScheduleRecurring(new Command { Id = 1 },nameof(Command), CommonCrons.Minutely);

        //// Executed only once and almost immediately after creation
        //_mediator.Enqueue(new Command { Id = 1 });

        return Ok();
    }
}

public record Command : IRequest
{
    public int Id { get; init; }
}
public class CommandHandler : IRequestHandler<Command, Unit>
{
    public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
    {
        return default;
    }
}