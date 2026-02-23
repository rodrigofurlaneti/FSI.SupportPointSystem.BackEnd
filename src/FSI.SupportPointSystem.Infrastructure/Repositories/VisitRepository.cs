using System.Data;
using Dapper;
using FSI.SupportPoint.Domain.Entities;
using FSI.SupportPoint.Domain.Interfaces.Repositories;
using FSI.SupportPoint.Infrastructure.Context;
using FSI.SupportPoint.Infrastructure.Mappings;

namespace FSI.SupportPointSystem.Infrastructure.Repositories;

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
        const string sql = "SELECT * FROM Checkins WHERE SellerId = @SellerId AND CheckoutTimestamp IS NULL LIMIT 1";
        var row = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { SellerId = sellerId });
        return row == null ? null : VisitMapper.ToDomain(row);
    }

    public async Task SaveCheckinAsync(Visit visit)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        // Parâmetros tipados que o Dapper limpa automaticamente
        var parameters = new DynamicParameters();
        parameters.Add("p_sellerid", visit.SellerId, DbType.Guid);
        parameters.Add("p_customerid", visit.CustomerId, DbType.Guid);
        parameters.Add("p_latcaptured", visit.CheckinLocation.Latitude, DbType.Decimal);
        parameters.Add("p_logcaptured", visit.CheckinLocation.Longitude, DbType.Decimal);
        parameters.Add("p_distance", visit.CheckinDistance, DbType.Double);

        // Acesso 100% via Procedure
        await connection.ExecuteAsync(
            "CALL SpRecordCheckin(@p_sellerid, @p_customerid, @p_latcaptured, @p_logcaptured, @p_distance)",
            parameters,
            commandType: CommandType.Text 
        );
    }

    public async Task SaveCheckoutAsync(Visit visit)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("p_sellerid", visit.SellerId, DbType.Guid);
        parameters.Add("p_customerid", visit.CustomerId, DbType.Guid);
        parameters.Add("p_latcaptured", visit.CheckoutLocation!.Latitude, DbType.Decimal);
        parameters.Add("p_logcaptured", visit.CheckoutLocation!.Longitude, DbType.Decimal);
        parameters.Add("p_distance", visit.CheckoutDistance, DbType.Double);

        await connection.ExecuteAsync(
            "CALL SpRecordCheckout(@p_sellerid, @p_customerid, @p_latcaptured, @p_logcaptured, @p_distance)",
            parameters
        );
    }
}