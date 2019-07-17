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
using System.Threading;

namespace Microsoft.Practices.CompositeUI.Tests
{
	[TestClass]
	public class StateElementFixture
	{
		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ThrowsIfIndexerKeyNull()
		{
			MockStateElement element = new MockStateElement();

			element[null] = new object();
		}

		[TestMethod]
		public void CanAssignNullValue()
		{
			MockStateElement element = new MockStateElement();

			bool contains = false;
			element["foo"] = null;
			foreach (string key in element.Keys)
			{
				if (key == "foo") contains = true;
			}

			Assert.IsTrue(contains);
			Assert.IsNull(element["foo"]);
		}

		[TestMethod]
		public void AssignValueRaisesStateChanged()
		{
			MockStateElement element = new MockStateElement();

			bool raised = false;
			element.StateChanged += delegate { raised = true; };

			element["foo"] = new object();

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void AssignNullValueRaisesStateChanged()
		{
			MockStateElement element = new MockStateElement();

			bool raised = false;
			element.StateChanged += delegate { raised = true; };

			element["foo"] = null;

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void StateChangedRisedOnParentIfChildElementChanges()
		{
			MockStateElement element = new MockStateElement();

			bool raised = false;
			MockStateElement parent = new MockStateElement();
			parent["bar"] = element;
			parent.StateChanged += delegate { raised = true; };

			element["foo"] = new object();

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void StateChangedNotRisedAfterChildElementIsRemoved()
		{
			MockStateElement element = new MockStateElement();

			bool raised = false;
			MockStateElement parent = new MockStateElement();
			parent["bar"] = element;
			parent["bar"] = null;
			parent.StateChanged += delegate { raised = true; };

			element["foo"] = new object();

			Assert.IsFalse(raised);
		}

		[TestMethod]
		public void MultiLevelStateChangedRisedOnParentIfChildElementChanges()
		{
			MockStateElement element = new MockStateElement();

			bool raised = false;
			MockStateElement parent = new MockStateElement();
			MockStateElement grandparent = new MockStateElement();
			grandparent["foo"] = parent;
			parent["bar"] = element;
			grandparent.StateChanged += delegate { raised = true; };

			element["foo"] = new object();

			Assert.IsTrue(raised);
		}

		[TestMethod]
		public void MultiLevelStateChangedNotRisedAfterChildElementIsRemoved()
		{
			MockStateElement element = new MockStateElement();

			bool raised = false;
			MockStateElement parent = new MockStateElement();
			MockStateElement grandparent = new MockStateElement();
			grandparent["foo"] = parent;
			parent["bar"] = element;
			grandparent["foo"] = null;
			grandparent.StateChanged += delegate { raised = true; };

			element["foo"] = new object();

			Assert.IsFalse(raised);
		}

		[TestMethod]
		public void CanRemoveValue()
		{
			MockStateElement element = new MockStateElement();

			element["foo"] = new object();

			element.Remove("foo");

			Assert.IsNull(element["foo"]);
		}

		[TestMethod]
		public void ChangedEventAttachedOnlyOnceIfMultipleAdds()
		{
			MockStateElement element = new MockStateElement();
			MockChangedElement obj = new MockChangedElement();
			obj.Value = "old value";

			element["Test"] = obj;

			MockChangedElement temp = (MockChangedElement)element["Test"];
			temp.Value = "new value";

			element["Test"] = temp;

			int counter = 0;
			element.StateChanged += delegate { counter++; };

			temp = (MockChangedElement)element["Test"];
			temp.Value = "final value";

			Assert.AreEqual(1, counter);
		}

		[TestMethod]
		public void CustomChangeNotificationElementRaisesStateChangedEvent()
		{
			MockStateElement element = new MockStateElement();

			bool changed = false;
			MockChangedElement item = new MockChangedElement();
			element["foo"] = item;
			element.StateChanged += delegate { changed = true; };

			item.Value = "Hello";

			Assert.IsTrue(changed);
		}

		#region Helper classes

		class MockChangedElement : IChangeNotification
		{
			#region IChangeNotification Members

			public event EventHandler Changed;

			#endregion

			private string value;

			public string Value
			{
				get { return this.value; }
				set { this.value = value; if (Changed != null) Changed(this, EventArgs.Empty); }
			}

		}

		class MockStateElement : StateElement
		{
			public new object this[string key]
			{
				get { return base[key]; }
				set { base[key] = value; }
			}
		}

		#endregion
	}
}
