using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using NuGet;
using NugetDependencyResolver.model;

namespace NugetDependencyResolver
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            var path = Path.Combine(Directory.GetCurrentDirectory(), @"conf\SourceDefinition.xml");
            textBox3.Text = path;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int icount = 0;
            //listBox1.Items.Clear();
            listView1.Items.Clear();
            listView1.View = View.Details;

            var man = new ResolveManager(textBox1.Text);
            var dirAll = man.ReadConfigDir(textBox3.Text);
            foreach (string dir in dirAll)
            {
                var result = man.Resolve(dir);

                foreach (PackageDto line in result)
                {
                    icount++;
                    //listBox1.Items.Add(line.Path);
                    listView1.Items.Add(new ListViewItem(new string[] { line.Path, line.Version, line.Target } ));
                }

                label3.Text = "" + icount;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "conf");
            openFileDialog1.InitialDirectory = path;
            openFileDialog1.Filter = "Xml Configuration |*.xml";

            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                textBox3.Text = openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new BrowserForm();
                form.ShowDialog(this);

                //RSSBrowser.DocumentText = sb.ToString();                
                //webBrowser1.Navigate(new Uri(url));
            }
            catch (System.UriFormatException)
            {
                return;
            }
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

        private void button4_Click(object sender, EventArgs e)
        {
            testMethod();
            var nuget1 = PackageRepositoryFactory.Default;

            var nuget = new PackageRepositoryFactory();

            //var xx = new PackageRepositoryBase();
            //var ret = xx.GetPackages();

            //var builder = new PackageBuilder();
        }

        private void testMethod()
        {
            PackageRepositoryFactory fac = new PackageRepositoryFactory();
            //IPackageRepository packageRepository = fac.CreateRepository(new PackageSource("http://retsolsrv051.retsol.infra.local/Sydney"));
            var packageRepository = fac.CreateRepository("http://retsolsrv051.retsol.infra.local/Sydney/nuget");
            IPackage package = packageRepository.FindPackage("CHBITS.BiersLibs.Common");
            var packages = packageRepository.GetPackages();

            foreach (IPackage pkg in packages)
            {
                var id = pkg.Id;
                var version = pkg.Version;
                var name = pkg.Title;
                var auth = pkg.Authors;
                var desc = pkg.Description;
            }
            FileInfo info = new FileInfo("CHBITS.BiersLibs.Common");
            DirectoryInfo directoryInfo = Directory.GetParent(info.DirectoryName);



            //var pm = new PackageManager(packageRepository,
            //                            new DefaultPackagePathResolver(Path.Combine(directoryInfo.FullName, "NuGet")),
            //                            new PhysicalFileSystem(Path.Combine(directoryInfo.FullName, "NuGet")));


            //var componentModel = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;

            //var installer = componentModel.GetService<IVsPackageInstaller>();
            //installer.InstallPackage(Path.Combine(directoryInfo.FullName, "NuGet"), project, package.Id, version: null, ignoreDependencies: false);
        }
    }
}
