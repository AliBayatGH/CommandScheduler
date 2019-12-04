# Integrate MediatR with Hangfire

To schedule your commands, execute them parallel with retry option and monitor them. `Hangfire` gives me all these kind of features but You have to have `public` method which You have to pass to `Hangifre` method (for example `BackgroundJob.Enqueue`). This is a problem – with mediator pattern You cannot pass public method of handler because You have decoupled it from invoker. So You need special way to integrate `MediatR` with `Hangfire` without affecting basic assumptions.

I presented the way of processing commands asynchronously using `MediatR` and `Hangfire`. With this approach we have:
1. Decoupled invokers and handlers of commands.
2. Scheduling commands mechanism.
3. Invoker and handler of command may be other processes.
4. Commands execution monitoring.
5. Commands execution retries mechanism.

These benefits are very important during development using eventual consistency approach. We have more control over commands processing and we can react quickly if problem will appear.

For this, In `CommandScheduler` we difined 4 classes:
* `CommandsScheduler`  serializes commands and sends them to `Hangfire`.
* `CommandsExecutor`  responods to `Hangfire` jobs execution, deserializes commands and sends them to handlers using `MediatR`.
* `MediatorSerializedObject`  wrapper class for serialized/deserialized commands with additional properties – command type and additional description.
*  `MediatorExtension` extension methods for `IMediator`

```csharp
public class MyMController : Controller
{
    private IMediator _mediator;
    
    public MyMicroserviceController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TestCommand testCommand)
    {
     _mediator.Enqueue(testCommand);

      return Ok();
    }
  }
```
And our commands are scheduled, invoked and monitored by `Hangfire`.

#### Installing CommandScheduler
[CommandScheduler](https://www.nuget.org/packages/CommandScheduler/) is available as a NuGet package. You can install it using the NuGet Package Console window:

`Install-Package CommandScheduler`

Or via the .NET Core command line interface:

`dotnet add package CommandScheduler`

Either commands, from Package Manager Console or .NET Core CLI, will download and install CommandScheduler and all required dependencies.
