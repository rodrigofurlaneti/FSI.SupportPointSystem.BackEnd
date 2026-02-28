using Dapper;
using FSI.SupportPointSystem.Domain.Entities;
using System;
using System.Data;

namespace FSI.SupportPointSystem.Infrastructure.Mappings
{
    public static class SalesTeamMapper
    {
        public static DynamicParameters ToParameters(SalesTeam team)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_Id", team.Id.ToString());
            parameters.Add("p_Name", team.Name);
            parameters.Add("p_Description", team.Description);
            parameters.Add("p_Active", team.Active ? 1 : 0);

            return parameters;
        }

        public static SalesTeam ToDomain(dynamic row)
        {
            if (row == null) return null;

            var team = (SalesTeam)Activator.CreateInstance(typeof(SalesTeam), true)!;
            Guid teamId = row.Id is Guid guid ? guid : Guid.Parse(row.Id.ToString());
            team.GetType().GetProperty("Id")?.SetValue(team, teamId);
            team.GetType().GetProperty("Name")?.SetValue(team, row.Name);
            team.GetType().GetProperty("Description")?.SetValue(team, row.Description);
            bool isActive = Convert.ToBoolean(row.Active);
            team.GetType().GetProperty("Active")?.SetValue(team, isActive);
            team.GetType().GetProperty("CreatedAt")?.SetValue(team, (DateTime)row.CreatedAt);
            return team;
        }
    }
}