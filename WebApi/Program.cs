var builder = WebApplication.CreateBuilder(args);

var Startup=new Startup(builder.Configuration);

Startup.ConfigureServices(builder.Services);
// Add services to the container.

    
var app = builder.Build();
Startup.Configure(app, builder.Environment);

