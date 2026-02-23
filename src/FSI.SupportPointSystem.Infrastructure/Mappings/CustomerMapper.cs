using Dapper;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.ValueObjects;
using System;
using System.Data;

namespace FSI.SupportPointSystem.Infrastructure.Mappings
{
    public static class CustomerMapper
    {
        public static Customer ToDomain(dynamic row)
        {
            if (row == null) return null;
            var location = new Coordinates(
                Convert.ToDecimal(row.LatTarget),
                Convert.ToDecimal(row.LogTarget)
            );
            return new Customer(
                (Guid)row.Id,
                (string)row.CompanyName,
                new Cnpj((string)row.Cnpj),
                location 
            );
        }

        public static DynamicParameters ToParameters(Customer customer)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Id", customer.Id, DbType.Guid);
            parameters.Add("@CompanyName", customer.CompanyName, DbType.String);
            parameters.Add("@Cnpj", customer.Cnpj.Value, DbType.String);
            parameters.Add("@LatTarget", (decimal)customer.LocationTarget.Latitude, DbType.Decimal);
            parameters.Add("@LogTarget", (decimal)customer.LocationTarget.Longitude, DbType.Decimal);

            return parameters;
        }
    }
}