using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
namespace NugetDependencyResolver.model
{
    public class NugetItem
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string includePackagesFile = "packages.config";
        private const string definePackagesFile = "nuget.nuspec";

        private readonly string _itemBasePath;
        private readonly string _nugetDefinitionPath;
        private readonly string _nugetIncludePath;


        private PackageDto _definedPackage;
        private List<PackageDto> _usedPackages;

        public NugetItem(string path)
        {
            _itemBasePath = path;
            _nugetDefinitionPath = Path.Combine(_itemBasePath, definePackagesFile);
            _nugetIncludePath = Path.Combine(_itemBasePath, includePackagesFile);
            _usedPackages = new List<PackageDto>();

            readDefinedPackages();
            readUsedPackages();


        }

        #region public
        /// <summary>
        /// Returns the Package Defined by this item
        /// </summary>
        /// <returns>The defined package or zero</returns>
        public PackageDto GetDefinedPackage()
        {
            return _definedPackage;
        }
    
        /// <summary>
        /// Returns all packages referenced by this item
        /// </summary>
        /// <returns>The referenced packages or an empty array</returns>
        public IEnumerable<PackageDto> GetUsedPackages()
        {
            return _usedPackages.ToArray();
        }


        #endregion

        #region private

        private void readDefinedPackages()
        {
            if (File.Exists(_nugetDefinitionPath))
            {
                var reader = new XmlTextReader(_nugetDefinitionPath);
                XmlDocument _configFile = new XmlDocument();
                _configFile.Load(reader);
                XmlNamespaceManager nm = new XmlNamespaceManager(_configFile.NameTable);
                nm.AddNamespace("nuspec", "http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd");


                var root = (XmlElement)_configFile.SelectSingleNode("/nuspec:package/nuspec:metadata", nm);
                if (root == null) return;

                var dto = new PackageDto();
                dto.Id = root.SelectSingleNode("nuspec:id", nm).InnerText;
                dto.Version = root.SelectSingleNode("nuspec:version", nm).InnerText;
                dto.Target = "N/A";
                dto.Path = this._nugetDefinitionPath;
                _definedPackage = dto;
            
            }
        }

        private void readUsedPackages()
        { 
            if(File.Exists(_nugetIncludePath))
            {

                var reader = new XmlTextReader(_nugetIncludePath);
                XmlDocument _configFile = new XmlDocument();
                _configFile.Load(reader);

                XmlNodeList items = _configFile.SelectNodes("//packages/package");
                foreach(XmlElement elem in items.OfType<XmlElement>())
                {
                    var dto = new PackageDto();
                    dto.Id = elem.GetAttribute("id");
                    dto.Version = elem.GetAttribute("version");
                    dto.Target = elem.GetAttribute("targetFramework");
                    dto.Path = this._nugetIncludePath;
                    _usedPackages.Add(dto);
                }

            
            }
        }

     

        #endregion

    }
}
