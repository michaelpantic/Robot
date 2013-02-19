using NugetDependencyResolver.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetDependencyResolver
{
    public class DependencyResolver
    {
        public List<DependencyDto> dependencies = new List<DependencyDto>();


        public Dictionary<int, Dependency> dependencyTree = new Dictionary<int, Dependency>();

        public DependencyResolver(string[] startPaths)
        {
            foreach (string s in startPaths)
            {
                var solutions = Solution.GetSolutions(s);
                foreach (Solution solution in solutions)
                {

                    dependencies.AddRange(solution.GetDependencies());
                }
            }


          
        }

        private Dependency GetOrCreateItemInTree(string name, string version, bool nuget)
        {

            // Get From Item
            if (dependencyTree.ContainsKey(Dependency.CreateHashCode(name, version)))
            {
                return dependencyTree[Dependency.CreateHashCode(name, version)];
            }
            else
            {
                Dependency dep = new Dependency();
                dep.Name = name;
                dep.Version = version;

                if (nuget)
                {
                    dep.ItemType = Dependency.DependencyItemType.NuGet;
                }
                dependencyTree.Add(dep.GetHashCode(), dep);

                return dep;
            }



        }

        public void BuildTree()
        {
            foreach (DependencyDto dto in dependencies)
            { 
                Dependency from; Dependency to;
                string fromName = dto.From;
                string fromVersion = "";
                bool fromNuget = false;
                if(dto.FromPkg != null)
                {  
                    fromName = dto.FromPkg.Id;
                    fromVersion = dto.FromPkg.Version;
                    fromNuget = true;
                }

                string toName = dto.To.Id;
                string toVersion = dto.To.Version;


                from = GetOrCreateItemInTree(fromName, fromVersion, fromNuget);
                to = GetOrCreateItemInTree(toName, toVersion, true);

                from.HasReferenceTo.Add(to);
                to.IsReferencedFrom.Add(from);
                
            }
        
        }


        public void DrawDependencies()
        { 
            
        
        
        }

    }
}
