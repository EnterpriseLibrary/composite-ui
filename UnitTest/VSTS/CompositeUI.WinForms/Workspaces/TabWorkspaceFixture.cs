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
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeUI.WinForms.Tests
{
	[TestClass]
	public class TabWorkspaceFixture
	{
		#region Setup
		
		private static TabWorkspace workspace;
		private static WorkItem workItem;
		private static MockSmartPart sp;
		private static Form owner;

		[TestInitialize]
		public void SetUp()
		{
			workItem = new TestableRootWorkItem();
			workspace = new TabWorkspace();
			workItem.Workspaces.Add(workspace);
			workItem.Services.Add(typeof(IWorkItemActivationService), new SimpleWorkItemActivationService());
			sp = new MockSmartPart();
			owner = new Form();
			owner.Controls.Add(workspace);
			owner.Show();
		}

		[TestCleanup]
		public void TearDown()
		{
			owner.Dispose();
		}

		#endregion
		
		#region ApplySPI

		[TestMethod]
		public void ApplyTabInfoDoesNotOverrideTitleIfNull()
		{
			TabSmartPartInfo info = new TabSmartPartInfo();
			info.Title = "foo";
			workspace.Show(sp, info);

			info = new TabSmartPartInfo();
			workspace.ApplySmartPartInfo(sp, info);

			Assert.AreEqual("foo", workspace.TabPages[0].Text);
		}

		#endregion

		#region Show

		[TestMethod]
		public void ParentChildWorkItemAndTabbedWorkspace()
		{
			WorkItem rootWorkItem = new TestableRootWorkItem();
			//rootWorkItem.Services.AddNew<SimpleWorkItemActivationService, IWorkItemActivationService>();

			TabWorkspace workspace = rootWorkItem.Items.AddNew<TabWorkspace>();

			ChildWorkItem childWorkItem = rootWorkItem.Items.AddNew<ChildWorkItem>();
			childWorkItem.Run(workspace);

			Assert.IsTrue(childWorkItem.Items.ContainsObject(childWorkItem.ContainedSmartPart));
			Assert.IsTrue(childWorkItem.Items.ContainsObject(childWorkItem.ContainingSmartPart));
			Assert.AreSame(childWorkItem.ContainedSmartPart, childWorkItem.ContainingSmartPart.Placeholder.SmartPart, "Placeholder was not correctly replaced.");
		}

		[TestMethod]
		public void SelectingTabFiresSmartPartActivatedEvent()
		{
			int activated = 0;
			WorkItem wi = workItem.Items.AddNew<WorkItem>();
			MockSmartPart smartPart1 = new MockSmartPart();
			smartPart1.Name = "SP1";
			MockSmartPart smartPart2 = new MockSmartPart();
			smartPart1.Name = "SP2";
			wi.Items.Add(smartPart1);
			wi.Items.Add(smartPart2);

			Form form = new Form();
			form.Controls.Add(workspace);
			workspace.Dock = DockStyle.Fill;
			form.Show();

			workspace.Show(smartPart1);
			workspace.Show(smartPart2);

			workspace.SmartPartActivated += delegate { activated++; };
			workspace.SelectedIndex = 0;

			Assert.AreEqual(1, activated);
		}

		[TestMethod]
		public void TestShowWithMorePages()
		{
			IContainer smartpartContainer = new Container();
			MockSmartPart sampleSmartPart1 = new MockSmartPart();
			smartpartContainer.Add(sampleSmartPart1);
			workspace.Show(sampleSmartPart1);
			MockSmartPart sampleSmartPart2 = new MockSmartPart();
			smartpartContainer.Add(sampleSmartPart2);
			workspace.Show(sampleSmartPart2);

			Assert.AreEqual(2, workspace.TabPages.Count);
			Assert.AreEqual(1, workspace.SelectedIndex);

			workspace.Show(sampleSmartPart1);

			//Returns 3
			Assert.AreEqual(2, workspace.TabPages.Count);
			Assert.AreEqual(0, workspace.SelectedIndex);
		}

		[TestMethod]
		public void CreatingTabWithDefaultSPIFiresSmartPartActivedEvent()
		{
			bool activated = false;
			MockSmartPart smartPart = new MockSmartPart();
			smartPart.Name = "SP";
			workItem.Items.Add(smartPart);
			workspace.SmartPartActivated += delegate { activated = true; };

			workspace.Show(smartPart);

			Assert.IsTrue(activated);
		}

		[TestMethod]
		public void WorkspaceShowsCorrectTabFromWorkItem()
		{
			WorkItem childWI = workItem.Items.AddNew<WorkItem>();
			MockSmartPart parentSP = workItem.Items.AddNew<MockSmartPart>("SP");
			MockSmartPart childSP = childWI.Items.AddNew<MockSmartPart>("SP");
			TabWorkspace tabWS = workItem.Items.AddNew<TabWorkspace>("TabWS");
			tabWS.Show(parentSP);
			tabWS.Show(childSP);

			tabWS.Show(parentSP);

			Assert.AreEqual(2, tabWS.TabPages.Count);
			Assert.AreSame(tabWS.TabPages[0], tabWS.SelectedTab);
		}

		[TestMethod]
		public void SmartPartActivatePassesCorrectEventArgs()
		{
			object argsSmartPart = null;
			MockSmartPart smartPart = new MockSmartPart();
			smartPart.Name = "SP";
			workItem.Items.Add(smartPart);
			workspace.SmartPartActivated +=
				delegate(object sender, WorkspaceEventArgs args) { argsSmartPart = args.SmartPart; };

			workspace.Show(smartPart);

			Assert.AreEqual(smartPart, argsSmartPart);
		}

		[TestMethod]
		public void ShowOnHiddenSPShowsTheTabPage()
		{
			workspace.Show(sp);
			workspace.Hide(sp);
			workspace.Show(sp);

			Assert.AreEqual(1, workspace.TabPages.Count);
			Assert.IsTrue(sp.Visible);
		}

		[TestMethod]
		public void ShowTabPageAddedAtDesignTime()
		{
			MockSmartPart sp1 = new MockSmartPart();
			workItem.Items.Add(sp1);

			TabPage page = new TabPage();
			page.Controls.Add(sp1);
			sp1.Dock = DockStyle.Fill;
			page.Name = Guid.NewGuid().ToString();

			workspace.TabPages.Add(page);

			workspace.Show(sp);
			workspace.Show(sp1);

			Assert.AreEqual(2, workspace.TabPages.Count);
			Assert.IsTrue(sp1.Visible);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowsIfSmartPartNotControlShow()
		{
			workspace.Show(new NonControlSmartPart());
		}

		[TestMethod]
		public void ShowAddsTabWithSPDockFill()
		{
			workspace.Show(sp);

			Assert.AreEqual(1, workspace.TabPages.Count);
			Assert.AreEqual(DockStyle.Fill, sp.Dock);
		}

		[TestMethod]
		public void ShowAddsTabsSelectsTabAndSetsText()
		{
			TabSmartPartInfo spInfo = new TabSmartPartInfo();
			spInfo.Title = "Title";
			spInfo.Description = "Description";
			workItem.RegisterSmartPartInfo(sp, spInfo);

			workspace.Show(sp);

			Assert.AreEqual(1, workspace.TabPages.Count);
			Assert.AreEqual("Title", workspace.SelectedTab.Text);
		}

		[TestMethod]
		public void ShowFocusesTabIfAlreadyContained()
		{
			workspace.TabPages.Add("Foo");
			workspace.Show(sp);
			workspace.TabPages.Add("Bar");

			workspace.SelectedIndex = 0;

			workspace.Show(sp);

			Assert.AreEqual(1, workspace.SelectedIndex);
		}

		[TestMethod]
		public void ShowTabWithNewInfo()
		{
			MockSmartPart part = new MockSmartPart();
			workItem.Items.Add(part);
			TabSmartPartInfo info = new TabSmartPartInfo();
			info.Title = "Updated";

			workspace.Show(part, info);

			Assert.AreEqual("Updated", workspace.SelectedTab.Text);
		}

		[TestMethod]
		public void TabPositionWithInfoIsSetCorrectly()
		{
			TabPage page = new TabPage();
			workspace.TabPages.Add(page);

			TabSmartPartInfo info = new TabSmartPartInfo();
			info.Position = TabPosition.Beginning;
			workspace.Show(sp, info);

			Assert.AreEqual(2, workspace.TabPages.Count);
			Assert.AreSame(workspace.TabPages[0], workspace.SelectedTab);
		}

		[TestMethod]
		public void TabPositionWithRegisteredInfoIsSetCorrectly()
		{
			TabPage page = new TabPage();
			workspace.TabPages.Add(page);

			TabSmartPartInfo info = new TabSmartPartInfo();
			info.Position = TabPosition.Beginning;

			workItem.RegisterSmartPartInfo(sp, info);
			workspace.Show(sp);

			Assert.AreEqual(2, workspace.TabPages.Count);
			Assert.AreSame(workspace.TabPages[0], workspace.SelectedTab);
		}

		[TestMethod]
		public void CanShowTabPositionAtEnd()
		{
			TabPage page = new TabPage();
			workspace.TabPages.Add(page);

			TabSmartPartInfo info = new TabSmartPartInfo();
			info.Position = TabPosition.End;
			workspace.Show(sp, info);

			Assert.AreEqual(2, workspace.TabPages.Count);
			Assert.AreEqual(1, workspace.SelectedIndex);
		}

		[TestMethod]
		public void WorkspaceExposesCollectionOfPages()
		{
			workspace.Show(sp);

			Assert.AreEqual(1, workspace.Pages.Count);
		}

		[TestMethod]
		public void CanShowSmartPartWithNullSite()
		{
			Control smartPartA = new Control();

			workspace.Show(smartPartA);

			Assert.AreEqual(1, workspace.Pages.Count);
		}

		[TestMethod]
		public void CanUseNotTabSPI()
		{
			SmartPartInfo info = new SmartPartInfo();
			info.Title = "Foo";

			workspace.Show(sp, info);
		}

		[TestMethod]
		public void UsesSPInfoIfNoTabSPInfoExists()
		{
			SmartPartInfo info = new SmartPartInfo();
			info.Title = "Foo";
			workItem.RegisterSmartPartInfo(sp, info);

			workspace.Show(sp);

			Assert.AreEqual("Foo", workspace.Pages[sp].Text);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CanShowSmartPartWithNullInfo()
		{
			workspace.Show(sp, null);
		}

		[TestMethod]
		public void AddingSmartPartAtBeginningFiresOneActivatedEvent()
		{
			MockSmartPart smartPartA = new MockSmartPart();
			MockSmartPart smartPartB = new MockSmartPart();

			TabSmartPartInfo smartPartInfoA = new TabSmartPartInfo();
			smartPartInfoA.Title = "Smart Part A";
			smartPartInfoA.Position = TabPosition.Beginning;

			TabSmartPartInfo smartPartInfoB = new TabSmartPartInfo();
			smartPartInfoB.Title = "Smart Part B";

			workspace.Show(smartPartB, smartPartInfoB);

			int activatedCalled = 0;
			workspace.SmartPartActivated += delegate(object sender, WorkspaceEventArgs e)
			{
				activatedCalled++;
				Assert.AreSame(e.SmartPart, smartPartA);
			};

			workspace.Show(smartPartA, smartPartInfoA);

			Assert.AreEqual(1, activatedCalled);
		}

		[TestMethod]
		public void NotSettingActivateDefaultsToTrue()
		{
			workspace.Show(sp);
			Control smartPart = new Control();
			workspace.Show(smartPart);

			Assert.AreEqual(1, workspace.SelectedIndex);
		}

		[TestMethod]
		public void CanActivateTabInSPI()
		{
			TabSmartPartInfo info = new TabSmartPartInfo();
			info.ActivateTab = true;
			workspace.Show(sp);
			Control smartPart = new Control();
			workspace.Show(smartPart, info);

			Assert.AreEqual(1, workspace.SelectedIndex);
		}

		[TestMethod]
		public void CanAvoidActivationTabInSPI()
		{
			TabSmartPartInfo info = new TabSmartPartInfo();
			info.ActivateTab = false;
			workspace.Show(sp);
			Control smartPart = new Control();
			workspace.Show(smartPart, info);

			Assert.AreEqual(0, workspace.SelectedIndex);
		}

		#endregion

		#region Hide

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowsIfSmartPartNotControlHide()
		{
			workspace.Hide(new NonControlSmartPart());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowsIfSPNotInManagerHide()
		{
			workspace.Hide(sp);
		}

		[TestMethod]
		public void CanHideTab()
		{
			workspace.Show(sp);
			workspace.Hide(sp);

			Assert.IsFalse(sp.Visible);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfSmartPartNullHide()
		{
			workspace.Hide(null);
		}

		[TestMethod]
		public void HideActivatesPreviousSmartPartIfThereIsOne()
		{
			MockSmartPart smartPartA = workItem.SmartParts.AddNew<MockSmartPart>();
			MockSmartPart smartPartB = workItem.SmartParts.AddNew<MockSmartPart>();
			workspace.Show(smartPartA);
			workspace.Show(smartPartB);

			workspace.Hide(smartPartB);

			Assert.AreSame(workspace.ActiveSmartPart, smartPartA);
		}

		[TestMethod]
		public void HideActivatesFollowingSiblingIfItIsFirstOne()
		{
			MockSmartPart smartPartA = workItem.SmartParts.AddNew<MockSmartPart>();
			MockSmartPart smartPartB = workItem.SmartParts.AddNew<MockSmartPart>();
			workspace.Show(smartPartA);
			workspace.Show(smartPartB);
			workspace.Show(smartPartA);

			workspace.Hide(smartPartA);

			Assert.AreSame(workspace.ActiveSmartPart, smartPartB);
		}

		[TestMethod]
		public void HideDoesNotActivateAnythingIfNoOtherTabsExist()
		{
			MockSmartPart smartPartA = workItem.SmartParts.AddNew<MockSmartPart>();
			MockSmartPart smartPartB = workItem.SmartParts.AddNew<MockSmartPart>();
			MockSmartPart smartPartC = workItem.SmartParts.AddNew<MockSmartPart>();
			workspace.Show(smartPartA);
			workspace.Show(smartPartB);
			workspace.Show(smartPartC);

			workspace.Close(smartPartC);

			workspace.Close(smartPartA);

			workspace.Activate(smartPartB);
			workspace.Hide(smartPartB);

			Assert.IsNull(workspace.ActiveSmartPart);
		}

        [TestMethod]
        public void HidindSmartPartDoesNothingAfterTheFirstHide()
        {
            MockSmartPart smartPartA = workItem.SmartParts.AddNew<MockSmartPart>();
			MockSmartPart smartPartB = workItem.SmartParts.AddNew<MockSmartPart>();
			MockSmartPart smartPartC = workItem.SmartParts.AddNew<MockSmartPart>();
			workspace.Show(smartPartA);
			workspace.Show(smartPartB);
			workspace.Show(smartPartC);

            workspace.Hide(smartPartC);
            Assert.AreSame(smartPartB, workspace.ActiveSmartPart);

            workspace.Hide(smartPartC);
            Assert.AreSame(smartPartB, workspace.ActiveSmartPart);
        }

		#endregion

		#region Close

		[TestMethod]
		public void WorkspaceFiresSmartPartClosing()
		{
			bool closing = false;
			MockSmartPart smartPart = new MockSmartPart();
			smartPart.Name = "SP";
			workItem.Items.Add(smartPart);
			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate { closing = true; };

			workspace.Close(smartPart);

			Assert.IsTrue(closing);
		}

		[TestMethod]
		public void CanCancelSmartPartClosing()
		{
			MockSmartPart smartPart = new MockSmartPart();
			smartPart.Name = "SP";
			workItem.Items.Add(smartPart);
			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate(object sender, WorkspaceCancelEventArgs args) { args.Cancel = true; };

			workspace.Close(smartPart);

			Assert.IsFalse(smartPart.IsDisposed);
		}
		
		[TestMethod]
		public void ClosingByDisposingControlDoesNotFireClosingEvent()
		{
			bool closing = false;
			MockSmartPart smartPart = new MockSmartPart();
			smartPart.Name = "SP";
			workItem.Items.Add(smartPart);
			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate { closing = true; };

			smartPart.Dispose();

			Assert.IsFalse(closing);
			Assert.AreEqual(0, workspace.SmartParts.Count);
		}

		[TestMethod]
		public void CanRetrieveSmartPartFromEventArgs()
		{
			object smartPartObject1 = null;
			MockSmartPart smartPart = new MockSmartPart();
			smartPart.Name = "SP";
			workItem.Items.Add(smartPart);
			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate(object sender, WorkspaceCancelEventArgs args) { smartPartObject1 = args.SmartPart; };

			workspace.Close(smartPart);

			Assert.AreEqual(smartPart, smartPartObject1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowsIfSmartPartNotControlClose()
		{
			workspace.Close(new NonControlSmartPart());
		}

		[TestMethod]
		public void CloseRemovesTabButNotDisposesIt()
		{
			workspace.Show(sp);
			workspace.Close(sp);

			Assert.AreEqual(0, workspace.TabPages.Count);
			Assert.IsFalse(sp.IsDisposed);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowsIfSPNotInManagerClose()
		{
			workspace.Close(sp);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfSmartPartNullShow()
		{
			workspace.Show(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfSmartPartNullClose()
		{
			workspace.Close(null);
		}

		[TestMethod]
		public void CloseTabPage()
		{
			MockSmartPart sampleSmartPart = new MockSmartPart();
			workItem.Items.Add(sampleSmartPart, "SampleSmartPart");
			workItem.Items.Add(workspace);
			workspace.Show(sampleSmartPart);

			workspace.Close(sampleSmartPart);

			Assert.AreEqual(0, workspace.TabPages.Count);
		}

		[TestMethod]
		public void RemovingSelectedTabFiresSelection()
		{
			int activated = 0;
			WorkItem cc = workItem.Items.AddNew<WorkItem>();
			MockSmartPart smartPart = new MockSmartPart();
			smartPart.Name = "SP";
			MockSmartPart smartPart2 = new MockSmartPart();
			smartPart.Name = "SP2";
			cc.Items.Add(smartPart);
			cc.Items.Add(smartPart2);

			Form form = new Form();
			form.Controls.Add(workspace);
			workspace.Dock = DockStyle.Fill;
			form.Show();

			workspace.Show(smartPart);
			workspace.Show(smartPart2);

			workspace.SmartPartActivated += delegate { activated++; };
			workspace.Close(smartPart2);

			Assert.AreEqual(1, activated);
		}

		[TestMethod]
		public void RemovingOneTabDoesNotFiresSelection()
		{
			int activated = 0;
			MockSmartPart smartPart = new MockSmartPart();
			smartPart.Name = "SP";
			workItem.Items.Add(smartPart);

			Form form = new Form();
			form.Controls.Add(workspace);
			workspace.Dock = DockStyle.Fill;
			form.Show();

			workspace.SmartPartActivated += delegate { activated++; };

			workspace.Show(smartPart);
			workspace.Close(smartPart);

			Assert.AreEqual(1, activated);
		}

		[TestMethod]
		public void CloseTabPageAddedAtDesignTime()
		{
			// Create new instance to simulate design-time creation.
			workspace = new TabWorkspace();
			workItem.Items.Add(workspace);
			MockSmartPart sp1 = new MockSmartPart();
			workItem.Items.Add(sp1);

			TabPage page = new TabPage();
			page.Controls.Add(sp1);
			sp1.Dock = DockStyle.Fill;
			page.Name = Guid.NewGuid().ToString();
			workspace.TabPages.Add(page);

			owner = new Form();
			owner.Controls.Add(workspace);
			owner.Show();

			workspace.Close(sp1);

			Assert.AreEqual(0, workspace.TabPages.Count);
		}

		#endregion
		
		#region Misc

		[TestMethod]
		public void SelectTabWithNoSmartPartResetsActiveSmartPart()
		{
			workspace.SelectedIndex = -1;

			workspace.TabPages.Add("Foo");
			workspace.Show(sp);
			workspace.TabPages.Add("Bar");

			workspace.SelectedIndex = 0;
			workspace.SelectedIndex = 1;
			workspace.SelectedIndex = 0;

			Assert.IsNull(workspace.ActiveSmartPart);
		}

		#endregion

		#region Dispose

		[TestMethod]
		public void TabIsRemovedWhenWorkItemIsDisposed()
		{
			TabWorkspace workspace = new TabWorkspace();
			workItem.Items.Add(workspace);

			workItem.Workspaces.Add(workspace, "TabWorkspace");

			WorkItem child = workItem.Items.AddNew<WorkItem>();
			Control smartPart = new Control();
			child.Items.Add(smartPart);

			TabWorkspace myWS = (TabWorkspace)child.Workspaces["TabWorkspace"];
			myWS.Show(smartPart);

			Assert.AreEqual(1, workspace.TabCount);
			Assert.IsTrue(workspace.Contains(smartPart));

			child.Dispose();

			Assert.IsFalse(workspace.Contains(smartPart));
			Assert.AreEqual(0, workspace.TabCount);

		}

		[TestMethod]
		public void TabIsRemovedWhenSmartPartIsDisposed()
		{
			workspace.Show(sp);
			Assert.AreEqual(1, workspace.TabPages.Count);

			sp.Dispose();

			Assert.AreEqual(0, workspace.TabPages.Count);
		}

		[TestMethod]
		public void TabWorkspaceFiresDisposedEvent()
		{
			bool disposed = false;
			Form form = new Form();
			form.Controls.Add(workspace);
			form.Show();

			workspace.Disposed += delegate { disposed = true; };
			form.Close();

			Assert.IsTrue(disposed);
		}

        [TestMethod]
        public void SPIsRemovedFromWorkspaceWhenDisposed1()
        {
            MockSmartPart smartPartA = new MockSmartPart();

            TabSmartPartInfo spInfoA = new TabSmartPartInfo();
            spInfoA.Title = "Smart Part A";

            workItem.SmartParts.Add(smartPartA);

            workspace.Show(smartPartA, spInfoA);

            Assert.AreEqual(1, workspace.TabPages.Count);

            smartPartA.Dispose();

            Assert.AreEqual(0, workspace.TabPages.Count);

            // Returns 1
            Assert.AreEqual(0, workspace.SmartParts.Count);
        }

		#endregion
		
		#region Activate
		
		[TestMethod]
		public void ActivatingMultipleTimesRaisesActivatedEventOnlyOnce()
		{
			int activateCalls = 0;
			TabSmartPartInfo info = new TabSmartPartInfo();
			info.ActivateTab = true;
			workspace.SmartPartActivated += delegate { activateCalls++; };

			// First call to event should happen here.
			workspace.Show(sp);
			Control smartPart = new Control();
			// Second call to event here.
			workspace.Show(smartPart, info);

			// No further calls should happen.
			workspace.Activate(smartPart);
			workspace.Activate(smartPart);

			Assert.AreEqual(2, activateCalls);
		}


	#endregion

		#region Supporting classes

		[SmartPart]
		private class SmartPartWithPlaceholder : UserControl
		{
			SmartPartPlaceholder placeholder = new SmartPartPlaceholder();

			public SmartPartPlaceholder Placeholder
			{
				get { return placeholder; }
			}

			public SmartPartWithPlaceholder()
			{
				placeholder.SmartPartName = "ChildControl";
				this.Controls.Add(placeholder);
			}
		}

		[SmartPart]
		private class SimpleUserControlSmartPart : UserControl
		{
		}

		private class ChildWorkItem : WorkItem
		{
			public SimpleUserControlSmartPart ContainedSmartPart;
			public SmartPartWithPlaceholder ContainingSmartPart;

			public void Run(IWorkspace workspace)
			{
				ContainedSmartPart = this.Items.AddNew<SimpleUserControlSmartPart>("ChildControl");
				ContainingSmartPart = this.Items.AddNew<SmartPartWithPlaceholder>("ParentControl");

				workspace.Show(ContainingSmartPart);
			}
		}

		[SmartPart]
		private class NonControlSmartPart : Object { }

		[SmartPart]
		private class MockSmartPart : Control
		{
			TabSmartPartInfo info = new TabSmartPartInfo();

			public MockSmartPart()
			{
				info.Title = "Title";
				info.Description = "Description";
			}
		}

		#endregion
	}
}
