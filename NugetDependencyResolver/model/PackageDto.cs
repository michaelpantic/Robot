using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetDependencyResolver.model
{
    public class PackageDto
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string Target { get; set; }
        public string Path { get; set; }
    }
}
