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

            // Ajuste 1: Conversão de ID (String do MySQL para Guid do C#)
            // Como no MySQL usamos CHAR(36), o Dapper retorna uma string.
            Guid sellerId = row.Id is Guid guid ? guid : Guid.Parse(row.Id.ToString());

            return new Seller(
                sellerId,
                (string)row.Name,
                new Cpf((string)row.Cpf),
                (string)row.Email,
                (string)row.Phone
            );
        }

        public static DynamicParameters ToParameters(Seller seller, string? passwordHash = null)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_Id", seller.Id.ToString());
            parameters.Add("p_Name", seller.Name);
            parameters.Add("p_Cpf", seller.Cpf.Value);
            parameters.Add("p_Email", seller.Email);
            parameters.Add("p_Phone", seller.Phone);

            if (!string.IsNullOrEmpty(passwordHash))
            {
                parameters.Add("p_PasswordHash", passwordHash);
            }

            return parameters;
        }
    }
}