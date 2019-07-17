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

namespace Microsoft.Practices.CompositeUI.Tests
{
	[TestClass]
	public class ModuleDependencyAttributeTestFixture
	{
		[TestMethod]
		public void ModuleDependencyAttributeIsAvailable()
		{
			ModuleDependencyAttribute attr = new ModuleDependencyAttribute("SomeModule");
			Assert.IsNotNull(attr);
			Assert.IsTrue(attr is Attribute);
		}

		[TestMethod]
		public void ModuleNameIsStored()
		{
			ModuleDependencyAttribute attr = new ModuleDependencyAttribute("SomeModule");
			Assert.AreEqual("SomeModule", attr.Name);
		}
	}
}
