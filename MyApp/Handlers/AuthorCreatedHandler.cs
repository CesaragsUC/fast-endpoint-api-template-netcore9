namespace MyApp.Handlers;

public class AuthorCreatedHandler : IEventHandler<AuthorCreatedEvent>
{
    private readonly ILogger<AuthorCreatedHandler> _logger;
    public AuthorCreatedHandler(ILogger<AuthorCreatedHandler> logger) => _logger = logger;

    public async Task HandleAsync(AuthorCreatedEvent eventModel, CancellationToken ct)
    {
        _logger.LogInformation($"author created event received:{eventModel.FirstName} with email: {eventModel.Email}");
         await Task.CompletedTask;
    }
}

public class GetFullName : ICommand<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class FullNameHandler : ICommandHandler<GetFullName, string>
{
    public Task<string> ExecuteAsync(GetFullName command, CancellationToken ct)
    {
        var result = command.FirstName + " " + command.LastName;
        return Task.FromResult(result);
    }
}