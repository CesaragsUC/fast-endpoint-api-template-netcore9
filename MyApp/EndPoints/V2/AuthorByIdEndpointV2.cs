using MyApp.Models;

namespace MyApp.EndPoints.V2;

//https://medium.com/@biswajitpanday/elevate-your-net-apis-with-the-repr-pattern-and-fastendpoints-10c1b53e31b6
//https://www.craigmattson.net/blog/2022/09/mediatr-vs-minimal-apis-vs-fastendpoints/
//https://deviq.com/design-patterns/repr-design-pattern
public class AuthorByIdEndpoint_V2 : Endpoint<AuthorByIdRequest, AuthorResponse>
{
    public override void Configure()
    {
        Get("/author/{id:guid}");
        Throttle(
            hitLimit: 120,
            durationSeconds: 60,
            headerName: "X-Client-Id" // this is optional
        );
        ResponseCache(60); //cache for 60 seconds
        AllowAnonymous();
        Version(2, deprecateAt: 3);// nao vai mostrar mais na versao 3
    }

    public override async Task HandleAsync(AuthorByIdRequest r, CancellationToken c)
    {
        if (r.Id == Guid.Empty)
            AddError("Id", "Id cannot be empty");

        ThrowIfAnyErrors();

        await Send.OkAsync(new AuthorResponse()
        {
            Message = $"Author {r.Id} founded!"
        });
    }
}
