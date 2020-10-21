using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleAppDeleteExclude
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = $@"F:\Clients\MuhraSoft\MuhraSoftSystemV1Store\MuhraSoftSystem - Temp\MuhraSoftSystem\MuhraSoftSystem.csproj";
            var filename = $@"{path}\MuhraSoftSystem.csproj";

            XDocument doc = XDocument.Load(filename);
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";

            void DumpMatches(IEnumerable<string> values)
            {
                foreach (string x in values)
                    AllFiles.Add(x);
            }
            
            DumpMatches(doc.Descendants(ns + "Compile").Select(x => x.Attribute("Include").Value));
            DumpMatches(doc.Descendants(ns + "AssemblyOriginatorKeyFile").Select(x => x.Value));

            var dirInfo = new DirectoryInfo(path);
            var allFilesOnDisk = dirInfo.GetFiles("*.*", SearchOption.AllDirectories).Select(s => s.FullName).ToList();
            var allFilesFromProjectFile = AllFiles.Select(s => $@"{path}\MuhraSoftSystem\{s}").ToList();
            var excludedFiles = allFilesOnDisk.Except(allFilesFromProjectFile).ToList();
            var ff = excludedFiles.Where(w => w.Contains("All2") && w.EndsWith(".cs")).ToList();

            foreach (var f in ff)
            {
                if (f.EndsWith(".cs"))
                {
                    File.Delete(f);
                    Debug.WriteLine(f);
                }
            }
        }

        public static List<string> AllFiles = new List<string>();


    }
}
