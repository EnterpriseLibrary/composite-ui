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
using Microsoft.Practices.CompositeUI.SmartParts;
using System.Collections.Generic;

namespace Microsoft.Practices.CompositeUI.Tests.SmartParts
{
	[TestClass]
	public class WorkspaceFixture
	{
		private static MockWorkspace workspace;
		private static WorkItem workItem;

		[TestInitialize]
		public void SetUp()
		{
			workItem = new TestableRootWorkItem();
			workspace = workItem.Items.AddNew<MockWorkspace>();
		}

		#region ActiveSmartPart

		[TestMethod]
		public void WorkspaceRemembersActiveSP()
		{
			MockSP sp = new MockSP();

			workspace.Show(sp);
			workspace.Activate(sp);

			Assert.AreSame(sp, workspace.ActiveSmartPart);
		}

		[TestMethod]
		public void DerivedCanSetActiveSmartPart()
		{
			MockSP sp1 = new MockSP();
			MockSP sp2 = new MockSP();
			workspace.Show(sp1);
			workspace.Show(sp2);

			workspace.SetActiveSmartPart(sp1);

			Assert.AreSame(sp1, workspace.ActiveSmartPart);
		}

		[TestMethod]
		public void DerivedCanSetActiveSmartPartNull()
		{
			MockSP sp1 = new MockSP();
			MockSP sp2 = new MockSP();
			workspace.Show(sp1);
			workspace.Show(sp2);

			workspace.SetActiveSmartPart(null);

			Assert.IsNull(workspace.ActiveSmartPart);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ThrowsIfActiveSmartPartSetNotShown()
		{
			MockSP sp1 = new MockSP();
			workspace.Show(sp1);

			workspace.SetActiveSmartPart(new MockSP());
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ThrowsIfActiveSmartPartNotCompatible()
		{
			MockSP sp1 = new MockSP();
			workspace.Show(sp1);

			workspace.SetActiveSmartPart(new object());
		}

		#endregion

		[TestMethod]
		public void CanEnumerateSPs()
		{
			int count = 0;
			MockSP sp = new MockSP();
			MockSP sp1 = new MockSP();
			MockSP sp2 = new MockSP();

			workspace.Show(sp);
			workspace.Show(sp1);
			workspace.Show(sp2);

			foreach (MockSP mock in workspace.SmartParts)
			{
				count++;
			}

			Assert.AreEqual(3, count);
		}

		#region Activate

		[TestMethod]
		public void ActivateCallsActivateDerived()
		{
			MockSP sp = new MockSP();
			workspace.Show(sp);

			workspace.Activate(sp);

			Assert.AreEqual(1, workspace.ShowCalls);
			Assert.AreEqual(1, workspace.ActivateCalls);
			Assert.AreEqual(0, workspace.ApplySPICalls);
		}

		[TestMethod]
		public void ActivateFiresActivateEvent()
		{
			bool eventCalled = false;
			MockSP sp = new MockSP();
			workspace.Show(sp);
			workspace.SmartPartActivated += delegate { eventCalled = true; };

			workspace.Activate(sp);

			Assert.IsTrue(eventCalled);
		}

		[TestMethod]
		public void CallingActivateOnActiveSmartPartDoesNotCallDerivedOrFireEvent()
		{
			int eventCalls = 0;
			MockSP sp = new MockSP();
			workspace.Show(sp);
			workspace.SmartPartActivated += delegate { eventCalls++; };

			workspace.Activate(sp);
			workspace.Activate(sp);

			Assert.AreEqual(1, eventCalls);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ThrowsIfActivateNullSP()
		{
			workspace.Activate(null);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ThrowsIfActivateUnsupportedSP()
		{
			workspace.Activate(new object());
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ThrowsIfActivateSPNotShown()
		{
			workspace.Activate(new MockSP());
		}

		#endregion

		#region ApplySmartPartInfo

		[TestMethod]
		public void ApplyCallsApplySPIDerived()
		{
			MockSP sp = new MockSP();
			workspace.Show(sp);

			workspace.ApplySmartPartInfo(sp, new MockSPI());

			Assert.AreEqual(1, workspace.ApplySPICalls);
		}

		[TestMethod]
		public void ApplyDoesNotActivate()
		{
			MockSP sp = new MockSP();
			workspace.Show(sp);

			workspace.ApplySmartPartInfo(sp, new MockSPI());

			Assert.AreEqual(0, workspace.ActivateCalls);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfApplyNullSP()
		{
			workspace.ApplySmartPartInfo(null, new MockSPI());
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ThrowsIfApplyNullSPI()
		{
			workspace.ApplySmartPartInfo(new MockSP(), null);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ThrowsIfApplyUnsupportedSP()
		{
			workspace.ApplySmartPartInfo(new object(), new MockSPI());
		}

		[TestMethod]
		public void CanApplyUnsupportedSPI()
		{
			MockSP sp = new MockSP();
			workspace.Show(sp);
			workspace.ApplySmartPartInfo(sp, new SmartPartInfo());
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ThrowsIfApplySPNotShown()
		{
			workspace.ApplySmartPartInfo(new MockSP(), new MockSPI());
		}

		#endregion

		#region Show

		[TestMethod]
		public void ResolveToProviderIfNoWorkItem()
		{
			MockSPI spi = new MockSPI();
			MockSPProvider sp = new MockSPProvider(spi);
			MockWorkspace ws = new MockWorkspace();

			ws.Show(sp);

			Assert.AreSame(spi, ws.LastSPI);
		}

		[TestMethod]
		public void ResolveToWorkItemIfConcreteSPIRegistered()
		{
			MockSPI spprovider = new MockSPI();
			MockSPI spworkitem = new MockSPI();
			MockSPProvider sp = new MockSPProvider(spprovider);
			workItem.RegisterSmartPartInfo(sp, spworkitem);

			workspace.Show(sp);

			Assert.AreSame(spworkitem, workspace.LastSPI);
		}

		[TestMethod]
		public void ResolveToProviderIfGenericSPIRegistered()
		{
			MockSPI spprovider = new MockSPI();
			SmartPartInfo spworkitem = new SmartPartInfo();
			MockSPProvider sp = new MockSPProvider(spprovider);
			workItem.RegisterSmartPartInfo(sp, spworkitem);

			workspace.Show(sp);

			Assert.AreSame(spprovider, workspace.LastSPI);
		}

		[TestMethod]
		public void ResolveToGenericInWorkItem()
		{
			SmartPartInfo spprovider = new SmartPartInfo("foo", "");
			SmartPartInfo spworkitem = new SmartPartInfo("bar", "");
			MockSPProvider sp = new MockSPProvider(spprovider);
			workItem.RegisterSmartPartInfo(sp, spworkitem);

			workspace.Show(sp);

			Assert.AreEqual("bar", workspace.LastSPI.Title);
		}

		[TestMethod]
		public void ShowWithGenericSPIRegisteredCallsConvertToConcreteType()
		{
			MockSP sp = new MockSP();
			workItem.RegisterSmartPartInfo(sp, new SmartPartInfo("foo", "bar"));

			workspace.Show(sp);

			Assert.AreEqual(1, workspace.ConvertCalls);
			Assert.AreEqual("foo", workspace.LastSPI.Title);
			Assert.AreEqual("bar", workspace.LastSPI.Description);
		}

		[TestMethod]
		public void ShowWithNoSPIAndNoWorkItemSetCreatesNew()
		{
			MockWorkspace ws = new MockWorkspace();
			ws.Show(new MockSP());

			Assert.AreEqual(1, ws.ShowCalls);
			Assert.IsNotNull(ws.LastSPI);
		}

		[TestMethod]
		public void ShowWithNoConcreteOrGenericSPIAndWorkItemSetCreatesNew()
		{
			workspace.Show(new MockSP());

			Assert.AreEqual(1, workspace.ShowCalls);
			Assert.IsNotNull(workspace.LastSPI);
		}

		[TestMethod]
		public void NewSPIForShowCanBeOverriden()
		{
			workspace.Show(new MockSP());

			Assert.IsTrue(workspace.LastSPI.Custom);
		}

		[TestMethod]
		public void ShowCallsOnShowDerived()
		{
			workspace.Show(new MockSP());

			Assert.AreEqual(1, workspace.ShowCalls);
		}

		[TestMethod]
		public void ShowWithSPICallsShowDerived()
		{
			workspace.Show(new MockSP(), new MockSPI());

			Assert.AreEqual(1, workspace.ShowCalls);
		}

		[TestMethod]
		public void ShowTwiceActivatesButNotShowsOrAppliesInfo()
		{
			MockSP sp = new MockSP();
			workspace.Show(sp);
			Assert.AreEqual(1, workspace.ShowCalls);
			Assert.AreEqual(0, workspace.ActivateCalls);

			workspace.Show(sp);

			Assert.AreEqual(1, workspace.ShowCalls);
			Assert.AreEqual(1, workspace.ActivateCalls);
			Assert.AreEqual(0, workspace.ApplySPICalls);
		}

		[TestMethod]
		public void ShowTwiceWithSPIActivatesAndAppliesInfoButNotShows()
		{
			MockSP sp = new MockSP();
			workspace.Show(sp);
			Assert.AreEqual(1, workspace.ShowCalls);
			Assert.AreEqual(0, workspace.ActivateCalls);

			workspace.Show(sp, new MockSPI());

			Assert.AreEqual(1, workspace.ShowCalls);
			Assert.AreEqual(1, workspace.ActivateCalls);
			Assert.AreEqual(1, workspace.ApplySPICalls);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfShowNullSP()
		{
			workspace.Show(null);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ThrowsIfShowNullSPI()
		{
			workspace.Show(new MockSP(), null);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ThrowsIfShowUnsupportedSP()
		{
			workspace.Show(new object());
		}

		[TestMethod]
		public void CanShowWithUnsupportedSPI()
		{
			workspace.Show(new MockSP(), new SmartPartInfo());
		}

		#endregion

		#region Hide

		[TestMethod]
		public void HideResetsActiveSmartPartIfHidingActiveSmartPart()
		{
			MockSP sp = new MockSP();
			workspace.Show(sp);
			workspace.Activate(sp);

			workspace.Hide(sp);

			Assert.IsNull(workspace.ActiveSmartPart);
		}

		[TestMethod]
		public void HideDoesNotResetsActiveSmartPartIfHidingNonActiveSmartPart()
		{
			MockSP sp = new MockSP();
			MockSP sp1 = new MockSP();
			workspace.Show(sp);
			workspace.Show(sp1);
			workspace.Activate(sp1);

			workspace.Hide(sp);

			Assert.AreSame(workspace.ActiveSmartPart, sp1);
		}

		[TestMethod]
		public void HideCallsHideDerived()
		{
			MockSP sp = new MockSP();
			workspace.Show(sp);

			workspace.Hide(sp);

			Assert.AreEqual(1, workspace.HideCalls);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ThrowsIfHideNullSP()
		{
			workspace.Hide(null);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ThrowsIfHideUnsupportedSP()
		{
			workspace.Hide(new object());
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ThrowsIfHideSPNotShown()
		{
			workspace.Hide(new MockSP());
		}

		#endregion

		#region Close

		[TestMethod]
		public void CloseRemovesFromSmartPartsDictionary()
		{
			MockSP sp = new MockSP();
			workspace.Show(sp);
			workspace.Show(new MockSP());

			workspace.Close(sp);

			Assert.AreEqual(1, workspace.SmartParts.Count);
		}

		[TestMethod]
		public void CloseResetsActiveSmartPartIfClosingActiveSmartPart()
		{
			MockSP sp = new MockSP();
			workspace.Show(sp);
			workspace.Activate(sp);

			workspace.Close(sp);

			Assert.IsNull(workspace.ActiveSmartPart);
		}

		[TestMethod]
		public void CloseDoesNotResetsActiveSmartPartIfClosingNonActiveSmartPart()
		{
			MockSP sp = new MockSP();
			MockSP sp1 = new MockSP();
			workspace.Show(sp);
			workspace.Show(sp1);
			workspace.Activate(sp1);

			workspace.Close(sp);

			Assert.IsNotNull(workspace.ActiveSmartPart);
			Assert.AreSame(workspace.ActiveSmartPart, sp1);
		}

		[TestMethod]
		public void CloseCallsCloseDerived()
		{
			MockSP sp = new MockSP();
			workspace.Show(sp);

			workspace.Close(sp);

			Assert.AreEqual(1, workspace.CloseCalls);
		}

		[TestMethod]
		public void CloseFiresSmartPartClosingEvent()
		{
			bool eventCalled = false;
			MockSP sp = new MockSP();
			workspace.Show(sp);
			workspace.SmartPartClosing += delegate { eventCalled = true; };

			workspace.Close(sp);

			Assert.IsTrue(eventCalled);
		}

		[TestMethod]
		public void DerivedOnCloseCalledIfClosingNotCancelled()
		{
			MockSP sp = new MockSP();
			workspace.Show(sp);
			workspace.SmartPartClosing += delegate(object sender, WorkspaceCancelEventArgs e) { e.Cancel = true; };

			workspace.Close(sp);

			Assert.AreEqual(0, workspace.CloseCalls);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ThrowsIfCloseNullSP()
		{
			workspace.Close(null);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ThrowsIfCloseUnsupportedSP()
		{
			workspace.Close(new object());
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ThrowsIfCloseSPNotShown()
		{
			workspace.Close(new MockSP());
		}

		#endregion



		[TestMethod]
		public void SmartPartShowInChildWorkItemParentWorkspaceUsesChildSmartPartInfo()
		{
			WorkItem workItemA = new TestableRootWorkItem();
			WorkItem workItemB = workItemA.WorkItems.AddNew<WorkItem>();
			MockSP sp = new MockSP();
            MockSPI spi = new MockSPI("Title", "Description");

			MockWorkspace workspace = workItemA.Workspaces.AddNew<MockWorkspace>();
			workItemB.RegisterSmartPartInfo(sp, spi);

			workspace.Show(sp);

            Assert.AreSame(spi, workspace.LastSPI);
		}


        [TestMethod]
        public void SmartPartInfoIsRemovedWhenChildWorkItemIsDisposed()
        {
         	WorkItem workItemA = new TestableRootWorkItem();
			WorkItem workItemB = workItemA.WorkItems.AddNew<WorkItem>();
			MockSP sp = new MockSP();
            MockSPI spi = new MockSPI("Title", "Description");

			MockWorkspace workspace = workItemA.Workspaces.AddNew<MockWorkspace>();
			workItemB.RegisterSmartPartInfo(sp, spi);

            workItemB.Dispose();

            workspace.Show(sp);

            Assert.IsFalse(spi == workspace.LastSPI);   
        }

		#region Helper classes

		class MockSPProvider : MockSP, ISmartPartInfoProvider
		{
			ISmartPartInfo spi;

			public MockSPProvider(ISmartPartInfo spi)
			{
				this.spi = spi;
			}

			#region ISmartPartInfoProvider Members

			public ISmartPartInfo GetSmartPartInfo(Type smartPartInfoType)
			{
				if (smartPartInfoType.IsAssignableFrom(spi.GetType()))
				{
					return spi;
				}

				return null;
			}

			#endregion
		}



		class MockSP
		{
		}

		class MockSPI : ISmartPartInfo
		{
			public MockSPI()
			{
			}

			public MockSPI(string title, string description)
			{
				this.title = title;
				this.description = description;
			}

			private string description;

			public string Description
			{
				get { return description; }
				set { description = value; }
			}

			private string title;

			public string Title
			{
				get { return title; }
				set { title = value; }
			}

			private bool custom = false;

			public bool Custom
			{
				get { return custom; }
				set { custom = value; }
			}
		}

		class MockWorkspace : Workspace<MockSP, MockSPI>
		{
			public int ActivateCalls;
			public int ApplySPICalls;
			public int ShowCalls;
			public int HideCalls;
			public int CloseCalls;
			public int ConvertCalls;
			public MockSPI LastSPI;

			MockSP activeSP;

			List<MockSP> smartParts = new List<MockSP>();

			protected override void OnApplySmartPartInfo(MockSP smartPart, MockSPI smartPartInfo)
			{
				ApplySPICalls++;
				LastSPI = smartPartInfo;
			}

			protected override void OnActivate(MockSP smartPart)
			{
				ActivateCalls++;
				activeSP = smartPart;
			}

			protected override void OnShow(MockSP smartPart, MockSPI smartPartInfo)
			{
				ShowCalls++;
				smartParts.Add(smartPart);
				LastSPI = smartPartInfo;
			}

			protected override void OnHide(MockSP smartPart)
			{
				HideCalls++;
			}

			protected override void OnClose(MockSP smartPart)
			{
				CloseCalls++;
			}

			protected override MockSPI ConvertFrom(ISmartPartInfo source)
			{
				ConvertCalls++;
				return base.ConvertFrom(source);
			}

			internal new void SetActiveSmartPart(object smartPart)
			{
				base.SetActiveSmartPart(smartPart);
			}

			protected override MockSPI CreateDefaultSmartPartInfo(MockSP forSmartPart)
			{
				MockSPI spi = base.CreateDefaultSmartPartInfo(forSmartPart);

				spi.Custom = true;

				return spi;
			}
		}

		#endregion
	}
}
