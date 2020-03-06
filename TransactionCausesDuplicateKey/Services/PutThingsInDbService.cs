using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TransactionCausesDuplicateKey.Db;

namespace TransactionCausesDuplicateKey.Services
{
    public class PutThingsInDbService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly TestDbContext _dbContext;

        public PutThingsInDbService(IConfiguration configuration, ILogger<PutThingsInDbService> logger, TestDbContext dbContext)
        {
            _configuration = configuration;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task ProcessNewhireEvent(string eventId, string personName, string personId, string personNumber)
        {
            Console.WriteLine($"Processing new hire event: EventId: {eventId}, PersonName: {personName}, PersonId: {personId}, PersonNumber: {personNumber}");

            var employeeToAdd = new DbEmployee()
            {
                PersonName = personName,
                PersonId = personId,
                PersonNumber = personNumber
            };

            var eventToAdd = new DbEvent()
            {
                UniqueEventId = eventId
            };

            try
            {
                //Default transaction level is read committed
                using (var trans = await _dbContext.Database.BeginTransactionAsync().ConfigureAwait(false))
                {
                    _dbContext.Employees.Add(employeeToAdd);
                    _dbContext.Events.Add(eventToAdd);

                    //Read Committed transaction will fail once this event ID has been added already. ReadCommitted is the default transaction level
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                    await trans.CommitAsync().ConfigureAwait(false);

                }

                Console.WriteLine("Completed successfully :)");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
            }
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //This is the first event and should complete successfully
            await ProcessNewhireEvent("TEST1", "John", "0001", "0001");

            //This is an event with a unique event id but the same person details. It should fail on a duplicate key for the Employee Table
            await ProcessNewhireEvent("TEST2", "John", "0001", "0001");
            //This is as expected

            //When we now submit a new completely unrelated event, it still fails on trying to add the previous event
            await ProcessNewhireEvent("TEST3", "Henk", "0002", "0002");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

        }
    }
}
