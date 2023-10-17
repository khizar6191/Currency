using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using WebApi.Models;
using WebApi.Services;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly IRepo _log;
        private IMemoryCache _cache;

        public CurrencyController(IRepo log, IMemoryCache Imem) {
            _cache = Imem;//caching injection
            _log = log;//ilogger injection
        }

        [HttpGet]
        public async Task<IActionResult> Get(string From, string To, double Amount)
        {
            FileStream fs = new FileStream("C:\\Users\\User\\source\\repos\\Currency\\WebApi\\log.txt", FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);//creating object for wrrting in file
            if (!_cache.TryGetValue("currencies", out Currecny data))//checking if the recived data is in cache or not if not means it new request
                    {
                        using (var _client = new HttpClient())//creating object to perform http operations
                        {
                            _client.BaseAddress = new Uri("https://v6.exchangerate-api.com/");
                            using (HttpResponseMessage response = await _client.GetAsync($"v6/bafa7500d6a917cc848033e3/latest/{From}"))//calling the uri
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    var json = await response.Content.ReadAsStringAsync();
                                    data = JsonConvert.DeserializeObject<Currecny>(json);//deserializing the dataaaa from json

                            _log.Ilog("the type of request send " + response.RequestMessage.Method + " on api " + response.RequestMessage.RequestUri);//console based logging
                            _cache.Set("currencies", data, TimeSpan.FromMinutes(10));//setting cahce
                                }
                                else
                                {

                                    return BadRequest();
                                }
                            }
                        }

                    }

            double amt = 0;
            if (data.Conversion_rates.ContainsKey(To))
            {
                double exchangeRate = data.Conversion_rates[To];
                amt = exchangeRate * Amount;
            }
            data.amt = Math.Round(amt, 2);
            _log.Ilog("the recive response " + From + " of amount " + Amount + " to " + To + " " + data.amt);
            //file based logging
            sw.WriteLine("the recive response " + From + " of amount " + Amount + " to " + To + " " + data.amt+ "\n");
                               sw.Close();
                               fs.Close();

            
        
            return Ok(data);
        }


            }
}
