// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyDescription("StealFocus.Tracer.Tests")]
[assembly: AssemblyTitle("StealFocus.Tracer.Tests")]
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: Guid("2954764e-a1ba-4551-9c18-4fa0efcc2a0a")]
[assembly: NeutralResourcesLanguage("en-GB")]
[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")] // Assembly is delay signed.
[assembly: SuppressMessage("Microsoft.Design", "CA1016:MarkAssembliesWithAssemblyVersion")] // Development version is "0.0.0.0"
[assembly: SuppressMessage("Microsoft.Usage", "CA2243:AttributeStringLiteralsShouldParseCorrectly")] // "AssemblyInformationalVersion" is a string

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]