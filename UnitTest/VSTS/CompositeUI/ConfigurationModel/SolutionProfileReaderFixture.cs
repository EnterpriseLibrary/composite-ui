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
using Microsoft.Practices.CompositeUI.Configuration.Xsd;
using System.Reflection;

namespace Microsoft.Practices.CompositeUI.Tests.ConfigurationModel
{
	[TestClass]
	public class SolutionProfileReaderFixture
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
			SolutionProfileReader slnReader = new SolutionProfileReader();
			Assert.AreEqual(SolutionProfileReader.DefaultCatalogFile, slnReader.CatalogFilePath);
		}

		[TestMethod]
		public void CanCreateSpecifyingFile()
		{
			SolutionProfileReader slnReader = new SolutionProfileReader("myCatalog.xml");
			Assert.IsTrue(slnReader.CatalogFilePath.EndsWith("myCatalog.xml"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfNullFile()
		{
			SolutionProfileReader slnReader = new SolutionProfileReader(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowsIfEmptyFile()
		{
			SolutionProfileReader slnReader = new SolutionProfileReader(String.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(SolutionProfileReaderException))]
		public void ThrowsIfFileIsNotUnderApplication()
		{
			SolutionProfileReader slnReader = new SolutionProfileReader(@"\\somemachine\someshare\somefile.xml");
		}

		[TestMethod]
		public void CurrentDirectoryNotUsedToCheckFile()
		{
			string current = Environment.CurrentDirectory;
			try
			{
				Environment.CurrentDirectory = "c:\\";
				SolutionProfileReader slnReader = new SolutionProfileReader("somefile.xml");
			}
			finally
			{
				Environment.CurrentDirectory = current;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(SolutionProfileReaderException))]
		public void ThrowsIfFileNotFound()
		{
			SolutionProfileReader slnReader = new SolutionProfileReader("NonExistingFile.xml");
			slnReader.ReadProfile();
		}

		[TestMethod]
		[ExpectedException(typeof(SolutionProfileReaderException))]
		public void ThrowsIfBadFormedFile()
		{
			string filename = fileHelper.CreateTempFile();
			File.WriteAllText(filename, "<blah />");
			SolutionProfileReader slnReader = new SolutionProfileReader(filename);
			slnReader.ReadProfile();
		}

		[TestMethod]
		public void UnAuthenticatedUserCanLoadModulesWithoutRoles()
		{
			IPrincipal originalPrincipal = null;

			using (TestResourceFile resFile1 = new TestResourceFile(@"Services\FileCatalogReaderServiceFixtureWithRoles.xml"))
			{
				try
				{
					originalPrincipal = Thread.CurrentPrincipal;
					if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity.IsAuthenticated)
					{
						Thread.CurrentPrincipal = new GenericPrincipal(WindowsIdentity.GetCurrent(), null);
					}
					SolutionProfileReader slnReader =
						new SolutionProfileReader(@"Services\FileCatalogReaderServiceFixtureWithRoles.xml");
					SolutionProfileElement profile = slnReader.ReadProfile();

					Assert.AreEqual(1, profile.Modules.Length, "The solution profile does not contains 1 module.");
					Assert.AreEqual("MyAssembly1.dll", profile.Modules[0].AssemblyFile, "The 1st module is not MyAssembly1.dll");
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

					SolutionProfileReader slnReader = new SolutionProfileReader(@"Services\FileCatalogReaderServiceFixtureWithRoles.xml");

					SolutionProfileElement profile = slnReader.ReadProfile();

					Assert.AreEqual(3, profile.Modules.Length, "The solution profile does not contains 3 modules.");
					Assert.AreEqual("MyAssembly1.dll", profile.Modules[0].AssemblyFile, "The 1st module is not MyAssembly1.dll");
					Assert.AreEqual("MyAssembly2.dll", profile.Modules[1].AssemblyFile, "The 2nd module is not MyAssembly2.dll");
					Assert.AreEqual("MyAssembly4.dll", profile.Modules[2].AssemblyFile, "The 3rd module is not MyAssembly4.dll");
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
				SolutionProfileReader slnReader =
							new SolutionProfileReader(@"Services\FileCatalogReaderServiceFixtureModuleWithoutUpdateLocation.xml");
				SolutionProfileElement profile = slnReader.ReadProfile();
			}
		}

		[TestMethod]
		public void CatalogExistsReturnsFalseOnInexistentFile()
		{
			SolutionProfileReader slnReader = new SolutionProfileReader("NonExistingFile.xml");
			Assert.IsFalse(File.Exists(slnReader.CatalogFilePath));
		}

		[TestMethod]
		public void CatalogExistsReturnsTrueOnExistentFile()
		{
			using (TestResourceFile resFile = new TestResourceFile(@"Services\FileCatalogReaderServiceFixtureModuleWithoutUpdateLocation.xml"))
			{
				SolutionProfileReader slnReader =
					new SolutionProfileReader(@"Services\FileCatalogReaderServiceFixtureModuleWithoutUpdateLocation.xml");
				Assert.IsTrue(File.Exists(slnReader.CatalogFilePath));
			}
		}

		[TestMethod]
		public void DoesNothingIfNoCatalogIsProvidedAndDefaultOneNotExists()
		{
			SolutionProfileReader slnReader = new SolutionProfileReader();
			SolutionProfileElement profile = slnReader.ReadProfile();

			//An empty solutionprofile is returned.
			Assert.IsNotNull(profile);
			Assert.IsNotNull(profile.Modules);
			Assert.AreEqual(0, profile.Modules.Length);
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
