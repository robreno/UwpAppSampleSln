using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using System.Xml.Linq;

using UwpSample.Dtos;
using System.Collections.Generic;

namespace UwpSample.Services
{
    public class StoryService : IStoryService
    {
        private readonly string uri = "Assets\\StoryData\\Stories.xml";

        public async Task<string> GetXmlStringAsync()
        {   
            var installedLocation = Package.Current.InstalledLocation; // \bin\x86\Debug\AppX
            var file = await installedLocation.GetFileAsync(uri);
            string xml = await FileIO.ReadTextAsync(file);
            return xml;
        }

        public async Task<XDocument> GetXDocumentAsync()
        {
            var installedLocation = Package.Current.InstalledLocation; // \bin\x86\Debug\AppX
            var file = await installedLocation.GetFileAsync(uri);
            string xml = await FileIO.ReadTextAsync(file);
            var xdoc = XDocument.Parse(xml);
            return xdoc;
        }

        public async Task<ObservableCollection<StoryDto>> GetCollectionAsync()
        {
            var installedLocation = Package.Current.InstalledLocation; // \bin\x86\Debug\AppX
            var file = await installedLocation.GetFileAsync(uri);
            string xml = await FileIO.ReadTextAsync(file);
            var xdoc = XDocument.Parse(xml);
            var root = xdoc.Root;
            var storyElements = root.Descendants("Story");
            var stories = new ObservableCollection<StoryDto>();

            StoryDto newStory = null;
            foreach (var story in storyElements)
            {
                newStory = new StoryDto()
                {
                    ID = Int32.Parse(story.Attribute("storyId").Value),
                    Name = story.Attribute("name").Value,
                    Title = story.Attribute("title").Value,
                };
                stories.Add(newStory);
            }
            return stories;
        }
    }
}
