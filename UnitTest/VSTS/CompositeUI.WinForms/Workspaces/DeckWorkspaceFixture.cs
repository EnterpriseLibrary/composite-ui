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
using System.Reflection;

namespace Microsoft.Practices.CompositeUI.WinForms.Tests
{
	[TestClass]
	public class DeckWorkspaceFixture
	{
		private static DeckWorkspace workspace;
		private static MockSmartPart smartPart;

		[TestInitialize]
		public void Setup()
		{
			workspace = new DeckWorkspace();
			smartPart = new MockSmartPart();
		}

		#region Show

		[TestMethod]
		public void ShowMakesControlVisible()
		{
			smartPart.Visible = false;

			workspace.Show(smartPart);

			Assert.IsTrue(smartPart.Visible);
		}

		[TestMethod]
		public void ShowHidesPreviouslyVisibleControl()
		{
			MockSmartPart c2 = new MockSmartPart();

			workspace.Show(smartPart);
			workspace.Show(c2);

			Assert.AreSame(smartPart, workspace.Controls[1], "Hiden control didn't go to the bottom of the deck");
			Assert.AreSame(c2, workspace.ActiveSmartPart);
		}

		[TestMethod]
		public void ShowSetsDockFill()
		{
			workspace.Show(smartPart);

			Assert.AreEqual(DockStyle.Fill, smartPart.Dock);
		}

		[TestMethod]
		public void ShowWhenControlAlreadyExistsShowsSameControl()
		{
			MockSmartPart c2 = new MockSmartPart();
			MockSmartPart c3 = new MockSmartPart();

			workspace.Show(smartPart);
			workspace.Show(c2);
			workspace.Show(c3);
			workspace.Show(c2);
			workspace.Show(smartPart);

			Assert.AreEqual(3, workspace.SmartParts.Count);
		}

		[TestMethod]
		public void CallingShowTwiceStillShowsControl()
		{
			workspace.Show(smartPart);
			workspace.Show(smartPart);

			Assert.AreSame(smartPart, workspace.ActiveSmartPart);
		}

		[TestMethod]
		public void FiresSmartPartActivateWhenShown()
		{
			object argsSmartPart = null;
			workspace.SmartPartActivated +=
				delegate(object sender, WorkspaceEventArgs args) { argsSmartPart = args.SmartPart; };

			workspace.Show(smartPart);

			Assert.AreEqual(smartPart, argsSmartPart);
		}

		#endregion

		#region Hide

		[TestMethod]
		public void HideDoesNotHideControl()
		{
			smartPart.Visible = false;
			workspace.Show(smartPart);

			bool visibleChanged = false;
			smartPart.VisibleChanged += delegate { visibleChanged = true; };

			workspace.Hide(smartPart);

			// The reasoning is that in a deck, the factor that causes 
			// a smart part to be hiden is that another one is shown on top.
			// There's no actual hiding of the previous control.
			Assert.IsTrue(smartPart.Visible);
			Assert.IsFalse(visibleChanged);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void HideNonExistSmartPartThrows()
		{
			workspace.Hide(smartPart);
		}

		[TestMethod]
		public void HidingShowsPreviousFireActivatedEvent()
		{
			object argsSmartPart = null;
			MockSmartPart sp1 = new MockSmartPart();
			workspace.Show(sp1);
			workspace.Show(smartPart);
			workspace.SmartPartActivated +=
				delegate(object sender, WorkspaceEventArgs args) { argsSmartPart = args.SmartPart; };

			workspace.Hide(smartPart);

			Assert.AreEqual(sp1, argsSmartPart);
		}

		[TestMethod]
		public void HideNonActiveSmartPartDoesNotChangeCurrentOne()
		{
			ControlSmartPart smartPartA = new ControlSmartPart();
			ControlSmartPart smartPartB = new ControlSmartPart();
			ControlSmartPart smartPartC = new ControlSmartPart();

			workspace.Show(smartPartA);
			workspace.Show(smartPartB);
			workspace.Show(smartPartC);

			workspace.Hide(smartPartB);
			
			Assert.AreSame(smartPartC, workspace.ActiveSmartPart);
		}

		[TestMethod]
		public void ShowHideKeepsOrder()
		{
			ControlSmartPart c1 = new ControlSmartPart();
			ControlSmartPart c2 = new ControlSmartPart();
			ControlSmartPart c3 = new ControlSmartPart();

			workspace.Show(c1);
			workspace.Show(c2);
			workspace.Show(c3);
			workspace.Show(c2);
			workspace.Hide(c2);

			Assert.AreSame(c3, workspace.ActiveSmartPart);
		}

		#endregion

		#region Close

		[TestMethod]
		public void CloseRemovesSmartPartButDoesNotDispose()
		{
			workspace.Show(smartPart);

			workspace.Close(smartPart);

			Assert.IsFalse(workspace.Controls.Contains(smartPart));
			Assert.AreEqual(0, workspace.SmartParts.Count);
			Assert.IsFalse(smartPart.IsDisposed);
		}

		[TestMethod]
		public void WorkspaceFiresSmartPartClosing()
		{
			bool closing = false;
			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate { closing = true; };

			workspace.Close(smartPart);

			Assert.IsTrue(closing);
		}

		[TestMethod]
		public void CanCancelSmartPartClosing()
		{
			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate(object sender, WorkspaceCancelEventArgs args) { args.Cancel = true; };

			workspace.Close(smartPart);

			Assert.IsFalse(smartPart.IsDisposed);
		}

		[TestMethod]
		public void ClosingByDisposingControlDoesNotFireClosingEvent()
		{
			bool closing = false;
			workspace.Show(smartPart);
			workspace.SmartPartClosing += delegate { closing = true; };

			smartPart.Dispose();

			Assert.IsFalse(closing);
			Assert.AreEqual(0, workspace.SmartParts.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ClosingNonExistSSmartPartThrows()
		{
			workspace.Close(smartPart);
		}

		[TestMethod]
		public void CloseShowsPreviouslyVisibleControl()
		{
			workspace = new DeckWorkspace();

			MockSmartPart c1 = new MockSmartPart();
			MockSmartPart c2 = new MockSmartPart();

			workspace.Show(c1);
			workspace.Show(c2);
			workspace.Close(c2);

			Assert.AreSame(c1, workspace.ActiveSmartPart);
		}

		#endregion

		#region Misc

		[TestMethod]
		public void DeckIsOderedCorrectly()
		{
			smartPart.Visible = false;
			MockSmartPart c2 = new MockSmartPart();
			c2.Visible = false;

			workspace.Show(smartPart);
			workspace.Show(c2);
			workspace.Show(smartPart);
			workspace.Hide(smartPart);

			Assert.AreSame(c2, workspace.ActiveSmartPart);
			Assert.AreSame(c2, workspace.SmartParts[1]);
		}

		#endregion

		#region Disposing

		[TestMethod]
		public void ControlIsRemovedWhenSmartPartIsDisposed()
		{
			workspace.Show(smartPart);
			Assert.AreEqual(1, workspace.SmartParts.Count);

			smartPart.Dispose();

			Assert.AreEqual(0, workspace.SmartParts.Count);
		}

		[TestMethod]
		public void PreviousSmartPartActivatedWhenActiveSmartPartDisposed()
		{
			MockSmartPart smartPartA = new MockSmartPart();
			MockSmartPart smartPartB = new MockSmartPart();
			workspace.Show(smartPartA);
			workspace.Show(smartPartB);

			smartPartB.Dispose();
			Assert.IsFalse(workspace.Contains(smartPartB));
			Assert.AreSame(smartPartA, workspace.ActiveSmartPart);
		}

		[TestMethod]
		public void WorkspaceFiresDisposedEvent()
		{
			bool disposed = false;
			DeckWorkspace workspace = new DeckWorkspace();
			Form form = new Form();
			form.Controls.Add(workspace);
			form.Show();

			workspace.Disposed += delegate { disposed = true; };
			form.Close();

			Assert.IsTrue(disposed);
		}

		[TestMethod]
		public void DisposeNonActiveSmartPartDoesNotChangeActiveOne()
		{
			ControlSmartPart smartPartA = new ControlSmartPart();
			ControlSmartPart smartPartB = new ControlSmartPart();
			ControlSmartPart smartPartC = new ControlSmartPart();

			workspace.Show(smartPartA);
			workspace.Show(smartPartB);
			workspace.Show(smartPartC);

			smartPartB.Dispose();

			Assert.AreSame(smartPartC, workspace.ActiveSmartPart);
		}

		#endregion

        [TestMethod]
        public void CanCloseWorkspaceWithTwoSmartparts()
        {
            Control parent = new Control();
            parent.Controls.Add(workspace);
            MockSmartPart sp1 = new MockSmartPart();
            MockSmartPart sp2 = new MockSmartPart();
            workspace.Show(sp1);
            workspace.Show(sp2);


            parent.Dispose();
        }

		[TestMethod]
		public void ShowHidingFiresCorrectNumberOfTimes()
		{
			int activated = 0;
			MockSmartPart sp1 = new MockSmartPart();
			MockSmartPart sp2 = new MockSmartPart();
			workspace.SmartPartActivated += delegate { activated++; };

			workspace.Show(sp1);
			workspace.Show(sp2);
			workspace.Show(smartPart);

			workspace.Hide(smartPart);

			Assert.AreEqual(4, activated);
		}

		[TestMethod]
		public void ShowingHidingMultipleTimesKeepsProperDeckOrdering()
		{
			ControlSmartPart smartPartA = new ControlSmartPart();
			ControlSmartPart smartPartB = new ControlSmartPart();
			ControlSmartPart smartPartC = new ControlSmartPart();

			workspace.Show(smartPartA);
			workspace.Show(smartPartB);
			workspace.Show(smartPartC);

			workspace.Hide(smartPartC);
			Assert.AreSame(smartPartB, workspace.ActiveSmartPart);
			
			workspace.Hide(smartPartB);
			Assert.AreSame(smartPartA, workspace.ActiveSmartPart);
			
			workspace.Close(smartPartA);
			Assert.AreSame(smartPartC, workspace.ActiveSmartPart);

			workspace.Hide(smartPartC);
			Assert.AreSame(smartPartB, workspace.ActiveSmartPart);

			workspace.Hide(smartPartB);
			Assert.AreSame(smartPartC, workspace.ActiveSmartPart);
		}

		#region Supporting classes

		[SmartPart]
		private class NonMockSmartPartSmartPart : Object { }

		[SmartPart]
		private class MockSmartPart : Control
		{
		}

		[SmartPart]
		class ControlSmartPart : Control
		{
		}

		#endregion
	}
}