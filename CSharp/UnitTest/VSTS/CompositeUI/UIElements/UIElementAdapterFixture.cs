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

using Microsoft.Practices.CompositeUI.UIElements;
using System.Collections.Generic;

namespace Microsoft.Practices.CompositeUI.Tests.UIElements
{
	[TestClass]
	public class UIElementAdapterFixture
	{
		private static MockUIElementAdapter adapter;

		[TestInitialize]
		public void Setup()
		{
			adapter = new MockUIElementAdapter();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddingWithNullElementThorws()
		{
			adapter.Add(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemovingWithNullElementThrows()
		{
			adapter.Remove(null);
		}

		[TestMethod]
		public void AddCallsTypedAdd()
		{
			adapter.Add("Test");

			Assert.IsTrue(adapter.AddCalled);
		}

		[TestMethod]
		public void RemoveCallsTypedRemove()
		{
			adapter.Remove("Test");

			Assert.IsTrue(adapter.RemoveCalled);
		}

		[TestMethod]
		public void AddReturnTypedElement()
		{
			string item = "Test";
			object returnItem = adapter.Add(item);

			Assert.AreEqual(item, returnItem);
			Assert.AreEqual(typeof(string), item.GetType());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddThrowsIfElementNotAssignable()
		{
			adapter.Add(25);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void RemoveThrowsIfElementNotAssignable()
		{
			adapter.Remove(25);
		}
		
		[TestMethod]
		public void AddingItemShowsInList()
		{
			string item = "Test";
			object returnItem = adapter.Add(item);

			Assert.AreEqual(1, adapter.Strings.Count);
			Assert.AreSame(item, returnItem);
		}
		
		[TestMethod]
		public void RemovingItemRemovesFromList()
		{
			string item = "Test";
			adapter.Add(item);

			adapter.Remove(item);

			Assert.AreEqual(0, adapter.Strings.Count);
		}

		class MockUIElementAdapter : UIElementAdapter<string>
		{
			public bool AddCalled = false;
			public bool RemoveCalled = false;
			public List<string> Strings = new List<string>();
			
			protected override string Add(string uiElement)
			{
				AddCalled = true;
				Strings.Add(uiElement);

				return uiElement;
			}

			protected override void Remove(string uiElement)
			{
				RemoveCalled = true;
				Strings.Remove(uiElement);
			}
			
		}

	}
}
