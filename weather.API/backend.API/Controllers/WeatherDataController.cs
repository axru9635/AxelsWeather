﻿
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WeatherDataController : ControllerBase
    {
        private HttpClient client = new HttpClient();

        private IWebHostEnvironment _hostEnvironment;

        private IDictionary<string, DateTime> ApiCallCache =  new Dictionary<string,DateTime>();

        string ApiKey = Environment.GetEnvironmentVariable("API_KEY_OPENWEATHERMAP");

        public WeatherDataController(IWebHostEnvironment environment)
        {
            _hostEnvironment = environment;
        }

        [HttpGet("{city}/{date}")]
        public async Task<WeatherData> Get(string city,DateTime date)
        {
            Console.WriteLine($"{city} {date}");
            HttpResponseMessage response = await client.GetAsync($"https://api.openweathermap.org/geo/1.0/direct?q={city},SE&limit=1&appid={ApiKey}");
            var rawCityDataJson = await response.Content.ReadAsStringAsync();

            dynamic rawCityData = JsonConvert.DeserializeObject(rawWeatherDataJson);

            int latitude = rawCityData.lat; int logitude = rawCityData.lon;

            dynamic rawWeatherData = await GetWeatherDataFromAPI(latitude, logitude);

            WeatherData weatherData = extractDesieredSubset(rawWeatherData, date);

            return weatherData;
        }

        async private Task<dynamic> GetWeatherDataFromAPI(int latitude,int logitude)
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

            //DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            //DateTime date = dtDateTime.AddSeconds((double)rawWeatherData.current.dt);

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
        
        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
