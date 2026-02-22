namespace FSI.SupportPointSystem.Domain.ValueObjects
{
    public record Coordinates(decimal Latitude, decimal Longitude)
    {
        public void Validate()
        {
            if (Latitude < -90 || Latitude > 90)
                throw new ArgumentException("Latitude inválida.");
            if (Longitude < -180 || Longitude > 180)
                throw new ArgumentException("Longitude inválida.");
        }
    }
}
