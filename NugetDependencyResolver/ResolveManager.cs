using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NugetDependencyResolver.model;

namespace NugetDependencyResolver
{
    public class ResolveManager
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private List<string> _list;
        private string _package;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="package"></param>
        public ResolveManager(string package)
        {
            _package = package;
        }

        #region public

        /// <summary>
        /// Traverse NugetPackages
        /// </summary>
        /// <param name="startDir"></param>
        /// <returns></returns>
        public PackageDto[] Resolve(string startDir)
        {
            List<PackageDto> myList = new List<PackageDto>();

            var res = DirSearch(startDir);

            foreach (string line in res)
            {
                string path1 = System.IO.Path.Combine(line);
                var pack = new NugetPackagesXml(path1);
                var dto = pack.ExistPackage(_package);

                if (dto != null) {
                    myList.Add(dto);
                }
            }

            return myList.ToArray();
        }

        /// <summary>
        /// Read Dir recursive
        /// </summary>
        /// <param name="startDir"></param>
        /// <returns></returns>
        public string[] DirSearch(string startDir)
        {
            _list = new List<string>();

            var pattern = "packages.config";

            try
            {
                foreach (string d in Directory.GetDirectories(startDir))
                {

                    foreach (string f in Directory.GetFiles(d, pattern))
                    {
                        _list.Add(f);
                        //Console.WriteLine("File found: {0}", f);
                    }

                    //DirSearch(d);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something unexpected happened: {0}", ex);
            }

            return _list.ToArray();
        }


        /// <summary>
        /// read the xml configuration
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A list of Directories to be traversed</returns>
        public string[] ReadConfigDir(string path)
        {
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "conf\\SourceDefinition.xml");
            var xml = new SourceDefXml(path);

            var dir = xml.GetAllPath();
            return dir;
        }

        #endregion

        #region private
        #endregion
    }
}
