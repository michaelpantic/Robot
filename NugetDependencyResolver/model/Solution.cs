using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NugetDependencyResolver.model
{
    public class Solution : NugetItem
    {
        private List<CSProject> projects = new List<CSProject>();

        private readonly string _solutionPath;

        public static IEnumerable<Solution> GetSolutions(string path)
        {
            var solDirectories = Directory.GetFiles(path, "*.sln", SearchOption.AllDirectories).Select(x => Path.GetDirectoryName(x));

            List<Solution> solutions = new List<Solution>();

            foreach (string solDirectory in solDirectories)
            {
                Solution sol = new Solution(solDirectory);
                solutions.Add(sol);
            }

            return solutions;
        
        }



        public Solution(string path) :base(path)
        {
            _solutionPath = path;
            projects = new List<CSProject>();
            LoadProjects();
        }


        public IEnumerable<DependencyDto> GetDependencies()
        {
            List<DependencyDto> dependencies = new List<DependencyDto>();

            foreach (CSProject proj in projects)
            {
                if (proj.GetUsedPackages() != null)
                {
                    foreach (PackageDto package in proj.GetUsedPackages())
                    {
                        DependencyDto dependency = new DependencyDto();
                        dependency.To = package;
                        dependency.Via = this.Name + "/" + proj.Name;
                        dependency.From = this.Name;
                        dependency.FromPkg = this.GetDefinedPackage();

                        if (dependency.FromPkg == null)
                        {
                            dependency.FromPkg = new PackageDto();
                            dependency.FromPkg.Id = this.Name;
                            dependency.FromPkg.Version = "N/A";
                            dependency.FromPkg.Nuget = false;
                        }

                        dependencies.Add(dependency);
                    
                    }                
                }

            
            }


            return dependencies;
        }

        public string Name
        {
            get {
                    string solutionFile = Directory.GetFiles(this._solutionPath, "*.sln", SearchOption.TopDirectoryOnly).First();
                    return Path.GetFileNameWithoutExtension(solutionFile);
                }
        }

        private void LoadProjects()
        { 
           var csprojDirectories = Directory.GetFiles(this._solutionPath,"*.csproj", SearchOption.AllDirectories).Select(x=>Path.GetDirectoryName(x));

           foreach (string csprojDirectory in csprojDirectories)
           {
               CSProject proj = new CSProject(csprojDirectory);
               this.projects.Add(proj);
           }
            
        
        }
        
           
        
    }
}
