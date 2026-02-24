using Dapper;
using FSI.SupportPointSystem.Domain.Entities;
using System.Data;

namespace FSI.SupportPointSystem.Infrastructure.Mappings
{
    public static class SalesTeamMapper
    {
        public static DynamicParameters ToParameters(SalesTeam team)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", team.Id, DbType.Guid);
            parameters.Add("@Name", team.Name, DbType.String);
            parameters.Add("@Description", team.Description, DbType.String);
            parameters.Add("@Active", team.Active, DbType.Boolean);
            return parameters;
        }

        public static SalesTeam ToDomain(dynamic row)
        {
            var team = (SalesTeam)Activator.CreateInstance(typeof(SalesTeam), true)!;
            team.GetType().GetProperty("Id")?.SetValue(team, row.Id);
            team.GetType().GetProperty("Name")?.SetValue(team, row.Name);
            team.GetType().GetProperty("Description")?.SetValue(team, row.Description);
            team.GetType().GetProperty("Active")?.SetValue(team, (bool)row.Active);
            team.GetType().GetProperty("CreatedAt")?.SetValue(team, row.CreatedAt);

            return team;
        }
    }
}