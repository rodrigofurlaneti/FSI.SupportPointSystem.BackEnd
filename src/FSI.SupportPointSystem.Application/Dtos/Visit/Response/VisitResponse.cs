namespace FSI.SupportPointSystem.Application.Dtos.Visit.Response
{
    public record VisitResponse
    {
        public Guid VisitId { get; init; }
        public string Message { get; init; } = string.Empty;
        public double DistanceMeters { get; init; }
        public DateTime Timestamp { get; init; }
    }
}