using NugetDependencyResolver.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NugetDependencyResolver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            

            DependencyResolver rsol = new DependencyResolver(new string[]{@"C:\Program Files\_INFRA\TFS\Retail\Main\"});
            rsol.BuildTree();

       

            rsol.WriteTree();
            PackageDto sr =new PackageDto();
            sr.Version = "1.0.1.0";
            sr.Id = "CHBITS.Tools.Common.Security";

            IEnumerable<PackageDto> test;
            int generation = 0;
            do
            {
                Console.WriteLine("Gen: " + generation);
                
                test = rsol.GetPackagesToRebuild(sr, generation++);


                
                foreach (PackageDto dto in test)
                {
                    Console.WriteLine(dto.Id);


                }
                Console.WriteLine();

            } while (test != null && test.Count() != 0);

            Console.ReadLine();

        }
    }
}
