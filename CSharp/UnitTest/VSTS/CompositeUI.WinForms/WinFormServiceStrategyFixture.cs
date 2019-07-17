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
using System.Text;

namespace Microsoft.Practices.CompositeUI.WinForms.Tests
{
	[TestClass]
	public class WinFormServiceStrategyFixture
	{
		[TestMethod]
		public void ControlActivationServiceCalledWhenControlAdded()
		{
			WorkItem workItem = new TestableRootWorkItem();
			WorkItem wi = workItem.WorkItems.AddNew<WorkItem>();

			Assert.IsTrue(wi.Services.ContainsLocal(typeof(IControlActivationService)));
		}
	}
}
