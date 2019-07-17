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

namespace Microsoft.Practices.CompositeUI.WinForms.Tests
{
	[TestClass]
	public class ControlActivationServiceFixture
	{
		[TestMethod]
		public void ServiceSuccessfullyHooksEventWhenAdded()
		{
			bool test = false;
			WorkItem workItem = new TestableRootWorkItem();
			workItem.Services.Add(typeof(IWorkItemActivationService), new SimpleWorkItemActivationService());
			IControlActivationService service = workItem.Services.Get<IControlActivationService>();
			workItem.Activated += delegate
			{
				test = true;
			};

			UserControl view = new UserControl();
			workItem.Items.Add(view);
			service.ControlAdded(view);

			Form f = new Form();
			f.Controls.Add(view);

			f.Show();

			Assert.IsTrue(test, "Control.Enter didn't cause WorkItem.Activate to be called");
		}

		[TestMethod]
		public void ContainerWorkItemIsActivated()
		{
			ActivationServiceMock activation = new ActivationServiceMock();
			WorkItem item = new TestableRootWorkItem();
			item.Services.Add(typeof(IWorkItemActivationService), activation);
			MyControl ctrl = new MyControl();

			IControlActivationService svc = item.Services.Get<IControlActivationService>();
			item.Items.Add(ctrl);
			svc.ControlAdded(ctrl);

			ctrl.FireEnter();

			Assert.AreEqual(WorkItemStatus.Active, item.Status);
		}


		[TestMethod]
		public void DisposedControlDoesNotActivateTheWorkItem()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			MyControl testControl = new MyControl();
			workItem.Items.Add(testControl);

			testControl.Dispose();

			testControl.FireEnter();
			Assert.IsFalse(workItem.Status == WorkItemStatus.Active);
		}


		[TestMethod]
		public void RemovedControlDoesNotActiveteTheWorkItem()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			MyControl testControl = new MyControl();
			workItem.Items.Add(testControl);

			workItem.Items.Remove(testControl);

			testControl.FireEnter();
			Assert.IsFalse(workItem.Status == WorkItemStatus.Active);
		}

		class MyControl : Control
		{
			public void FireEnter()
			{
				OnEnter(EventArgs.Empty);
			}

			protected override void OnEnter(EventArgs e)
			{
				base.OnEnter(e);
			}
		}

		class ActivationServiceMock : IWorkItemActivationService
		{
			public void ChangeStatus(WorkItem item)
			{
			}
		}
	}
}
