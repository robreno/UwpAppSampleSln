using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using System.Xml.Linq;
using System.Linq;

using UwpSample.Models;

namespace UwpSample.Services
{
    public class TermsService : ITermsService
    {
        public async Task<string> GetXmlStringAsync()
        {
            var installedLocation = Package.Current.InstalledLocation; // \bin\x86\Debug\AppX
            var file = await installedLocation.GetFileAsync("Assets\\XmlData\\StemmedLexicon.xml");
            string xml = await FileIO.ReadTextAsync(file);
            return xml;
        }

        public async Task<XDocument> GetXDocumentAsync()
        {
            var installedLocation = Package.Current.InstalledLocation; // \bin\x86\Debug\AppX
            var file = await installedLocation.GetFileAsync("Assets\\XmlData\\StemmedLexicon.xml");
            string xml = await FileIO.ReadTextAsync(file);
            var xdoc = XDocument.Parse(xml);
            return xdoc;
        }
        public async Task<ObservableCollection<Term>> GetCollectionAsync()
        {
            var installedLocation = Package.Current.InstalledLocation; // \bin\x86\Debug\AppX
            var file = await installedLocation.GetFileAsync("Assets\\XmlData\\StemmedLexicon.xml");
            string xml = await FileIO.ReadTextAsync(file);
            var xdoc = XDocument.Parse(xml);
            var root = xdoc.Root;
            var tokens = root.Descendants("Token");
            var terms = new ObservableCollection<Term>();

            foreach (var token in tokens)
            {
                Term newTerm = new Term()
                {
                    TokenID = Int32.Parse(token.Attribute("tokenId").Value),
                    StableID = token.Attribute("stableId").Value,
                    Lexeme= token.Attribute("lexeme").Value,
                    Stemmed = token.Attribute("stemmed").Value
                };
                terms.Add(newTerm);
            }
            return terms;
        }
    }
}
