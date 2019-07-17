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
using Microsoft.Practices.CompositeUI.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Practices.CompositeUI.Tests.SmartParts
{
	[TestClass]
	public class WorkspaceComposerFixture
	{
		MockWorkspace workspace;
		WorkspaceComposer<MockSP, SmartPartInfo> composer;

		[TestInitialize]
		public void SetUp()
		{
			workspace = new MockWorkspace();
			composer = new WorkspaceComposer<MockSP, SmartPartInfo>(workspace);
		}

		[TestMethod]
		public void ShowOnComposerCallsComposedWorkspace()
		{
			composer.Show(new MockSP());

			Assert.AreEqual(1, workspace.OnShowCalls);
		}

		[TestMethod]
		public void ShowWithSPICallsComposedWorkspace()
		{
			composer.Show(new MockSP(), new SmartPartInfo());

			Assert.AreEqual(1, workspace.OnShowCalls);
		}

		[TestMethod]
		public void ActivateCallsComposedWorkspace()
		{
			MockSP sp = new MockSP();
			composer.Show(sp);

			composer.Activate(sp);

			Assert.AreEqual(1, workspace.OnActivateCalls);
		}

		[TestMethod]
		public void HideCallsComposedWorkspace()
		{
			MockSP sp = new MockSP();
			composer.Show(sp);

			composer.Hide(sp);

			Assert.AreEqual(1, workspace.OnHideCalls);
		}

		[TestMethod]
		public void CloseCallsComposedWorkspace()
		{
			MockSP sp = new MockSP();
			composer.Show(sp);

			composer.Close(sp);

			Assert.AreEqual(1, workspace.OnCloseCalls);			
		}

		[TestMethod]
		public void ActivateEventCallsRaiseOnComposedWorkspace()
		{
			MockSP sp = new MockSP();
			composer.Show(sp);

			composer.Activate(sp);

			Assert.AreEqual(1, workspace.RaiseSmartPartActivatedCalls);
		}

		[TestMethod]
		public void ClosingEventCallsRaiseOnComposedWorkspace()
		{
			MockSP sp = new MockSP();
			composer.Show(sp);

			composer.Close(sp);

			Assert.AreEqual(1, workspace.RaiseSmartPartClosingCalls);
		}

		[TestMethod]
		public void CancellingClosingEventDoesNotCallCloseOnComposedWorkspace()
		{
			MockSP sp = new MockSP();
			composer.Show(sp);
			workspace.SmartPartClosing += delegate(object sender, WorkspaceCancelEventArgs e) { e.Cancel = true; };

			composer.Close(sp);

			Assert.AreEqual(0, workspace.OnCloseCalls);
		}

		class MockSP
		{
		}

		class MockWorkspace : IComposableWorkspace<MockSP, SmartPartInfo>
		{
			public int OnActivateCalls;
			public int OnApplySmartPartInfoCalls;
			public int OnShowCalls;
			public int OnHideCalls;
			public int OnCloseCalls;
			public int RaiseSmartPartActivatedCalls;
			public int RaiseSmartPartClosingCalls;

			List<MockSP> smartParts = new List<MockSP>();

			#region IComposedWorkspace<MockSP,SmartPartInfo> Members

			public SmartPartInfo ConvertFrom(ISmartPartInfo source)
			{
				return SmartPartInfo.ConvertTo<SmartPartInfo>(source);
			}

			public void OnActivate(MockSP smartPart)
			{
				OnActivateCalls++;
			}

			public void OnApplySmartPartInfo(MockSP smartPart, SmartPartInfo smartPartInfo)
			{
				OnApplySmartPartInfoCalls++;
			}

			public void OnShow(MockSP smartPart, SmartPartInfo smartPartInfo)
			{
				OnShowCalls++;
				smartParts.Add(smartPart);
			}

			public void OnHide(MockSP smartPart)
			{
				OnHideCalls++;
			}

			public void OnClose(MockSP smartPart)
			{
				OnCloseCalls++;
			}

			public void RaiseSmartPartActivated(WorkspaceEventArgs e)
			{
				RaiseSmartPartActivatedCalls++;
				if (SmartPartActivated != null)
				{
					SmartPartActivated(this, e);
				}
			}

			public void RaiseSmartPartClosing(WorkspaceCancelEventArgs e)
			{
				RaiseSmartPartClosingCalls++;
				if (SmartPartClosing != null)
				{
					SmartPartClosing(this, e);
				}
			}

			#endregion

			#region IWorkspace Members

			public event EventHandler<WorkspaceCancelEventArgs> SmartPartClosing;

			public event EventHandler<WorkspaceEventArgs> SmartPartActivated;

			public void Show(object smartPart, ISmartPartInfo smartPartInfo)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void Show(object smartPart)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void Hide(object smartPart)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void Close(object smartPart)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void Activate(object smartPart)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public void ApplySmartPartInfo(object smartPart, ISmartPartInfo smartPartInfo)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public ReadOnlyCollection<object> SmartParts
			{
				get
				{
					throw new Exception("The method or operation is not implemented.");
				}
			}

			public object ActiveSmartPart
			{
				get
				{
					throw new Exception("The method or operation is not implemented.");
				}
			}

			#endregion
		}
	}
}
