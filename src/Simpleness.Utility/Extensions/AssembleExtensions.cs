using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simpleness.Utility.Extensions
{
    /// <summary>
    ///https://stackoverflow.com/questions/37895278/how-to-load-assemblies-located-in-a-folder-in-net-core-console-app
    /// </summary>
    public static class AssemblyExtensions
    {
        public static Assembly FindAssemblyBy(this string assemblyName)
        {
            DependencyContext deps = DependencyContext.Default;
            System.Collections.Generic.List<CompilationLibrary> res = deps.CompileLibraries.Where(d => d.Name.Contains(assemblyName)).ToList();
            Assembly assembly = Assembly.Load(new AssemblyName(res.First().Name));
            return assembly;
        }
    }
}
