namespace BodvedVS.DataLibrary;
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-8.0&tabs=visual-studio#asynchronous-timed-background-task
public class TimedHostedService : BackgroundService
{
	private readonly ILogger<TimedHostedService> _logger;
	private readonly IAllUsrs allUsrs;
	private int _executionCount;
	private int oldActUsr;

	public TimedHostedService(ILogger<TimedHostedService> logger, IAllUsrs allUsrs)
	{
		_logger = logger;
		this.allUsrs = allUsrs;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("Timed Hosted Service running.");

		// When the timer should have no due-time, then do the work once now.
		// DoWork(); Hemen yapmasına gerek yok

		using PeriodicTimer timer = new(TimeSpan.FromMinutes(0.1));

		try
		{
			while (await timer.WaitForNextTickAsync(stoppingToken))
			{
				DoWork();
			}
		}
		catch (OperationCanceledException)
		{
			_logger.LogInformation("Timed Hosted Service is stopping.");
		}
	}

	// Could also be a async method, that can be awaited in ExecuteAsync above
	private void DoWork()
	{
		//int count = Interlocked.Increment(ref _executionCount);

		var actUsr = allUsrs.ClearExpiredUsrs();

		if (oldActUsr != actUsr) {
			_logger.LogInformation("Timed Hosted Service {RunTime} #Usr: {actUsr}", DateTime.Now, actUsr);
			oldActUsr = actUsr;
		}
		// Console.WriteLine($"Timed Hosted Service. ActiveUserCnt: {actUsr}");
	}
}
