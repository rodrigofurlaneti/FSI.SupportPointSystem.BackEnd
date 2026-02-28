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

            // Ajuste 1: Conversão defensiva de Decimal. 
            // O MySQL pode retornar tipos numéricos que o Convert.ToDecimal lida melhor que o cast direto.
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

            // Ajuste 2: Conversão de ID (Guid).
            // No MySQL CHAR(36), o Dapper pode entregar uma string. 
            // Usamos Guid.Parse para garantir que o domínio receba o tipo correto.
            Guid customerId = row.Id is Guid guid ? guid : Guid.Parse(row.Id.ToString());

            return new Customer(
                customerId,
                (string)row.CompanyName,
                new Cnpj((string)row.Cnpj),
                address,
                location
            );
        }

        public static DynamicParameters ToParameters(Customer customer)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_Id", customer.Id.ToString());
            parameters.Add("p_CompanyName", customer.CompanyName);
            parameters.Add("p_Cnpj", customer.Cnpj.Value);
            parameters.Add("p_ZipCode", customer.Address.ZipCode);
            parameters.Add("p_Street", customer.Address.Street);
            parameters.Add("p_Number", customer.Address.Number);
            parameters.Add("p_Complement", customer.Address.Complement);
            parameters.Add("p_Neighborhood", customer.Address.Neighborhood);
            parameters.Add("p_City", customer.Address.City);
            parameters.Add("p_State", customer.Address.State);
            parameters.Add("p_LatTarget", customer.LocationTarget.Latitude);
            parameters.Add("p_LogTarget", customer.LocationTarget.Longitude);

            return parameters;
        }
    }
}