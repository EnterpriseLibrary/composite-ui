//===============================================================================
// Microsoft patterns & practices
// CompositeUI Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#endif

using System;
using System.IO;
using Microsoft.Practices.CompositeUI.Configuration;
using Microsoft.Practices.CompositeUI.Services;

namespace Microsoft.Practices.CompositeUI.Tests.Services
{
	[TestClass]
	public class ReflectionModuleEnumeratorFixture
	{
		private static ReflectionModuleEnumerator enumerator;
		private static string location;

		static ReflectionModuleEnumeratorFixture()
		{
			enumerator = new ReflectionModuleEnumerator();
			location = AppDomain.CurrentDomain.BaseDirectory;

			ModuleLoaderServiceFixture.CompileFile(
				"Microsoft.Practices.CompositeUI.Tests.Mocks.Src.ReflectionModule1.cs",
				@".\Reflection1\Module1.dll");

			ModuleLoaderServiceFixture.CompileFile(
				"Microsoft.Practices.CompositeUI.Tests.Mocks.Src.ReflectionModule2.cs",
				@".\Reflection2\Module2.dll");

			ModuleLoaderServiceFixture.CompileFile(
				"Microsoft.Practices.CompositeUI.Tests.Mocks.Src.ReflectionModule3.cs",
				@".\Reflection2\Recurse\Module3.dll");
		}

		[TestMethod]
		public void BasePathDefaultIsApplicationPath()
		{
			Assert.AreEqual(location, enumerator.BasePath);
		}

		[TestMethod]
		public void EnumeratorFindsModules()
		{
			enumerator.BasePath = Path.Combine(location, "Reflection1");

			IModuleInfo[] modules = enumerator.EnumerateModules();

			Assert.AreEqual(1, modules.Length);
			Assert.AreEqual(Path.Combine(enumerator.BasePath, "Module1.dll"), modules[0].AssemblyFile);
		}

		[TestMethod]
		public void EnumeratorSearchDirectoryRecursively()
		{
			enumerator.BasePath = Path.Combine(location, "Reflection2");

			IModuleInfo[] modules = enumerator.EnumerateModules();

			Assert.AreEqual(2, modules.Length);
		}


	}
}
