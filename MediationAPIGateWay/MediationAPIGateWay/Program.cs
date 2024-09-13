using Azure.Storage.Blobs;
using Common.FileStorage;
using MediationAPIGateWay.Middleware;
using MediationAPIGateWay.Service;
using MediationAPIGateWay.Service.Hosted;
using MediationAPIGateWay.Service.검색;
using MediationAPIGateWay.미들웨어;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Nest;
using 국토교통부_공공데이터Common;
using 근로Infra;
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

builder.Services.AddAutoMapper(typeof(주문자집단MappingProfile));
builder.Services.AddScoped<공동주택To주문자집단MappingService>();
builder.Services.AddScoped<공동주택검색Service>();
builder.Services.AddHostedService<데이터초기화Service>();
// MongoDB 클라이언트 등록
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDb");
    return new MongoClient(connectionString);
});

// PKI X509 서비스 등록
builder.Services.AddSingleton<PKI_X509Service>();
// 근로DbContext를 DI에 등록
builder.Services.AddDbContext<근로DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 엘라스틱 클라이언트를 DI에 등록
builder.Services.AddSingleton<IElasticClient>(new ElasticClient(new ConnectionSettings(new Uri(builder.Configuration["ElasticSearch:Uri"]))));
// Azure Blob Storage 클라이언트 등록
builder.Services.AddSingleton(x =>
    new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage")));

// 파일 서비스 등록
builder.Services.AddScoped<IFileService, AzureBlobStorageService>();
// ElasticSearchService를 DI에 등록
builder.Services.AddScoped<생산자SearchService>();

// WorkerApplicationIndexer를 DI에 등록
builder.Services.AddHostedService<근로매칭Indexer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMiddleware<IpBlockingMiddleware>();         // IP 차단 미들웨어
    app.UseMiddleware<RateLimitingMiddleware>();       // Rate Limiting 미들웨어
    app.UseMiddleware<SqlInjectionProtectionMiddleware>(); // SQL 인젝션 필터링 미들웨어
    app.UseMiddleware<XssProtectionMiddleware>();      // XSS 차단 미들웨어
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

