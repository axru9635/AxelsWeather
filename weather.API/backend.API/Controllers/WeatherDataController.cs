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

        public WeatherDataController(IWebHostEnvironment environment)
        {
            _hostEnvironment = environment;
        }

        // GET: api/<ValuesController>

        [HttpGet("{date}")]
        public async Task<WeatherData> Get(DateTime date)
        {

            dynamic rawWeatherData = await GetWeatherDataFromAPI();

            WeatherData weatherData = extractDesieredSubset(rawWeatherData, date);

            return weatherData;
        }

        async private Task<dynamic> GetWeatherDataFromAPI()
        {
            Console.WriteLine("At the start");
            var useAPI = false;
            var writeRawWeatherJson = false;
            var rawWeatherJsonFileName = "RawWeatherData.json";
            string rawWeatherDataJson;

            if (useAPI == true)
            {
                HttpResponseMessage response = await client.GetAsync("https://api.openweathermap.org/data/3.0/onecall?lat=59.334591&lon=18.063240&appid=bcf857d9581c78ebc75746abe500e4b0");

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

            string mainWeatherTag = rawWeatherData.current.weather[0].main;
            string summary = rawWeatherData.daily[0].summary;

            Console.WriteLine(rawWeatherData.current.temp);
            Console.WriteLine("gust " + rawWeatherData.current.wind_gust);

            var tempKelivn = rawWeatherData.current.temp; // maybe also check that it exsits
            var tempNow = (int)(tempKelivn-273.15);

            var windSpeed = (int)rawWeatherData.current.wind_speed;
            int? windGustSpeed = ( rawWeatherData.current.wind_gust == null ?
                (int?)rawWeatherData.current.wind_gust : null ) ;
            int clouds = rawWeatherData.current.clouds;
            string icon = rawWeatherData.current.weather[0].icon;

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
