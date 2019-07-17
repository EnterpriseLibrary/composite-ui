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
using Microsoft.Practices.CompositeUI.UIElements;
using System.Collections;

namespace Microsoft.Practices.CompositeUI.Tests
{
	[TestClass]
	public class UIExtensionSiteCollectionFixture
	{
		#region Contains

		[TestMethod]
		public void CanFindOutIfSiteIsContainedInCollection()
		{
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection();

			collection.RegisterSite("Foo", new MockAdapter());

			Assert.IsTrue(collection.Contains("Foo"));
			Assert.IsFalse(collection.Contains("Foo2"));
		}

		[TestMethod]
		public void ContainsChecksParent()
		{
			UIExtensionSiteCollection parent = new UIExtensionSiteCollection();
			UIExtensionSiteCollection child = new UIExtensionSiteCollection(parent);

			parent.RegisterSite("Foo", new MockAdapter());

			Assert.IsTrue(child.Contains("Foo"));
		}

		#endregion

		#region Count

		[TestMethod]
		public void CanCountSitesInCollection()
		{
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection();

			collection.RegisterSite("Foo", new MockAdapter());

			Assert.AreEqual(1, collection.Count);
		}

		[TestMethod]
		public void CountIncludesParentSites()
		{
			UIExtensionSiteCollection parent = new UIExtensionSiteCollection();
			UIExtensionSiteCollection child = new UIExtensionSiteCollection(parent);

			parent.RegisterSite("Foo", new MockAdapter());
			child.RegisterSite("Bar", new MockAdapter());

			Assert.AreEqual(1, parent.Count);
			Assert.AreEqual(2, child.Count);
		}

		[TestMethod]
		public void CountDoesNotIncludeDuplicates()
		{
			UIExtensionSiteCollection parent = new UIExtensionSiteCollection();
			UIExtensionSiteCollection child = new UIExtensionSiteCollection(parent);

			parent.RegisterSite("Foo", new MockAdapter());
			UIExtensionSite childSite = child["Foo"];

			Assert.AreEqual(1, child.Count);
		}

		#endregion

		#region Indexer

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void InvalidSiteNameThrows()
		{
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection();

			UIExtensionSite unused = collection["Foo"];
		}

		[TestMethod]
		public void CanRetrieveSiteViaIndexer()
		{
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection();

			collection.RegisterSite("Foo", new MockAdapter());

			Assert.IsNotNull(collection["Foo"]);
		}

		[TestMethod]
		public void CanRetrieveParentSiteViaIndexer()
		{
			UIExtensionSiteCollection parent = new UIExtensionSiteCollection();
			UIExtensionSiteCollection child = new UIExtensionSiteCollection(parent);

			parent.RegisterSite("Foo", new MockAdapter());
			child.RegisterSite("Bar", new MockAdapter());

			Assert.IsNotNull(child["Foo"]);
			Assert.IsNotNull(child["Bar"]);
		}

		[TestMethod]
		public void ItemsAddedToLocalSiteDoNotAffectItemsInParentSite()
		{
			UIExtensionSiteCollection parent = new UIExtensionSiteCollection();
			UIExtensionSiteCollection child = new UIExtensionSiteCollection(parent);
			parent.RegisterSite("Foo", new MockAdapter());

			UIExtensionSite parentSite = parent["Foo"];
			UIExtensionSite childSite = child["Foo"];
			parentSite.Add(new object());
			childSite.Add(new object());

			Assert.AreEqual(1, parentSite.Count);
			Assert.AreEqual(1, childSite.Count);
		}

		#endregion

		#region RegisterSite (with object)

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullObjectThrows()
		{
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection();

			collection.RegisterSite("Foo", (object)null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullSiteNameWithObjectThrows()
		{
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection();

			collection.RegisterSite(null, new object());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptySiteNameWithObjectThrows()
		{
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection();

			collection.RegisterSite("", new object());
		}

		[TestMethod]
		public void CanRegisterSiteFromObjectAndFactoryCatalogIsUsed()
		{
			TestableRootWorkItem wi = new TestableRootWorkItem();
			wi.Services.Remove<IUIElementAdapterFactoryCatalog>();
			MockFactoryCatalog factoryCatalog = wi.Services.AddNew<MockFactoryCatalog, IUIElementAdapterFactoryCatalog>();
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection(wi);
			object obj = new object();

			collection.RegisterSite("Foo", new object());
			collection["Foo"].Add(obj);

			Assert.AreSame(obj, factoryCatalog.Adapter.AddedElement);
		}

		#endregion

		#region RegisterSite (with adapter)

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullAdapterThrows()
		{
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection();

			collection.RegisterSite("Foo", (IUIElementAdapter)null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullSiteNameWithAdapterThrows()
		{
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection();

			collection.RegisterSite(null, new MockAdapter());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptySiteNameWithAdapterThrows()
		{
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection();

			collection.RegisterSite("", new MockAdapter());
		}

		[TestMethod]
		public void CanRegisterAdapterForSiteAndItIsUsedWithinTheSite()
		{
			MockAdapter adapter = new MockAdapter();
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection();
			object obj = new object();

			collection.RegisterSite("Foo", adapter);
			collection["Foo"].Add(obj);

			Assert.AreSame(obj, adapter.AddedElement);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void RegisterSiteAdapterTwiceThrows()
		{
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection();

			collection.RegisterSite("Foo", new MockAdapter());
			collection.RegisterSite("Foo", new MockAdapter());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void TryingToOverrideParentSiteThrows()
		{
			UIExtensionSiteCollection parent = new UIExtensionSiteCollection();
			UIExtensionSiteCollection child = new UIExtensionSiteCollection(parent);

			parent.RegisterSite("Foo", new MockAdapter());
			child.RegisterSite("Foo", new MockAdapter());
		}

		#endregion

		#region UnregisterSite

		[TestMethod]
		public void CanUnregisterSiteCreatedWithObject()
		{
			TestableRootWorkItem wi = new TestableRootWorkItem();
			wi.Services.Remove<IUIElementAdapterFactoryCatalog>();
			wi.Services.AddNew<MockFactoryCatalog, IUIElementAdapterFactoryCatalog>();
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection(wi);

			collection.RegisterSite("foo", new object());
			collection.UnregisterSite("foo");

			Assert.IsFalse(collection.Contains("foo"));
		}

		[TestMethod]
		public void CanUnregisterSiteCreatedWithAdapter()
		{
			UIExtensionSiteCollection collection = new UIExtensionSiteCollection();

			collection.RegisterSite("foo", new MockAdapter());
			collection.UnregisterSite("foo");

			Assert.IsFalse(collection.Contains("foo"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void UnregisterSiteCreatedInParentThrows()
		{
			UIExtensionSiteCollection parent = new UIExtensionSiteCollection();
			UIExtensionSiteCollection child = new UIExtensionSiteCollection(parent);

			parent.RegisterSite("Foo", new MockAdapter());
			child.UnregisterSite("Foo");
		}

		#endregion

		[TestMethod]
		public void DisposingWorkItemClearsUIExtensionSites()
		{
			WorkItem parent = new TestableRootWorkItem();
			MockUIAdapter uiAdapter = new MockUIAdapter();

			parent.UIExtensionSites.RegisterSite("Foo", uiAdapter);
			parent.UIExtensionSites["Foo"].Add(new object());
			parent.Dispose();

			Assert.AreEqual(0, uiAdapter.Items.Count);
		}

		public class MockUIAdapter : Microsoft.Practices.CompositeUI.UIElements.IUIElementAdapter
		{
			public List<object> Items = new List<object>();

			public object Add(object uiElement)
			{
				Items.Add(uiElement);
				return uiElement;
			}

			public void Remove(object uiElement)
			{
				Items.Remove(uiElement);
			}
		}

		#region Helpers

		class MockAdapter : IUIElementAdapter
		{
			public object AddedElement = null;
			public object RemovedElement = null;

			public object Add(object uiElement)
			{
				AddedElement = uiElement;
				return uiElement;
			}

			public void Remove(object uiElement)
			{
				RemovedElement = uiElement;
			}
		}

		class MockFactory : IUIElementAdapterFactory
		{
			MockAdapter adapter;

			public MockFactory(MockAdapter adapter)
			{
				this.adapter = adapter;
			}

			public IUIElementAdapter GetAdapter(object uiElement)
			{
				return adapter;
			}

			public bool Supports(object uiElement)
			{
				return true;
			}
		}

		class MockFactoryCatalog : IUIElementAdapterFactoryCatalog
		{
			public MockAdapter Adapter = new MockAdapter();
			List<IUIElementAdapterFactory> factories = new List<IUIElementAdapterFactory>();

			public MockFactoryCatalog()
			{
				factories.Add(new MockFactory(Adapter));
			}

			public IList<IUIElementAdapterFactory> Factories
			{
				get { return factories.AsReadOnly(); }
			}

			public IUIElementAdapterFactory GetFactory(object element)
			{
				return factories[0];
			}

			public void RegisterFactory(IUIElementAdapterFactory factory)
			{
				throw new NotImplementedException();
			}
		}

		#endregion
	}
}
