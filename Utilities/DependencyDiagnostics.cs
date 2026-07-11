using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TrOCR.Helper
{
    public static class DependencyDiagnostics
    {
        public sealed class AssemblyInfo
        {
            public AssemblyInfo(string path, string name, string version)
            {
                Path = path;
                Name = name;
                Version = version;
            }

            public string Path { get; }
            public string Name { get; }
            public string Version { get; }
        }

        public static IReadOnlyList<AssemblyInfo> FindManagedDlls(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                return Array.Empty<AssemblyInfo>();
            }

            return Directory
                .EnumerateFiles(directory, "*.dll", SearchOption.AllDirectories)
                .Select(TryReadAssemblyInfo)
                .Where(info => info != null)
                .ToArray();
        }

        public static IReadOnlyList<string> FindDuplicateAssemblyNames(string directory)
        {
            return FindManagedDlls(directory)
                .GroupBy(info => info.Name, StringComparer.OrdinalIgnoreCase)
                .Where(group => group.Select(info => info.Version).Distinct(StringComparer.OrdinalIgnoreCase).Count() > 1)
                .Select(group => group.Key)
                .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        private static AssemblyInfo TryReadAssemblyInfo(string path)
        {
            try
            {
                var name = AssemblyName.GetAssemblyName(path);
                return new AssemblyInfo(path, name.Name, name.Version.ToString());
            }
            catch (BadImageFormatException)
            {
                return null;
            }
            catch (FileLoadException)
            {
                return null;
            }
        }
    }
}
