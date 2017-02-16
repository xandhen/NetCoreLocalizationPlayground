// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace WebApp
{
    /// <summary>
    /// An <see cref="IStringLocalizerFactory"/> that creates instances of <see cref="ResourceManagerStringLocalizer"/>
    /// and will properly handle the resources of ClassLibraries.
    /// </summary>
    public class ClassLibraryStringLocalizerFactory : ResourceManagerStringLocalizerFactory
    {
        private IReadOnlyDictionary<string, string> _resourcePathMappings;

        public ClassLibraryStringLocalizerFactory(
            IHostingEnvironment hostingEnvironment,
            IOptions<LocalizationOptions> localizationOptions)
                : base(hostingEnvironment, localizationOptions)
        {
            
        }

        protected override string GetResourcePrefix(TypeInfo typeInfo)
        {
            var assemblyName = typeInfo.Assembly.GetName().Name;
            return GetResourcePrefix(typeInfo, assemblyName, GetResourcePath(assemblyName));
        }

        protected override string GetResourcePrefix(TypeInfo typeInfo, string baseNamespace, string resourcesRelativePath)
        {
            if (typeInfo.Name.Equals("LocalClass")) {
                string z = base.GetResourcePrefix(typeInfo, baseNamespace, resourcesRelativePath);
                z = z.Substring(z.IndexOf('.') + 1);
                z = z.Substring(z.IndexOf('.') + 1);
                return z;
            }

            var assemblyName = new AssemblyName(typeInfo.Assembly.FullName);
            var thisAssembly = new AssemblyName(this.GetType().GetTypeInfo().Assembly.FullName);
            var baseVal = base.GetResourcePrefix(typeInfo, baseNamespace, resourcesRelativePath);
            var baseVal2 = baseVal.Substring(baseVal.IndexOf('.') + 1);
            baseVal2 = baseVal2.Substring(baseVal2.IndexOf('.') + 1);
            //baseVal2 = resourcesRelativePath + baseVal2;
            /*if (!baseVal2.StartsWith($"{thisAssembly.Name}."))
            {
                baseVal2 = baseVal.Substring(baseVal.IndexOf('.'));
                if (baseVal2.StartsWith(resourcesRelativePath))
                    baseVal2 = baseVal2.Substring(baseVal2.IndexOf('.') + 1);
                baseVal2 = this.GetType().Namespace + baseVal2;
            }*/
            
            string x = base.GetResourcePrefix(this.GetType().GetTypeInfo(), baseNamespace, resourcesRelativePath);
            string y = base.GetResourcePrefix(typeInfo, baseNamespace, resourcesRelativePath);

            return baseVal2;
            //return base.GetResourcePrefix(typeInfo, baseNamespace, GetResourcePath(assemblyName.Name));
        }

        private string GetResourcePath(string assemblyName)
        {
            string resourcePath = null;
            /*if (!_resourcePathMappings.TryGetValue(assemblyName, out resourcePath))
            {
                throw new KeyNotFoundException("Attempted to access an assembly which doesn't have a resourcePath set.");
            }*/

            if (!string.IsNullOrEmpty(resourcePath))
            {
                resourcePath = resourcePath.Replace(Path.AltDirectorySeparatorChar, '.')
                    .Replace(Path.DirectorySeparatorChar, '.') + ".";
            }

            return resourcePath;
        }
    }
}
