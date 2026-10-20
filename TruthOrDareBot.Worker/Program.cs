using TruthOrDareBot.Game.Manager;
using TruthOrDareBot.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(new ContentProvider(
    Path.Combine(AppContext.BaseDirectory, "Content", "content.json")));

builder.Services.AddHostedService<BotWorker>();
builder.Build().Run();