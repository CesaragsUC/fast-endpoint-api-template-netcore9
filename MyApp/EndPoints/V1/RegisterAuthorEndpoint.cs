using MyApp.Handlers;
using MyApp.Models;

namespace MyApp.EndPoints.V1;

//https://medium.com/@biswajitpanday/elevate-your-net-apis-with-the-repr-pattern-and-fastendpoints-10c1b53e31b6
//https://www.craigmattson.net/blog/2022/09/mediatr-vs-minimal-apis-vs-fastendpoints/
//https://deviq.com/design-patterns/repr-design-pattern
public class RegisterAuthorEndpoint : Endpoint<AuthorRequest, AuthorResponse>
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
        Version(1, deprecateAt: 2);
    }

    public override async Task HandleAsync(AuthorRequest r, CancellationToken c)
    {
        if (r.FirstName == "Cesar" || r.LastName == "Santos")
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
