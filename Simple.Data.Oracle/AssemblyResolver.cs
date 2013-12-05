using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Simple.Data.Oracle
{
    public class AssemblyResolver
    {
        public AssemblyResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.IndexOf(".resources", StringComparison.Ordinal) <= 0)
            {
                var executingAssembly = Assembly.GetExecutingAssembly();
                var requestedAssembly = TryLoadAssemblyWithVersionMatch(executingAssembly.CodeBase, args.Name);
                if (requestedAssembly == null)
                    requestedAssembly = TryLoadAssemblyWithNameMatch(executingAssembly.CodeBase, args.Name);
                if (requestedAssembly == null)
                    requestedAssembly = TryGetAssemblyFromGac(args.Name);
                return requestedAssembly;
            }
            return null;
        }

        private static Assembly TryLoadAssemblyWithVersionMatch(string codeBase, string requestedAssemblyFullName)
        {
            var candidateAssemblies = from Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()
                                      where !(assembly is System.Reflection.Emit.AssemblyBuilder) &&
                                      assembly.GetType().FullName != "System.Reflection.Emit.InternalAssemblyBuilder" &&
                                      !assembly.GlobalAssemblyCache &&
                                      assembly.CodeBase != codeBase
                                      select assembly;

            foreach (var bindingAssembly in candidateAssemblies)
            {
                if (bindingAssembly.FullName == requestedAssemblyFullName)
                {
                    return bindingAssembly;
                }
                else
                {
                    var baseDir = GetCodeBaseDirectory(bindingAssembly.CodeBase);
                    foreach (var assemblyName in bindingAssembly.GetReferencedAssemblies())
                    {
                        if (assemblyName.FullName == requestedAssemblyFullName)
                        {
                            var filename = assemblyName.FullName.Substring(0, assemblyName.FullName.IndexOf(',')) + ".dll";
                            var assembly = LoadReferencedAssembly(baseDir, filename);
                            if (assembly != null)
                            {
                                return assembly;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private static Assembly TryLoadAssemblyWithNameMatch(string codeBase, string requestedAssemblyFullName)
        {
            var baseDir = GetCodeBaseDirectory(codeBase);
            var candidateFiles = from FileInfo fileinfo in new DirectoryInfo(baseDir).EnumerateFiles("*.dll")
                                      select fileinfo.Name;

            var assemblyName = new AssemblyName(requestedAssemblyFullName);
            foreach (var filename in candidateFiles)
            {
                if (Path.GetFileNameWithoutExtension(filename) == assemblyName.Name)
                {
                    var assembly = LoadReferencedAssembly(baseDir, filename);
                    if (assembly != null)
                    {
                        return assembly;
                    }
                }
            }
            return null;
        }

        private static Assembly TryGetAssemblyFromGac(string requestedAssemblyFullName)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "assembly", "GAC_MSIL", new AssemblyName(requestedAssemblyFullName).Name);
            var assemblyDirectory = new DirectoryInfo(path);
            var fileinfo = assemblyDirectory.GetDirectories().OrderByDescending(x => x.Name).First().GetFiles()[0];
            return Assembly.LoadFrom(fileinfo.FullName);
        }

        private static Assembly LoadReferencedAssembly(string baseDir, string filename)
        {
            var filepath = Path.Combine(baseDir, filename);

            if (File.Exists(filepath))
            {
                try
                {
                    return Assembly.LoadFrom(filepath);
                }
                catch (Exception ex)
                {
                }
            }

            return null;
        }

        private static string GetCodeBaseDirectory(string codeBase)
        {
            return Path.GetDirectoryName(codeBase.Substring(@"file:\\\".Length));
        }
    }
}
