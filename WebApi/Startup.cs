using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApi.Services;


public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
       
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddMemoryCache();
        services.Add(new ServiceDescriptor(typeof(IRepo),new ConsoleLogger()));
       


    }
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
 

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}