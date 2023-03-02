using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Xml.Linq;
using UwpSample.Models;

namespace UwpSample.Services
{
    public interface IPostingListsService
    {
        Task<string> GetXmlStringAsync();

        Task<XDocument> GetXDocumentAsync();

        Task<ObservableCollection<PostingList>> GetCollectionAsync();
    }
}
