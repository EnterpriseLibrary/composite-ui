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
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI.SmartParts;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.IO;

namespace Microsoft.Practices.CompositeUI.WinForms.Tests
{
	[TestClass]
	public class WindowWorkspaceFixture
	{
		private const uint WM_SYSCOMMAND = 0x0112;
		private const int SC_CLOSE = 0xF060;

		[DllImport("user32.dll")]
		static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		#region Show

		[TestMethod]
		public void ShowShowsNewFormWithControl()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);

			Form form = workspace.Windows[smartPart];
			Assert.AreSame(smartPart, form.Controls[0]);
			Assert.IsTrue(workspace.Windows[smartPart].Visible);
			Assert.IsTrue(smartPart.Visible);
		}

		[TestMethod]
		public void ShowShowsNewFormWithOwnerAndControl()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			Form owner = new Form();
			WindowWorkspace ws = new WindowWorkspace(owner);

			ws.Show(smartPart);

			Form form = ws.Windows[smartPart];
			Assert.AreSame(owner, form.Owner);
		}

		[TestMethod]
		public void ShowingSetFormTextFromWindowSmartPartInfo()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			WindowSmartPartInfo info = new WindowSmartPartInfo();
			info.Title = "Mock Smart Part";
			workItem.RegisterSmartPartInfo(smartPart, info);
			workspace.Show(smartPart, info);

			Assert.AreEqual("Mock Smart Part", workspace.Windows[smartPart].Text);
		}

		[TestMethod]
		public void ShowingSmartPartISPInfoProviderSetFormText()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			ISmartPartInfo info = new WindowSmartPartInfo();
			info.Title = "Smart Part";
			workItem.RegisterSmartPartInfo(smartPart, info);

			workspace.Show(smartPart);

			Assert.AreEqual("Smart Part", workspace.Windows[smartPart].Text);
		}

		[TestMethod]
		public void CanShowModal()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			WindowSmartPartInfo info = new WindowSmartPartInfo();
			info.Title = "Mock Smart Part";
			info.Modal = true;

			Thread thread = new Thread(new ThreadStart(delegate
				{
					workspace.Show(smartPart, info);
				}));

			try
			{
				thread.Start();
				Thread.Sleep(1000);

				Assert.IsTrue(workspace.Windows[smartPart].Visible);
			}
			finally
			{
				SendMessage(workspace.Windows[smartPart].Handle, WM_SYSCOMMAND, SC_CLOSE, 0);
				thread.Join();
			}
		}

		[TestMethod]
		public void CanShowModalWithOwner()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Form owner = new Form();
			WindowWorkspace workspace = new WindowWorkspace(owner);
			workItem.Workspaces.Add(workspace);
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			WindowSmartPartInfo info = new WindowSmartPartInfo();
			info.Title = "Mock Smart Part";
			info.Modal = true;

			Thread thread = new Thread(new ThreadStart(delegate
				{
					workspace.Show(smartPart, info);
				}));

			try
			{
				thread.Start();
				Thread.Sleep(1000);

				Assert.IsTrue(workspace.Windows[smartPart].Visible);
				Assert.AreSame(owner, workspace.Windows[smartPart].Owner);
			}
			finally
			{
				SendMessage(workspace.Windows[smartPart].Handle, WM_SYSCOMMAND, SC_CLOSE, 0);
				thread.Join();
			}
		}

		[TestMethod]
		public void CanShowNonModal()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			WindowSmartPartInfo info = new WindowSmartPartInfo();
			info.Title = "Mock Smart Part";
			info.Modal = false;

			workspace.Show(smartPart, info);

			Assert.IsTrue(workspace.Windows[smartPart].Visible);
		}

		[TestMethod]
		public void FormSizeIsCorrectSize()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			smartPart.Size = new System.Drawing.Size(150, 125);

			workspace.Show(smartPart);

			Assert.AreEqual(150, workspace.Windows[smartPart].Size.Width);
			Assert.AreEqual(145, workspace.Windows[smartPart].Size.Height);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ShowingNonControlThrows()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();

			workspace.Show(new object());
		}

		[TestMethod]
		public void ShowingFiresActivatedEvent()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			bool activated = false;
			workspace.SmartPartActivated += delegate { activated = true; };
			workspace.Show(smartPart);

			Assert.IsTrue(activated);
		}

		[TestMethod]
		public void ShowingFiresActivatedWithSmartPart()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			object argsSmartPart = null;
			workspace.SmartPartActivated +=
				delegate(object sender, WorkspaceEventArgs args) { argsSmartPart = args.SmartPart; };
			workspace.Show(smartPart);

			Assert.AreEqual(smartPart, argsSmartPart);
		}

		[TestMethod]
		public void SettingFocusOnWindowFiresActivated()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			object argsSmartPart = null;
			MockSmartPart smartPart2 = workItem.SmartParts.AddNew<MockSmartPart>();
			workspace.Show(smartPart2);
			workspace.Show(smartPart);

			workspace.SmartPartActivated +=
				delegate(object sender, WorkspaceEventArgs args) { argsSmartPart = args.SmartPart; };

			workspace.Windows[smartPart2].Focus();

			Assert.AreEqual(smartPart2, argsSmartPart);
		}

		[TestMethod]
		public void WindowActivatedFiresCorrectNumberOfTimes()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			int activated = 0;
			MockSmartPart smartPart2 = workItem.SmartParts.AddNew<MockSmartPart>();
			workspace.SmartPartActivated +=
				delegate(object sender, WorkspaceEventArgs args) { activated++; };

			workspace.Show(smartPart2);
			workspace.Show(smartPart);
			workspace.Windows[smartPart2].Focus();

			Assert.AreEqual(3, activated);
		}

		[TestMethod]
		public void ShowExistingFormBringsToFront()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();
			MockSmartPart smartPart2 = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);
			workspace.Show(smartPart2);
			workspace.Show(smartPart);

			Assert.IsTrue(smartPart.Focused);
		}

		[TestMethod]
		public void CanSpecifySize()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
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
		public void CanSpecifyLocation()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			WindowSmartPartInfo info = new WindowSmartPartInfo();
			info.Title = "Mock Smart Part";
			info.Location = new Point(10, 50);

			workspace.Show(smartPart, info);

			Assert.AreEqual(10, workspace.Windows[smartPart].Location.X);
			Assert.AreEqual(50, workspace.Windows[smartPart].Location.Y);
		}

		[TestMethod]
		public void CanSetWindowOptions()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
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
		public void CanApplyWindowOptions()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			WindowSmartPartInfo info = new WindowSmartPartInfo();
			info.Title = "Mock Smart Part";
			info.Width = 400;

			workspace.Show(smartPart, info);

			Assert.AreEqual(400, workspace.Windows[smartPart].Width);

			info.Width = 500;
			workspace.ApplySmartPartInfo(smartPart, info);

			Assert.AreEqual(500, workspace.Windows[smartPart].Width);
		}

		[TestMethod]
		public void CanShowIfSPINotWindowSPI()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			SmartPartInfo info = new SmartPartInfo();
			info.Title = "Foo";

			workspace.Show(smartPart, info);
		}

		[TestMethod]
		public void UsesSPInfoIfNoWindowSPInfoExists()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			SmartPartInfo info = new SmartPartInfo();
			info.Title = "Foo";
			workItem.RegisterSmartPartInfo(smartPart, info);

			workspace.Show(smartPart);

			Assert.AreEqual("Foo", workspace.Windows[smartPart].Text);
		}

		[TestMethod]
		public void FiresOneEventOnlyIfSmartPartIsShownMultipleTimes()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPartA = new MockSmartPart();
			MockSmartPart smartPartB = new MockSmartPart();

			workspace.Show(smartPartA);
			workspace.Show(smartPartB);

			int activatedCalled = 0;
			workspace.SmartPartActivated += delegate(object sender, WorkspaceEventArgs e)
			{
				activatedCalled++;
				Assert.AreSame(e.SmartPart, smartPartA);
			};

			workspace.Show(smartPartA);

			Assert.AreEqual(1, activatedCalled);
		}

		[TestMethod]
		public void ShowTwiceReusesForm()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = new MockSmartPart();

			workspace.Show(smartPart);
			workspace.Show(smartPart);

			Assert.AreEqual(1, workspace.Windows.Count);
		}

		[TestMethod]
		public void ShowSetsVisible()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();

			MockSmartPart smartPart = new MockSmartPart();
			smartPart.Visible = false;

			workspace.Show(smartPart);

			Assert.IsTrue(smartPart.Visible);
		}

		#endregion

		#region Hide

		[TestMethod]
		public void CanHideWithSmartPart()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);

			workspace.Hide(smartPart);

			Assert.IsFalse(workspace.Windows[smartPart].Visible);
			Assert.IsFalse(smartPart.Visible);
		}

		[TestMethod]
		public void CanShowSameWindowAfterHidden()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);
			workspace.Hide(smartPart);

			workspace.Show(smartPart);

			Assert.IsNotNull(workspace.Windows[smartPart]);
			Assert.IsTrue(smartPart.Visible);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void HideNonExistControlThrows()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Hide(smartPart);
		}

		[TestMethod]
		public void HidingSmartPartDoesNotAutomaticallyShowPreviousForm()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPartA = new MockSmartPart();
			smartPartA.Visible = false;
			MockSmartPart smartPartB = new MockSmartPart();
			smartPartB.Visible = false;

			WindowSmartPartInfo smartPartInfoB = new WindowSmartPartInfo();
			smartPartInfoB.Title = "Window SmartPart B";

			WindowSmartPartInfo smartPartInfoA = new WindowSmartPartInfo();
			smartPartInfoA.Title = "Window SmartPart A";

			workspace.Show(smartPartA, smartPartInfoA);
			Assert.IsTrue(smartPartA.Visible);

			// Force the form to non-visible so it doesn't fire
			// his own Activated event after we hide the following 
			// smart part, therefore making the condition impossible 
			// to test.

			workspace.Windows[smartPartA].Hide();

			workspace.Show(smartPartB, smartPartInfoB);
			Assert.IsTrue(smartPartB.Visible);

			workspace.Hide(smartPartB);

			Assert.IsNull(workspace.ActiveSmartPart);
		}

		#endregion

		#region Close

		[TestMethod]
		public void CloseDisposesAndClosesWindow()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);

			Form form = workspace.Windows[smartPart];
			workspace.Close(smartPart);

			Assert.IsTrue(form.IsDisposed, "Form not disposed");
			Assert.IsFalse(form.Visible, "Form is visible");
            Assert.IsFalse(smartPart.IsDisposed);
		}

		[TestMethod]
		public void CloseRemovesEntriesInWindowsAndSmartParts()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);

			workspace.Close(smartPart);

			Assert.AreEqual(0, workspace.Windows.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CloseNonExistControlThrows()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Close(smartPart);
		}

		[TestMethod]
		public void WorkspaceFiresSmartPartClosing()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			bool closing = false;
			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate { closing = true; };

			workspace.Close(smartPart);

			Assert.IsTrue(closing);
		}

		[TestMethod]
		public void CanCancelSmartPartClosing()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate(object sender, WorkspaceCancelEventArgs args) { args.Cancel = true; };

			workspace.Close(smartPart);

			Assert.IsFalse(smartPart.IsDisposed, "Smart Part Was Disposed");
		}

		[TestMethod]
		public void ClosingIsCalledWhenClosed()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			bool closing = false;
			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate { closing = true; };

			workspace.Windows[smartPart].Close();

			Assert.IsTrue(closing);
		}

		[TestMethod]
		public void ClosingDoesNotFireIfNoControlsOnForm()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			bool closing = false;
			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate { closing = true; };

			workspace.Windows[smartPart].Controls.Clear();

			Assert.IsFalse(closing);
		}

		[TestMethod]
		public void ClosedIsCalledWhenClosed()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			bool closed = false;
			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate { closed = true; };

			workspace.Windows[smartPart].Close();

			Assert.IsTrue(closed);
		}

		[TestMethod]
		public void CloseRemovesWindow()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);
			workspace.Close(smartPart);

			Assert.IsFalse(workspace.Windows.ContainsKey(smartPart));
		}

		[TestMethod]
		public void ClosedDoesNotFireIfNoControlsOnForm()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			bool closing = false;
			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate { closing = true; };

			workspace.Windows[smartPart].Controls.Clear();

			Assert.IsFalse(closing);
		}

		[TestMethod]
		public void CanCancelCloseWhenFormClose()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate(object sender, WorkspaceCancelEventArgs args) { args.Cancel = true; };

			workspace.Windows[smartPart].Close();

			Assert.IsFalse(smartPart.IsDisposed);
		}

		[TestMethod]
		public void CloseSmartPartDoesNotDisposeIt()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);
			workspace.Close(smartPart);

			Assert.IsFalse(smartPart.IsDisposed);
		}

		#endregion

		[TestMethod]
		public void ControlIsRemovedWhenSmartPartIsDisposed()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPart = workItem.SmartParts.AddNew<MockSmartPart>();

			workspace.Show(smartPart);
			Assert.AreEqual(1, workspace.Windows.Count);

			smartPart.Dispose();

			Assert.AreEqual(0, workspace.Windows.Count);
			Assert.AreEqual(0, workspace.SmartParts.Count);
		}

		[TestMethod]
		public void SPIsRemovedFromWorkspaceWhenDisposed()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			WindowWorkspace workspace = workItem.Workspaces.AddNew<WindowWorkspace>();
			MockSmartPart smartPartA = new MockSmartPart();
			workspace.Show(smartPartA);
			bool Called = false;

			workspace.SmartPartClosing += delegate(object s, WorkspaceCancelEventArgs e)
			{
				Called = true;
			};

			smartPartA.Dispose();

			Assert.IsFalse(Called);
			Assert.AreEqual(0, workspace.Windows.Count);
			Assert.AreEqual(0, workspace.SmartParts.Count);
		}



		#region Helper classes

		[SmartPart]
		class MockSmartPart : Control
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
