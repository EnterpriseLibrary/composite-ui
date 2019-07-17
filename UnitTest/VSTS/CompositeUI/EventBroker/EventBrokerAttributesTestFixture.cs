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
using System.Reflection;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace Microsoft.Practices.CompositeUI.Tests.EventBroker
{
	[TestClass]
	public class EventBrokerAttributesTestFixture
	{
		[TestMethod]
		public void EventSourceAttributeIsAvailable()
		{
			EventPublicationAttribute attr = new EventPublicationAttribute("MyEvent");
			Assert.IsNotNull(attr);
			Assert.AreEqual(PublicationScope.Global, attr.Scope);
		}

		[TestMethod]
		public void EventNameIsStored()
		{
			EventPublicationAttribute attr = new EventPublicationAttribute("MyEvent");
			Assert.AreEqual("MyEvent", attr.Topic);
		}

		[TestMethod]
		public void EventNameAndWorkItemAreStored()
		{
			EventPublicationAttribute attr = new EventPublicationAttribute("MyEvent", PublicationScope.WorkItem);
			Assert.AreEqual("MyEvent", attr.Topic);
			Assert.AreEqual(PublicationScope.WorkItem, attr.Scope);
		}

		[TestMethod]
		public void AttributesDiscover()
		{
			int attributeCount = 0;
			Type type = typeof (ClassUsingAttributes);

			foreach (EventInfo info in type.GetEvents())
			{
				EventPublicationAttribute[] attrs =
					(EventPublicationAttribute[]) info.GetCustomAttributes(typeof (EventPublicationAttribute), true);
				foreach (EventPublicationAttribute attr in attrs)
				{
					switch (attr.Topic)
					{
						case "MyGlobalEvent":
							Assert.AreEqual(PublicationScope.Global, attr.Scope);
							Assert.AreEqual("GlobalEvent", info.Name);
							break;
						case "MyLocalEvent":
							Assert.AreEqual(PublicationScope.WorkItem, attr.Scope);
							Assert.AreEqual("LocalEvent", info.Name);
							break;
						case "MyOneEvent":
						case "MyTwoEvent":
							Assert.AreEqual(PublicationScope.Global, attr.Scope);
							Assert.AreEqual("SeveralEvents", info.Name);
							break;
						default:
							Assert.Fail("Invalid event attribute encountered");
							break;
					}
					attributeCount++;
				}
			}
			Assert.AreEqual(4, attributeCount);
		}
	}

	[TestClass]
	public class EventHandleAttributeTestFixture
	{
		[TestMethod]
		public void EventhandlerAttributeIsAvailable()
		{
			EventSubscriptionAttribute attr = new EventSubscriptionAttribute("MyEvent");
			Assert.IsNotNull(attr);
		}

		[TestMethod]
		public void EventNameIsStored()
		{
			EventSubscriptionAttribute attr = new EventSubscriptionAttribute("MyEvent");
			Assert.AreEqual("MyEvent", attr.Topic);
		}

	}

	#region Test support classes

	public class ClassUsingAttributes
	{
		[EventPublication("MyGlobalEvent")]
		public event EventHandler GlobalEvent;

		[EventPublication("MyLocalEvent", PublicationScope.WorkItem)]
		public event EventHandler LocalEvent;

		[EventPublication("MyOneEvent")]
		[EventPublication("MyTwoEvent")]
		public event EventHandler SeveralEvents;

		[EventSubscriptionAttribute("MyGlobalEvent")]
		public void OnGlobalEvent(object sender, EventArgs args)
		{
		}

		[EventSubscriptionAttribute("MyLocalEvent")]
		public void OnLocalEvent(object sender, EventArgs args)
		{
		}

		[EventSubscriptionAttribute("MyOneEvent")]
		[EventSubscriptionAttribute("MyTwoEvent")]
		public void OnSeveralEvents(object sender, EventArgs args)
		{
		}

		private void ResolveCompilerWarnings()
		{
			GlobalEvent(this, EventArgs.Empty);
			LocalEvent(this, EventArgs.Empty);
			SeveralEvents(this, EventArgs.Empty);
		}
	}

	#endregion
}
