namespace FSI.SupportPointSystem.Domain.ValueObjects
{
    public record Coordinates
    {
        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }

        public Coordinates(decimal latitude, decimal longitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new ArgumentException("Latitude inválida.");
            if (longitude < -180 || longitude > 180)
                throw new ArgumentException("Longitude inválida.");

            Latitude = latitude;
            Longitude = longitude;
        }
    }
}