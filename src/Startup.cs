using EventSourcingDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSingleton<EventStore>();
        services.AddSingleton<SubscriberService>();
        services.AddEndpointsApiExplorer(); // Add for Swagger
        services.AddSwaggerGen();           // Add for Swagger
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        Console.WriteLine($"Environment: {env.EnvironmentName}");
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();    // Add for Swagger
            app.UseSwaggerUI();  // Add for Swagger
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}