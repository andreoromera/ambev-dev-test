using Ambev.Dev.Test.Api.Handlers;
using Ambev.Dev.Test.Domain.Validation;
using Ambev.Dev.Test.IoC.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddConfigs();
builder.Services.AddAuth();
builder.Services.AddDatabase();

//Configuring error handling
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = (context) =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

        var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
});

//Adding Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Setting Routing options
builder.Services.AddRouting(x => x.LowercaseUrls = true);

//COnfiguring Automatic Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CredentialsValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //Swagger only for development purposes
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Converts unhandled exceptions into Problem Details responses
app.UseExceptionHandler();

// Returns the Problem Details response for (empty) non-successful responses
app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
