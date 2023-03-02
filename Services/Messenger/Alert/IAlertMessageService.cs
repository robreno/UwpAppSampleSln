using System.Collections.Generic;
using System.Threading.Tasks;

namespace UwpSample.Services
{
    public interface IAlertMessageService
    {
        Task ShowAsync(string message, string title);

        Task ShowAsync(string message, string title, IEnumerable<DialogCommand> dialogCommands);
    }
}