using MyApp.Models;

namespace MyApp.EndPoints.V2;

//https://medium.com/@biswajitpanday/elevate-your-net-apis-with-the-repr-pattern-and-fastendpoints-10c1b53e31b6
//https://www.craigmattson.net/blog/2022/09/mediatr-vs-minimal-apis-vs-fastendpoints/
//https://deviq.com/design-patterns/repr-design-pattern
public class AuthorByNameEndpoint_V2 : Endpoint<AuthorByNameRequest, AuthorResponse>
{
    public override void Configure()
    {
        Get("/author/{username}");
        Throttle(
            hitLimit: 120,
            durationSeconds: 60,
            headerName: "X-Client-Id" // this is optional
        );
        ResponseCache(60); //cache for 60 seconds
        AllowAnonymous();
        Version(2, deprecateAt: 3);
    }

    public override async Task HandleAsync(AuthorByNameRequest r, CancellationToken c)
    {
        if (string.IsNullOrWhiteSpace(r.UserName))
            AddError("UserName", "UserName cannot be empty");

        ThrowIfAnyErrors();

        await Send.OkAsync(new AuthorResponse()
        {
            Message = $"Author {r.UserName} founded!"
        });
    }
}
