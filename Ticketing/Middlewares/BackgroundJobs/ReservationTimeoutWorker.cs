using MediatR;
using Ticketing.Application.UseCases.Reservation.Commands.CancelExpiredReservations;

namespace Ticketing.Middlewares.BackgroundJobs
{
    public class ReservationTimeoutWorker : BackgroundService
    {
        private readonly ILogger<ReservationTimeoutWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _period = TimeSpan.FromMinutes(1);

        public ReservationTimeoutWorker(ILogger<ReservationTimeoutWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Reservation Timeout Worker running.");

            using PeriodicTimer timer = new(_period);

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await DoWork(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Reservation Timeout Worker stopped.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reservation Timeout Worker failed.");
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Checking for expired reservations...");

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new CancelExpiredReservationsCommand(), stoppingToken);
            }

            _logger.LogInformation("Finished checking for expired reservations.");
        }
    }
}
