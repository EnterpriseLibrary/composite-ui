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

namespace Microsoft.Practices.CompositeUI.Tests.Services
{
	[TestClass]
	public class ServiceAttributeTestFixture
	{
		[TestMethod]
		public void ServiceAttributeIsAvailable()
		{
			ServiceAttribute attr = new ServiceAttribute();
			Assert.IsNotNull(attr);
			Assert.IsNull(attr.RegisterAs);
		}

		[TestMethod]
		public void TypeGetStored()
		{
			ServiceAttribute attr = new ServiceAttribute(typeof (ServiceAttributeTestFixture));
			Assert.AreEqual(typeof (ServiceAttributeTestFixture), attr.RegisterAs);
		}

		[TestMethod]
		public void HasCreateOnDemandProperty()
		{
			ServiceAttribute attr = new ServiceAttribute();
			Assert.IsFalse(attr.AddOnDemand);
		}
	}
}
