using ChecklistApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ChecklistContext>(options => options.UseSqlite($"DataSource = Checklist.db"));

using IHost host = builder.Build();

await host.RunAsync(); 