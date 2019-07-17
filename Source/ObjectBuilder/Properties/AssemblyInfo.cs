//===============================================================================
// Microsoft patterns & practices
// Object Builder Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyCopyright("Copyright © Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyDescription("Microsoft Object Builder Application Block")]
[assembly: AssemblyTitle("Microsoft.Practices.ObjectBuilder")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyVersion("1.0.51205.0")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

