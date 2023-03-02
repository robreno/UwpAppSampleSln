using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using System.Xml.Linq;

using UwpSample.Models;

namespace UwpSample.Services
{
    public class DocumentsService : IDocumentsService
    {
        public async Task<string> GetXmlStringAsync()
        {   
            var installedLocation = Package.Current.InstalledLocation; // \bin\x86\Debug\AppX
            var file = await installedLocation.GetFileAsync("Assets\\XmlData\\DocumentList.xml");
            string xml = await FileIO.ReadTextAsync(file);
            return xml;
        }

        public async Task<XDocument> GetXDocumentAsync()
        {
            var installedLocation = Package.Current.InstalledLocation; // \bin\x86\Debug\AppX
            var file = await installedLocation.GetFileAsync("Assets\\XmlData\\DocumentList.xml");
            string xml = await FileIO.ReadTextAsync(file);
            var xdoc = XDocument.Parse(xml);
            return xdoc;
        }

        public async Task<ObservableCollection<Document>> GetCollectionAsync()
        {
            var installedLocation = Package.Current.InstalledLocation; // \bin\x86\Debug\AppX
            var file = await installedLocation.GetFileAsync("Assets\\XmlData\\DocumentList.xml");
            string xml = await FileIO.ReadTextAsync(file);
            var xdoc = XDocument.Parse(xml);
            var root = xdoc.Root;
            var docs = root.Descendants("Document");
            var documents = new ObservableCollection<Document>();

            Document newDoc = null;
            foreach (var doc in docs)
            {
                newDoc = new Document()
                {
                    ID = Int32.Parse(doc.Attribute("id").Value),
                    Title = doc.Attribute("title").Value,
                    ETextID = doc.Attribute("etextId").Value,
                    FileName = doc.Attribute("source").Value,
                    Uri = "ms-appdata:///local/" + doc.Attribute("source").Value
                };
                documents.Add(newDoc);
            }
            return documents;
        }
    }
}
