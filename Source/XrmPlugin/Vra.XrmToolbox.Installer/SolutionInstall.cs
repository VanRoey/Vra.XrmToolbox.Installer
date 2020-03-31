using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Vra.XrmToolbox.Installer
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Solution Installer"),
        ExportMetadata("Description", "This plugin can install multiple solutions at once."),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAEnQAABJ0Ad5mH3gAAAASdEVYdFNvZnR3YXJlAEdyZWVuc2hvdF5VCAUAAANiSURBVFhHxZfbSxRRHMe/e3PV9RLe1ktlJloiiaUSQUG+FVGU2IMkUUQlUhH0pBBSQX9AmGT4IBUSFRlFT9GNwgixHjKRjNq8ZXlJ3dzZu/3OzsmZdqd1dneiDxyG+e3unO/5ze/7O2d165ufLOI/oudXzfk+78L4rEDDyYcAu+Dln0r8kwzYph3oadqKeJMefv70lAQj2p/bcKNnDAlxBjFIaJ6BBZcPjdvzsTE/FSW5ySjNE8eqtAR0vhyBmUTJ0TQDi4uL8Pr8SDQbKOUuHgWS440YnhZQZLVAp9PxqIimAibtbnSfqMS24nQeEZm0u7Cu+QmsKfE8IqGZAD+tfpRWeXZPMSaoAH9jTTHjWs8ofFQMBv2fq2doIoBN/tPpReeRcggeP70KMc4K78pTG159moVFVnhyNBHgoMJrqM5H064iHpEwH3uItZmJ/C4UTQR8o5R7vH64vL6l1TOrZSSZYTLqoA8qPDkxC2CTdxwqQ01FLo+IfJlyoOrCC6QnxfGIMjH1AQ9ZriAjIWRyxt7WXqSQ/ZYjJgEjP5y401jF7yTu9n2FjTJgMi7/+KgFCG4vdpRmokChwM7cfI/MZDO/C09UAljHm3f6cKuxkkckWu4NwkFWVPK8EooCHLS6mQX30ph1ePgnIvPk+Qbq92aFFF98OIQ0i4nfLU+ICxZcXtRW5KC6JINs5Q9YyE3Xk139ZKu4QNOZosqfbt3JfyFRQ4X3dniOdkHlpqNEyBLYZGwn212ejdrKXKrwHLrmBDodS+rEnBMdh8vFL8voH7Pj8cAkZUX95AzFV8DsJUfw+AJXFi+lLXYfiQpm/+VeZFHfD9NzFFFdhOzBn8la149u4hGJ273jgQ3IaFD9uCVU/0Kgqq/bnIfCLAuPSJyi+shSabtgVAkI9Heqgbb6MjEgo6V7MPAFvUrbBaNKwDwdJo/vLEQyba9yXFQb5x8MYYUlfL8Ph+pX0HYwdPV17X1YQ3tBLKgSsJIOlMFb6quPM3g0MBWR55VQnYFgDlx9g+zU0DNepEQloP2ZDXZyhTHKwpMTlYBz9z9E1O/DoSjAFKahnO56F2jJ0douGMXNqH5LHnZssMLJWzCD/aEw0Nh96TVWp/39kBkpimdCJkJwS5P/holY7owXKZqciqMH+AVumEBHvyW9lQAAAABJRU5ErkJggg=="),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class SolutionInstall : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new SolutionInstallControl();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public SolutionInstall()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}