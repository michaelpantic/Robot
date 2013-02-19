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

            foreach (DependencyDto dto in rsol.dependencies)
            {
                Console.WriteLine(dto.From + " \t " + dto.Via + " \t " + dto.To.Id);
            
            
            }

            
            Console.ReadLine();

        }
    }
}
