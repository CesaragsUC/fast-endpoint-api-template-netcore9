using MyApp.Handlers;
using MyApp.Models;

namespace MyApp.EndPoints.V2;

public class RegisterAuthorEndpoint_V2 : Endpoint<AuthorRequest, AuthorResponse>
{
    public override void Configure()
    {
        Post("/author/signup");
        Throttle(
            hitLimit: 120,
            durationSeconds: 60,
            headerName: "X-Client-Id" // this is optional
        );
        ResponseCache(60); //cache for 60 seconds
        AllowAnonymous();
        Version(2, deprecateAt: 3);
    }

    public override async Task HandleAsync(AuthorRequest r, CancellationToken c)
    {
        if (r.FirstName == "Elba")
            AddError("Forbidden name", "You cannot use this name");

        ThrowIfAnyErrors();

        await PublishAsync(new AuthorCreatedEvent
        {
            FirstName = r.FirstName,
            LastName = r.LastName,
            Email = r.Email,
            UserName = r.UserName
        });

        await Send.OkAsync(new AuthorResponse()
        {
            Message = $"Author {r.FirstName} {r.LastName} registered successfully!"
        });
    }
}
