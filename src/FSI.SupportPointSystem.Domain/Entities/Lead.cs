namespace FSI.SupportPointSystem.Domain.Entities
{
    public class Lead
    {
        public Guid Id { get; private set; }
        public string CompanyName { get; private set; }
        public string? ContactName { get; private set; }
        public string? Phone { get; private set; }
        public string? Address { get; private set; }
        public string? OpportunityDescription { get; private set; }
        public string Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        protected Lead() { }

        public Lead(string companyName, string? contactName, string? phone, string? address, string? opportunityDescription)
        {
            Id = Guid.NewGuid();
            CompanyName = companyName;
            ContactName = contactName;
            Phone = phone;
            Address = address;
            OpportunityDescription = opportunityDescription;
            Status = "AVAILABLE";
            CreatedAt = DateTime.Now;
        }
    }
}