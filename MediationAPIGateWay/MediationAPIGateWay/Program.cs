using Azure.Storage.Blobs;
using Common.FileStorage;
using MediationAPIGateWay.Middleware;
using MediationAPIGateWay.Service;
using MediationAPIGateWay.Service.Hosted;
using MediationAPIGateWay.Service.�˻�;
using MediationAPIGateWay.�̵����;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Nest;
using ���䱳���_����������Common;
using �ٷ�Infra;
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

builder.Services.AddAutoMapper(typeof(�ֹ�������MappingProfile));
builder.Services.AddScoped<��������To�ֹ�������MappingService>();
builder.Services.AddScoped<�������ð˻�Service>();
builder.Services.AddHostedService<�������ʱ�ȭService>();
// MongoDB Ŭ���̾�Ʈ ���
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDb");
    return new MongoClient(connectionString);
});

// PKI X509 ���� ���
builder.Services.AddSingleton<PKI_X509Service>();
// �ٷ�DbContext�� DI�� ���
builder.Services.AddDbContext<�ٷ�DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ����ƽ Ŭ���̾�Ʈ�� DI�� ���
builder.Services.AddSingleton<IElasticClient>(new ElasticClient(new ConnectionSettings(new Uri(builder.Configuration["ElasticSearch:Uri"]))));
// Azure Blob Storage Ŭ���̾�Ʈ ���
builder.Services.AddSingleton(x =>
    new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage")));

// ���� ���� ���
builder.Services.AddScoped<IFileService, AzureBlobStorageService>();
// ElasticSearchService�� DI�� ���
builder.Services.AddScoped<������SearchService>();

// WorkerApplicationIndexer�� DI�� ���
builder.Services.AddHostedService<�ٷθ�ĪIndexer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMiddleware<IpBlockingMiddleware>();         // IP ���� �̵����
    app.UseMiddleware<RateLimitingMiddleware>();       // Rate Limiting �̵����
    app.UseMiddleware<SqlInjectionProtectionMiddleware>(); // SQL ������ ���͸� �̵����
    app.UseMiddleware<XssProtectionMiddleware>();      // XSS ���� �̵����
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

