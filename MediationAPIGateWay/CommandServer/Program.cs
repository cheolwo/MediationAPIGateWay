using Quartz;
using ��Common.Command;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Quartz.NET Hosted Service ���
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
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
// ���ø����̼� ���� �� Quartz.NET �����ٷ� ���� ����
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await ScheduleJobs(serviceProvider);
}

app.Run();
static async Task ScheduleJobs(IServiceProvider serviceProvider)
{
    var schedulerFactory = serviceProvider.GetRequiredService<ISchedulerFactory>();
    var scheduler = await schedulerFactory.GetScheduler();

    var job = JobBuilder.Create<CommandProcessingJob>()
        .WithIdentity("commandProcessorJob", "group1")
        .Build();

    var trigger = TriggerBuilder.Create()
        .WithIdentity("trigger1", "group1")
        .StartNow()
        .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(10) // 10�ʸ��� ����
            .RepeatForever())
        .Build();

    await scheduler.ScheduleJob(job, trigger);
    await scheduler.Start();
}
