using WebApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "swagger", Version = "v1" }));
builder.Services.AddAuth0Authentication(builder.Configuration);
builder.Services.AddCommandQueries();
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