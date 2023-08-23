using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Identity.Web;
using Seguridad.Core.Application;
using Seguridad.Core.Data;
using Seguridad.Core.Entities;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers().AddFluentValidation(fv =>fv.RegisterValidatorsFromAssemblyContaining<Registro>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddDbContext<ApplicationDbContext>( options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// builder.Services.AddMediatR(typeof(Registro.RegistroUsuarioCommand).Assembly);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddAutoMapper(typeof(Registro.RegistroUsuarioHandler));
builder.Services.AddControllers();
//builder.Services.AddValidatorsFromAssemblyContaining<Registro.RegistroUsuarioCommand>();
builder.Services.AddAuthorization();

builder.Services.AddIdentityCore<Usuario>();
var identityBuilder = new IdentityBuilder(typeof(Usuario), builder.Services);
identityBuilder.AddEntityFrameworkStores<ApplicationDbContext>();
identityBuilder.AddSignInManager<SignInManager<Usuario>>();
builder.Services.TryAddSingleton<ISystemClock, SystemClock>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
