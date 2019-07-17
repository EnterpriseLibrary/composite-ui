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

namespace Microsoft.Practices.CompositeUI.Tests.Collections
{
	[TestClass]
	public class UIExtensionSiteFixture
	{
		[TestMethod]
		public void AddingItemToSiteAddsToAdapter()
		{
			MockAdapter adapter = new MockAdapter();
			UIExtensionSite site = new UIExtensionSite(adapter);
			object obj = new object();

			object result = site.Add(obj);

			Assert.AreSame(obj, result);
			Assert.AreSame(obj, adapter.AddedElement);
		}

		[TestMethod]
		public void AddingItemToSiteShowsItemInSiteCollection()
		{
			MockAdapter adapter = new MockAdapter();
			UIExtensionSite site = new UIExtensionSite(adapter);
			object obj = new object();
			
			site.Add(obj);

			Assert.AreEqual(1, site.Count);
			Assert.IsTrue(site.Contains(obj));
		}

		[TestMethod]
		public void CanEnumerateItemsInSite()
		{
			MockAdapter adapter = new MockAdapter();
			UIExtensionSite site = new UIExtensionSite(adapter);
			object obj1 = new object();
			object obj2 = new object();
			site.Add(obj1);
			site.Add(obj2);

			bool foundObj1 = false;
			bool foundObj2 = false;

			foreach (object obj in site)
			{
				if (object.ReferenceEquals(obj, obj1))
					foundObj1 = true;
				else if (object.ReferenceEquals(obj, obj2))
					foundObj2 = true;
			}

			Assert.IsTrue(foundObj1);
			Assert.IsTrue(foundObj2);
		}

		[TestMethod]
		public void RemovingItemFromSiteRemovesItFromAdapter()
		{
			MockAdapter adapter = new MockAdapter();
			UIExtensionSite site = new UIExtensionSite(adapter);
			object obj = new object();

			site.Add(obj);
			site.Remove(obj);

			Assert.AreSame(obj, adapter.RemovedElement);
			Assert.AreEqual(0, site.Count);
		}

		[TestMethod]
		public void RemovingItemNotInSiteDoesNotRemoveFromAdapter()
		{
			MockAdapter adapter = new MockAdapter();
			UIExtensionSite site = new UIExtensionSite(adapter);
			object obj = new object();

			site.Remove(obj);

			Assert.IsNull(adapter.RemovedElement);
		}

		[TestMethod]
		public void ClearingItemsRemovesFromAdapter()
		{
			MockAdapter adapter = new MockAdapter();
			UIExtensionSite site = new UIExtensionSite(adapter);
			object obj = new object();

			site.Add(obj);
			site.Clear();

			Assert.AreSame(obj, adapter.RemovedElement);
			Assert.AreEqual(0, site.Count);
		}

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
	}
}
