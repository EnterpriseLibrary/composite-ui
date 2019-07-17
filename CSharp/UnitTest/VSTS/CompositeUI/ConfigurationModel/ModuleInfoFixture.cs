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
using Microsoft.Practices.CompositeUI.Configuration;

namespace Microsoft.Practices.CompositeUI.Tests.ConfigurationModel
{
	[TestClass]
	public class ModuleInfoFixture
	{
		[TestMethod]
		public void InitializesCorrectly()
		{
			ModuleInfo mInfo = new ModuleInfo();
			Assert.AreEqual(0, mInfo.AllowedRoles.Count);
		}

		[TestMethod]
		public void CanInitializeWithAssemblyFile()
		{
			ModuleInfo mInfo = new ModuleInfo("MyAssembly.dll");
			Assert.AreEqual("MyAssembly.dll", mInfo.AssemblyFile);
		}

		[TestMethod]
		public void CanSetUpdateLocation()
		{
			ModuleInfo mInfo = new ModuleInfo();
			mInfo.SetUpdateLocation("http://somelocation/someapplication");
			Assert.AreEqual("http://somelocation/someapplication", mInfo.UpdateLocation);
		}

		[TestMethod]
		public void CanAddSingleRole()
		{
			ModuleInfo mInfo = new ModuleInfo();
			mInfo.AddRoles("role1");
			Assert.AreEqual(1, mInfo.AllowedRoles.Count);
			Assert.AreEqual("role1", mInfo.AllowedRoles[0]);
		}

		[TestMethod]
		public void CanAddSeveralRoles()
		{
			ModuleInfo mInfo = new ModuleInfo();
			mInfo.AddRoles("role1", "role2");
			Assert.AreEqual(2, mInfo.AllowedRoles.Count);
			Assert.AreEqual("role1", mInfo.AllowedRoles[0]);
			Assert.AreEqual("role2", mInfo.AllowedRoles[1]);
		}

		[TestMethod]
		public void CanClearRoles()
		{
			ModuleInfo mInfo = new ModuleInfo();
			mInfo.AddRoles("role1", "role2");
			mInfo.ClearRoles();
			Assert.AreEqual(0, mInfo.AllowedRoles.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfAddingNullRole()
		{
			ModuleInfo mInfo = new ModuleInfo();
			mInfo.AddRoles(null);
		}

	}
}
