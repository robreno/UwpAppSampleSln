using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSample.Services
{
    public interface IResourceLoaderService
    {
        /// <summary>
        /// Gets the value of the named resource.
        /// </summary>
        /// <param name="resource">The resource name.</param>
        /// <returns>The named resource value.</returns>
        public string GetString(string resource);
    }
}
