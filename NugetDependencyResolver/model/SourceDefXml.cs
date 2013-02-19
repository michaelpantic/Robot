using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NugetDependencyResolver.model
{
    public class SourceDefXml
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Uri _paPath;

        private const string PACKAGES_NAME = "\\SourceDefinition.xml";

        #region public

        /// <summary>
        /// Constructor
        /// </summary>
        public SourceDefXml(string path)
        {
            //_paPath = new Uri(path + PACKAGES_NAME);
            _paPath = new Uri(path); // new Uri(path + PACKAGES_NAME);
        }


        public string[] GetAllPath()
        {
            List<string> list = new List<string>();

            if (System.IO.File.Exists(_paPath.LocalPath))
            {
                var reader = new XmlTextReader(_paPath.LocalPath);
                //readFile(reader);

                XmlDocument _configFile = new XmlDocument();
                _configFile.Load(reader);

                var root = (XmlElement)_configFile.SelectSingleNode("//SourceDefinitions/Packages");
                var nodes = _configFile.SelectNodes("//TeamProject");
                if (nodes == null) return list.ToArray();

                foreach (XmlElement node in nodes)
                {
                    var path = node.GetAttribute("Base");
                    var name = node.GetAttribute("Name");

                    var innerNodes = node.SelectNodes("Directory");
                    foreach (XmlElement innerNode in innerNodes)
                    {
                        var x = Path.Combine(path, innerNode.InnerText, "src");
                        var uri = new Uri(x);

                        list.Add(uri.LocalPath);
                    }
                }
            }
            else
            {
                _log.ErrorFormat("Could not find file '{0}'! ", _paPath.LocalPath);
            }

            return list.ToArray();
        }

        //public void UpdateValues(PosBusinessValuesDto dto, string posnbr)
        //{
        //    if (System.IO.File.Exists(_paPath.LocalPath))
        //    {
        //        XmlDocument _configFile = new XmlDocument();
        //        _configFile.Load(_paPath.LocalPath);

        //        var root = (XmlElement)_configFile.SelectSingleNode("//POSData");
        //        var nodes = _configFile.SelectNodes("//POSData");
        //        if (nodes == null) return;

        //        foreach (XmlElement node in nodes)
        //        {
        //            //updateNode(node, "GT", dto.GrandTotal.ToString());
        //            updateNode(node, "GTPrv", dto.PreviousGrandTotal.ToString());
        //            updateNode(node, "NextBillNumber", (dto.BillNbr + 1).ToString());
        //            updateNode(node, "WorkDayCounter", dto.WorkDayCounter.ToString());
        //            updateNode(node, "ZCounter", dto.ZCounter.ToString());
        //            updateNode(node, "SessionNumber", dto.SessionNbr.ToString());
        //            updateNode(node, "LastSession", dto.SessionNbr.ToString());
        //            updateNode(node, "POSNumber", posnbr);
        //        }
        //        try
        //        {
        //            _configFile.Save(_paPath.LocalPath);
        //        }
        //        catch (Exception e) 
        //        {
        //            _log.ErrorFormat("Could not save file {0}!", _paPath.LocalPath);
        //        }

        //    }
        //    else
        //    {
        //        _log.ErrorFormat("Could not update file '{0}'! Please initialize the Profitage POS (start once)!", _paPath.LocalPath);
        //    }

        //}

        /// <summary>
        /// Does the file exist
        /// </summary>
        /// <returns></returns>
        public bool FileExist()
        {
            return (System.IO.File.Exists(_paPath.LocalPath));
        }

        #endregion

        #region private


        private static int GetNodeValueInt(XmlElement node, string keyvalue)
        {
            var element = node.SelectSingleNode(keyvalue);
            if (element != null)
            {
                return Convert.ToInt32(element.InnerText);
            }

            return -1;
        }

        private static bool GetNodeValueBool(XmlElement node, string keyvalue)
        {
            var element = node.SelectSingleNode(keyvalue);
            if (element != null)
            {
                return Convert.ToBoolean(element.InnerText);
            }

            return false;
        }

        private static string GetNodeValue(XmlElement node, string keyvalue)
        {
            var element = node.SelectSingleNode(keyvalue);
            if (element != null)
            {
                return element.InnerText;
            }

            return "";
        }

        private void updateNode(XmlElement node, string keyvalue, string value)
        {
            var element = node.SelectSingleNode(keyvalue);
            if (element != null)
            {
                element.InnerText = value;
            }
        }

        #endregion

    }
}
