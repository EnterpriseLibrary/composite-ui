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
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.Tests.Mocks;
using Microsoft.Practices.CompositeUI.BuilderStrategies;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeUI.Tests.BuilderStrategies
{
	[TestClass]
	public class EventBrokerStrategyFixture
	{
		[TestMethod]
		public void EventStrategyAcceptsEventBrokerServices()
		{
			WorkItem workItem = new TestableRootWorkItem();
			EventBrokerStrategy strat = new EventBrokerStrategy();
			MockBuilderContext context = new MockBuilderContext(strat);
			MockEventObject thing = new MockEventObject();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(WorkItem), null), workItem);

			strat.BuildUp(context, typeof(MockEventObject), thing, null);

			Assert.IsTrue(workItem.EventTopics.Contains("topic1"));
			Assert.IsTrue(workItem.EventTopics.Get("topic1").ContainsPublication(thing, "SomeEvent"));

			Assert.IsTrue(workItem.EventTopics.Contains("globalTopic"));
			Assert.IsTrue(workItem.EventTopics.Get("globalTopic").ContainsPublication(thing, "SomeEvent2"));

			Assert.IsTrue(workItem.EventTopics.Contains("localSubscriptionTopic"));
			Assert.IsTrue(workItem.EventTopics.Get("localSubscriptionTopic").ContainsSubscription(thing, "SomeHandler"));

			Assert.IsTrue(workItem.EventTopics.Contains("globalSubscriptionTopic"));
			Assert.IsTrue(workItem.EventTopics.Get("globalSubscriptionTopic").ContainsSubscription(thing, "SomeHandle2"));
		}

		[TestMethod]
		public void RemovingItemRemovesSubscribersAndPublishers()
		{
			WorkItem workItem = new TestableRootWorkItem();
			EventBrokerStrategy strat = new EventBrokerStrategy();
			MockBuilderContext context = new MockBuilderContext(strat);
			MockEventObject thing = new MockEventObject();
			MockEventSubscriber sub = new MockEventSubscriber();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(WorkItem), null), workItem);

			strat.BuildUp(context, typeof(MockEventObject), thing, null);
			strat.BuildUp(context, typeof(MockEventSubscriber), sub, null);

			Assert.IsFalse(thing.SomeEventIsNull());
			Assert.IsFalse(thing.SomeEvent2IsNull());

			strat.TearDown(context, thing);

			Assert.IsTrue(thing.SomeEventIsNull());
			Assert.IsTrue(thing.SomeEvent2IsNull());
		}

		[TestMethod]
		public void AddedServicesGetInspectedAndRegistered()
		{
			WorkItem workItem = new TestableRootWorkItem();
			EventBrokerStrategy strat = new EventBrokerStrategy();
			MockBuilderContext context = new MockBuilderContext(strat);
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(WorkItem), null), workItem);

			EventTopic handle = workItem.EventTopics["GlobalEvent"];

			MockService mock = new MockService();
			context.HeadOfChain.BuildUp(context, typeof(MockService), mock, null);

			Assert.AreEqual(1, handle.SubscriptionCount, "The subscription was not registered.");
			Assert.AreEqual(1, handle.PublicationCount, "The publication was not registered.");
		}

		[TestMethod]
		public void ChildContainerGetsEventsRegistered()
		{
			EventTopic handle = new EventTopic();

			WorkItem workItem = CreateWorkItem(handle);
			workItem.WorkItems.AddNew<MockWorkItem>();

			Assert.AreEqual(1, handle.SubscriptionCount, "The subscription was not registered.");
			Assert.AreEqual(1, handle.PublicationCount, "The publication was not registered.");
		}

		private WorkItem CreateWorkItem(EventTopic handle)
		{
			WorkItem result = new TestableRootWorkItem();
			result.EventTopics.Add(handle, "GlobalEvent");
			return result;
		}

		private class MockWorkItem : WorkItem
		{
			[EventPublication("GlobalEvent")]
			public event EventHandler Event;

			[EventSubscription("GlobalEvent")]
			public void OnEvent(object sender, EventArgs e)
			{
			}

			public void FireEvent()
			{
				if (Event != null)
				{
					Event(this, EventArgs.Empty);
				}
			}
		}

		public class MockService
		{
			[EventPublication("GlobalEvent")]
			public event EventHandler Event;

			[EventSubscription("GlobalEvent")]
			public void OnEvent(object sender, EventArgs e)
			{
			}

			public void FireEvent()
			{
				if (Event != null)
				{
					Event(this, EventArgs.Empty);
				}
			}
		}

		private class MockEventSubscriber
		{
			[EventSubscription("topic1")]
			public void topic1Handler(object sender, EventArgs e)
			{
			}

			[EventSubscription("globalTopic")]
			public void globalTopicHandler(object sender, EventArgs e)
			{
			}
		}

		private class MockEventObject
		{
			[EventPublication("topic1")]
			public event EventHandler SomeEvent;

			[EventPublication("globalTopic")]
			public event EventHandler SomeEvent2;

			[EventSubscription("localSubscriptionTopic")]
			public void SomeHandler(object sender, EventArgs eh)
			{
			}

			[EventSubscription("globalSubscriptionTopic", Thread = ThreadOption.Background)]
			public void SomeHandle2(object sender, EventArgs eh)
			{
			}

			public bool SomeEventIsNull()
			{
				return (SomeEvent == null);
			}

			public bool SomeEvent2IsNull()
			{
				return (SomeEvent2 == null);
			}

			private void CompilerWarningEradicator()
			{
				if (SomeEvent != null)
					SomeEvent(null, null);
				if (SomeEvent2 != null)
					SomeEvent2(null, null);
			}
		}
	}
}
