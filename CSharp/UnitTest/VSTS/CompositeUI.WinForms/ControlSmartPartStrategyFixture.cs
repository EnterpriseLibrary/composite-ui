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
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeUI.WinForms.Tests
{
	[TestClass]
	public class ControlSmartPartStrategyFixture
	{
		private static Control control;
		private static WorkItem workItem;
		private static ControlSmartPartStrategy strat;
		private static MockBuilderContext context;

		[TestInitialize]
		public void Setup()
		{
			control = new Control();
			workItem = new TestableRootWorkItem();
			strat = new ControlSmartPartStrategy();
			context = new MockBuilderContext(strat);
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(WorkItem), null), workItem);
		}

		[TestMethod]
		public void AddingControlWithPlaceholderReplacesWSP()
		{
			SmartPartPlaceholder placeholder = new SmartPartPlaceholder();
			placeholder.SmartPartName = "SP1";
			control.Controls.Add(placeholder);
			MockSmartPart smartPart1 = new MockSmartPart();

			workItem.Items.Add(smartPart1, "SP1");
			workItem.Items.Add(control);

			Assert.AreSame(smartPart1, placeholder.SmartPart);
		}

		[TestMethod]
		public void TestSmartPartHolderHavingNoSmartPartInContainerNoOp()
		{
			SmartPartPlaceholder smartpartHolder = new SmartPartPlaceholder();
			smartpartHolder.SmartPartName = "SampleSmartPart";
			control.Controls.Add(smartpartHolder);

			workItem.Items.Add(control);

			Assert.IsNull(smartpartHolder.SmartPart);
		}

		[TestMethod]
		public void RemovingControlRemovesSmartParts()
		{
			int originalCount = workItem.Items.Count;

			MockSmartPart smartPart1 = new MockSmartPart();
			smartPart1.Name = "SmartPart1";
			MockSmartPart smartPart2 = new MockSmartPart();
			smartPart2.Name = "SmartPart2";
			smartPart1.Controls.Add(smartPart2);
			control.Controls.Add(smartPart1);
			workItem.Items.Add(control);

			Assert.AreEqual(3, workItem.Items.Count - originalCount);

			workItem.Items.Remove(control);

			Assert.AreEqual(0, workItem.Items.Count - originalCount);
		}

		[TestMethod]
		public void MonitorCallsRegisterWorkspace()
		{
			MockControlWithWorkspace control = new MockControlWithWorkspace();

			workItem.Items.Add(control);

			Assert.AreEqual(control.Workspace, workItem.Workspaces[control.Workspace.Name]);
		}

		[TestMethod]
		public void WorkspacesAreRegisteredWithName()
		{
			MockControlWithWorkspace mockControl = new MockControlWithWorkspace();

			workItem.Items.Add(mockControl);

			Assert.AreEqual(mockControl.Workspace, workItem.Workspaces[mockControl.Workspace.Name]);
		}

		[TestMethod]
		public void EmptyStringNameIsReplaceWhenAdded()
		{
			Control control = new Control();
			TabWorkspace workspace = new TabWorkspace();
			control.Controls.Add(workspace);

			workItem.Items.Add(control);

			ICollection<TabWorkspace> tabWorkSpaces = workItem.Workspaces.FindByType<TabWorkspace>();

			Assert.IsNull(workItem.Workspaces[workspace.Name]);
			Assert.IsTrue(tabWorkSpaces.Contains(workspace));
		}


		#region Supporting Classes

		[SmartPart]
		private class MockSmartPart : UserControl { }

		private class MockControlWithWorkspace : UserControl
		{
			private TabWorkspace workspace;
			private Button button;

			public TabWorkspace Workspace
			{
				get { return workspace; }
			}

			public MockControlWithWorkspace()
			{
				workspace = new TabWorkspace();
				workspace.Name = "TestName";

				button = new Button();

				this.Controls.Add(workspace);
				this.Controls.Add(button);
			}
		}
		#endregion
	}
}
