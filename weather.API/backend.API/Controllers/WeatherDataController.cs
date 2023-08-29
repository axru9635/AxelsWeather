using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using System.Diagnostics;
//using System.Timers;
//using System.Diagnostics;
//using System.Diagnostics.Metrics;
//using Microsoft.AspNetCore.RateLimiting;



namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WeatherDataController : ControllerBase
    {
        private HttpClient client = new HttpClient();

        private IWebHostEnvironment _hostEnvironment;

        private IDictionary<string, WeatherData> ApiCallCache =  new Dictionary<string, WeatherData>();

        string ApiKey = Environment.GetEnvironmentVariable("API_KEY_OPENWEATHERMAP");


        public WeatherDataController(IWebHostEnvironment environment)
        {
            // Remeber the controller is not created until you call it!!
            _hostEnvironment = environment;
            
        }

        [HttpGet("{city}/{date}")]
        public async Task<WeatherData> Get(string city, DateTime date) // can I return a http request instead?
        {
        
            var ( latitiude, logitude) = await GetGeoCoordsFromAPI(city);

            dynamic rawWeatherData = await GetWeatherDataFromAPI(latitiude, logitude);

            WeatherData weatherData = extractDesieredSubset(rawWeatherData, date);

            return weatherData;
            
        }

        async private Task<(double,double)> GetGeoCoordsFromAPI(string city)
        {

            HttpResponseMessage response = await client.GetAsync($"https://api.openweathermap.org/geo/1.0/direct?q={city},SE&limit=1&appid={ApiKey}");
            var rawCityDataJson = await response.Content.ReadAsStringAsync();

            dynamic rawCityData = JsonConvert.DeserializeObject(rawCityDataJson);

            return ((double)rawCityData[0].lat, (double)rawCityData[0].lon);

        }

        async private Task<dynamic> GetWeatherDataFromAPI(double latitude, double logitude)
        {
            var useAPI = false;
            var writeRawWeatherJson = false;
            var rawWeatherJsonFileName = "RawWeatherData.json";
            string rawWeatherDataJson;

            var token = Environment.GetEnvironmentVariable("API_KEY_OPENWEATHERMAP");

            if (useAPI == true)
            {
                HttpResponseMessage response = await client.GetAsync($"https://api.openweathermap.org/data/3.0/onecall?lat={latitude}&lon={logitude}&appid={token}");

                rawWeatherDataJson = await response.Content.ReadAsStringAsync();

                if (writeRawWeatherJson)
                {
                    System.IO.File.WriteAllText(Path.Combine(_hostEnvironment.WebRootPath, rawWeatherJsonFileName), rawWeatherDataJson);
                }
            }
            else
            {

                rawWeatherDataJson = System.IO.File.ReadAllText(Path.Combine(_hostEnvironment.WebRootPath, rawWeatherJsonFileName));

            }

            
            dynamic rawWeatherData = JsonConvert.DeserializeObject(rawWeatherDataJson);
            
            return rawWeatherData;

        }
        private WeatherData extractDesieredSubset(dynamic rawWeatherData, DateTime dateDesieredForecast)
        { 

            DateTime date = DateTime.Now.Date;
            
            var dailyIndex = (dateDesieredForecast - date).Days;

            string mainWeatherTag = rawWeatherData.daily[dailyIndex].weather[0].main;
            string summary = rawWeatherData.daily[dailyIndex].summary;

            var tempKelivn = rawWeatherData.daily[dailyIndex].temp.day; // maybe also check that it exsits
            var tempNow = (int)(tempKelivn - 273.15);

            var windSpeed = (int)rawWeatherData.daily[dailyIndex].wind_speed;
            int? windGustSpeed = (rawWeatherData.daily[dailyIndex].wind_gust == null ?
                (int?)rawWeatherData.daily[dailyIndex].wind_gust : null);
            int clouds = rawWeatherData.daily[dailyIndex].clouds;
            string icon = rawWeatherData.daily[dailyIndex].weather[0].icon;

            WeatherData weatherData = new WeatherData {
                Date = date,
                MainWeatherTag = mainWeatherTag,
                Summary = summary,
                TempNow = tempNow,
                WindSpeed = windSpeed,
                WindGustSpeed= windGustSpeed,
                Clouds = clouds,
                Icon = icon,
            };

            return weatherData;
        }
        
        
        
    }
}
