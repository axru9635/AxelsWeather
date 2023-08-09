using System.Security.Principal;

namespace backend.API
{
    public record WeatherData
    {
        // Everything is daily for starters

        public DateTime Date { get; set; }

        public string MainWeatherTag { get; set; }

        public string Summary { get; set; }
      
        public int TempNow { get; set; }

        public int WindSpeed { get; set; }

        public int? WindGustSpeed { get; set; }
        
        public int Clouds { get; set; }

        public string Icon { get; set; }

    }
}
