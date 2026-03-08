using System.Data;
using Dapper;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Infrastructure.Context;
using FSI.SupportPointSystem.Infrastructure.Mappings;

namespace FSI.SupportPointSystem.Infrastructure.Repositories
{
    public class VisitRepository : IVisitRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public VisitRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> HasPendingCheckinAsync(Guid sellerId)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            parameters.Add("p_SellerId", sellerId.ToString());
            parameters.Add("p_HasPending", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "sp_CheckPendingCheckout",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<bool>("p_HasPending");
        }

        public async Task<Visit?> GetActiveVisitBySellerIdAsync(Guid sellerId)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SpGetActiveVisitBySellerId",
                new { p_SellerId = sellerId.ToString() },
                commandType: CommandType.StoredProcedure
            );

            return row == null ? null : VisitMapper.ToDomain(row);
        }

        public async Task SaveCheckinAsync(Visit visit)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                "SpRecordCheckin",
                new
                {
                    p_Id = visit.Id.ToString(),
                    p_SellerId = visit.SellerId.ToString(),
                    p_CustomerId = visit.CustomerId.ToString(),
                    p_LatitudeCaptured = visit.CheckinLocation.Latitude,
                    p_LongitudeCaptured = visit.CheckinLocation.Longitude,
                    p_DistanceMeters = visit.CheckinDistance
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task SaveCheckoutAsync(Visit visit)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();

            parameters.Add("p_SellerId", visit.SellerId.ToString());
            parameters.Add("p_CustomerId", visit.CustomerId.ToString());
            parameters.Add("p_LatCaptured", visit.CheckoutLocation!.Latitude);
            parameters.Add("p_LogCaptured", visit.CheckoutLocation!.Longitude);
            parameters.Add("p_Distance", visit.CheckoutDistance);
            parameters.Add("p_Summary", visit.SummaryCheckOut);

            await connection.ExecuteAsync(
                "SpRecordCheckout",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}