using System.Threading.Tasks;

namespace UwpSample.Services.Logging
{
    public interface ILoggingService
    {
        Task Log(string message);
    }
}
