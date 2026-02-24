using System;

namespace FSI.SupportPointSystem.Application.Dtos.SalesTeam.Response
{
    public class SalesTeamResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}