using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NugetDependencyResolver.model
{
    public class PackageDto
    {

        

        public const string partOfOwner = "CHBITS";

        public string Id { get; set; }
        public string Version { get; set; }
        public string Target { get; set; }
        public string Path { get; set; }
        public bool ThirdParty
        {
            get
            {

                return !(Regex.IsMatch(this.Id, partOfOwner, RegexOptions.IgnoreCase));
            }
        }

        public override int GetHashCode()
        {
            return string.Concat(Id, Version).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            PackageDto other = (PackageDto)obj;

            return other.GetHashCode() == this.GetHashCode();


        }
    }
}
