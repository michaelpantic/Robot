using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NugetDependencyResolver.model
{
    public class CSProject : NugetItem
    {

        private readonly string _projectPath;

        public CSProject(string path):base(path)
        {
            this._projectPath = path;


            if (this.GetDefinedPackage() != null)
            {
                throw new Exception("CSProject defines a package!");
            }
        }

        public string Name
        {
            get
            {
                string solutionFile = Directory.GetFiles(this._projectPath, "*.csproj", SearchOption.TopDirectoryOnly).First();
                return Path.GetFileNameWithoutExtension(solutionFile);
            }
        }

    }
}
