using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using WebApi.Models;
using WebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
       private readonly IRepo _log;
        private IMemoryCache _cache;

        public CurrencyController(IRepo log,IMemoryCache Imem) {
        _cache = Imem;
        _log = log;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string From, string To, double Amount)
        {
            if (!_cache.TryGetValue("currencies", out Currecny data))
            {
                using (var _client = new HttpClient())
                {
                    _client.BaseAddress = new Uri("https://v6.exchangerate-api.com/");
                    using (HttpResponseMessage response = await _client.GetAsync($"v6/bafa7500d6a917cc848033e3/latest/{From}"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            data = JsonConvert.DeserializeObject<Currecny>(json);//deserializing the dataaaa

                           
                            _cache.Set("currencies", data,  TimeSpan.FromMinutes(10));
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
            return Ok(data);
        }


        //implemented ilogger and filebased logging
        //public async Task<IActionResult> Get(string From,String To,double Amount )
        //{

        //    using (FileStream fs = new FileStream("C:\\Users\\User\\source\\repos\\Currency\\WebApi\\log.txt", FileMode.Append, FileAccess.Write))
        //    {
        //        StreamWriter sw = new StreamWriter(fs);

        //        using (var _client = new HttpClient())
        //        {
        //            _client.BaseAddress = new Uri("https://v6.exchangerate-api.com/");
        //            using (HttpResponseMessage response = await _client.GetAsync($"v6/bafa7500d6a917cc848033e3/latest/{From}"))
        //            {
        //                string cacheEntry="";
        //                
        //                _log.Ilog("the type of request send " + response.RequestMessage.Method + " on api " + response.RequestMessage.RequestUri);
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    var json = await response.Content.ReadAsStringAsync();
        //                    var data = JsonConvert.DeserializeObject<Currecny>(json);
        //                    double amt = 0;

        //                    if (data.Conversion_rates.ContainsKey(To))
        //                    {
        //                        double exchangeRate = data.Conversion_rates[To];
        //                        amt = exchangeRate * Amount;
        //                    }


        //                    data.amt = Math.Round(amt, 2);
        //                    _log.Ilog("the recive response " + From + " of amount " + Amount + " to " + To + " " + data.amt);
        //                    sw.WriteLine("the recive response " + From + " of amount " + Amount + " to " + To + " " + data.amt+ "\n");
        //                    sw.Close();
        //                    fs.Close();
        //                    data.Conversion_rates = null;
        //                    return Ok(data);
        //                }
        //                else
        //                {
        //                    // Handle API error responses
        //                    return BadRequest();
        //                }
        //            }
        //        }
        //    }
        //}

            }
}
