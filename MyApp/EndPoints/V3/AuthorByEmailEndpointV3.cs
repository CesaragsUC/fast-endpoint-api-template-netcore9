using MyApp.Models;

namespace MyApp.EndPoints.V3;

//https://medium.com/@biswajitpanday/elevate-your-net-apis-with-the-repr-pattern-and-fastendpoints-10c1b53e31b6
//https://www.craigmattson.net/blog/2022/09/mediatr-vs-minimal-apis-vs-fastendpoints/
//https://deviq.com/design-patterns/repr-design-pattern
public class AuthorByEmailEndpoint_V3 : Endpoint<AuthorByEmailRequest, AuthorResponse>
{
    public override void Configure()
    {
        Get("/author/{email}");
        Throttle(
            hitLimit: 120,
            durationSeconds: 60,
            headerName: "X-Client-Id" // this is optional
        );
        ResponseCache(60); //cache for 60 seconds
        AllowAnonymous();
        Version(3, deprecateAt: 4);
    }

    public override async Task HandleAsync(AuthorByEmailRequest r, CancellationToken c)
    {
        if (string.IsNullOrWhiteSpace(r.Email))
            AddError("Email", "Email cannot be empty");

        ThrowIfAnyErrors();
        await Send.OkAsync(new AuthorResponse()
        {
            Message = $"Author {r.Email} founded!"
        });
    }
}
