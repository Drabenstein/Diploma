using Application.Common;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(PagedResultDto<>).Assembly);
builder.Services.AddAmazonClients();
builder.Services.AddCaching();

string connectionString = builder.Configuration.GetConnectionString("DiplomaDb");
builder.Services.AddDatabaseServices(connectionString);
builder.Services.AddAmazonClients();
builder.Services.AddAmazonServices();

builder.Services.AddHttpContextServices(builder.Configuration);
builder.Services.AddAuth0Authentication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseGlobalExceptionHandling();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();