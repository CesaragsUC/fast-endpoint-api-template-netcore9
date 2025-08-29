
global using FastEndpoints;
using FastEndpoints.Swagger; //add this

var builder = WebApplication.CreateBuilder(args);


//https://fast-endpoints.com/docs/model-binding#from-headers
builder.Services
    .AddFastEndpoints()
    .AddResponseCaching();

builder.Services
   .SwaggerDocument(o =>
   {
       o.MaxEndpointVersion = 1;
       o.DocumentSettings = s =>
       {
           s.DocumentName = "Release v1.0.0";
           s.Title = "My API";
           s.Version = "v1";
       };
   })
   .SwaggerDocument(o =>
   {
       o.MaxEndpointVersion = 2;
       o.DocumentSettings = s =>
       {
           s.DocumentName = "Release v2.0.0";
           s.Title = "My API";
           s.Version = "v2";
       };
   }).SwaggerDocument(o =>
   {
       o.MaxEndpointVersion = 3;
       o.DocumentSettings = s =>
       {
           s.DocumentName = "Release v3.0.0";
           s.Title = "My API";
           s.Version = "v3";
       };
   });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseFastEndpoints(config =>
{
    config.Errors.UseProblemDetails(
       x =>
       {
           x.AllowDuplicateErrors = true;  //allows duplicate errors for the same error name
           x.IndicateErrorCode = true;     //serializes the fluentvalidation error code
           x.IndicateErrorSeverity = true; //serializes the fluentvalidation error severity
           x.TypeValue = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1";
           x.TitleValue = "One or more validation errors occurred.";
           x.TitleTransformer = pd => pd.Status switch
           {
               400 => "Validation Error",
               401 => "Unauthorized",
               404 => "Not Found",
               409 => "Conflict",
               _ => "One or more errors occurred!"
           };
       });
    config.Versioning.Prefix = "v";

}).UseResponseCaching();



app.UseSwaggerGen(); //add this
app.Run();