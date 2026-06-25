namespace Neighborhood.Services.Application.Shared
{
    public class GeocodingResultDto
    {
        public string FormattedAddress { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Structured place fields from reverse-geocoding (any may be null). The formatted line can
        // omit the city at neighborhood-level coordinates, so callers that need the city read these.
        public string? City { get; set; }
        public string? County { get; set; }
        public string? State { get; set; }
    }
}
