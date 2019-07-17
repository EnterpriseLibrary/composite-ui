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
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeUI.WinForms.Tests
{
	[TestClass]
	public class ControlActivationStrategyFixture
	{
		[TestMethod]
		public void StrategyAddsActivationService()
		{
			WorkItem workItem = new TestableRootWorkItem();
			workItem.Services.Remove(typeof(IControlActivationService));
			MockControlActivationService service = new MockControlActivationService();
			workItem.Services.Add<IControlActivationService>(service);
			MockBuilderContext context = new MockBuilderContext();
			ControlActivationStrategy strat = new ControlActivationStrategy();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(WorkItem), null), workItem);

			Control view = new Control();
			workItem.Items.Add(view);

			Assert.IsTrue(service.ControlToMonitorCalled, "Control.Enter didn't cause WorkItem.Activate to be called");
		}

		private class MockControlActivationService : IControlActivationService
		{
			public bool ControlToMonitorCalled = false;

			#region IControlActivationService Members

			public void ControlAdded(Control control)
			{
				ControlToMonitorCalled = true;
			}

			public void ControlRemoved(Control control)
			{
				throw new System.Exception("The method or operation is not implemented.");
			}

			#endregion
		}
	}
}
