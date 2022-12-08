using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StoreService_AT.Model;
using StoreService_AT.RabbitMQ.Consumer;
using StoreService_AT.RabbitMQ.Sender;
using StoreService_AT.Repository;
using StoreService_AT.Service.ProductService;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();
builder.Services.AddHostedService<RabbitMQMessageConsumer>();

builder.Services.Configure<StoreDatabaseSettings>(builder.Configuration.GetSection(nameof(StoreDatabaseSettings)));
builder.Services.AddSingleton<IStoreDatabaseSettings>(sp => sp.GetRequiredService<IOptions<StoreDatabaseSettings>>().Value);

builder.Services.AddHttpClient<IProductService, ProductService>(c =>
         c.BaseAddress = new Uri(builder.Configuration["ProductAPI"])
    );
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "redis:6379"; // redis is the container name of the redis service. 6379 is the default port
    options.InstanceName = "SampleInstance";
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();

app.MapControllers();

app.Run();