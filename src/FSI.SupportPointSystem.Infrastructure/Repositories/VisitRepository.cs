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
            // Ajuste: p_SellerId e .ToString() para compatibilidade com CHAR(36)
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
            // Ajuste: Nomes dos parâmetros p_ e IDs convertidos para String
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

            // Ajuste: Prefixo p_ e remoção de DbType explícito (o driver infere melhor)
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