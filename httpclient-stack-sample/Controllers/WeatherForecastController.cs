using Dasync.Collections;
using Microsoft.AspNetCore.Mvc;
using Sentry;

namespace httpclient_stack_sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        const string baseUri = "https://localhost:7057/weatherforecast";

        public WeatherForecastController(HttpClient httpClient) => _httpClient = httpClient;

        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            var response = await _httpClient.GetAsync($"{baseUri}/GetAnimals");
            return response;
        }

        [HttpGet("GetAnimals")]
        public async Task<IEnumerable<int>> GetDogs()
        {

            List<int> dogs = new List<int>();
            List<Task> waiter = new();
            for (int i = 0; i < 200; i++)
            {
                dogs.Add(i);
            }
            await dogs.ParallelForEachAsync(async dog =>
            {
                await _httpClient.GetAsync($"{baseUri}/GetDog/{dog}");
            }, 6);
                
            return dogs;
        }

        [HttpGet("GetDog/{id}")]
        public async Task<int> GetDog(int id)
        {
            SentrySdk.ConfigureScope(scope => scope.Transaction = null);
            var time = (DateTime.Now.Ticks & 10) + id + 1;
            _ = await _httpClient.GetAsync($"{baseUri}/Wait/{time}");
            return id;
        }

        [HttpGet("Wait/{ms}")]
        public async Task Wait(int ms)
        {
            SentrySdk.ConfigureScope(scope => scope.Transaction = null);
            await Task.Delay(ms);
        }
    }
}