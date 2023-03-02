using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Xml.Linq;
using UwpSample.Dtos;

namespace UwpSample.Services
{
    public interface IStoryService
    {
        Task<string> GetXmlStringAsync();

        Task<XDocument> GetXDocumentAsync();

        Task<ObservableCollection<StoryDto>> GetCollectionAsync();
    }
}
