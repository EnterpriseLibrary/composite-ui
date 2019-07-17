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
using Microsoft.Practices.CompositeUI.Collections;
using Microsoft.Practices.CompositeUI.Utility;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeUI.Tests.Collections
{
	[TestClass]
	public class ManagedObjectCollectionFixture
	{
		#region Add

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddingNullObjectThrows()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();
			collection.Add(null);
		}

		[TestMethod]
		public void AddedObjectStoredInProvidedContainer()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();
			object obj = new object();

			collection.Add(obj);

			Assert.IsTrue(collection.LifetimeContainer.Contains(obj));
		}

		[TestMethod]
		public void AddedObjectIsAddedToLocator()
		{
			object obj = new object();
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			collection.Add(obj);

			IReadableLocator locator = collection.Locator.FindBy(delegate(KeyValuePair<object, object> pair)
			{
				return pair.Value == obj;
			});

			Assert.AreEqual(1, locator.Count);
		}

		[TestMethod]
		public void AddingObjectRunsTheBuilder()
		{
			BuilderAwareObject obj = new BuilderAwareObject();
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			collection.Add(obj);

			Assert.IsTrue(obj.BuilderWasRun);
		}

		[TestMethod]
		public void AddingUnnamedObjectTwiceYieldsSingleObjectInContainer()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();
			object obj = new object();

			collection.Add(obj);
			collection.Add(obj);

			Assert.AreEqual(1, collection.LifetimeContainer.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddingSameObjectWithSameNameThrows()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();
			object obj = new object();

			collection.Add(obj, "Foo");
			collection.Add(obj, "Foo");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddingTwoDifferentObjectsWithSameNameThrows()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			collection.Add(new object(), "Foo");
			collection.Add("Bar", "Foo");
		}

		[TestMethod]
		public void AddingObjectTwiceOnlyInjectsDependenciesOnce()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			MockDataObject obj = new MockDataObject();

			PropertySetterPolicy policy1 = new PropertySetterPolicy();
			policy1.Properties.Add("IntProperty", new PropertySetterInfo("IntProperty", new ValueParameter<int>(19)));
			collection.Builder.Policies.Set<IPropertySetterPolicy>(policy1, typeof(MockDataObject), "Foo");

			PropertySetterPolicy policy2 = new PropertySetterPolicy();
			policy2.Properties.Add("IntProperty", new PropertySetterInfo("IntProperty", new ValueParameter<int>(36)));
			collection.Builder.Policies.Set<IPropertySetterPolicy>(policy2, typeof(MockDataObject), "Bar");

			collection.Add(obj, "Foo");
			Assert.AreEqual(19, obj.IntProperty);

			collection.Add(obj, "Bar");
			Assert.AreEqual(19, obj.IntProperty);
		}

		[TestMethod]
		public void CanAddSameObjectWithManyNames()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();
			object obj = new object();

			collection.Add(obj, "Foo");
			collection.Add(obj, "Bar");

			Assert.AreEqual(1, collection.LifetimeContainer.Count);
			Assert.AreSame(obj, collection.Get("Foo"));
			Assert.AreSame(obj, collection.Get("Bar"));
		}

		#endregion

		#region AddNew - Generic

		[TestMethod]
		public void AddNewWillCreateANewObjectAndGiveItToMe_Generic()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			object obj = collection.AddNew<object>();

			Assert.IsNotNull(obj);
		}

		[TestMethod]
		public void AddNewNamedWillCreateANewObjectAndGiveItToMe_Generic()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			object obj = collection.AddNew<object>("Foo");

			Assert.IsNotNull(obj);
		}

		[TestMethod]
		public void AddNewAddsToLocatorAndLifetimeContainer_Generic()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			object obj = collection.AddNew<object>("Foo");

			Assert.IsTrue(collection.LifetimeContainer.Contains(obj));
			Assert.AreEqual(obj, collection.Get("Foo"));
		}

		[TestMethod]
		public void AddNewOnlyCallsBuilderOnce_Generic()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			BuilderAwareObject obj = collection.AddNew<BuilderAwareObject>();

			Assert.AreEqual(1, obj.BuilderRunCount);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatingTwoObjectsOfDifferentTypesButTheSameNameThrows_Generic()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			collection.AddNew<object>("One");
			collection.AddNew<int>("One");
		}

		#endregion

		#region AddNew - Non-Generic

		[TestMethod]
		public void AddNewWillCreateANewObjectAndGiveItToMe()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			object obj = collection.AddNew(typeof(object));

			Assert.IsNotNull(obj);
		}

		[TestMethod]
		public void AddNewNamedWillCreateANewObjectAndGiveItToMe()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			object obj = collection.AddNew(typeof(object), "Foo");

			Assert.IsNotNull(obj);
		}

		[TestMethod]
		public void AddNewAddsToLocatorAndLifetimeContainer()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			object obj = collection.AddNew(typeof(object), "Foo");

			Assert.IsTrue(collection.LifetimeContainer.Contains(obj));
			Assert.AreEqual(obj, collection.Get("Foo"));
		}

		[TestMethod]
		public void AddNewOnlyCallsBuilderOnce()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			BuilderAwareObject obj = (BuilderAwareObject)collection.AddNew(typeof(BuilderAwareObject));

			Assert.AreEqual(1, obj.BuilderRunCount);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatingTwoObjectsOfDifferentTypesButTheSameNameThrows()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			collection.AddNew(typeof(object), "One");
			collection.AddNew(typeof(int), "One");
		}

		#endregion

		#region Contains

		[TestMethod]
		public void CanFindOutIfCollectionContainsNamedObject()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();
			object obj = new object();

			collection.Add(obj, "Foo");

			Assert.IsTrue(collection.Contains("Foo"));
		}

		[TestMethod]
		public void ContainsDoesNotCheckParent()
		{
			TestableManagedObjectCollection<object> parentCollection = CreateManagedObjectCollection();
			TestableManagedObjectCollection<object> childCollection = new TestableManagedObjectCollection<object>(parentCollection);
			object obj = new object();

			parentCollection.Add(obj, "Foo");

			Assert.IsFalse(childCollection.Contains("Foo"));
		}

		[TestMethod]
		public void CanFindOutIfCollectionContainsObject()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();
			object obj = new object();

			collection.Add(obj, "Foo");

			Assert.IsTrue(collection.ContainsObject(obj));
		}

		#endregion

		#region Count

		[TestMethod]
		public void EmptyCollectionHasNoItemsInIt()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			Assert.AreEqual(0, collection.Count);
		}

		[TestMethod]
		public void CountReflectsNumberOfRegistrations()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();
			object obj = new object();

			collection.Add(obj, "Foo");
			collection.Add(obj, "Bar");

			Assert.AreEqual(2, collection.Count);
		}

		[TestMethod]
		public void CountReturnsLocalObjectCountOnly()
		{
			TestableManagedObjectCollection<object> parentCollection = CreateManagedObjectCollection();
			TestableManagedObjectCollection<object> childCollection = new TestableManagedObjectCollection<object>(parentCollection);

			parentCollection.AddNew<object>();

			Assert.AreEqual(1, parentCollection.Count);
			Assert.AreEqual(0, childCollection.Count);
		}

		[TestMethod]
		public void CouldIgnoresObjectsOfTheWrongType()
		{
			TestableManagedObjectCollection<object> objectCollection = CreateManagedObjectCollection();
			TestableManagedObjectCollection<string> stringCollection =
				new TestableManagedObjectCollection<string>(objectCollection.LifetimeContainer, objectCollection.Locator, objectCollection.Builder, objectCollection.SearchMode, null, null, null);

			object obj1 = new object();
			object obj2 = new object();
			string obj3 = "Hello there!";

			objectCollection.Add(obj1);
			objectCollection.Add(obj2);
			objectCollection.Add(obj3);

			Assert.AreEqual(3, objectCollection.Count);
			Assert.AreEqual(1, stringCollection.Count);
		}

		#endregion

		#region FindByType - Generic

		[TestMethod]
		public void CanFindObjectsByAssignableType_Generic()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			collection.Add("Hello world");

			Assert.AreEqual(1, collection.FindByType<string>().Count);
			Assert.AreEqual(1, collection.FindByType<object>().Count);
			Assert.AreEqual(0, collection.FindByType<int>().Count);
		}

		[TestMethod]
		public void FindByTypeSearchesParentContainerWhenConfiguredForSearchUp_Generic()
		{
			TestableManagedObjectCollection<object> parentCollection = CreateManagedObjectCollection();
			ManagedObjectCollection<object> childCollection = 
				new ManagedObjectCollection<object>(new LifetimeContainer(), new Locator(parentCollection.Locator), parentCollection.Builder, SearchMode.Up, null, null, parentCollection);

			parentCollection.AddNew<object>();

			Assert.AreEqual(1, childCollection.FindByType<object>().Count);
		}

		#endregion

		#region FindByType - Non-Generic

		[TestMethod]
		public void CanFindObjectsByAssignableType()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			collection.Add("Hello world");

			Assert.AreEqual(1, collection.FindByType(typeof(string)).Count);
			Assert.AreEqual(1, collection.FindByType(typeof(object)).Count);
			Assert.AreEqual(0, collection.FindByType(typeof(int)).Count);
		}

		[TestMethod]
		public void FindByTypeSearchesParentContainerWhenConfiguredForSearchUp()
		{
			TestableManagedObjectCollection<object> parentCollection = CreateManagedObjectCollection();
			ManagedObjectCollection<object> childCollection = 
				new ManagedObjectCollection<object>(new LifetimeContainer(), new Locator(parentCollection.Locator), parentCollection.Builder, SearchMode.Up, null, null, parentCollection);

			parentCollection.AddNew<object>();

			Assert.AreEqual(1, childCollection.FindByType(typeof(object)).Count);
		}

		#endregion

		#region Get - Generic

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetWithNullIDThrows_Generic()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			collection.Get<object>(null);
		}

		[TestMethod]
		public void CanAddNamedObjectAndFindItByName_Generic()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			object obj = collection.AddNew<object>("foo");

			Assert.AreSame(obj, collection.Get<object>("foo"));
		}

		[TestMethod]
		public void GetForObjectNotInCollectionReturnsNull_Generic()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			Assert.IsNull(collection.Get<object>("bar"));
		}

		[TestMethod]
		public void GetDoesNotCheckParentCollection_Generic()
		{
			TestableManagedObjectCollection<object> parentCollection = CreateManagedObjectCollection();
			TestableManagedObjectCollection<object> childCollection = new TestableManagedObjectCollection<object>(parentCollection);
			object obj = new object();

			parentCollection.Add(obj, "Foo");

			Assert.IsNull(childCollection.Get<object>("Foo"));
		}

		#endregion

		#region Get - Non-Generic

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetWithNullIDThrows()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			collection.Get(null);
		}

		[TestMethod]
		public void CanAddNamedObjectAndFindItByName()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			object obj = collection.AddNew<object>("foo");

			Assert.AreSame(obj, collection.Get("foo"));
		}

		[TestMethod]
		public void GetForObjectNotInCollectionReturnsNull()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			Assert.IsNull(collection.Get("bar"));
		}

		[TestMethod]
		public void GetDoesNotCheckParentCollection()
		{
			TestableManagedObjectCollection<object> parentCollection = CreateManagedObjectCollection();
			TestableManagedObjectCollection<object> childCollection = new TestableManagedObjectCollection<object>(parentCollection);
			object obj = new object();

			parentCollection.Add(obj, "Foo");

			Assert.IsNull(childCollection.Get("Foo"));
		}

		#endregion

		#region Indexer

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IndexerWithNullIDThrows()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			object unused = collection[null];
		}

		[TestMethod]
		public void CanAddNamedObjectAndIndexItByName()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			object obj = collection.AddNew<object>("foo");

			Assert.AreSame(obj, collection["foo"]);
		}

		[TestMethod]
		public void IndexerForObjectNotInCollectionReturnsNull()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			Assert.IsNull(collection["bar"]);
		}

		[TestMethod]
		public void IndexerDoesNotCheckParentCollection()
		{
			TestableManagedObjectCollection<object> parentCollection = CreateManagedObjectCollection();
			TestableManagedObjectCollection<object> childCollection = new TestableManagedObjectCollection<object>(parentCollection);
			object obj = new object();

			parentCollection.Add(obj, "Foo");

			Assert.IsNull(childCollection["Foo"]);
		}

		#endregion

		#region Remove

		[TestMethod]
		public void RemovingNullDoesntThrow()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			collection.Remove(null);
		}

		[TestMethod]
		public void RemoveRemovesFromLifetimeContainer()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();
			object obj = new object();

			collection.Add(obj);
			collection.Remove(obj);

			Assert.IsFalse(collection.LifetimeContainer.Contains(obj));
		}

		[TestMethod]
		public void RemoveRemovesFromLocator()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();
			object obj = new object();

			collection.Add(obj, "Foo");
			collection.Remove(obj);

			Assert.IsFalse(collection.Locator.Contains("Foo"));
		}

		[TestMethod]
		public void RemovingObjectNotInCollectionNoThrow()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			collection.Remove(new object());
		}

		[TestMethod]
		public void RemovingNamedObjectCausesNameToBeAvailableAgain()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			object obj = collection.AddNew<object>("Foo");
			collection.Remove(obj);
			object obj2 = collection.AddNew<object>("Foo");

			Assert.IsNotNull(obj2);
			Assert.IsTrue(obj != obj2);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void RemovingWorkItemThrows()
		{
			TestableRootWorkItem root = new TestableRootWorkItem();
			WorkItem child = root.Items.AddNew<WorkItem>();

			root.Items.Remove(child);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void RemovingWorkItemDerivedClassThrows()
		{
			TestableRootWorkItem root = new TestableRootWorkItem();
			MockWorkItem child = root.Items.AddNew<MockWorkItem>();

			root.Items.Remove(child);
		}

		[TestMethod]
		public void RemovingObjectCausesItToBeTornDown()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();
			MockTearDownStrategy strategy = new MockTearDownStrategy();
			collection.Builder.Strategies.Add(strategy, BuilderStage.PreCreation);

			object obj = collection.AddNew<object>();
			collection.Remove(obj);

			Assert.IsTrue(strategy.TearDownCalled);
		}

		#endregion

		#region IEnumerable

		[TestMethod]
		public void CanEnumerateCollection()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();
			object obj1 = new object();
			object obj2 = new object();

			collection.Add(obj1);
			collection.Add(obj2);

			bool o1Found = false;
			bool o2Found = false;
			foreach (KeyValuePair<string, object> pair in collection)
			{
				if (pair.Value == obj1)
					o1Found = true;
				if (pair.Value == obj2)
					o2Found = true;
			}

			Assert.IsTrue(o1Found);
			Assert.IsTrue(o2Found);
		}

		[TestMethod]
		public void EnumeratorIgnoresItemsAddedDirectlyToLocator()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();
			object obj1 = new object();
			object obj2 = new object();
			object obj3 = new object();

			collection.Add(obj1);
			collection.Add(obj2);
			collection.Locator.Add("Foo", obj3);

			bool o3Found = false;
			foreach (KeyValuePair<string, object> pair in collection)
			{
				if (pair.Value == obj3)
					o3Found = true;
			}

			Assert.IsFalse(o3Found);
		}

		[TestMethod]
		public void EnumeratorIgnoresItemsOfTheWrongTypeInTheLocator()
		{
			TestableManagedObjectCollection<object> objectCollection = CreateManagedObjectCollection();
			TestableManagedObjectCollection<string> stringCollection =
				new TestableManagedObjectCollection<string>(objectCollection.LifetimeContainer, objectCollection.Locator, objectCollection.Builder, objectCollection.SearchMode, null, null, null);

			object obj1 = new object();
			object obj2 = new object();
			string obj3 = "Hello there!";

			objectCollection.Add(obj1);
			objectCollection.Add(obj2);
			objectCollection.Add(obj3);

			bool o1Found = false;
			bool o2Found = false;
			bool o3Found = false;

			foreach (KeyValuePair<string, string> pair in stringCollection)
			{
				if (object.ReferenceEquals(pair.Value, obj1))
					o1Found = true;
				if (object.ReferenceEquals(pair.Value, obj2))
					o2Found = true;
				if (object.ReferenceEquals(pair.Value, obj3))
					o3Found = true;
			}

			Assert.IsFalse(o1Found);
			Assert.IsFalse(o2Found);
			Assert.IsTrue(o3Found);
		}

		#endregion

		#region SearchMode Flags

		[TestMethod]
		public void GetCanSearchUpTheLocatorChain()
		{
			TestableManagedObjectCollection<object> parentCollection = CreateManagedObjectCollection(SearchMode.Up);
			TestableManagedObjectCollection<object> childCollection = new TestableManagedObjectCollection<object>(parentCollection);

			object obj = parentCollection.AddNew<object>("Foo");

			Assert.AreSame(obj, childCollection.Get("Foo"));
		}

		[TestMethod]
		public void EnumerationCanSearchUpTheLocatorChain()
		{
			TestableManagedObjectCollection<object> parentCollection = CreateManagedObjectCollection(SearchMode.Up);
			TestableManagedObjectCollection<object> childCollection = new TestableManagedObjectCollection<object>(parentCollection);

			object obj1 = parentCollection.AddNew<object>("Foo");
			object obj2 = childCollection.AddNew<object>("Bar");

			bool o1Found = false;
			bool o2Found = false;
			foreach (KeyValuePair<string, object> pair in childCollection)
			{
				if (pair.Value == obj1)
					o1Found = true;
				if (pair.Value == obj2)
					o2Found = true;
			}

			Assert.IsTrue(o1Found);
			Assert.IsTrue(o2Found);
		}

		[TestMethod]
		public void GetDoesNotReturnReplacedObject()
		{
			TestableManagedObjectCollection<object> parentCollection = CreateManagedObjectCollection(SearchMode.Up);
			TestableManagedObjectCollection<object> childCollection = new TestableManagedObjectCollection<object>(parentCollection);

			object obj1 = parentCollection.AddNew<object>("Foo");
			object obj2 = childCollection.AddNew<object>("Foo");

			Assert.AreSame(obj2, childCollection.Get("Foo"));
		}

		[TestMethod]
		public void EnumerationDoesNotReturnReplacedObjects()
		{
			TestableManagedObjectCollection<object> parentCollection = CreateManagedObjectCollection(SearchMode.Up);
			TestableManagedObjectCollection<object> childCollection = new TestableManagedObjectCollection<object>(parentCollection);

			object obj1 = parentCollection.AddNew<object>("Foo");
			object obj2 = childCollection.AddNew<object>("Foo");

			bool o1Found = false;
			bool o2Found = false;
			foreach (KeyValuePair<string, object> pair in childCollection)
			{
				if (pair.Value == obj1)
					o1Found = true;
				if (pair.Value == obj2)
					o2Found = true;
			}

			Assert.IsFalse(o1Found);
			Assert.IsTrue(o2Found);
		}

		#endregion

		#region IndexerBehavior Flags

		[TestMethod]
		public void IndexerCreatesWhenFlagSetToCreateAndItemDoesNotExist()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection(SearchMode.Local, delegate { return new object(); });

			object foo = collection["Foo"];

			Assert.IsNotNull(foo);
		}

		[TestMethod]
		public void CreatedIndexerItemIsRetrievedForSecondIndex()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection(SearchMode.Local, delegate { return new object(); });

			object foo = collection["Foo"];
			object foo2 = collection["Foo"];

			Assert.AreSame(foo, foo2);
		}

		#endregion

		#region Filters

		[TestMethod]
		public void GetFiltersReturnedCollectionWithPredicate()
		{
			Predicate<object> filter = delegate(object obj)
			{
				return (obj is MockDataObject);
			};

			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection(SearchMode.Local, null, filter);

			object o1 = collection.AddNew<object>("One");
			MockDataObject o2 = collection.AddNew<MockDataObject>("Two");

			Assert.IsNull(collection.Get("One"));
			Assert.AreSame(o2, collection.Get("Two"));
		}

		[TestMethod]
		public void IndexFiltersReturnedCollectionWithPredicate()
		{
			Predicate<object> filter = delegate(object obj)
			{
				return (obj is MockDataObject);
			};

			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection(SearchMode.Local, null, filter);

			object o1 = collection.AddNew<object>("One");
			MockDataObject o2 = collection.AddNew<MockDataObject>("Two");

			Assert.IsNull(collection["One"]);
			Assert.AreSame(o2, collection["Two"]);
		}

		[TestMethod]
		public void FilteredObjectWillNotBeReplacedByCreateNewIndexerBehavior()
		{
			Predicate<object> filter = delegate(object obj)
			{
				return (obj is MockDataObject);
			};

			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection(SearchMode.Local, delegate { return new object(); }, filter);

			object o1 = collection.AddNew<object>("One");
			Assert.IsNull(collection["One"]);
		}

		[TestMethod]
		public void EnumerationFiltersReturnedCollectionWithPredicate()
		{
			Predicate<object> filter = delegate(object obj)
			{
				return (obj is MockDataObject);
			};

			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection(SearchMode.Local, null, filter);

			object o1 = collection.AddNew<object>("One");
			MockDataObject o2 = collection.AddNew<MockDataObject>("Two");

			bool o1Found = false;
			bool o2Found = false;

			foreach (KeyValuePair<string, object> pair in collection)
			{
				if (pair.Value.Equals(o1))
					o1Found = true;
				if (pair.Value.Equals(o2))
					o2Found = true;
			}

			Assert.IsFalse(o1Found);
			Assert.IsTrue(o2Found);
		}

		#endregion

		#region Located For DI Resolution

		[TestMethod]
		public void AddedItemCanBeLocatedByTypeIDPair()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();

			object obj = collection.AddNew<object>("Foo");

			Assert.AreSame(obj, collection.Locator.Get(new DependencyResolutionLocatorKey(typeof(object), "Foo")));
		}

		[TestMethod]
		public void RemovingItemRemovesTypeIdPairFromLocator()
		{
			TestableManagedObjectCollection<object> collection = CreateManagedObjectCollection();
			object obj = collection.AddNew<object>("Foo");

			collection.Remove(obj);

			Assert.IsNull(collection.Locator.Get(new DependencyResolutionLocatorKey(typeof(object), "Foo")));
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

		private TestableManagedObjectCollection<object> CreateManagedObjectCollection()
		{
			return CreateManagedObjectCollection(SearchMode.Local);
		}

		private TestableManagedObjectCollection<object> CreateManagedObjectCollection(SearchMode searchMode)
		{
			return CreateManagedObjectCollection(searchMode, null);
		}

		private TestableManagedObjectCollection<object> CreateManagedObjectCollection(SearchMode searchMode, ManagedObjectCollection<object>.IndexerCreationDelegate indexerCreationDelegate)
		{
			return CreateManagedObjectCollection(searchMode, indexerCreationDelegate, delegate(object obj) { return true; });
		}

		private TestableManagedObjectCollection<object> CreateManagedObjectCollection(SearchMode searchMode, ManagedObjectCollection<object>.IndexerCreationDelegate indexerCreationDelegate, Predicate<object> filter)
		{
			LifetimeContainer container = new LifetimeContainer();
			Locator locator = new Locator();
			locator.Add(typeof(ILifetimeContainer), container);

			return new TestableManagedObjectCollection<object>(container, locator, CreateBuilder(), searchMode, indexerCreationDelegate, filter, null);
		}

		private Builder CreateBuilder()
		{
			Builder result = new Builder();
			result.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			return result;
		}

		private class TestableManagedObjectCollection<TItem> : ManagedObjectCollection<TItem>
		{
			private IBuilder<BuilderStage> builder;
			private ILifetimeContainer container;
			private IReadWriteLocator locator;
			private SearchMode searchMode;
			private IndexerCreationDelegate indexerCreationDelegate;
			private Predicate<TItem> predicate;

			public TestableManagedObjectCollection(ILifetimeContainer container, IReadWriteLocator locator,
				IBuilder<BuilderStage> builder, SearchMode searchMode, IndexerCreationDelegate indexerCreationDelegate,
				Predicate<TItem> predicate, TestableManagedObjectCollection<TItem> parentCollection)
				: base(container, locator, builder, searchMode, indexerCreationDelegate, predicate, parentCollection)
			{
				this.builder = builder;
				this.container = container;
				this.locator = locator;
				this.searchMode = searchMode;
				this.indexerCreationDelegate = indexerCreationDelegate;
				this.predicate = predicate;
			}

			public TestableManagedObjectCollection(TestableManagedObjectCollection<TItem> parent)
				: this(parent.container, new Locator(parent.locator), parent.builder, parent.searchMode,
				parent.indexerCreationDelegate, parent.predicate, parent)
			{
				locator.Add(typeof(ILifetimeContainer), parent.locator.Get<ILifetimeContainer>());
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

			public SearchMode SearchMode
			{
				get { return searchMode; }
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

		class MockDataObject
		{
			private int intProperty;

			public int IntProperty
			{
				get { return intProperty; }
				set { intProperty = value; }
			}
		}

		class MockWorkItem : WorkItem
		{
		}

		#endregion
	}
}
