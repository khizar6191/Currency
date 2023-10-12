namespace WebApi.Services
{
    public class ConsoleLogger : IRepo
    {
        public void Ilog(string message)
        {
           Console.WriteLine(message);
           Console.WriteLine(message);
        }
    }
}
