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
        public async Task<Visit?> GetActiveVisitBySellerIdAsync(Guid sellerId)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var row = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SpGetActiveVisitBySellerId",
                new { SellerId = sellerId },
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
                    Id = visit.Id,
                    SellerId = visit.SellerId,
                    CustomerId = visit.CustomerId,
                    LatitudeCaptured = visit.CheckinLocation.Latitude,
                    LongitudeCaptured = visit.CheckinLocation.Longitude,
                    DistanceMeters = visit.CheckinDistance
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task SaveCheckoutAsync(Visit visit)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@SellerId", visit.SellerId, DbType.Guid);
            parameters.Add("@CustomerId", visit.CustomerId, DbType.Guid);
            parameters.Add("@LatCaptured", visit.CheckoutLocation!.Latitude, DbType.Decimal);
            parameters.Add("@LogCaptured", visit.CheckoutLocation!.Longitude, DbType.Decimal);
            parameters.Add("@Distance", visit.CheckoutDistance, DbType.Double);
            await connection.ExecuteAsync(
                "SpRecordCheckout",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}