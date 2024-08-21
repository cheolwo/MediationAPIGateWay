using Elastic.Clients.Elasticsearch;
using MediationAPIGateWay.Service;
using Microsoft.EntityFrameworkCore;
using ���䱳���_����������Common;
using �ֹ�Infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure Elasticsearch client

builder.Services.AddDbContext<��������DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("��������Connection")));
builder.Services.AddDbContext<�ֹ�DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("�ֹ�Connection")));

builder.Services.AddSingleton(sp =>
{
    var settings = new ElasticsearchClientSettings(new Uri(builder.Configuration["Elasticsearch:Uri"]));
    return new ElasticsearchClient(settings);
});

builder.Services.AddAutoMapper(typeof(�ֹ�������MappingProfile));
builder.Services.AddScoped<��������To�ֹ�������MappingService>();
builder.Services.AddScoped<�������ð˻�Service>();
builder.Services.AddHostedService<�������ʱ�ȭService>();

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
