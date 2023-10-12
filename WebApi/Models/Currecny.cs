namespace WebApi.Models
{
    public class Currecny
    {

    public string Result { get; set; }


        public string Base_code { get; set; }

       

        public Dictionary<string, double> Conversion_rates{ get; set; }

        public double amt {  get; set; }    

    }
}
