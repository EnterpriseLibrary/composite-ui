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
using Microsoft.Practices.CompositeUI.EventBroker;

namespace Microsoft.Practices.CompositeUI.Tests.EventBroker
{
	[TestClass]
	public class EventTopicScopingFixture
	{
		private static WorkItem item1;
		private static WorkItem item2;
		private static WorkItem item3;
		private static WorkItem item4;
		
		private static EventTopic topic;
		private static TestPublisher publisher1;
		private static TestSubscriber subscriber1;
		private static TestSubscriber subscriber2;
		private static TestSubscriber subscriber3;
		private static TestSubscriber subscriber4;

		static EventTopicScopingFixture()
		{
			item1 = new TestableRootWorkItem();
			item2 = item1.WorkItems.AddNew<WorkItem>();
			item3 = item1.WorkItems.AddNew<WorkItem>();
			item4 = item2.WorkItems.AddNew<WorkItem>();
		}

		[TestInitialize]
		public void Setup()
		{
			topic = new EventTopic();

			publisher1 = new TestPublisher();
			subscriber1 = new TestSubscriber();
			subscriber2 = new TestSubscriber();
			subscriber3 = new TestSubscriber();
			subscriber4 = new TestSubscriber();

			topic.AddSubscription(subscriber1, "TestEventHandler", item1, ThreadOption.Publisher);
			topic.AddSubscription(subscriber2, "TestEventHandler", item2, ThreadOption.Publisher);
			topic.AddSubscription(subscriber3, "TestEventHandler", item3, ThreadOption.Publisher);
			topic.AddSubscription(subscriber4, "TestEventHandler", item4, ThreadOption.Publisher);
		}


		[TestMethod]
		public void LocalScopePublicationAreHandledLocally()
		{
			topic.AddPublication(publisher1, "TestEvent", item2, PublicationScope.WorkItem);
			publisher1.FireTestEvent();

			Assert.IsFalse(subscriber1.TestEventHandlerCalled);
			Assert.IsTrue(subscriber2.TestEventHandlerCalled);
			Assert.IsFalse(subscriber3.TestEventHandlerCalled);
			Assert.IsFalse(subscriber4.TestEventHandlerCalled);
		}


		[TestMethod]
		public void GlobalScopePublicationIsHandledInAllSubscribers()
		{
			topic.AddPublication(publisher1, "TestEvent", item2, PublicationScope.Global);
			publisher1.FireTestEvent();

			Assert.IsTrue(subscriber1.TestEventHandlerCalled);
			Assert.IsTrue(subscriber2.TestEventHandlerCalled);
			Assert.IsTrue(subscriber3.TestEventHandlerCalled);
			Assert.IsTrue(subscriber4.TestEventHandlerCalled);
		}


		[TestMethod]
		public void DescendantScopedFormRootIsHandledByAllSubscribers()
		{
			topic.AddPublication(publisher1, "TestEvent", item1, PublicationScope.Descendants);
			publisher1.FireTestEvent();

			Assert.IsTrue(subscriber1.TestEventHandlerCalled);
			Assert.IsTrue(subscriber2.TestEventHandlerCalled);
			Assert.IsTrue(subscriber3.TestEventHandlerCalled);
			Assert.IsTrue(subscriber4.TestEventHandlerCalled);
		}

		[TestMethod]
		public void DescendatScopedIsHandledOnWorkItemAndChildren()
		{
			topic.AddPublication(publisher1, "TestEvent", item2, PublicationScope.Descendants);
			publisher1.FireTestEvent();

			Assert.IsFalse(subscriber1.TestEventHandlerCalled);
			Assert.IsTrue(subscriber2.TestEventHandlerCalled);
			Assert.IsFalse(subscriber3.TestEventHandlerCalled);
			Assert.IsTrue(subscriber4.TestEventHandlerCalled);
		}

		[TestMethod]
		public void AddedMiddleSubscriberGetCalled()
		{
			TestSubscriber added = new TestSubscriber();
			topic.AddPublication(publisher1, "TestEvent", item1, PublicationScope.Descendants);
			topic.AddSubscription(added, "TestEventHandler", item3, ThreadOption.Publisher);
			publisher1.FireTestEvent();

			Assert.IsTrue(added.TestEventHandlerCalled);
		}

		class TestPublisher
		{
			public event EventHandler TestEvent;

			public void FireTestEvent()
			{
				if (TestEvent != null)
				{
					TestEvent(this, EventArgs.Empty);
				}

			}
		}

		class TestSubscriber
		{
			public bool TestEventHandlerCalled = false;

			public void TestEventHandler(object sender, EventArgs e)
			{
				TestEventHandlerCalled = true;
			}
		}

	}
}
