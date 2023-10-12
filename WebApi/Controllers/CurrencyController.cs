using Microsoft.AspNetCore.Mvc;
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
        IRepo _log;
        public CurrencyController(IRepo log) { 
        _log = log;
        }
        [HttpGet]
        public async Task<IActionResult> Get(string From,String To,double Amount )
        {
            using (var _client = new HttpClient())
            {
                _client.BaseAddress = new Uri("https://v6.exchangerate-api.com/");
                using (HttpResponseMessage response = await _client.GetAsync($"v6/bafa7500d6a917cc848033e3/latest/{From}"))
                {
                    _log.Ilog("the type of request send "+response.RequestMessage.Method+" on api "+response.RequestMessage.RequestUri);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<Currecny>(json);
                        double amt=0;
                       
                        if (data.Conversion_rates.ContainsKey(To))
                        {
                            double exchangeRate = data.Conversion_rates[To];    
                            amt = exchangeRate*Amount;
                        }
                        
                       
                        data.amt = Math.Round(amt,2);
                        _log.Ilog("the recive response "+From + " of amount " + Amount + " to "+To+" "+ data.amt);
                        data.Conversion_rates = null;
                        return Ok(data);
                    }
                    else
                    {
                        // Handle API error responses
                        return BadRequest();
                    }
                }
            }
        }

        //public IActionResult GetApi()
        //{
        //    using (var _client = new HttpClient())
        //    {
        //        _client.BaseAddress = new Uri("http://data.fixer.io/");
        //        using (HttpResponseMessage response = _client.GetAsync("api/latest?access_key=b691f5f8b1416727f7598f582f8ea77b&symbols=USD,AUD,CAD,PLN,MXN").Result)
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var json = response.Content.ReadAsStringAsync().Result;//here result is used to block the current thread to block and wait till the operation is complete
        //                var data = JsonConvert.DeserializeObject<Currecny>(json);
        //                return Ok(data);
        //            }
        //            else
        //            {
        //                // Handle API error responses
        //                return StatusCode((int)response.StatusCode, "Failed to fetch data from the API.");
        //            }
        //        }
        //    }
        //}
        //working code
        //public async Task<IActionResult> Get()
        //{
        //    using (var _client = new HttpClient())
        //    {
        //        _client.BaseAddress = new Uri("http://data.fixer.io/");
        //        using (HttpResponseMessage response =await _client.GetAsync("api/latest?access_key=b691f5f8b1416727f7598f582f8ea77b&symbols=USD,AUD,CAD,PLN,MXN")) 
        //        {
        //            var res = response.Content.ReadAsStringAsync().Result;
        //            response.EnsureSuccessStatusCode();
        //            return Ok(res);
        //        }
        //    }
        //}



        //public List<Currecny> Get()
        //{
        //    List<Currecny> list = new List<Currecny>();
        //    HttpResponseMessage response = _client.GetAsync(baseAddress).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string data = response.Content.ReadAsStringAsync().Result;
        //        list = JsonConvert.DeserializeObject<List<Currecny>>(data);
        //    }
        //    return list;
        //}

    }
}
