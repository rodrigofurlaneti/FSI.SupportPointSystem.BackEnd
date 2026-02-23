using Dapper;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.ValueObjects;
using System;
using System.Data;

namespace FSI.SupportPointSystem.Infrastructure.Mappings
{
    public static class SellerMapper
    {
        public static Seller ToDomain(dynamic row)
        {
            if (row == null) return null;
            return new Seller(
                (Guid)row.Id,
                (string)row.Name,
                new Cpf((string)row.Cpf),
                (string)row.Email,
                (string)row.Phone
            );
        }

        public static DynamicParameters ToParameters(Seller seller, string? passwordHash = null)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Id", seller.Id, DbType.Guid);
            parameters.Add("@Name", seller.Name, DbType.String);
            parameters.Add("@Cpf", seller.Cpf.Value, DbType.String);
            parameters.Add("@Email", seller.Email, DbType.String);
            parameters.Add("@Phone", seller.Phone, DbType.String);
            if (!string.IsNullOrEmpty(passwordHash))
            {
                parameters.Add("@PasswordHash", passwordHash, DbType.String);
            }
            return parameters;
        }
    }
}