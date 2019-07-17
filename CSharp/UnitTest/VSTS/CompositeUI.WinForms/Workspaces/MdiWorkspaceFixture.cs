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
using System.Drawing;
using System.Reflection;
using System.IO;

namespace Microsoft.Practices.CompositeUI.WinForms.Tests
{
	[TestClass]
	public class MdiWorkspaceFixture
	{
		[TestMethod]
		public void MDIWorkspaceIsMDIContainer()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			Assert.IsTrue(workspace.ParentMdiForm.IsMdiContainer);
		}

		#region Show

		[TestMethod]
		public void ShowShowsNewMDIChildWithSmartPart()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);

			Assert.AreEqual(1, workspace.ParentMdiForm.MdiChildren.Length);
			Assert.IsTrue(workspace.Windows[smartPart].IsMdiChild);
			Assert.AreEqual(smartPart, workspace.ParentMdiForm.MdiChildren[0].Controls[0]);
		}

		[TestMethod]
		public void ShowSizesFormCorrectly()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			smartPart.Size = new System.Drawing.Size(300, 200);
			workspace.Show(smartPart);

			Assert.AreEqual(300, workspace.ParentMdiForm.MdiChildren[0].Size.Width);
			Assert.AreEqual(220, workspace.ParentMdiForm.MdiChildren[0].Size.Height);
		}

		[TestMethod]
		public void ShowSetTextOnFormFromSPInfo()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			ISmartPartInfo info = new WindowSmartPartInfo();
			info.Title = "Smart Part";
			workItem.RegisterSmartPartInfo(smartPart, info);

			workspace.Show(smartPart);

			Assert.AreEqual("Smart Part", workspace.ParentMdiForm.MdiChildren[0].Text);
		}

		[TestMethod]
		public void ShowSetTextFromWindowSPInfo()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			WindowSmartPartInfo info = new WindowSmartPartInfo();
			info.Title = "SP";

			workspace.Show(smartPart, info);

			Assert.AreEqual("SP", workspace.ParentMdiForm.MdiChildren[0].Text);
		}

		[TestMethod]
		public void ShowSmartpartTwice()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			parentForm.Show();
			workspace.Show(smartPart);
			workspace.Show(smartPart);

			Assert.IsTrue(smartPart.Visible);
		}

		[TestMethod]
		public void ShowingFiresActivatedEvent()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			bool activated = false;
			parentForm.Show();
			workspace.SmartPartActivated += delegate { activated = true; };

			workspace.Show(smartPart);

			Assert.IsTrue(activated);
		}

		[TestMethod]
		public void ShowingFiresActivatedWithSmartPart()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			object argsSmartPart = null;
			parentForm.Show();
			workspace.SmartPartActivated +=
				delegate(object sender, WorkspaceEventArgs args) { argsSmartPart = args.SmartPart; };

			workspace.Show(smartPart);

			Assert.AreEqual(smartPart, argsSmartPart);
		}

		[TestMethod]
		public void ShowExistingFormShouldBringToFront()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);

			parentForm.Show();
			MockSmartPart smartPart = new MockSmartPart();
			MockSmartPart smartPart2 = new MockSmartPart();

			workspace.Show(smartPart);
			workspace.Show(smartPart2);

			workspace.Show(smartPart);

			Assert.IsTrue(smartPart.Focused);
		}

		[TestMethod]
		public void CanSpecifySizeOnShow()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			WindowSmartPartInfo info = new WindowSmartPartInfo();
			info.Title = "Mock Smart Part";
			info.Width = 300;
			info.Height = 400;

			workspace.Show(smartPart, info);

			Assert.AreEqual(300, workspace.Windows[smartPart].Width);
			Assert.AreEqual(400, workspace.Windows[smartPart].Height);
		}

		[TestMethod]
		public void CanSpecifyLocationOnShow()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			WindowSmartPartInfo info = new WindowSmartPartInfo();
			info.Title = "Mock Smart Part";
			info.Location = new Point(10, 50);

			workspace.Show(smartPart, info);

			Assert.AreEqual(10, workspace.Windows[smartPart].Location.X);
			Assert.AreEqual(50, workspace.Windows[smartPart].Location.Y);
		}

		[TestMethod]
		public void CanSetWindowOptionsOnShow()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			WindowSmartPartInfo info = new WindowSmartPartInfo();
			info.Title = "Mock Smart Part";
			info.ControlBox = false;
			info.MinimizeBox = false;
			info.MaximizeBox = false;
			Icon icon = null;
			Assembly asm = Assembly.GetExecutingAssembly();

			using (Stream imgStream =
				asm.GetManifestResourceStream("Microsoft.Practices.CompositeUI.WinForms.Tests.test.ico"))
			{
				icon = new Icon(imgStream);
			}
			info.Icon = icon;

			workspace.Show(smartPart, info);

			Assert.IsFalse(workspace.Windows[smartPart].ControlBox);
			Assert.IsFalse(workspace.Windows[smartPart].MinimizeBox);
			Assert.IsFalse(workspace.Windows[smartPart].MaximizeBox);
			Assert.AreSame(icon, workspace.Windows[smartPart].Icon);
		}

		[TestMethod]
		public void CanShowWithNonWindowSPI()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			SmartPartInfo info = new SmartPartInfo();
			info.Title = "Foo";

			workspace.Show(smartPart, info);
		}

		[TestMethod]
		public void UsesSPInfoIfNoWindowSPInfoExists()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			SmartPartInfo info = new SmartPartInfo();
			info.Title = "Foo";
			workItem.RegisterSmartPartInfo(smartPart, info);

			workspace.Show(smartPart);

			Assert.AreEqual("Foo", workspace.Windows[smartPart].Text);
		}

		[TestMethod]
		public void CloseSmartPartDoesNotDisposeIt()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			MdiWorkspace workspace = workItem.Workspaces.AddNew<MdiWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);
			workspace.Close(smartPart);

			Assert.IsFalse(smartPart.IsDisposed);
		}

		#endregion
		
		#region Hide

		[TestMethod]
		public void CanHideFormWithSmartPart()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);
			workspace.Hide(smartPart);

			Assert.IsFalse(smartPart.Visible);
			Assert.IsFalse(workspace.Windows[smartPart].Visible);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void HideNonExistSmartPartThrows()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Hide(smartPart);
		}

		#endregion

		#region Close

		[TestMethod]
		public void CanCloseMdiChild()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);
			Form form = workspace.Windows[smartPart];

			workspace.Close(smartPart);

			Assert.IsTrue(form.IsDisposed);
            Assert.IsFalse(smartPart.IsDisposed);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CloseNonExistSmartPartThrows()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Close(smartPart);
		}

		[TestMethod]
		public void WorkspaceFiresSmartPartClosing()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			bool closing = false;
			parentForm.Show();
			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate { closing = true; };

			workspace.Close(smartPart);

			Assert.IsTrue(closing);
		}

		[TestMethod]
		public void CanCancelSmartPartClosing()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			parentForm.Show();
			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate(object sender, WorkspaceCancelEventArgs args) { args.Cancel = true; };

			workspace.Close(smartPart);

			Assert.IsFalse(smartPart.IsDisposed, "Smart Part Was Disposed");
		}

		#endregion
		
		[TestMethod]
		public void SettingFocusOnWindowFiresActivated()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			object argsSmartPart = null;
			parentForm.Show();
			MockSmartPart sp1 = new MockSmartPart();
			workspace.Show(sp1);
			workspace.Show(smartPart);

			workspace.SmartPartActivated +=
				delegate(object sender, WorkspaceEventArgs args) { argsSmartPart = args.SmartPart; };

			workspace.Windows[sp1].Focus();

			Assert.AreEqual(sp1, argsSmartPart);
		}

		[TestMethod]
		public void WindowActivatedFiresCorrectNumberOfTimes()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form parentForm = workItem.Items.AddNew<Form>();
			MdiWorkspace workspace = new MdiWorkspace(parentForm);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			int activated = 0;
			parentForm.Show();
			MockSmartPart sp1 = new MockSmartPart();
			workspace.SmartPartActivated +=
				delegate(object sender, WorkspaceEventArgs args) { activated++; };

			workspace.Show(sp1);
			workspace.Show(smartPart);
			workspace.Windows[sp1].Focus();

			Assert.AreEqual(3, activated);
		}

		#region Helper classes

		[SmartPart]
		private class MockSmartPart : Control
		{
			private SmartPartInfo info = new SmartPartInfo();

			public MockSmartPart()
			{
				info.Title = "Smart Part";
			}
		}

		#endregion
	}
}
