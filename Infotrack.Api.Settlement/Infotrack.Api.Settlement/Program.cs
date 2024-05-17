using FluentValidation;
using Infotrack.Api.Settlement.Handlers;
using Infotrack.Api.Settlement.Infrastructure.Data;
using Infotrack.Api.Settlement.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services
  .AddAutoMapper(Assembly.GetEntryAssembly());
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IBookingSettlementService, BookingSettlementService>();

builder.Services.AddDbContext<ApiDbContext>(options =>
options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("InMemoryDb")!));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

var assembly = typeof(Program).Assembly;
var serviceDescriptor = assembly.DefinedTypes
    .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
    .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
    .ToArray();

builder.Services.Add(serviceDescriptor);

var app = builder.Build();

app.UseForwardedHeaders();
app.UseExceptionHandler();

var endpoints = app.Services
    .GetRequiredService<IEnumerable<IEndpoint>>();

IEndpointRouteBuilder routeBuilder = app;
var apiVersionSet = routeBuilder.NewApiVersionSet()
    .HasApiVersion(new Asp.Versioning.ApiVersion(1))
    .ReportApiVersions()
    .Build();

routeBuilder
    .MapGroup("/api/Settlement")
    .WithApiVersionSet(apiVersionSet);

foreach (var endpoint in endpoints)
{
    endpoint.MapEndpoint(routeBuilder);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}





app.UseAuthorization();
app.UseHttpsRedirection();

app.Run();
