using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using NuGet;

namespace NugetDependencyResolver
{
    public partial class BrowserForm : Form
    {
        public BrowserForm()
        {
            InitializeComponent();
            //ShowBrowser();
            ShowBrowser2();
        }


        private void ShowBrowser()
        {
            var url = "http://retsolsrv051.retsol.infra.local/Sydney/nuget/Packages";
            var rssXml = GetIt(url);

            //Instantiate an XmlNamespaceManager object. 
            var xmlnsManager = new XmlNamespaceManager(rssXml.NameTable);
            //Add the namespaces used in books.xml to the XmlNamespaceManager.
            xmlnsManager.AddNamespace("rss", "http://www.w3.org/2005/Atom");
            //var rssType = rssXml.DocumentElement.Name;
            //var rootXPath = GetRootXPath(artsType);

            var root = (XmlElement) rssXml.SelectSingleNode("//rss:feed", xmlnsManager);
            var nodes = root.SelectNodes("//rss:entry", xmlnsManager);

            StringBuilder sb = new StringBuilder();
            foreach (XmlNode rssNode in nodes)
            {
                XmlNode rssSubNode;
                rssSubNode = rssNode.SelectSingleNode("rss:title", xmlnsManager);
                string title = rssSubNode != null ? rssSubNode.InnerText : "";
                rssSubNode = rssNode.SelectSingleNode("rss:summary", xmlnsManager);
                string link = rssSubNode != null ? rssSubNode.InnerText : "";
                rssSubNode = rssNode.SelectSingleNode("rss:author/rss:name", xmlnsManager);
                string desc = rssSubNode != null ? rssSubNode.InnerText : "";
                rssSubNode = rssNode.SelectSingleNode("rss:updated", xmlnsManager);
                string datrel = rssSubNode != null ? rssSubNode.InnerText : "";

                sb.Append("<font face='arial'><p><b><a href='");
                sb.Append(link);
                sb.Append("'>");
                sb.Append(title);
                sb.Append("</a></b><br/>");
                sb.Append(desc);
                sb.Append("</a></b><br/>");
                sb.Append(datrel);
                sb.Append("</a></b><br/>");
                sb.Append(link);
                sb.Append("</p></font>");
            }

            webBrowser1.Navigate("about:blank");
            HtmlDocument doc = this.webBrowser1.Document;
            doc.Write(String.Empty);

            //browser.DocumentText = _emailHTML
            webBrowser1.DocumentText = sb.ToString();
            //webBrowser1.Show();
        }

        private void ShowBrowser2()
        {
            PackageRepositoryFactory fac = new PackageRepositoryFactory();
            //IPackageRepository packageRepository = fac.CreateRepository(new PackageSource("http://retsolsrv051.retsol.infra.local/Sydney"));
            var packageRepository = fac.CreateRepository("http://retsolsrv051.retsol.infra.local/Sydney/nuget");
            IPackage package = packageRepository.FindPackage("CHBITS.BiersLibs.Common");

            StringBuilder sb = new StringBuilder();
            var packages = packageRepository.GetPackages();
            foreach (IPackage pkg in packages)
            {
                var id = pkg.Id;
                var version = pkg.Version;
                var name = pkg.Title;
                var auth = pkg.Authors;
                var desc = pkg.Description;
                var pub = pkg.Published.ToString();

                sb.Append("<font face='arial'><p><b>");
                sb.Append(id);
                sb.Append("</a></b><br/>");
                sb.Append(version);
                sb.Append("</a></b><br/>");
                sb.Append(desc);
                sb.Append("</a></b><br/>");
                sb.Append(pub);
                sb.Append("</p></font>");

            }

            webBrowser1.Navigate("about:blank");
            HtmlDocument doc = this.webBrowser1.Document;
            doc.Write(String.Empty);
            webBrowser1.DocumentText = sb.ToString();

        }

        private XmlDocument GetIt(string url)
        {
            // Create web client simulating IE6.
            using (WebClient client = new WebClient())
            {
                client.Headers["User-Agent"] =
                "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " +
                "(compatible; MSIE 6.0; Windows NT 5.1; " +
                ".NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                // Download data.
                //byte[] arr = client.DownloadData(url);
                var value = client.DownloadString(url);

                // Write values.
                Console.WriteLine("--- WebClient result ---");
                //Console.WriteLine(arr.Length);

                XmlDocument RSSXml = new XmlDocument();
                //var reader = new XmlReader(arr.ToString());
                RSSXml.LoadXml(value);

                return RSSXml;
            }
            return null;

        }

        private void BrowserForm_Load(object sender, EventArgs e)
        {

        }


    }

}
