using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetDependencyResolver.model
{
    public class DependencyDto
    {
        public string From;

        public PackageDto FromPkg;

        public string Via;

        public PackageDto To;



        public bool IsNuget
        {
            get
            {
                return FromPkg != null;
            }
        }
        
    }
}
