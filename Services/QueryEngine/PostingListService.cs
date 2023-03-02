using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using System.Xml.Linq;

using UwpSample.Models;

namespace UwpSample.Services
{
    public class PostingListsService : IPostingListsService
    {
        public async Task<string> GetXmlStringAsync()
        {
            var installedLocation = Package.Current.InstalledLocation; // \bin\x86\Debug\AppX
            var file = await installedLocation.GetFileAsync("Assets\\XmlData\\PostingLists.xml");
            string xml = await FileIO.ReadTextAsync(file);
            return xml;
        }
        public async Task<XDocument> GetXDocumentAsync()
        {
            var installedLocation = Package.Current.InstalledLocation; // \bin\x86\Debug\AppX
            var file = await installedLocation.GetFileAsync("Assets\\XmlData\\PostingLists.xml");
            string xml = await FileIO.ReadTextAsync(file);
            var xdoc = XDocument.Parse(xml);
            return xdoc;
        }
        public async Task<ObservableCollection<PostingList>> GetCollectionAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(3)).ConfigureAwait(false);

            var installedLocation = Package.Current.InstalledLocation; // \bin\x86\Debug\AppX
            var file = await installedLocation.GetFileAsync("Assets\\XmlData\\PostingLists.xml");
            string xml = await FileIO.ReadTextAsync(file);
            var xdoc = XDocument.Parse(xml);
            var root = xdoc.Root;
            var plists = root.Descendants("PostingList");
            var postingLists = new ObservableCollection<PostingList>();

            PostingList tocpl = null;
            foreach (var pl in plists)
            {
                tocpl = new PostingList()
                {
                    TokenID = Int32.Parse(pl.Attribute("tokenId").Value),
                    StableID = pl.Attribute("stableId").Value,
                    Lexeme = pl.Attribute("lexeme").Value,
                    TokenOccurrences = new List<TokenOccurrence>()
                };
                
                var occs = pl.Descendants("TokenOccurrence");
                TokenOccurrence toc = null;
                foreach (var occ in occs)
                {
                    toc = new TokenOccurrence()
                    {
                        DocumentID = Int32.Parse(occ.Attribute("did").Value),
                        SequenceID = Int32.Parse(occ.Attribute("sid").Value),
                        DocumentPosition = Int32.Parse(occ.Attribute("dpo").Value),
                        TermPosition = Int32.Parse(occ.Attribute("tpo").Value),
                        ParagraphID = occ.Attribute("pid").Value
                    };
                    tocpl.TokenOccurrences.Add(toc);
                }
                postingLists.Add(tocpl);
            }
            return postingLists;
        }
    }
}
