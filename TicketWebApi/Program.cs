using Microsoft.EntityFrameworkCore;
using TicketWebApi.Models;

var MiConfigCors = "AllHosts";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => {
    options.AddPolicy(name: MiConfigCors,
        policy => {
            policy.AllowAnyHeader();
            policy.WithMethods("PUT", "GET", "OPTIONS", "POST", "DELETE");
            policy.AllowAnyOrigin();
        });
});

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
// Add Entity framework
builder.Services.AddDbContext<TicketDbContext>(opt => opt.UseSqlServer("Name=ConnectionStrings:Chinook"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticket API V1");
    });
}

app.UseHttpsRedirection();

app.UseCors(MiConfigCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
