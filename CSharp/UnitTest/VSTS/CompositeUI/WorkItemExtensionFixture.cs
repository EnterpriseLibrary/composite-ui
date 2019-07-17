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
using Microsoft.Practices.CompositeUI.Services;

namespace Microsoft.Practices.CompositeUI.Tests
{
	[TestClass]
	public class WorkItemExtensionFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfWorkItemNull()
		{
			MockExtension extension = new MockExtension();

			extension.Initialize(null);
		}

		[TestMethod]
		public void VirtualMethodsAreCalled()
		{
			IWorkItemExtensionService extensions = new WorkItemExtensionService();
			WorkItem parentWorkItem = new TestableRootWorkItem();
			parentWorkItem.Services.Add(typeof(IWorkItemActivationService), new SimpleWorkItemActivationService());
			parentWorkItem.Services.Add(typeof(IWorkItemExtensionService), extensions);
			extensions.RegisterExtension(typeof(WorkItem), typeof(MockExtension));

			WorkItem childWorkItem = parentWorkItem.WorkItems.AddNew<WorkItem>();
			childWorkItem.Activate();
			childWorkItem.Run();
			childWorkItem.Deactivate();
			childWorkItem.Terminate();

			Assert.IsTrue(MockExtension.InitializedCalled);
			Assert.IsTrue(MockExtension.ActivatedCalled);
			Assert.IsTrue(MockExtension.DeactivatedCalled);
			Assert.IsTrue(MockExtension.RunStartedCalled);
			Assert.IsTrue(MockExtension.TerminatedCalled);
		}


		[TestMethod]
		public void ExtensionMarkedForRootWorkItemAreAppliedOnlyToRoot()
		{
			WorkItem rootWorkItem = new MyTestableRootWorkItem();

			Assert.IsTrue(MockExtension.InitializedCalled);
			MockExtension.InitializedCalled = false;

			WorkItem child = rootWorkItem.WorkItems.AddNew<WorkItem>();

			Assert.AreEqual(1, RootWorkItemExtension.OnInializedCount);
			Assert.AreSame(rootWorkItem, RootWorkItemExtension.InitializedWorkItem);
			Assert.IsTrue(MockExtension.InitializedCalled);
		}

		#region Helper classes

		class MyTestableRootWorkItem : TestableRootWorkItem
		{
			protected override void TestableAddServices()
			{
				IWorkItemExtensionService extensions = Services.AddNew<WorkItemExtensionService, IWorkItemExtensionService>();
				extensions.RegisterRootExtension(typeof(RootWorkItemExtension));
				extensions.RegisterExtension(typeof(WorkItem), typeof(MockExtension));
				FinishInitialization();
			}
		}

		private class MockExtension : WorkItemExtension
		{
			public static bool ActivatedCalled = false;
			public static bool DeactivatedCalled = false;
			public static bool InitializedCalled = false;
			public static bool RunStartedCalled = false;
			public static bool TerminatedCalled = false;

			protected override void OnActivated()
			{
				ActivatedCalled = true;
			}

			protected override void OnDeactivated()
			{
				DeactivatedCalled = true;
			}

			protected override void OnInitialized()
			{
				InitializedCalled = true;
			}

			protected override void OnRunStarted()
			{
				RunStartedCalled = true;
			}

			protected override void OnTerminated()
			{
				TerminatedCalled = true;
			}
		}

		[RootWorkItemExtension]
		class RootWorkItemExtension : WorkItemExtension
		{
			public static int OnInializedCount = 0;
			public static WorkItem InitializedWorkItem;

			protected override void OnInitialized()
			{
				OnInializedCount++;
				InitializedWorkItem = base.WorkItem;
				base.OnInitialized();
			}
		}

		#endregion
	}
}
