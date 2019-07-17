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
using System.ComponentModel;

namespace Microsoft.Practices.CompositeUI.WinForms.Tests
{
	[TestClass]
	public class ZoneWorkspaceFixture
	{
		private static ZoneWorkspace workspace;
		private static Control control;
		private static WorkItem workItem;

		[TestInitialize]
		public void SetUp()
		{
			workItem = new TestableRootWorkItem();
			workspace = new ZoneWorkspace();
			control = new Control();

			workItem.Services.Add(typeof(IWorkItemActivationService), new SimpleWorkItemActivationService());
			workItem.Workspaces.Add(workspace);
			ISmartPartInfo info = new ZoneSmartPartInfo("Main");
			workItem.RegisterSmartPartInfo(control, info);
		}

		[TestMethod]
		public void CanSetControlZoneName()
		{
			workspace.Controls.Add(control);

			workspace.SetZoneName(control, "Main");

			Assert.AreEqual(1, workspace.Zones.Count);
			Assert.AreSame(control, workspace.Zones["Main"]);
			Assert.AreEqual("Main", workspace.GetZoneName(control));
		}

		[TestMethod]
		public void CanExtendScrollableControl()
		{
			ContainerControl cc = new ContainerControl();
			workspace.Controls.Add(cc);
			SplitContainer sc = new SplitContainer();
			workspace.Controls.Add(sc);
			FlowLayoutPanel fp = new FlowLayoutPanel();
			workspace.Controls.Add(fp);

			Assert.IsTrue(((IExtenderProvider)workspace).CanExtend(cc));
			Assert.IsTrue(((IExtenderProvider)workspace).CanExtend(sc.Panel1));
			Assert.IsTrue(((IExtenderProvider)workspace).CanExtend(sc));
			Assert.IsTrue(((IExtenderProvider)workspace).CanExtend(fp));
		}

		[TestMethod]
		public void SetControlZoneNameNotDescendentNoOp()
		{
			workspace.SetZoneName(control, "Main");

			Assert.AreEqual(0, workspace.Zones.Count);
		}

		[TestMethod]
		public void RemoveControlRemovesZoneName()
		{
			workspace.Controls.Add(control);
			workspace.SetZoneName(control, "Main");

			workspace.Controls.Remove(control);

			Assert.AreEqual(0, workspace.Zones.Count);
		}

		[TestMethod]
		public void DisposeControlRemovesZoneName()
		{
			workspace.Controls.Add(control);
			workspace.SetZoneName(control, "Main");

			control.Dispose();

			Assert.AreEqual(0, workspace.Zones.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShowNoZonesThrows()
		{
			workspace.Show(new Control());
		}


		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ShowWithNonExistingZoneNameThrows()
		{
			Control zone = new Control();
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Main");
			workspace.Show(control, new ZoneSmartPartInfo("Blah"));
		}

		[TestMethod]
		public void ShowOnZoneAddsControlAndSetsVisible()
		{
			Control zone = new Control();
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Main");

			workspace.Show(control, new ZoneSmartPartInfo("Main"));

			Assert.AreEqual(1, zone.Controls.Count);
			Assert.IsTrue(control.Visible);
		}

		[TestMethod]
		public void HideSmartPartHidesControl()
		{
			Control zone = new Control();
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Main");

			workspace.Show(control, new ZoneSmartPartInfo("Main"));
			workspace.Hide(control);

			Assert.AreEqual(1, zone.Controls.Count);
			Assert.IsFalse(control.Visible);
		}

		[TestMethod]
		public void CloseSmartPartRemovesControl()
		{
			Control zone = new Control();
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Main");

			workspace.Show(control, new ZoneSmartPartInfo("Main"));
			workspace.Close(control);

			Assert.AreEqual(0, zone.Controls.Count);
            Assert.IsFalse(control.IsDisposed);
		}

		[TestMethod]
		public void CloseSmartPartFiresClosing()
		{
			Control zone = new Control();
			bool closingcalled = false;
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Main");
			workspace.SmartPartClosing += delegate { closingcalled = true; };

			workspace.Show(control, new ZoneSmartPartInfo("Main"));
			workspace.Close(control);

			Assert.IsTrue(closingcalled);
		}

		[TestMethod]
		public void CancelClosingDoesNotRemoveControl()
		{
			Control zone = new Control();
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Main");
			workspace.SmartPartClosing += delegate(object sender, WorkspaceCancelEventArgs e) { e.Cancel = true; };

			workspace.Show(control, new ZoneSmartPartInfo("Main"));
			workspace.Close(control);

			Assert.AreEqual(1, zone.Controls.Count);
		}

		[TestMethod]
		public void ClosingByDisposingControlDoesNotFireClosingEvent()
		{
			bool closing = false;
			Control zone = new Control();
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Main");
			workspace.SmartPartClosing += delegate { closing = true; };

			workspace.Show(control, new ZoneSmartPartInfo("Main"));
			control.Dispose();

			Assert.IsFalse(closing);
			Assert.AreEqual(0, workspace.SmartParts.Count);
		}

		[TestMethod]
		public void ShowNoParamsShowsInDefaultZoneDesigner()
		{
			ZoneWorkspaceForm form = new ZoneWorkspaceForm();
			workItem.Items.Add(form.Workspace);
			MonthCalendar calendar = new MonthCalendar();
			workItem.RegisterSmartPartInfo(calendar, new ZoneSmartPartInfo("ContentZone"));

			form.Workspace.Show(calendar);

			Assert.AreEqual(1, form.Workspace.Zones["ContentZone"].Controls.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowsIfHideNotShownControl()
		{
			Control sampleControl = new Control();

			workspace.Hide(sampleControl);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowsIfCloseNotShownControl()
		{
			Control sampleControl = new Control();

			workspace.Close(sampleControl);
		}

		[TestMethod]
		public void FocusingSmartPartFiresActivated()
		{
			ZoneWorkspaceForm form = CreateFormAddWorkspace();
			Control zone = new Control();
			bool activated = false;

			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Main");
			workspace.SmartPartActivated += delegate { activated = true; };
			form.Show();

			workspace.Show(control, new ZoneSmartPartInfo("Main"));
			workspace.Zones["Main"].Focus();

			Assert.IsTrue(activated);
		}

		[TestMethod]
		public void FocusingSmartPartFiresActivatedWithSmartPart()
		{
			ZoneWorkspaceForm form = CreateFormAddWorkspace();
			Control zone = new Control();
			object argsSmartPart = null;

			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Main");
			workspace.SmartPartActivated +=
				delegate(object sender, WorkspaceEventArgs args) { argsSmartPart = args.SmartPart; };
			form.Show();

			workspace.Show(control, new ZoneSmartPartInfo("Main"));
			workspace.Zones["Main"].Focus();

			Assert.AreEqual(control, argsSmartPart);
		}

		[TestMethod]
		public void ActivatedFiresCorrectNumberOfTimes()
		{
			ZoneWorkspaceForm form = CreateFormAddWorkspace();
			Control zone = new Control();
			Control zone1 = new Control();
			Control control2 = new Control();
			int activated = 0;

			AddZones(zone, zone1);
			workspace.SmartPartActivated +=
				delegate(object sender, WorkspaceEventArgs args) { activated++; };
			form.Show();

			workspace.Show(control, new ZoneSmartPartInfo("Main"));
			workspace.Show(control2, new ZoneSmartPartInfo("Main1"));

			control.Select();
			control2.Select();
			control.Select();

			//Will fire five times because it fires when show is called.
			Assert.AreEqual(5, activated);
		}

		[TestMethod]
		public void ControlIsRemovedWhenSmartPartIsDisposed()
		{
			Control zone = new Control();
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Main");
			workspace.Show(control);
			Assert.IsTrue(workspace.Zones["Main"].Contains(control));

			control.Dispose();

			Assert.IsFalse(workspace.Zones["Main"].Contains(control));
		}

		[TestMethod]
		public void ZoneDocksControlCorrectly()
		{
			Control zone = new Control();
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Main");
			ZoneSmartPartInfo info = new ZoneSmartPartInfo();
			info.Dock = DockStyle.Fill;
			workspace.Show(control, info);

			Assert.AreEqual(DockStyle.Fill, workspace.Zones["Main"].Controls[0].Dock);
		}

		[TestMethod]
		public void WorkspaceGetsRegisteredSPI()
		{
			Control zone = new Control();
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Main");
			ZoneSmartPartInfo info = new ZoneSmartPartInfo();
			info.Dock = DockStyle.Fill;

			workItem.RegisterSmartPartInfo(control, info);
			workspace.Show(control, info);

			Assert.AreEqual(DockStyle.Fill, workspace.Zones["Main"].Controls[0].Dock);
		}

		[TestMethod]
		public void ZoneDoesNotFireSPActivatedEventIfNoSPPresent()
		{
			Control zone1 = new Control();
			workspace.Controls.Add(zone1);

			workspace.SetZoneName(zone1, "TestZone");

			Assert.AreEqual("TestZone", workspace.GetZoneName(zone1));

			bool activatedCalled = false;
			workspace.SmartPartActivated += delegate(object sender, WorkspaceEventArgs e)
			{
				activatedCalled = true;
			};

			zone1.Focus();

			Assert.IsFalse(activatedCalled);
		}

		[TestMethod]
		public void CanShowInDefaultZone()
		{
			Control zone = new Control();
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "TestZone");
			workspace.SetIsDefaultZone(zone, true);

			Control smartPartA = new Control();
			workspace.Show(smartPartA);

			Assert.IsTrue(workspace.GetIsDefaultZone(zone));
			Assert.AreSame(smartPartA, workspace.Zones["TestZone"].Controls[0]);
		}

		[TestMethod]
		public void CanShowInDefaultZoneWithInfoNoZoneName()
		{
			Control zone = new Control();
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "TestZone");
			workspace.SetIsDefaultZone(zone, true);

			Control smartPartA = new Control();
			ZoneSmartPartInfo info = new ZoneSmartPartInfo();
			info.Dock = DockStyle.Fill;
			info.Title = "Test";
			workspace.Show(smartPartA, info);

			Assert.IsTrue(workspace.GetIsDefaultZone(zone));
			Assert.AreSame(smartPartA, workspace.Zones["TestZone"].Controls[0]);
		}

		[TestMethod]
		public void RemovingZoneUnregistersGotFocusEvent()
		{
			Control zone1 = new Control();
			workspace.Controls.Add(zone1);

			workspace.SetZoneName(zone1, "TestZone");

			Assert.AreEqual("TestZone", workspace.GetZoneName(zone1));

			workspace.Controls.Remove(zone1);

			Assert.IsNull(workspace.GetZoneName(zone1));
			Assert.AreEqual(0, workspace.Zones.Count);

			bool activatedCalled = false;
			workspace.SmartPartActivated += delegate(object sender, WorkspaceEventArgs e)
			{
				activatedCalled = true;
			};

			ControlSmartPart sp = new ControlSmartPart();
			zone1.Controls.Add(sp);
			Form form1 = new Form();
			form1.Controls.Add(zone1);
			form1.Show();

			Assert.IsFalse(activatedCalled);
		}

		[TestMethod]
		public void ActivatingChildControlRaisesSmartPartActivatedForContainingSP()
		{
			ZoneWorkspaceForm zoneForm = new ZoneWorkspaceForm();
			zoneForm.Show();
			Control sp = new Control();
			sp.Size = new System.Drawing.Size(50, 50);
			sp.Text = "Foo";
			TextBox tb = new TextBox();
			sp.Controls.Add(tb);
			zoneForm.Workspace.Show(sp);
			zoneForm.Workspace.Show(new Control());

			Control received = null;
			zoneForm.Workspace.SmartPartActivated += delegate(object sender, WorkspaceEventArgs e)
			{
				received = (Control)e.SmartPart;
			};

			tb.Select();

			Assert.AreSame(sp, received);
		}

		[TestMethod]
		public void RenamingZoneDoesNotRegisterEventsMultipleTimes()
		{
			int activatedCalled = 0;
			ZoneWorkspaceForm zoneForm = new ZoneWorkspaceForm();
			zoneForm.Show();
			Control zone1 = new Control();

			zoneForm.Workspace.Controls.Add(zone1);
			zoneForm.Workspace.SetZoneName(zone1, "TestZone");

			//rename
			zoneForm.Workspace.SetZoneName(zone1, "NewZone");

			zoneForm.Workspace.SmartPartActivated += delegate(object sender, WorkspaceEventArgs e)
			{
				activatedCalled++;
			};

			zoneForm.Workspace.Show(new Control(), new ZoneSmartPartInfo("NewZone"));

			Assert.AreEqual("NewZone", zoneForm.Workspace.GetZoneName(zone1));
			Assert.AreEqual(1, activatedCalled);
		}

		[TestMethod]
		public void ShowFiresActivatedEventWithSPAsParameter()
		{
			ControlSmartPart smartPartA = new ControlSmartPart();
			ZoneSmartPartInfo smartPartInfoA = new ZoneSmartPartInfo();
			smartPartInfoA.ZoneName = "Zone";

			Control zone = new Control();
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Zone");

			bool activatedCalled = false;
			workspace.SmartPartActivated += delegate(object sender, WorkspaceEventArgs e)
			{
				activatedCalled = true;
				Assert.AreSame(e.SmartPart, smartPartA);
			};

			workspace.Show(smartPartA, smartPartInfoA);
			Assert.IsTrue(smartPartA.Visible);

			Assert.IsTrue(activatedCalled);
		}

        [TestMethod]
        public void RemovingZoneFromWorkspaceRemovesContainedSmartPart()
        {
            Control zone = new Control();
            workspace.Controls.Add(zone);

            workspace.SetZoneName(zone, "TestZone3");

            ControlSmartPart smartPartA = new ControlSmartPart();
            ZoneSmartPartInfo spInfoA = new ZoneSmartPartInfo();
            spInfoA.ZoneName = "TestZone3";

            workspace.Show(smartPartA, spInfoA);
            zone.Focus();
            Form f = new Form();
            f.Controls.Add(zone);

            //Fails
            Assert.IsFalse(workspace.SmartParts.Contains(smartPartA));
        }


		[TestMethod]
		public void RemovingControlChainUnregistersZone()
		{
			Control parent = new Control();
			Control zone1 = new Control();
			parent.Controls.Add(zone1);
			workspace.Controls.Add(parent);
			workspace.SetZoneName(zone1, "TestZone");

			workspace.Controls.Remove(parent);

			Assert.IsNull(workspace.GetZoneName(zone1));
			Assert.AreEqual(0, workspace.Zones.Count);
		}

		[TestMethod]
		public void SetZoneWithMultipleNames()
		{
			Control zone = new Control();
			workspace.Controls.Add(zone);

			workspace.SetZoneName(zone, "TestZone");
			workspace.SetZoneName(zone, "TestZone1");
			workspace.SetZoneName(zone, "TestZone2");
			workspace.SetZoneName(zone, "TestZone3");

			Assert.AreEqual("TestZone3", workspace.GetZoneName(zone));
			Assert.AreEqual(1, workspace.Zones.Count);
		}

		[TestMethod]
		public void RemovingControlChainUnregistersZones()
		{
			Control parent = new Control();
			Control zone1 = new Control();
			Control zone2 = new Control();

			parent.Controls.Add(zone1);
			parent.Controls.Add(zone2);

			workspace.Controls.Add(parent);
			workspace.SetZoneName(zone1, "TestZone");
			workspace.SetZoneName(zone2, "TestZone2");

			workspace.Controls.Remove(parent);

			Assert.IsNull(workspace.GetZoneName(zone1));
			Assert.AreEqual(0, workspace.Zones.Count);
		}

		[TestMethod]
		public void ShowGetsSmartPartInfoRegisteredWithWorkItem1()
		{
			ControlSmartPart smartPartA = new ControlSmartPart();

			ZoneSmartPartInfo smartPartInfoA = new ZoneSmartPartInfo();
			smartPartInfoA.ZoneName = "ZoneA";
			smartPartInfoA.Dock = DockStyle.Left;

			workItem.RegisterSmartPartInfo(smartPartA, smartPartInfoA);

			Control zoneA = new Control();
			workspace.Controls.Add(zoneA);
			workspace.SetZoneName(zoneA, "ZoneA");

			workspace.Show(smartPartA);

			Assert.IsTrue(workspace.Zones["ZoneA"].Controls.Contains(smartPartA));
			Assert.AreEqual(DockStyle.Left, smartPartA.Dock);
		}

		[TestMethod]
		public void FocusOnInnerControlActivatesContainingSmartPart()
		{
			ZoneWorkspaceForm form = new ZoneWorkspaceForm();
			form.Show();

			ControlSmartPart sp1 = new ControlSmartPart();
			sp1.Size = new System.Drawing.Size(50, 50);
			TextBox tb1 = new TextBox();
			sp1.Controls.Add(tb1);

			ControlSmartPart sp2 = new ControlSmartPart();
			sp2.Size = new System.Drawing.Size(50, 50);
			TextBox tb2 = new TextBox();
			sp2.Controls.Add(tb2);

			form.Workspace.Show(sp1, new ZoneSmartPartInfo("LeftZone"));
			form.Workspace.Show(sp2, new ZoneSmartPartInfo("ContentZone"));

			Assert.AreSame(sp2, form.Workspace.ActiveSmartPart);
			tb1.Select();
			Assert.AreSame(sp1, form.Workspace.ActiveSmartPart);
			tb2.Select();
			Assert.AreSame(sp2, form.Workspace.ActiveSmartPart);
		}

		[TestMethod]
		public void ShowWithNoSPIDoesNotOverrideDockInformation()
		{
			ControlSmartPart sp = new ControlSmartPart();
			sp.Dock = DockStyle.Fill;

			AddZones(new Control(), new Control());
			workspace.Show(sp);

			Assert.AreEqual(DockStyle.Fill, sp.Dock);
		}

        [TestMethod]
        public void FiresOneEventOnlyIfSmartPartIsShownMultipleTimes()
        {
            // Show First SmartPart
            ControlSmartPart smartPartA = new ControlSmartPart();
            ZoneSmartPartInfo smartPartInfoA = new ZoneSmartPartInfo();
            smartPartInfoA.ZoneName = "Zone";

            Control zone = new Control();
            workspace.Controls.Add(zone);
            workspace.SetZoneName(zone, "Zone");

            workspace.Show(smartPartA, smartPartInfoA);
            Assert.IsTrue(smartPartA.Visible);

            // Show Second SmartPart
            ControlSmartPart smartPartB = new ControlSmartPart();
            ZoneSmartPartInfo smartPartInfoB = new ZoneSmartPartInfo();
            smartPartInfoB.ZoneName = "Zone1";

            Control zone1 = new Control();
            workspace.Controls.Add(zone1);
            workspace.SetZoneName(zone1, "Zone1");

            workspace.Show(smartPartB, smartPartInfoB);
            Assert.IsTrue(smartPartB.Visible);

            // Show first SmartPart again
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
		public void DesignTimeControlsProcessedAtEndInit()
		{
			bool activated = false;
			workspace.SmartPartActivated += delegate { activated = true; };
			((ISupportInitialize)workspace).BeginInit();

			Control zone = new Control();
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Zone");

			zone.Controls.Add(new MonthCalendar());

			((ISupportInitialize)workspace).EndInit();

			Assert.AreEqual(1, workspace.SmartParts.Count);
			Assert.IsTrue(activated);
		}

		[TestMethod]
		public void DesignTimeControlsOnlyFirstLevelProcessed()
		{
			((ISupportInitialize)workspace).BeginInit();

			Control zone = new Control();
			workspace.Controls.Add(zone);
			workspace.SetZoneName(zone, "Zone");

			Control sp = new Control();
			Control inner = new Control();
			sp.Controls.Add(inner);

			zone.Controls.Add(sp);

			((ISupportInitialize)workspace).EndInit();

			Assert.AreEqual(1, workspace.SmartParts.Count);

			inner.Dispose();
			Assert.AreEqual(1, workspace.SmartParts.Count);
		}

        [TestMethod]
        public void RemovingControlChainUnregistersZone1()
        {
            Control parent = new Control();
            Control zone1 = new Control();

            parent.Controls.Add(zone1);

            workspace.Controls.Add(parent);
            workspace.SetZoneName(zone1, "TestZone");

            workspace.Controls.Remove(parent);

            // Debug here. It goes in the OnZoneParentChanged method of ZoneWorkspace
            parent.Controls.Remove(zone1);

            Assert.IsNull(workspace.GetZoneName(zone1));
            Assert.AreEqual(0, workspace.Zones.Count);
        }

		private static void AddZones(Control zone, Control zone1)
		{
			workspace.Controls.Add(zone);
			workspace.Controls.Add(zone1);
			workspace.SetZoneName(zone, "Main");
			workspace.SetZoneName(zone1, "Main1");
		}

		private static ZoneWorkspaceForm CreateFormAddWorkspace()
		{
			ZoneWorkspaceForm form = new ZoneWorkspaceForm();
			form.Controls.Add(workspace);
			return form;
		}


		[SmartPart]
		class ControlSmartPart : Control
		{
		}


	}
}
