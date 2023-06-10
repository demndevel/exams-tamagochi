using Exagochi.Api.Persistence;

namespace Exagochi.Api.Jobs;

public class ExagochiRecurringJob
{
    private readonly ILogger<ExagochiRecurringJob> _logger;
    private readonly AppDbContext _db;

    public ExagochiRecurringJob(ILogger<ExagochiRecurringJob> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public void Execute()
    {
        _logger.LogInformation("Executing exagochi starving job…");
        
        _db.Exagochis.ToList().ForEach(x =>
        {
            x.Starve();
            _logger.LogInformation($"Exagochi {x.Name} starved");
        });
        
        _db.SaveChanges();
        
        _logger.LogInformation("Finished exagochi starving job");
    }
}