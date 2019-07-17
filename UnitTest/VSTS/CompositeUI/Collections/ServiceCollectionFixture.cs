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
using System.Text;
using Microsoft.Practices.CompositeUI.Collections;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.CompositeUI.Utility;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeUI.Tests.Collections
{
	[TestClass]
	public class ServiceCollectionFixture
	{
		#region Add - Generic

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddingNullServiceThrows_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.Add<object>(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CannotAddSameServiceTypeTwiceSinceServicesAreSingletons_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.Add<MockDataObject>(new MockDataObject());
			services.Add<MockDataObject>(new MockDataObject());
		}

		#endregion

		#region Add - Non-Generic

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddingNullServiceThrows()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.Add(typeof(object), null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddingNullServiceTypeThrows()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.Add(null, new object());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CannotAddSameServiceTypeTwiceSinceServicesAreSingletons()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.Add(typeof(MockDataObject), new MockDataObject());
			services.Add(typeof(MockDataObject), new MockDataObject());
		}

		[TestMethod]
		public void AddingServiceRunsTheBuilder()
		{
			BuilderAwareObject obj = new BuilderAwareObject();
			TestableServiceCollection services = CreateServiceCollection();

			services.Add(typeof(BuilderAwareObject), obj);

			Assert.IsTrue(obj.BuilderWasRun);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void RegistrationWithIncompatibleTypeThrows()
		{
			Service srv = new Service();
			TestableServiceCollection services = CreateServiceCollection();

			services.Add(typeof(IStartable), srv);
		}

		public interface IStartable { }
		public interface IService { }
		public class Service : IService { }

		#endregion

		#region AddOnDemand - Generic

		[TestMethod]
		public void CanAddDemandAddServiceAndRetrieveService_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.AddOnDemand<MockDemandService>();

			Assert.IsTrue(services.Contains<MockDemandService>());
			Assert.IsNotNull(services.Get<MockDemandService>());
		}

		[TestMethod]
		public void CanAddDemandAddServiceOfOneTypeAndRegisterAsOtherType_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.AddOnDemand<MockDemandService, IMockDemandService>();

			Assert.IsFalse(services.Contains<MockDemandService>());
			Assert.IsTrue(services.Contains<IMockDemandService>());
			Assert.IsNull(services.Get<MockDemandService>());
			Assert.IsNotNull(services.Get<IMockDemandService>());
		}

		[TestMethod]
		public void CanAddDemandAddServiceAndItWontBeCreatedUntilAskedFor_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();
			MockDemandService.WasCreated = false;

			services.AddOnDemand<MockDemandService>();
			Assert.IsFalse(MockDemandService.WasCreated);
			Assert.IsTrue(services.Contains<MockDemandService>());
			Assert.IsFalse(MockDemandService.WasCreated);

			MockDemandService svc = services.Get<MockDemandService>();
			Assert.IsTrue(MockDemandService.WasCreated);
		}

		[TestMethod]
		public void DemandAddedServiceFromParentGetsReplacedInParentEvenWhenAskedForFromChild_Generic()
		{
			TestableServiceCollection parent = CreateServiceCollection();
			TestableServiceCollection child = new TestableServiceCollection(parent);

			parent.AddOnDemand<MockDemandService>();
			MockDemandService svc = child.Get<MockDemandService>();

			Assert.AreSame(svc, parent.Locator.Get(new DependencyResolutionLocatorKey(typeof(MockDemandService), null)));
		}

		#endregion

		#region AddOnDemand - Non-Generic

		[TestMethod]
		public void CanAddDemandAddServiceAndRetrieveService()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.AddOnDemand(typeof(MockDemandService));

			Assert.IsTrue(services.Contains<MockDemandService>());
			Assert.IsNotNull(services.Get<MockDemandService>());
		}

		[TestMethod]
		public void CanAddDemandAddServiceOfOneTypeAndRegisterAsOtherType()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.AddOnDemand(typeof(MockDemandService), typeof(IMockDemandService));

			Assert.IsFalse(services.Contains<MockDemandService>());
			Assert.IsTrue(services.Contains<IMockDemandService>());
			Assert.IsNull(services.Get<MockDemandService>());
			Assert.IsNotNull(services.Get<IMockDemandService>());
		}

		[TestMethod]
		public void CanAddDemandAddServiceAndItWontBeCreatedUntilAskedFor()
		{
			TestableServiceCollection services = CreateServiceCollection();
			MockDemandService.WasCreated = false;

			services.AddOnDemand(typeof(MockDemandService));
			Assert.IsFalse(MockDemandService.WasCreated);
			Assert.IsTrue(services.Contains<MockDemandService>());
			Assert.IsFalse(MockDemandService.WasCreated);

			MockDemandService svc = services.Get<MockDemandService>();
			Assert.IsTrue(MockDemandService.WasCreated);
		}

		[TestMethod]
		public void NestedWorkItemIntegrationTest()
		{
			WorkItem parentWorkItem = new TestableRootWorkItem();
			WorkItem childWorkItem = parentWorkItem.WorkItems.AddNew<WorkItem>();

			MockA a = new MockA();
			MockA b = new MockA();

			parentWorkItem.Services.Add(typeof(MockA), a);
			childWorkItem.Services.Add(typeof(MockA), b);

			Assert.AreSame(a, parentWorkItem.Services.Get<MockA>());
			Assert.AreSame(b, childWorkItem.Services.Get<MockA>());

			MockB c = new MockB();

			parentWorkItem.Services.Add(typeof(MockB), c);

			// Throws ArgumentException
			childWorkItem.Services.AddOnDemand(typeof(MockB));
		}

		class MockA { }
		class MockB { }

		#endregion

		#region AddNew - Generic

		[TestMethod]
		public void CanCreateService_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();

			MockDataObject svc = services.AddNew<MockDataObject>();

			Assert.IsNotNull(svc);
			Assert.AreSame(svc, services.Get<MockDataObject>());
		}

		[TestMethod]
		public void CanCreateServiceRegisteredAsOtherType_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();

			MockDataObject svc = services.AddNew<MockDataObject, IMockDataObject>();

			Assert.IsNotNull(svc);
			Assert.IsNull(services.Get<MockDataObject>());
			Assert.AreSame(svc, services.Get<IMockDataObject>());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CannotCreateSameServiceTypeTwiceSinceServicesAreSingletons_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.AddNew<MockDataObject>();
			services.AddNew<MockDataObject>();
		}

		[TestMethod]
		public void CanCreateServiceInChildWhenServiceExistsInParent_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();
			TestableServiceCollection childServices = new TestableServiceCollection(services);

			MockDataObject parentService = services.AddNew<MockDataObject>();
			MockDataObject childService = childServices.AddNew<MockDataObject>();

			Assert.AreSame(parentService, services.Get<MockDataObject>());
			Assert.AreSame(childService, childServices.Get<MockDataObject>());
		}

		#endregion

		#region AddNew - Non-Generic

		[TestMethod]
		public void CanCreateService()
		{
			TestableServiceCollection services = CreateServiceCollection();

			object svc = services.AddNew(typeof(MockDataObject));

			Assert.IsNotNull(svc);
			Assert.AreSame(svc, services.Get(typeof(MockDataObject)));
		}

		[TestMethod]
		public void CanCreateServiceRegisteredAsOtherType()
		{
			TestableServiceCollection services = CreateServiceCollection();

			object svc = services.AddNew(typeof(MockDataObject), typeof(IMockDataObject));

			Assert.IsNotNull(svc);
			Assert.IsNull(services.Get(typeof(MockDataObject)));
			Assert.AreSame(svc, services.Get(typeof(IMockDataObject)));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CannotCreateSameServiceTypeTwiceSinceServicesAreSingletons()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.AddNew(typeof(MockDataObject));
			services.AddNew(typeof(MockDataObject));
		}

		[TestMethod]
		public void CanCreateServiceInChildWhenServiceExistsInParent()
		{
			TestableServiceCollection services = CreateServiceCollection();
			TestableServiceCollection childServices = new TestableServiceCollection(services);

			object parentService = services.AddNew(typeof(MockDataObject));
			object childService = childServices.AddNew(typeof(MockDataObject));

			Assert.AreSame(parentService, services.Get(typeof(MockDataObject)));
			Assert.AreSame(childService, childServices.Get(typeof(MockDataObject)));
		}

		#endregion

		#region Get - Generic

		[TestMethod]
		public void AddingServiceToWorkItemAllowsRetrievalOfService_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();
			MockDataObject svc = new MockDataObject();

			services.Add(typeof(MockDataObject), svc);
			MockDataObject result = services.Get<MockDataObject>();

			Assert.AreSame(svc, result);
		}

		[TestMethod]
		public void GetServiceCanReturnNullWhenServiceDoesntExist_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();

			Assert.IsNull(services.Get<MockDataObject>());
		}

		[TestMethod]
		public void CanAskForBuilderToEnsureServiceExists_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();
			MockDataObject svc = new MockDataObject();

			services.Add(typeof(MockDataObject), svc);
			MockDataObject result = services.Get<MockDataObject>(true);

			Assert.AreSame(svc, result);
		}

		[TestMethod]
		[ExpectedException(typeof(ServiceMissingException))]
		public void GetServiceCanThrowOnMissingService_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.Get<MockDataObject>(true);
		}

		#endregion

		#region Get - Non-Generic

		[TestMethod]
		public void AddingServiceToWorkItemAllowsRetrievalOfService()
		{
			TestableServiceCollection services = CreateServiceCollection();
			MockDataObject svc = new MockDataObject();

			services.Add(typeof(MockDataObject), svc);
			MockDataObject result = (MockDataObject)services.Get(typeof(MockDataObject));

			Assert.AreSame(svc, result);
		}

		[TestMethod]
		public void GetServiceCanReturnNullWhenServiceDoesntExist()
		{
			TestableServiceCollection services = CreateServiceCollection();

			Assert.IsNull(services.Get(typeof(MockDataObject)));
		}

		[TestMethod]
		public void CanAskForBuilderToEnsureServiceExists()
		{
			TestableServiceCollection services = CreateServiceCollection();
			MockDataObject svc = new MockDataObject();

			services.Add(typeof(MockDataObject), svc);
			MockDataObject result = (MockDataObject)services.Get(typeof(MockDataObject), true);

			Assert.AreSame(svc, result);
		}

		[TestMethod]
		[ExpectedException(typeof(ServiceMissingException))]
		public void GetServiceCanThrowOnMissingService()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.Get(typeof(MockDataObject), true);
		}

		#endregion

		#region Remove - Generic

		[TestMethod]
		public void CanRemoveService_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.Add<MockDataObject>(new MockDataObject());
			services.Remove<MockDataObject>();

			Assert.IsNull(services.Get<MockDataObject>());
		}

		[TestMethod]
		public void RemovingServiceAllowsNewServiceToBeRegisteredForGivenServiceType_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.AddNew<object>();
			services.Remove<object>();
			services.AddNew<object>();
		}

		[TestMethod]
		public void RemovingServiceRemovesStrongReferenceToService_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();
			WeakReference wr = new WeakReference(services.AddNew<object>());

			services.Remove<object>();
			GC.Collect();

			Assert.IsNull(wr.Target);
		}

		[TestMethod]
		public void RemovingMultipleRegisteredServiceOnlyRemovesStrongReferenceWhenLastInstanceIsGone_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();
			MockDataObject mdo = new MockDataObject();
			WeakReference wr = new WeakReference(mdo);
			services.Add<IMockDataObject>(mdo);
			services.Add<IMockDataObject2>(mdo);
			mdo = null;

			services.Remove<IMockDataObject>();
			GC.Collect();
			Assert.IsNotNull(wr.Target);

			services.Remove<IMockDataObject2>();
			GC.Collect();
			Assert.IsNull(wr.Target);
		}

		[TestMethod]
		public void RemovingServiceCausesItToBeTornDown_Generic()
		{
			TestableServiceCollection services = CreateServiceCollection();
			MockTearDownStrategy strategy = new MockTearDownStrategy();
			services.Builder.Strategies.Add(strategy, BuilderStage.PreCreation);

			services.AddNew<object>();
			services.Remove<object>();

			Assert.IsTrue(strategy.TearDownCalled);
		}

		#endregion

		#region Remove - Non-Generic

		[TestMethod]
		public void CanRemoveService()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.Add(typeof(MockDataObject), new MockDataObject());
			services.Remove(typeof(MockDataObject));

			Assert.IsNull(services.Get(typeof(MockDataObject)));
		}

		[TestMethod]
		public void RemovingServiceAllowsNewServiceToBeRegisteredForGivenServiceType()
		{
			TestableServiceCollection services = CreateServiceCollection();

			services.AddNew(typeof(object));
			services.Remove(typeof(object));
			services.AddNew(typeof(object));
		}

		[TestMethod]
		public void RemovingServiceRemovesStrongReferenceToService()
		{
			TestableServiceCollection services = CreateServiceCollection();
			WeakReference wr = new WeakReference(services.AddNew<object>());

			services.Remove(typeof(object));
			GC.Collect();

			Assert.IsNull(wr.Target);
		}

		#endregion

		#region IEnumerable

		[TestMethod]
		public void CanEnumerateCollection()
		{
			TestableServiceCollection collection = CreateServiceCollection();
			object obj1 = new object();
			string obj2 = "Hello world";

			collection.Add(obj1);
			collection.Add(obj2);

			bool o1Found = false;
			bool o2Found = false;
			foreach (KeyValuePair<Type, object> pair in collection)
			{
				if (pair.Value.Equals(obj1))
					o1Found = true;
				if (pair.Value.Equals(obj2))
					o2Found = true;
			}

			Assert.IsTrue(o1Found);
			Assert.IsTrue(o2Found);
		}

		[TestMethod]
		public void EnumeratorIgnoresItemsAddedDirectlyToLocator()
		{
			TestableServiceCollection collection = CreateServiceCollection();
			object obj1 = new object();
			string obj2 = "Hello world";
			object obj3 = new object();

			collection.Add(obj1);
			collection.Add(obj2);
			collection.Locator.Add(typeof(object), obj3);

			bool o3Found = false;
			foreach (KeyValuePair<Type, object> pair in collection)
			{
				if (pair.Value == obj3)
					o3Found = true;
			}

			Assert.IsFalse(o3Found);
		}

		#endregion

		#region Added/Removed Events

		[TestMethod]
		public void CreatingAServiceFiresEvent()
		{
			bool EventFired = false;
			object ServiceInEvent = null;
			TestableServiceCollection services = CreateServiceCollection();

			services.Added += delegate(object sender, DataEventArgs<object> e) { EventFired = true; ServiceInEvent = e.Data; };

			object obj = services.AddNew<object>();

			Assert.IsTrue(EventFired);
			Assert.AreSame(obj, ServiceInEvent);
		}

		[TestMethod]
		public void AddingAServiceFiresEvent()
		{
			bool EventFired = false;
			object ServiceInEvent = null;
			object obj = new object();
			TestableServiceCollection services = CreateServiceCollection();

			services.Added += delegate(object sender, DataEventArgs<object> e) { EventFired = true; ServiceInEvent = e.Data; };
			services.Add(obj);

			Assert.IsTrue(EventFired);
			Assert.AreSame(obj, ServiceInEvent);
		}

		[TestMethod]
		public void AddingAServiceTwiceFiresEventOnce()
		{
			int EventFireCount = 0;
			string str = "Hello world";
			TestableServiceCollection services = CreateServiceCollection();

			services.Added += delegate { EventFireCount++; };
			services.Add<string>(str);
			services.Add<object>(str);

			Assert.AreEqual(1, EventFireCount);
		}

		[TestMethod]
		public void RemovingServiceFiresEvent()
		{
			bool EventFired = false;
			object ServiceInEvent = null;
			TestableServiceCollection services = CreateServiceCollection();

			services.Removed += delegate(object sender, DataEventArgs<object> e) { EventFired = true; ServiceInEvent = e.Data; };
			object obj = services.AddNew<object>();
			services.Remove(typeof(object));

			Assert.IsTrue(EventFired);
			Assert.AreSame(obj, ServiceInEvent);
		}

		[TestMethod]
		public void RemovingServiceTwiceFiresEventOnce()
		{
			int EventFireCount = 0;
			TestableServiceCollection services = CreateServiceCollection();

			services.Removed += delegate { EventFireCount++; };
			services.AddNew<object>();
			services.Remove(typeof(object));
			services.Remove(typeof(object));

			Assert.AreEqual(1, EventFireCount);
		}

		[TestMethod]
		public void RemovingServiceNotInWorkItemDoesntFireEvent()
		{
			bool EventFired = false;
			TestableServiceCollection services = CreateServiceCollection();

			services.Removed += delegate { EventFired = true; };
			services.Remove(typeof(object));

			Assert.IsFalse(EventFired);
		}

		[TestMethod]
		public void RemovingMultiplyRegisteredServiceInstanceDoesntFireEventUntilLastInstanceIsRemoved()
		{
			bool EventFired = false;
			string str = "Hello world";
			TestableServiceCollection services = CreateServiceCollection();

			services.Removed += delegate { EventFired = true; };

			services.Add<string>(str);
			services.Add<object>(str);

			services.Remove<object>();
			Assert.IsFalse(EventFired);
			services.Remove<string>();
			Assert.IsTrue(EventFired);
		}

		[TestMethod]
		public void DemandAddedServiceEventsAreFiredAtTheRightTime()
		{
			bool AddedEventFired = false;
			bool RemovedEventFired = false;
			TestableServiceCollection services = CreateServiceCollection();

			services.Added += delegate { AddedEventFired = true; };
			services.Removed += delegate { RemovedEventFired = true; };

			services.AddOnDemand<object>();
			Assert.IsFalse(AddedEventFired);
			Assert.IsFalse(RemovedEventFired);

			services.Get<object>();
			Assert.IsTrue(AddedEventFired);
			Assert.IsFalse(RemovedEventFired);

			services.Remove<object>();
			Assert.IsTrue(RemovedEventFired);
		}

		#endregion

		#region Located For DI Resolution

		[TestMethod]
		public void AddedServiceCanBeLocatedByTypeIDPair()
		{
			TestableServiceCollection collection = CreateServiceCollection();

			object obj = collection.AddNew<object>();

			Assert.AreSame(obj, collection.Locator.Get(new DependencyResolutionLocatorKey(typeof(object), null)));
		}

		[TestMethod]
		public void RemovingItemRemovesTypeIdPairFromLocator()
		{
			TestableServiceCollection collection = CreateServiceCollection();
			object obj = collection.AddNew<object>();

			collection.Remove(typeof(object));

			Assert.IsNull(collection.Locator.Get(new DependencyResolutionLocatorKey(typeof(object), null)));
		}

		[TestMethod]
		public void RemovingSpecificServiceTypeRegistrationsRemovesOnlyThoseDependencyKeys()
		{
			TestableServiceCollection collection = CreateServiceCollection();
			string str = "Hello world";

			collection.Add<string>(str);
			collection.Add<object>(str);

			collection.Remove(typeof(object));
			Assert.IsNull(collection.Locator.Get(new DependencyResolutionLocatorKey(typeof(object), null)));
			Assert.IsNotNull(collection.Locator.Get(new DependencyResolutionLocatorKey(typeof(string), null)));

			collection.Remove(typeof(string));
			Assert.IsNull(collection.Locator.Get(new DependencyResolutionLocatorKey(typeof(string), null)));
		}

		#endregion

		#region Helpers

		class MockTearDownStrategy : BuilderStrategy
		{
			public bool TearDownCalled = false;

			public override object TearDown(IBuilderContext context, object item)
			{
				TearDownCalled = true;
				return base.TearDown(context, item);
			}
		}

		private TestableServiceCollection CreateServiceCollection()
		{
			LifetimeContainer container = new LifetimeContainer();
			Locator locator = new Locator();
			locator.Add(typeof(ILifetimeContainer), container);

			return new TestableServiceCollection(container, locator, CreateBuilder());
		}

		private Builder CreateBuilder()
		{
			Builder result = new Builder();
			result.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			return result;
		}

		private class TestableServiceCollection : ServiceCollection
		{
			private IBuilder<BuilderStage> builder;
			private ILifetimeContainer container;
			private IReadWriteLocator locator;

			public TestableServiceCollection(ILifetimeContainer container, IReadWriteLocator locator, IBuilder<BuilderStage> builder)
				: this(container, locator, builder, null)
			{
			}

			public TestableServiceCollection(TestableServiceCollection parent)
				: this(parent.container, new Locator(parent.locator), parent.builder, parent)
			{
				locator.Add(typeof(ILifetimeContainer), parent.locator.Get<ILifetimeContainer>());
			}

			private TestableServiceCollection(ILifetimeContainer container, IReadWriteLocator locator, IBuilder<BuilderStage> builder, TestableServiceCollection parent)
				: base(container, locator, builder, parent)
			{
				this.builder = builder;
				this.container = container;
				this.locator = locator;
			}

			public ILifetimeContainer LifetimeContainer
			{
				get { return container; }
			}

			public IReadWriteLocator Locator
			{
				get { return locator; }
			}

			public IBuilder<BuilderStage> Builder
			{
				get { return builder; }
			}
		}

		public class BuilderAwareObject : IBuilderAware
		{
			public bool BuilderWasRun = false;
			public int BuilderRunCount = 0;

			public void OnBuiltUp(string id)
			{
				BuilderWasRun = true;
				BuilderRunCount++;
			}

			public void OnTearingDown()
			{
				BuilderWasRun = true;
				BuilderRunCount++;
			}
		}

		interface IMockDataObject { }

		interface IMockDataObject2 { }

		class MockDataObject : IMockDataObject, IMockDataObject2
		{
			private int intProp;

			public int IntProperty
			{
				get { return intProp; }
				set { intProp = value; }
			}
		}

		class MockDependencyService { }

		interface IMockDependingService { }

		class MockDependingService : IMockDependingService
		{
			MockDependencyService injectedService;

			[ServiceDependency]
			public MockDependencyService InjectedService
			{
				get { return injectedService; }
				set { injectedService = value; }
			}
		}

		private class SomeService
		{
			private object myDependency;

			[ServiceDependency]
			public object MyDependency
			{
				get { return myDependency; }
				set { myDependency = value; }
			}
		}

		interface IMockDemandService { }

		class MockDemandService : IMockDemandService
		{
			public static bool WasCreated;

			public MockDemandService()
			{
				WasCreated = true;
			}
		}

		#endregion
	}
}
