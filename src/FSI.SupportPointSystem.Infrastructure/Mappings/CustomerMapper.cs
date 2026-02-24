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
            var address = new Address(
                (string)row.ZipCode,
                (string)row.Street,
                (string)row.Number,
                (string)row.Neighborhood,
                (string)row.City,
                (string)row.State,
                (string)row.Complement
            );
            return new Customer(
                (Guid)row.Id,
                (string)row.CompanyName,
                new Cnpj((string)row.Cnpj),
                address, 
                location
            );
        }

        public static DynamicParameters ToParameters(Customer customer)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", customer.Id, DbType.Guid);
            parameters.Add("@CompanyName", customer.CompanyName, DbType.String);
            parameters.Add("@Cnpj", customer.Cnpj.Value, DbType.String);
            parameters.Add("@ZipCode", customer.Address.ZipCode, DbType.String);
            parameters.Add("@Street", customer.Address.Street, DbType.String);
            parameters.Add("@Number", customer.Address.Number, DbType.String);
            parameters.Add("@Complement", customer.Address.Complement, DbType.String);
            parameters.Add("@Neighborhood", customer.Address.Neighborhood, DbType.String);
            parameters.Add("@City", customer.Address.City, DbType.String);
            parameters.Add("@State", customer.Address.State, DbType.String);
            parameters.Add("@LatTarget", (decimal)customer.LocationTarget.Latitude, DbType.Decimal);
            parameters.Add("@LogTarget", (decimal)customer.LocationTarget.Longitude, DbType.Decimal);
            return parameters;
        }
    }
}