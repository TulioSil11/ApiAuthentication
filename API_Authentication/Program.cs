using API_Authentication.IoC;
using API_Authentication.Extensions;
using API_Authentication.ApiEndpoints;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceSwagger();
builder.RegisterServices(builder.Configuration);
builder.AddAuthentication(builder.Configuration);


var app = builder.Build();

app.MapLoginEndpoints();
app.AddSwaggerConfiguration();

app.UseCors(x => x.AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();

app.Run();
