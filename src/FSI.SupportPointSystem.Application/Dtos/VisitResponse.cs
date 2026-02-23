namespace FSI.SupportPointSystem.Application.Dtos
{
    public record VisitResponse
    {
        public Guid VisitId { get; init; }
        public string Message { get; init; } = string.Empty;
        public double DistanceMeters { get; init; }
        public DateTime Timestamp { get; init; }
    }
}