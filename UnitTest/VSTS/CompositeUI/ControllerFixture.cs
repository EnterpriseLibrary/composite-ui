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
	public class ControllerFixture
	{
		[TestMethod]
		public void ControllerStateIsNullByDefault()
		{
			Controller controller = new Controller();

			Assert.IsNull(controller.State);
		}

		[TestMethod]
		public void ControllerWorkItemIsNullByDefault()
		{
			Controller controller = new Controller();

			Assert.IsNull(controller.WorkItem);
		}

		[TestMethod]
		public void ContollerReceivedWorkItemAndStateFromWorkItem()
		{
			WorkItem wi = new TestableRootWorkItem();
			Controller controller = wi.Items.AddNew<Controller>();

			Assert.AreSame(wi, controller.WorkItem);
			Assert.AreSame(wi.State, controller.State);
		}
	}
}
