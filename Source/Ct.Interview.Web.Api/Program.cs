using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Ct.Interview.Web.Api.Data;
using Ct.Interview.Web.Api.Services.IServices;
using Ct.Interview.Web.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Add services to the container.
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// cors
services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .Build());
});

// ioc
services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(databaseName: "Test"));

services.AddControllers();

// enable caching
services.AddMemoryCache();

//DI services.
services.AddScoped<ICompanyServices, CompanyServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseCors();

app.Run();