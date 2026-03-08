namespace FSI.SupportPointSystem.Application.Dtos.Lead.Response
{
    public class LeadResponse
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string? ContactName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? OpportunityDescription { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}