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
using System.Diagnostics;

namespace Microsoft.Practices.CompositeUI.Tests.EventBroker
{
	[TestClass]
	public class EventTopicFixture
	{
		private static TestEventTopic topic;
		private WorkItem workItem;

		[TestInitialize]
		public void Setup()
		{
			topic = new TestEventTopic();
			workItem = new TestableRootWorkItem();
		}

		[TestMethod]
		public void CanAddPublication()
		{
			TestPublisher publisher = new TestPublisher();
			Assert.IsFalse(topic.ContainsPublication(publisher, "TestEvent"));
			topic.AddPublication(publisher, "TestEvent", workItem, PublicationScope.WorkItem);

			Assert.IsTrue(topic.ContainsPublication(publisher, "TestEvent"));
		}


		[TestMethod]
		[ExpectedException(typeof(EventBrokerException))]
		public void ThrowIfEventNotFound()
		{
			topic.AddPublication(new TestPublisher(), "InexistentEvent", workItem, PublicationScope.WorkItem);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowIfEmptyEventName()
		{
			topic.AddPublication(new TestPublisher(), String.Empty, workItem, PublicationScope.WorkItem);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowIfNullEventName()
		{
			topic.AddPublication(new TestPublisher(), null, workItem, PublicationScope.WorkItem);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfNotPublisherSpecified()
		{
			topic.AddPublication(null, "TestEvent", workItem, PublicationScope.WorkItem);
		}


		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowIfNoWorkItemSpecified()
		{
			topic.AddPublication(new TestPublisher(), "TestEvent", null, PublicationScope.WorkItem);
		}

		[TestMethod]
		[ExpectedException(typeof(EventBrokerException))]
		public void ThrowIfAddingSamePublication()
		{
			TestPublisher publisher = new TestPublisher();
			topic.AddPublication(publisher, "TestEvent", workItem, PublicationScope.WorkItem);
			topic.AddPublication(publisher, "TestEvent", workItem, PublicationScope.WorkItem);
		}


		[TestMethod]
		public void AcceptSeveralPublicationFromDifferentEventsOnSamePublisher()
		{
			TestPublisher publisher = new TestPublisher();
			topic.AddPublication(publisher, "TestEvent", workItem, PublicationScope.WorkItem);
			topic.AddPublication(publisher, "AnotherTestEvent", workItem, PublicationScope.WorkItem);

			Assert.IsTrue(topic.ContainsPublication(publisher, "TestEvent"));
			Assert.IsTrue(topic.ContainsPublication(publisher, "AnotherTestEvent"));
		}

		[TestMethod]
		public void CanRemoveOnePublicationFromSamePublisher()
		{
			TestPublisher publisher = new TestPublisher();
			topic.AddPublication(publisher, "TestEvent", workItem, PublicationScope.WorkItem);
			topic.AddPublication(publisher, "AnotherTestEvent", workItem, PublicationScope.WorkItem);

			topic.RemovePublication(publisher, "TestEvent");

			Assert.IsTrue(topic.ContainsPublication(publisher, "AnotherTestEvent"));
			Assert.IsFalse(topic.ContainsPublication(publisher, "TestEvent"));
		}

		[TestMethod]
		public void AddedPublisheEventIsHookedUp()
		{
			TestPublisher publisher = new TestPublisher();
			topic.AddPublication(publisher, "TestEvent", workItem, PublicationScope.WorkItem);

			Assert.AreEqual(1, publisher.InvocationListLength);
		}

		[TestMethod]
		public void AddTwoPublishers()
		{
			TestPublisher publisher1 = new TestPublisher();
			TestPublisher publisher2 = new TestPublisher();
			topic.AddPublication(publisher1, "TestEvent", workItem, PublicationScope.WorkItem);
			topic.AddPublication(publisher2, "TestEvent", workItem, PublicationScope.WorkItem);

			Assert.AreEqual(1, publisher1.InvocationListLength);
			Assert.AreEqual(1, publisher2.InvocationListLength);
		}

		[TestMethod]
		public void RemoveOnePublisher()
		{
			TestPublisher publisher1 = new TestPublisher();
			TestPublisher publisher2 = new TestPublisher();
			topic.AddPublication(publisher1, "TestEvent", workItem, PublicationScope.WorkItem);
			topic.AddPublication(publisher2, "TestEvent", workItem, PublicationScope.WorkItem);
			topic.RemovePublication(publisher1, "TestEvent");

			Assert.AreEqual(0, publisher1.InvocationListLength);
			Assert.AreEqual(1, publisher2.InvocationListLength);
		}

		[TestMethod]
		public void FiringThePublisherFiresTheTopic()
		{
			TestPublisher publisher = new TestPublisher();
			topic.AddPublication(publisher, "TestEvent", workItem, PublicationScope.WorkItem);

			publisher.FireTestEvent();

			Assert.IsTrue(topic.FireCalled);
		}


		[TestMethod]
		public void TopicCanBeFiredManually()
		{
			topic.Fire(this, EventArgs.Empty, workItem, PublicationScope.WorkItem);

			Assert.IsTrue(topic.FireCalled);
		}



		[TestMethod]
		public void CanAddSubscription()
		{
			TestSubscriber subscriber = new TestSubscriber();
			topic.AddSubscription(subscriber, "TestEventHandler", workItem, ThreadOption.Publisher);

			Assert.IsTrue(topic.ContainsSubscription(subscriber, "TestEventHandler"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowIfAddingNullSubscriber()
		{
			topic.AddSubscription(null, "TestEventHandler", workItem, ThreadOption.Publisher);
		}


		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowIfNullMethodHandler()
		{
			topic.AddSubscription(new TestPublisher(), null, workItem, ThreadOption.Publisher);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowIfEmptyMethodHandler()
		{
			topic.AddSubscription(new TestPublisher(), String.Empty, workItem, ThreadOption.Publisher);
		}

		[TestMethod]
		[ExpectedException(typeof(EventBrokerException))]
		public void ThrowIfInvalidMethodHandlerName()
		{
			topic.AddSubscription(new TestPublisher(), "NonExistingHandler", workItem, ThreadOption.Publisher);
		}

		[TestMethod]
		public void CannAddTwoSubscribers()
		{
			TestSubscriber subscriber1 = new TestSubscriber();
			TestSubscriber subscriber2 = new TestSubscriber();
			topic.AddSubscription(subscriber1, "TestEventHandler", workItem, ThreadOption.Publisher);
			topic.AddSubscription(subscriber2, "AnotherTestEventHandler", workItem, ThreadOption.Publisher);

			Assert.IsTrue(topic.ContainsSubscription(subscriber1, "TestEventHandler"));
			Assert.IsTrue(topic.ContainsSubscription(subscriber2, "AnotherTestEventHandler"));
		}

		[TestMethod]
		public void CanRemoveOneSubscriber()
		{
			TestSubscriber subscriber1 = new TestSubscriber();
			TestSubscriber subscriber2 = new TestSubscriber();
			topic.AddSubscription(subscriber1, "TestEventHandler", workItem, ThreadOption.Publisher);
			topic.AddSubscription(subscriber2, "TestEventHandler", workItem, ThreadOption.Publisher);

			topic.RemoveSubscription(subscriber2, "TestEventHandler");

			Assert.IsTrue(topic.ContainsSubscription(subscriber1, "TestEventHandler"));
			Assert.IsFalse(topic.ContainsSubscription(subscriber2, "TestEventHandler"));
		}

		[TestMethod]
		public void FiringTopicCallsSubscriber()
		{
			TestSubscriber subscriber = new TestSubscriber();
			topic.AddSubscription(subscriber, "TestEventHandler", workItem, ThreadOption.Publisher);

			topic.Fire(this, EventArgs.Empty, workItem, PublicationScope.WorkItem);

			Assert.IsTrue(subscriber.TestEventHandlerCalled);
		}


		[TestMethod]
		public void TopicIsEnabledByDefault()
		{
			Assert.IsTrue(topic.Enabled);
		}

		[TestMethod]
		public void FiringDisableTopicNotCallSubscriber()
		{
			TestSubscriber subscriber = new TestSubscriber();
			topic.AddSubscription(subscriber, "TestEventHandler", workItem, ThreadOption.Publisher);

			topic.Enabled = false;
			topic.Fire(this, EventArgs.Empty, workItem, PublicationScope.WorkItem);

			Assert.IsFalse(subscriber.TestEventHandlerCalled);
		}

		[TestMethod]
		public void FiringAndEnabledTopicCallsHandler()
		{
			topic.Enabled = false;
			TestSubscriber subscriber = new TestSubscriber();
			topic.AddSubscription(subscriber, "TestEventHandler", workItem, ThreadOption.Publisher);

			topic.Enabled = true;
			topic.Fire(this, EventArgs.Empty, workItem, PublicationScope.WorkItem);

			Assert.IsTrue(subscriber.TestEventHandlerCalled);
		}

		[TestMethod]
		public void FinalizedSubscriberIsRemovedOnClean()
		{
			TestSubscriber subscriber = new TestSubscriber();
			topic.AddSubscription(subscriber, "TestEventHandler", workItem, ThreadOption.Publisher);
			Assert.AreEqual(1, topic.SubscriptionCount);

			subscriber = null;
			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.AreEqual(0, topic.SubscriptionCount);
		}


		[TestMethod]
		public void FinalizedPublisherIsRemovedOnClean()
		{
			TestPublisher publisher = new TestPublisher();
			topic.AddPublication(publisher, "TestEvent", workItem, PublicationScope.WorkItem);
			Assert.AreEqual(1, topic.PublicationCount);

			publisher = null;
			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.AreEqual(0, topic.PublicationCount);
		}

		[TestMethod]
		public void EventTopicExposesPublicationCount()
		{
			Assert.AreEqual(0, topic.PublicationCount);
			topic.AddPublication(new TestPublisher(), "TestEvent", workItem, PublicationScope.WorkItem);
			Assert.AreEqual(1, topic.PublicationCount);
		}


		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowIfFiringWithInvalidScope()
		{
			topic.Fire(this, EventArgs.Empty, workItem, (PublicationScope)10);
		}


		[TestMethod]
		public void SubscriberCanTakeSpecializedEventArgs()
		{
			TestSubscriber subscriber = new TestSubscriber();
			InheritedEventArgs args = new InheritedEventArgs();
			topic.AddSubscription(subscriber, "InheritedEventArgsHandler", workItem, ThreadOption.Publisher);

			topic.Fire(this, args, workItem, PublicationScope.WorkItem);

			Assert.IsTrue(subscriber.InheritedEventArgsHandlerCalled);
		}


		[TestMethod]
		public void EventTopicThrowsWithExceptionsOccurredInSubscribers()
		{
			TestSubscriber subscriber1 = new TestSubscriber();
			TestSubscriber subscriber2 = new TestSubscriber();

			topic.AddSubscription(subscriber1, "FailingHandler", workItem, ThreadOption.Publisher);
			topic.AddSubscription(subscriber2, "FailingHandler", workItem, ThreadOption.Publisher);

			try
			{
				topic.Fire(this, EventArgs.Empty, workItem, PublicationScope.WorkItem);
			}
			catch (EventTopicException ex)
			{
				Assert.AreEqual(2, ex.Exceptions.Count);
				Assert.AreEqual("FailingHandler", ex.Exceptions[0].Message);
				Assert.AreEqual("FailingHandler", ex.Exceptions[1].Message);
			}
		}


		[TestMethod]
		public void CanRegisterGenericEventHandlerSignatures()
		{
			TestPublisher publisher = new TestPublisher();
			topic.AddPublication(publisher, "GenericEvent", workItem, PublicationScope.WorkItem);
			Assert.IsTrue(topic.ContainsPublication(publisher, "GenericEvent"));
		}


		[TestMethod]
		public void SubscriberHandlesGenericEvent()
		{
			TestPublisher publisher = new TestPublisher();
			TestSubscriber subscriber = new TestSubscriber();
			topic.AddPublication(publisher, "GenericEvent", workItem, PublicationScope.WorkItem);
			topic.AddSubscription(subscriber, "InheritedEventArgsHandler", workItem, ThreadOption.Publisher);

			publisher.FireGenericEvent();
			Assert.IsTrue(subscriber.InheritedEventArgsHandlerCalled);
		}


		[TestMethod]
		public void NotFailWhenSubscriberHasBeenFinalized()
		{
			TestSubscriber subscriber = new TestSubscriber();
			topic.AddSubscription(subscriber, "TestEventHandler", workItem, ThreadOption.Publisher);
			subscriber = null;

			GC.Collect();
			GC.WaitForPendingFinalizers();

			topic.Fire(this, EventArgs.Empty, workItem, PublicationScope.WorkItem);
		}


		[TestMethod]
		public void PublishersAreReleasedWhenDisposed()
		{
			TestPublisher publisher = new TestPublisher();
			topic.AddPublication(publisher, "TestEvent", workItem, PublicationScope.WorkItem);

			topic.Dispose();
			Assert.AreEqual(0, publisher.InvocationListLength);
			Assert.AreEqual(0, topic.PublicationCount);
		}


		[TestMethod]
		public void SubscribersAreReleasedWhenDisposed()
		{
			TestSubscriber subscriber = new TestSubscriber();
			topic.AddSubscription(subscriber, "TestEventHandler", workItem, ThreadOption.Publisher);

			topic.Dispose();
			Assert.AreEqual(0, topic.SubscriptionCount);
		}


		[TestMethod]
		[ExpectedException(typeof(EventBrokerException))]
		public void CannotRegisterStaticPublisher()
		{
			StaticPublisher publisher = new StaticPublisher();
			topic.AddPublication(publisher, "StaticEvent", workItem, PublicationScope.WorkItem);
		}

		[TestMethod]
		[ExpectedException(typeof(EventBrokerException))]
		public void CannotRegisterStaticSubscriber()
		{
			StaticSubscriber subscriber = new StaticSubscriber();
			topic.AddSubscription(subscriber, "StaticEventHandler", workItem, ThreadOption.Publisher);
		}

		[TestMethod]
		public void DoesNotFailsWIthMultipleOverloads()
		{
			WorkItem wi = new TestableRootWorkItem();

			pubA pa = new pubA();
			subA sa = new subA();

			wi.Items.Add(pa);
			wi.Items.Add(sa);

			pa.Fire();
		}

		[TestMethod]
		[ExpectedException(typeof(EventBrokerException))]
		public void InvalidPublicationSignatureThrows()
		{
			WorkItem wi = new TestableRootWorkItem();
			EventTopic topic = wi.EventTopics.AddNew<EventTopic>("Foo");
			topic.AddPublication(this, "InvalidPublicationSignature", wi, PublicationScope.WorkItem);
		}

		public delegate void MyDelegate(int count);

		[TestMethod]
		[ExpectedException(typeof(EventBrokerException))]
		public void InvalidSubscriptionSignatureThrows()
		{
			WorkItem wi = new TestableRootWorkItem();
			EventTopic topic = wi.EventTopics.AddNew<EventTopic>("Foo");
			topic.AddSubscription(this, "InvalidSubscriptionSignature", wi, ThreadOption.Publisher);
		}

		public void InvalidSubscriptionSignature(int count) { }

		[TestMethod]
		public void AddingRepeatedSubscriptionNoops()
		{
			WorkItem wi = new TestableRootWorkItem();
			EventTopic topic = new EventTopic();
			TestSubscriber subscriber = new TestSubscriber();
			topic.AddSubscription(subscriber, "TestEventHandler", wi, ThreadOption.Publisher);
			topic.AddSubscription(subscriber, "TestEventHandler", wi, ThreadOption.Publisher);

			Assert.AreEqual(1, topic.SubscriptionCount);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemovePublisherNullPublisherThrows()
		{
			EventTopic topic = new EventTopic();
			topic.RemovePublication(null, "Test");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemovePublisherNullEventThrows()
		{
			EventTopic topic = new EventTopic();
			TestPublisher pub = new TestPublisher();
			topic.RemovePublication(pub, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemoveSubscriberNullSubscriberThrows()
		{
			EventTopic topic = new EventTopic();
			topic.RemoveSubscription(null, "Test");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemoveSubscriberNullEventThrows()
		{
			EventTopic topic = new EventTopic();
			TestSubscriber subscriber = new TestSubscriber();
			topic.RemoveSubscription(subscriber, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ContainsSubscriberNullSubscriberThrows()
		{
			EventTopic topic = new EventTopic();
			topic.ContainsSubscription(null, "Test");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ContainsSubscriberNullEventThrows()
		{
			EventTopic topic = new EventTopic();
			TestSubscriber subscriber = new TestSubscriber();
			topic.ContainsSubscription(subscriber, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ContainsPublisherNullSubscriberThrows()
		{
			EventTopic topic = new EventTopic();
			topic.ContainsPublication(null, "Test");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ContainsPublisherNullEventThrows()
		{
			EventTopic topic = new EventTopic();
			TestPublisher pub = new TestPublisher();
			topic.ContainsPublication(pub, null);
		}

		[TestMethod]
		[ExpectedException(typeof(EventBrokerException))]
		public void AddPublisherWithThreeParamDelegate()
		{
			TestableRootWorkItem wi = new TestableRootWorkItem();

			pubC a = new pubC();

			wi.Items.Add(a);
		}

		[TestMethod]
		public void TraceSourceIsInjected()
		{
			WorkItem container = new TestableRootWorkItem();
			ITraceSourceCatalogService ts = container.Services.Get<ITraceSourceCatalogService>();
			TestEventTopic topic = new TestEventTopic();
			workItem.EventTopics.Add(topic);

			Assert.IsFalse(String.IsNullOrEmpty(topic.TraceSourceName));
		}

		[TestMethod]
		[ExpectedException(typeof(EventBrokerException))]
		public void StaticPublisherThrows()
		{
			WorkItem workItem = new TestableRootWorkItem();
			EventTopic topic = new EventTopic();
			StaticPublisher pub = new StaticPublisher();

			topic.AddPublication(pub, "StaticEvent", workItem, PublicationScope.WorkItem);
		}


		[TestMethod]
		public void EventTopicCreatedThroughIndexerGetsItsName()
		{
			WorkItem workItem = new TestableRootWorkItem();
			EventTopic topic = workItem.EventTopics["Foo"];

			Assert.AreEqual("Foo", topic.Name);
		}


		[TestMethod]
		public void EventTopicCreatedThroughAddNewGetItsName()
		{
			WorkItem workItem = new TestableRootWorkItem();
			EventTopic topic = workItem.EventTopics.AddNew<EventTopic>("Foo");

			Assert.AreEqual("Foo", topic.Name);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowsWhenRemovingEventTopicFromEventTopicCollection()
		{
			WorkItem workItem = new TestableRootWorkItem();
			EventTopic topic = workItem.EventTopics.AddNew<EventTopic>("Foo");
			workItem.EventTopics.Remove(topic);
		}

		[TestMethod]
		public void AddedTopicIsCreatedWithGivenName()
		{
			WorkItem workItem = new TestableRootWorkItem();

			EventTopic topic = workItem.EventTopics.AddNew<EventTopic>("Foo1");
			Assert.AreEqual("Foo1", topic.Name);

			topic = workItem.EventTopics["Foo2"];
			Assert.AreEqual("Foo2", topic.Name);

		}

		[TestMethod]
		public void GenericSubscribersCanBeUsed()
		{
			WorkItem workItem = new TestableRootWorkItem();
			pubA pub = workItem.Items.AddNew<pubA>();
			TSubA<object> sub = workItem.Items.AddNew<TSubA<object>>();

			pub.Fire();

			Assert.IsTrue(sub.HandlerACalled);
		}


		#region Supporting Classes

		public delegate void Temp(object sender, EventArgs e, int i);
		public class pubC
		{
			[EventPublication("testA")]
			public event Temp A;

			public void Fire()
			{
				A(null, null, 0);
			}
		}

		public class pubA
		{
			[EventPublication("testA")]
			public event EventHandler A;

			public void Fire()
			{
				A(null, null);
			}
		}

		public class subA
		{
			[EventSubscription("testA")]
			public void HandlerA(object sender, EventArgs e)
			{
			}

			public void HandlerA()
			{
			}
		}

		public class TSubA<T>
		{
			public T dummy;

			public bool HandlerACalled;

			[EventSubscription("testA")]
			public void HandlerA(object sender, EventArgs e)
			{
				HandlerACalled = true;
			}

			public void HandlerA()
			{
			}
		}

		public class TSubC
		{
			public bool HandlerACalled;

			[EventSubscription("testA")]
			public void HandlerA(object sender, EventArgs e)
			{
				HandlerACalled = true;
			}
		}

		class TestEventTopic : EventTopic
		{
			public bool FireCalled = false;

			public override void Fire(object sender, EventArgs e, WorkItem workItem, PublicationScope scope)
			{
				FireCalled = true;
				base.Fire(sender, e, workItem, scope);
			}

			public string TraceSourceName
			{
				get { return base.TraceSource.Name; }
			}
		}

		class TestPublisher
		{
			public event EventHandler TestEvent;

			public event EventHandler AnotherTestEvent;

			public event EventHandler<InheritedEventArgs> GenericEvent;

			public void FireTestEvent()
			{
				if (TestEvent != null)
				{
					TestEvent(this, EventArgs.Empty);
				}
			}

			public void FireGenericEvent()
			{
				if (GenericEvent != null)
				{
					GenericEvent(this, new InheritedEventArgs());
				}
			}

			public int InvocationListLength
			{
				get
				{
					if (TestEvent != null)
					{
						return TestEvent.GetInvocationList().Length;
					}
					return 0;
				}
			}

			private void ResolveCompilerWarnings()
			{
				AnotherTestEvent(null, null);
			}
		}

		class InheritedEventArgs : EventArgs
		{
		}

		class TestSubscriber
		{
			public bool TestEventHandlerCalled = false;

			public void TestEventHandler(object sender, EventArgs e)
			{
				TestEventHandlerCalled = true;
			}

			public void AnotherTestEventHandler(object sender, EventArgs e)
			{
			}

			public bool InheritedEventArgsHandlerCalled = false;

			public void InheritedEventArgsHandler(object sender, InheritedEventArgs e)
			{
				InheritedEventArgsHandlerCalled = true;
			}

			public void FailingHandler(object sender, EventArgs e)
			{
				throw new Exception("FailingHandler");
			}
		}

		class StaticPublisher
		{
			public static event EventHandler StaticEvent;

			private static void CompilerWarnings()
			{
				StaticEvent(null, null);
			}
		}

		class StaticSubscriber
		{
			public static void StaticEventHandler(object sender, EventArgs e)
			{

			}
		}

		#endregion
	}
}
