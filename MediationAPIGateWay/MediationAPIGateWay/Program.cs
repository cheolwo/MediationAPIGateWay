using Elastic.Clients.Elasticsearch;
using MediationAPIGateWay.Service;
using Microsoft.EntityFrameworkCore;
using 국토교통부_공공데이터Common;
using 주문Infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure Elasticsearch client

builder.Services.AddDbContext<공동주택DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("공동주택Connection")));
builder.Services.AddDbContext<주문DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("주문Connection")));

builder.Services.AddSingleton(sp =>
{
    var settings = new ElasticsearchClientSettings(new Uri(builder.Configuration["Elasticsearch:Uri"]));
    return new ElasticsearchClient(settings);
});

builder.Services.AddAutoMapper(typeof(주문자집단MappingProfile));
builder.Services.AddScoped<공동주택To주문자집단MappingService>();
builder.Services.AddScoped<공동주택검색Service>();
builder.Services.AddHostedService<데이터초기화Service>();

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
