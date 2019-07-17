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
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.CompositeUI.Utility;
using Microsoft.Practices.ObjectBuilder;
using System.Windows.Forms;

namespace Microsoft.Practices.CompositeUI.Tests
{
	[TestClass]
	public class WorkItemFixture
	{
		// Construction

		#region Constructors

		[TestMethod]
		public void EmptyConstructorCreatesABuilderAndLocator()
		{
			TestableRootWorkItem wi = new TestableRootWorkItem();

			Assert.IsNotNull(wi.Builder);
			Assert.IsNotNull(wi.Locator);
		}

		#endregion

		// WorkItem hierarchy

		#region WorkItem hierarchy

		[TestMethod]
		public void GetParentOnChildWorkItemReturnsParentWorkItem()
		{
			WorkItem parentWorkItem = new TestableRootWorkItem();
			WorkItem childWorkItem = parentWorkItem.WorkItems.AddNew<WorkItem>();

			Assert.AreSame(parentWorkItem, childWorkItem.Parent);
		}

		[TestMethod]
		public void GetParentOnRootWorkItemReturnNull()
		{
			WorkItem wi = new TestableRootWorkItem();

			Assert.IsNull(wi.Parent);
		}

		[TestMethod]
		public void CanAddAWorkItemToAWorkItem()
		{
			WorkItem wi1 = new TestableRootWorkItem();
			WorkItem wi2 = new WorkItem();

			wi1.WorkItems.Add(wi2);

			Assert.IsTrue(wi1.WorkItems.ContainsObject(wi2));
			Assert.AreSame(wi1, wi2.Parent);
		}

		[TestMethod]
		public void UsingCreateToMakeWorkItemProperlySetsParentRelationship()
		{
			WorkItem parentWorkItem = new TestableRootWorkItem();
			WorkItem childWorkItem = parentWorkItem.WorkItems.AddNew<WorkItem>();

			Assert.AreSame(parentWorkItem, childWorkItem.Parent);
		}

		[TestMethod]
		public void UsingCreateToMakeWorkItemDerivedClassProperlySetsParentRelationship()
		{
			WorkItem parentWorkItem = new TestableRootWorkItem();
			WorkItem childWorkItem = parentWorkItem.WorkItems.AddNew<ChildWorkItem>();

			Assert.AreSame(parentWorkItem, childWorkItem.Parent);
		}

		[TestMethod]
		public void CanDiscoverRootWorkItem()
		{
			WorkItem grandparent = new TestableRootWorkItem();
			WorkItem parent = grandparent.WorkItems.AddNew<WorkItem>();
			WorkItem child = parent.WorkItems.AddNew<WorkItem>();

			Assert.AreSame(grandparent, grandparent.RootWorkItem);
			Assert.AreSame(grandparent, parent.RootWorkItem);
			Assert.AreSame(grandparent, child.RootWorkItem);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CannotAddWorkItemToItself()
		{
			WorkItem wi = new TestableRootWorkItem();

			wi.WorkItems.Add(wi);
		}

		#endregion

		#region Disposal

		[TestMethod]
		public void DisposingWorkItemCausesContainedObjectsToBeDisposed()
		{
			WorkItem wi = new TestableRootWorkItem();
			MockDisposableObject obj = wi.Items.AddNew<MockDisposableObject>();

			wi.Dispose();

			Assert.IsTrue(obj.WasDisposed);
		}

		[TestMethod]
		public void TerminatingWorkItemCausesContainedObjectsToBeDisposed()
		{
			WorkItem wi = new TestableRootWorkItem();
			MockDisposableObject obj = wi.Items.AddNew<MockDisposableObject>();

			wi.Terminate();

			Assert.IsTrue(obj.WasDisposed);
		}

		[TestMethod]
		public void DisposingContainerCausesContainedObjectsToBeTornDown()
		{
			TestableRootWorkItem wi = new TestableRootWorkItem();
			MockTearDownStrategy strategy = new MockTearDownStrategy();
			wi.Builder.Strategies.Add(strategy, BuilderStage.PreCreation);

			wi.Items.AddNew<object>();
			wi.Dispose();

			Assert.IsTrue(strategy.TearDownCalled);
		}

		[TestMethod]
		public void TerminatingWorkItemCausesItToBeRemovedFromParent()
		{
			TestableRootWorkItem parent = new TestableRootWorkItem();
			WorkItem child = parent.WorkItems.AddNew<WorkItem>();

			Assert.AreEqual(1, parent.WorkItems.Count);
			child.Terminate();
			Assert.AreEqual(0, parent.WorkItems.Count);
		}

		#endregion

		// Collections

		#region Collection Events

		[TestMethod]
		public void AddingItemToWorkItemDoesNotFireServicesAddedEvent()
		{
			TestableRootWorkItem wi = new TestableRootWorkItem();
			bool addedCalled = false;
			wi.Services.Added += delegate { addedCalled = true; };

			object obj = wi.Items.AddNew<object>();

			Assert.IsFalse(addedCalled);
		}

		[TestMethod]
		public void AdddingServiceToWorkItemDoesNotFireItemsAddedEvent()
		{
			TestableRootWorkItem wi = new TestableRootWorkItem();
			bool addedCalled = false;
			wi.Items.Added += delegate { addedCalled = true; };

			object obj = wi.Services.AddNew<object>();

			Assert.IsFalse(addedCalled);
		}

		[TestMethod]
		public void RemovingItemFromWorkItemDoesNotFireServicesRemovedEvent()
		{
			TestableRootWorkItem wi = new TestableRootWorkItem();
			bool removeCalled = false;
			wi.Services.Removed += delegate { removeCalled = true; };

			object obj = wi.Items.AddNew<object>();
			wi.Items.Remove(obj);

			Assert.IsFalse(removeCalled);
		}

		[TestMethod]
		public void RemovingServiceFromWorkItemDoesNotFireItemsRemovedEvent()
		{
			TestableRootWorkItem wi = new TestableRootWorkItem();
			bool removeCalled = false;
			wi.Items.Removed += delegate { removeCalled = true; };

			object obj = wi.Services.AddNew<object>();
			wi.Services.Remove<object>();

			Assert.IsFalse(removeCalled);
		}

		#endregion

		// Features

		#region Activation

		[TestMethod]
		public void StatusIsInactiveWhenCreated()
		{
			WorkItem wi = new TestableRootWorkItem();

			Assert.AreEqual(WorkItemStatus.Inactive, wi.Status);
		}

		[TestMethod]
		public void StatusIsActiveWhenActivateCalled()
		{
			WorkItem wi = new TestableRootWorkItem();

			wi.Activate();

			Assert.AreEqual(WorkItemStatus.Active, wi.Status);
		}

		[TestMethod]
		public void StatusIsInactiveAfterDeactivateCalled()
		{
			WorkItem wi = new TestableRootWorkItem();

			wi.Activate();
			wi.Deactivate();

			Assert.AreEqual(WorkItemStatus.Inactive, wi.Status);
		}

		[TestMethod]
		public void StatusIsTerminatedAfterTerminateCalled()
		{
			WorkItem wi = new TestableRootWorkItem();

			wi.Terminate();

			Assert.AreEqual(WorkItemStatus.Terminated, wi.Status);
		}

		[TestMethod]
		public void WorkItemCallsActivationServiceWhenActivated()
		{
			WorkItem wi = new TestableRootWorkItem();
			MockWorkItemActivationService svc = wi.Services.AddNew<MockWorkItemActivationService, IWorkItemActivationService>();

			wi.Activate();

			Assert.IsTrue(svc.ChangeStatusCalled);
		}

		[TestMethod]
		public void WorkItemCallsActivationServiceWhenDeactivated()
		{
			WorkItem wi = new TestableRootWorkItem();
			MockWorkItemActivationService svc = wi.Services.AddNew<MockWorkItemActivationService, IWorkItemActivationService>();

			wi.Activate();
			wi.Deactivate();

			Assert.IsTrue(svc.ChangeStatusCalled);
		}

		[TestMethod]
		public void WorkItemCallsActivationServiceWhenTerminated()
		{
			WorkItem wi = new TestableRootWorkItem();
			MockWorkItemActivationService svc = wi.Services.AddNew<MockWorkItemActivationService, IWorkItemActivationService>();

			wi.Terminate();

			Assert.IsTrue(svc.ChangeStatusCalled);
		}

		[TestMethod]
		public void WorkItemPassesItselfToActivationServiceWhenActivated()
		{
			WorkItem wi = new TestableRootWorkItem();
			MockWorkItemActivationService svc = wi.Services.AddNew<MockWorkItemActivationService, IWorkItemActivationService>();

			wi.Activate();

			Assert.AreEqual(wi, svc.LastChangedItem);
		}

		[TestMethod]
		public void WorkItemPassesItselfToActivationServiceWhenDeactivated()
		{
			WorkItem wi = new TestableRootWorkItem();
			MockWorkItemActivationService svc = wi.Services.AddNew<MockWorkItemActivationService, IWorkItemActivationService>();

			wi.Activate();
			wi.Deactivate();

			Assert.AreEqual(wi, svc.LastChangedItem);
		}

		[TestMethod]
		public void WorkItemPassesItselfToActivationServiceWhenTerminated()
		{
			WorkItem wi = new TestableRootWorkItem();
			MockWorkItemActivationService svc = wi.Services.AddNew<MockWorkItemActivationService, IWorkItemActivationService>();

			wi.Terminate();

			Assert.AreEqual(wi, svc.LastChangedItem);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ActivateOnTerminatedWorkItemThrows()
		{
			WorkItem wi = new TestableRootWorkItem();

			wi.Terminate();
			wi.Activate();
		}

		[TestMethod]
		public void FiresActivatingEventWhenActivated()
		{
			WorkItem wi = new TestableRootWorkItem();

			bool activating = false;
			wi.Activating += delegate { activating = true; };
			wi.Activate();

			Assert.IsTrue(activating);
		}

		[TestMethod]
		public void CanCancelActivation()
		{
			WorkItem wi = new TestableRootWorkItem();

			bool activated = false;
			wi.Activating += delegate(object sender, CancelEventArgs e) { e.Cancel = true; };
			wi.Activated += delegate { activated = true; };
			wi.Activate();

			Assert.IsFalse(activated);
		}

		[TestMethod]
		public void FiresActivatedEventWhenAcivated()
		{
			WorkItem wi = new TestableRootWorkItem();

			bool activated = false;
			wi.Activated += delegate { activated = true; };
			wi.Activate();

			Assert.IsTrue(activated);
		}

		[TestMethod]
		public void FiresDeactivatedEventWhenDeactivated()
		{
			WorkItem wi = new TestableRootWorkItem();

			bool deactivated = false;
			wi.Deactivated += delegate { deactivated = true; };
			wi.Activate();
			wi.Deactivate();

			Assert.IsTrue(deactivated);
		}

		[TestMethod]
		public void FiresDeactivatingEventWhenDeactivated()
		{
			WorkItem wi = new TestableRootWorkItem();

			bool deactivating = false;
			wi.Deactivating += delegate { deactivating = true; };
			wi.Activate();
			wi.Deactivate();

			Assert.IsTrue(deactivating);
		}

		[TestMethod]
		public void CanCancelDeactivation()
		{
			WorkItem wi = new TestableRootWorkItem();

			bool deactivated = false;
			wi.Deactivated += delegate { deactivated = true; };
			wi.Deactivating += delegate(object sender, CancelEventArgs e) { e.Cancel = true; };
			wi.Activate();
			wi.Deactivate();

			Assert.IsFalse(deactivated);
		}

		[TestMethod]
		public void FiresTerminatedEventWhenTerminated()
		{
			WorkItem wi = new TestableRootWorkItem();

			bool terminated = false;
			wi.Terminated += delegate { terminated = true; };
			wi.Terminate();

			Assert.IsTrue(terminated);
		}

		[TestMethod]
		public void FiresTerminatingBeforeTerminated()
		{
			WorkItem wi = new TestableRootWorkItem();

			int calledFirst = 0;
			wi.Terminating += delegate { calledFirst = 1; };
			wi.Terminated += delegate { if (calledFirst != 1) { calledFirst = 2; } };
			wi.Terminate();

			Assert.AreEqual(1, calledFirst);
		}

		#endregion

		#region Dependency injection

		[TestMethod]
		public void CreatedDependenciesAreInItemsCollection()
		{
			WorkItem wi = new TestableRootWorkItem();
			int originalCount = wi.Items.Count;

			wi.Items.AddNew<MockDependingObject>();

			Assert.AreEqual(2, wi.Items.Count - originalCount);
		}

		#endregion

		#region Simple properties

		[TestMethod]
		public void NewWorkItemHasUniqueId()
		{
			WorkItem item1 = new TestableRootWorkItem();
			WorkItem item2 = new TestableRootWorkItem();

			Assert.IsTrue(item1.ID != item2.ID);
		}

		[TestMethod]
		public void WorkItemStateHasSameIdAsWorkItem()
		{
			WorkItem wi = new TestableRootWorkItem();

			Assert.AreEqual(wi.ID, wi.State.ID);
		}

		[TestMethod]
		public void CanGetCorrectNumberOfSmartParts()
		{
			TestableRootWorkItem wi = new TestableRootWorkItem();

			wi.SmartParts.Add(new Mock1());
			wi.SmartParts.Add(new Mock2());
			wi.SmartParts.Add(new Mock3());

			// Returns 3
			Assert.AreEqual(2, wi.SmartParts.Count);
		}

		[SmartPart]
		public class Mock1 { }

		[SmartPart]
		public class Mock2 { }

		public class Mock3 { }

		#endregion

		#region SmartPartInfo

		[TestMethod]
		public void GettingForNotRegisteredReturnsNull()
		{
			WorkItem wi = new TestableRootWorkItem();
			Control ctrl = new Control();
			wi.RegisterSmartPartInfo(ctrl, new SmartPartInfo("foo", "bar"));

			MySmartPartInfo info = wi.GetSmartPartInfo<MySmartPartInfo>(ctrl);

			Assert.IsNull(info);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CanGetSmartPartInfoThrowsForNull()
		{
			WorkItem wi = new TestableRootWorkItem();

			wi.GetSmartPartInfo<MySmartPartInfo>(null);
		}

		[TestMethod]
		public void CanRegisterASmartPartInfoForAControl()
		{
			WorkItem wi = new TestableRootWorkItem();
			Control ctrl = new Control();
			ISmartPartInfo info = new MySmartPartInfo();
			info.Title = "Title";
			info.Description = "Description";

			wi.RegisterSmartPartInfo(ctrl, info);

			Assert.AreSame(info, wi.GetSmartPartInfo<MySmartPartInfo>(ctrl));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RegisterSmartPartInfoIsGuardedForNullControl()
		{
			WorkItem wi = new TestableRootWorkItem();

			wi.RegisterSmartPartInfo(null, null);
		}

		[TestMethod]
		public void CanRegisterSeveralTypesOfSmartPartInfos()
		{
			WorkItem wi = new TestableRootWorkItem();
			MySmartPartInfo info1 = new MySmartPartInfo();
			MyOtherSmartPartInfo info2 = new MyOtherSmartPartInfo();
			Control ctrl = new Control();

			wi.RegisterSmartPartInfo(ctrl, info1);
			wi.RegisterSmartPartInfo(ctrl, info2);

			Assert.AreSame(info1, wi.GetSmartPartInfo<MySmartPartInfo>(ctrl));
			Assert.AreSame(info2, wi.GetSmartPartInfo<MyOtherSmartPartInfo>(ctrl));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RegisterSmartPartInfoIsGuardedForNullSmartPartInfo()
		{
			WorkItem wi = new TestableRootWorkItem();

			wi.RegisterSmartPartInfo(new Control(), null);
		}

		[TestMethod]
		public void RegisteringSameTypeSmartPartInfoKeepsTheLastOne()
		{
			WorkItem wi = new TestableRootWorkItem();
			MySmartPartInfo info1 = new MySmartPartInfo();
			MySmartPartInfo info2 = new MySmartPartInfo();
			Control ctrl = new Control();

			wi.RegisterSmartPartInfo(ctrl, info1);
			wi.RegisterSmartPartInfo(ctrl, info2);

			Assert.AreSame(info2, wi.GetSmartPartInfo<MySmartPartInfo>(ctrl));
		}

		#endregion

		#region State and persistence

		[TestMethod]
		[ExpectedException(typeof(ServiceMissingException))]
		public void LoadMethodThrowsWhenNoServicePresent()
		{
			WorkItem wi = new TestableRootWorkItem();

			wi.Load();
		}

		[TestMethod]
		[ExpectedException(typeof(ServiceMissingException))]
		public void SaveMethodThrowsIfNoPersistenceService()
		{
			WorkItem wi = new TestableRootWorkItem();

			wi.Save();
		}

		[TestMethod]
		public void LoadMethodCallsPersistenceService()
		{
			WorkItem wi = new TestableRootWorkItem();
			MockPersistenceService svc = wi.Services.AddNew<MockPersistenceService, IStatePersistenceService>();

			wi.Load();

			Assert.IsTrue(svc.LoadCalled);
		}

		[TestMethod]
		public void LoadMethodSetsWorkItemState()
		{
			WorkItem wi = new TestableRootWorkItem();
			MockPersistenceService svc = wi.Services.AddNew<MockPersistenceService, IStatePersistenceService>();

			wi.Load();

			Assert.AreSame(svc.LoadedState, wi.State);
		}

		[TestMethod]
		public void NewWorkItemHasState()
		{
			WorkItem wi = new TestableRootWorkItem();

			Assert.IsNotNull(wi.State);
		}

		[TestMethod]
		public void CanRemoveWorkItemState()
		{
			WorkItem wi = new TestableRootWorkItem();
			MockPersistenceService svc = wi.Services.AddNew<MockPersistenceService, IStatePersistenceService>();

			wi.DeleteState();

			Assert.IsTrue(svc.RemoveCalled);
		}

		[TestMethod]
		public void SaveMethodCallsPersistenceServiceWithWorkItemState()
		{
			WorkItem wi = new TestableRootWorkItem();
			MockPersistenceService svc = wi.Services.AddNew<MockPersistenceService, IStatePersistenceService>();

			wi.Save();

			Assert.IsTrue(svc.SaveCalled);
			Assert.AreSame(wi.State, svc.SavedState);
		}

		[TestMethod]
		public void EventBrokerEventArgsHasNewData()
		{
			MockStateWorkItem item = new MockStateWorkItem();
			object obj1 = new object();
			item.State["Test"] = obj1;

			Assert.IsNotNull(item.StateEventArgs);
			Assert.AreSame(item.StateEventArgs.NewValue, obj1);
		}

		[TestMethod]
		public void ChangingStateMoreThanOnceFiresEventMultipleTimes()
		{
			MockStateWorkItem item = new MockStateWorkItem();
			item.State["Test"] = new object();
			item.State["Test"] = new object();
			item.State["Test2"] = new object();
			item.State["Test"] = new object();

			Assert.AreEqual(3, item.StateChangeCalled);
		}

		[TestMethod]
		public void EventBrokerEventArgsHasNewDataAfterMultipleChanges()
		{
			MockStateWorkItem item = new MockStateWorkItem();
			object obj1 = new object();
			item.State["Test"] = obj1;
			object obj2 = new object();
			item.State["Test"] = obj2;
			object obj3 = new object();
			item.State["Test"] = obj3;

			Assert.IsNotNull(item.StateEventArgs);
			Assert.AreEqual(3, item.StateChangeCalled);
			Assert.AreSame(item.StateEventArgs.NewValue, obj3);
			Assert.AreSame(item.StateEventArgs.OldValue, obj2);
		}

		[TestMethod]
		public void SaveMethodResetsHasChangesFlag()
		{
			WorkItem wi = new TestableRootWorkItem();
			MockPersistenceService svc = wi.Services.AddNew<MockPersistenceService, IStatePersistenceService>();
			wi.State["foo"] = "foo";

			Assert.IsTrue(wi.State.HasChanges);

			wi.Save();

			Assert.IsFalse(wi.State.HasChanges);
		}

		#endregion

		// Test support

		#region Helper classes

		class MockTearDownStrategy : BuilderStrategy
		{
			public bool TearDownCalled = false;

			public override object TearDown(IBuilderContext context, object item)
			{
				TearDownCalled = true;
				return base.TearDown(context, item);
			}
		}

		class ChildWorkItem : WorkItem
		{
		}

		class MockStateWorkItem : TestableRootWorkItem
		{
			public int StateChangeCalled = 0;
			public StateChangedEventArgs StateEventArgs = null;

			[StateChanged("Test", ThreadOption.Publisher)]
			public void TestStateChanged(object sender, StateChangedEventArgs args)
			{
				StateChangeCalled++;
				StateEventArgs = args;
			}
		}

		interface IMockDataObject
		{
			int IntProperty { get; set; }
		}

		interface IMockDataObject2 { }

		class MockDataObject : IMockDataObject, IMockDataObject2
		{
			private int intProperty;

			public int IntProperty
			{
				get { return intProperty; }
				set { intProperty = value; }
			}
		}

		class MockWorkItemActivationService : IWorkItemActivationService
		{
			public bool ChangeStatusCalled = false;
			public WorkItem LastChangedItem;

			public void ChangeStatus(WorkItem item)
			{
				LastChangedItem = item;
				ChangeStatusCalled = true;
			}
		}

		class MockDisposableObject : IDisposable
		{
			public bool WasDisposed = false;

			public void Dispose()
			{
				WasDisposed = true;
			}
		}

		class MySmartPartInfo : ISmartPartInfo
		{
			private string description = "Default";
			private string title = "Default";

			public string Description
			{
				get { return description; }
				set { description = value; }
			}

			public string Title
			{
				get { return title; }
				set { title = value; }
			}
		}

		class MyOtherSmartPartInfo : MySmartPartInfo { }

		private class MockPersistenceService : IStatePersistenceService
		{
			public bool SaveCalled = false;
			public State SavedState = null;

			public bool LoadCalled = false;
			public State LoadedState = null;

			public bool RemoveCalled = false;

			public void Save(State state)
			{
				SaveCalled = true;
				SavedState = state;
			}

			public State Load(string id)
			{
				LoadCalled = true;
				LoadedState = new State(id);
				return LoadedState;
			}

			public void Remove(string id)
			{
				RemoveCalled = true;
			}

			public bool Contains(string id)
			{
				throw new NotImplementedException();
			}
		}

		public class MockDependingObject
		{
			[ComponentDependency("Foo", CreateIfNotFound = true)]
			public MockDependentObject DependentObject
			{
				set { }
			}
		}

		public class MockDependentObject
		{
		}

		#endregion
	}
}
