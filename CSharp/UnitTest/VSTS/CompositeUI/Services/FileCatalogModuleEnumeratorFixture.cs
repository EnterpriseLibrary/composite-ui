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
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Threading;
using Microsoft.Practices.CompositeUI.Configuration;
using Microsoft.Practices.CompositeUI.Services;

namespace Microsoft.Practices.CompositeUI.Tests.Services
{
	[TestClass]
	public class FileCatalogModuleEnumeratorFixture
	{
		private static TempFileHelper fileHelper;

		[TestInitialize]
		public void FixtureSetup()
		{
			fileHelper = new TempFileHelper();
		}

		[TestCleanup]
		public void FixtureTearDown()
		{
			try
			{
				fileHelper.Dispose();
			}
			catch { }
		}

		[TestMethod]
		public void CanCreate()
		{
			FileCatalogModuleEnumerator fcEnumerator = new FileCatalogModuleEnumerator();
			Assert.AreEqual(SolutionProfileReader.DefaultCatalogFile, fcEnumerator.CatalogFilePath);
		}

		[TestMethod]
		public void CanCreateSpecifyingFile()
		{
			FileCatalogModuleEnumerator fcEnumerator = new FileCatalogModuleEnumerator("myCatalog.xml");
			Assert.AreEqual("myCatalog.xml", fcEnumerator.CatalogFilePath);
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentNullException))]
		public void ThrowsIfNullFile()
		{
			FileCatalogModuleEnumerator fcEnumerator = new FileCatalogModuleEnumerator(null);
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentException))]
		public void ThrowsIfEmptyFile()
		{
			FileCatalogModuleEnumerator fcEnumerator = new FileCatalogModuleEnumerator(String.Empty);
		}

		[TestMethod]
		public void UnAuthenticatedUserCanLoadModulesWithoutRoles()
		{
			IPrincipal originalPrincipal = null;

			using (TestResourceFile resFile = new TestResourceFile(@"Services\FileCatalogReaderServiceFixtureWithRoles.xml"))
			{
				try
				{
					originalPrincipal = Thread.CurrentPrincipal;
					if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
					{
						Thread.CurrentPrincipal = new GenericPrincipal(WindowsIdentity.GetCurrent(), null);
					}
					FileCatalogModuleEnumerator fcEnumerator =
						new FileCatalogModuleEnumerator(@"Services\FileCatalogReaderServiceFixtureWithRoles.xml");
					IModuleInfo[] mInfos = fcEnumerator.EnumerateModules();

					Assert.AreEqual(1, mInfos.Length, "The solution profile does not contains 1 module.");
					Assert.AreEqual("MyAssembly1.dll", mInfos[0].AssemblyFile, "The 1st module is not MyAssembly1.dll");
				}
				finally
				{
					Thread.CurrentPrincipal = originalPrincipal;
				}
			}
		}

		[TestMethod]
		public void AuthenticateUserCanLoadModulesAccordingToHisRoles()
		{
			IPrincipal cachedPrincipal = Thread.CurrentPrincipal;

			using (TestResourceFile resFile = new TestResourceFile(@"Services\FileCatalogReaderServiceFixtureWithRoles.xml"))
			{
				try
				{
					GenericIdentity identity = new GenericIdentity("Me");
					GenericPrincipal principal = new GenericPrincipal(identity, new string[] { "Users" });
					Thread.CurrentPrincipal = principal;

					FileCatalogModuleEnumerator fcEnumerator =
						new FileCatalogModuleEnumerator(@"Services\FileCatalogReaderServiceFixtureWithRoles.xml");

					IModuleInfo[] mInfos = fcEnumerator.EnumerateModules();

					Assert.AreEqual(3, mInfos.Length, "The solution profile does not contains 3 modules.");
					Assert.AreEqual("MyAssembly1.dll", mInfos[0].AssemblyFile, "The 1st module is not MyAssembly1.dll");
					Assert.AreEqual("MyAssembly2.dll", mInfos[1].AssemblyFile, "The 2nd module is not MyAssembly2.dll");
					Assert.AreEqual("MyAssembly4.dll", mInfos[2].AssemblyFile, "The 3rd module is not MyAssembly4.dll");
				}
				finally
				{
					Thread.CurrentPrincipal = cachedPrincipal;
				}
			}
		}

		[TestMethod]
		public void CanLoadProfileContainingModuleWithoutUpdateLocation()
		{
			using (TestResourceFile resFile = new TestResourceFile(@"Services\FileCatalogReaderServiceFixtureModuleWithoutUpdateLocation.xml"))
			{
				FileCatalogModuleEnumerator fcEnumerator =
					new FileCatalogModuleEnumerator(@"Services\FileCatalogReaderServiceFixtureModuleWithoutUpdateLocation.xml");
				IModuleInfo[] mInfos = fcEnumerator.EnumerateModules();
			}
		}

		class TempFileHelper : IDisposable
		{
			private List<string> createdFiles = new List<string>();

			public string CreateTempFile()
			{
				string filename = Path.GetFileName(Path.GetTempFileName());
				createdFiles.Add(filename);
				return filename;
			}

			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			protected void Dispose(bool disposing)
			{
				if (disposing)
				{
					foreach (string filename in createdFiles)
					{
						if (File.Exists(filename))
						{
							File.Delete(filename);
						}
					}
				}
			}
		}
	}
}
