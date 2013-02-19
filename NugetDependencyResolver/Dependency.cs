using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetDependencyResolver
{
    public class Dependency
    {
        [Flags]
        public enum DependencyItemType
        {
            NuGet,
            CSProject,
            Solution
        }


        public List<Dependency> HasReferenceTo = new List<Dependency>();
        public List<Dependency> IsReferencedFrom = new List<Dependency>();

        public string Name;
        public string Version;
        public DependencyItemType ItemType;

        public override int GetHashCode()
        {
            return (Name + Version).GetHashCode();
        }

        public static int CreateHashCode(string Name, string Version)
        {
            return (Name + Version).GetHashCode();
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            Dependency other = (Dependency)obj;

            return (other.GetHashCode() == this.GetHashCode());
                       
        }
        
    }
}
