using Ambev.Dev.Test.Domain.Validation;
using Ambev.Dev.Test.IoC.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddServices();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
