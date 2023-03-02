using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace UwpSample.Services
{
    /// <summary>
    /// The ResourceLoader class abstracts the Windows.ApplicationModel.Resources.ResourceLoader. 
    /// Adapted from Microsoft.Practices.Prism.StoreApps code.
    /// A ResourceLoader represents a class that reads the assembly resource file and looks for a named resource.
    /// This class simply passes method invocations to an underlying Windows.ApplicationModel.Resources.ResourceLoader object.
    /// </summary>
    public class ResourceLoaderAdapter : IResourceLoaderService
    {
        private readonly ResourceLoader _resourceLoader;

        // <summary>
        /// Initializes a new instance of the <see cref="ResourceLoaderAdapter"/> class.
        /// </summary>
        /// <param name="resourceLoader">The resource loader.</param>
        public ResourceLoaderAdapter(ResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
        }

        /// <summary>
        /// Gets the value of the named resource.
        /// </summary>
        /// <param name="resource">The resource name.</param>
        /// <returns>
        /// The named resource value.
        /// </returns>
        public string GetString(string resource) => _resourceLoader.GetString(resource);
    }
}
