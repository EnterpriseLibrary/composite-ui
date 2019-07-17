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
using System.Threading;
using Microsoft.Practices.CompositeUI.EventBroker;
using System.Windows.Forms;
using System.Reflection;

namespace Microsoft.Practices.CompositeUI.Tests.EventBroker
{
	[TestClass]
	public class EventTopicSubscriptionModesFixture
	{
		private static WorkItem item;
		private static EventTopic topic;

		[TestInitialize]
		public void Setup()
		{
			item = new TestableRootWorkItem();
			topic = new EventTopic();
		}

		
		[TestMethod]
		public void RunInPublisherThreadCallsRunInCallerThread()
		{
			TestSubscriber subscriber = new TestSubscriber();
			topic.AddSubscription(subscriber, "TestEventHandler", item, ThreadOption.Publisher);

			topic.Fire(this, EventArgs.Empty, item, PublicationScope.WorkItem);

			Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, subscriber.ManagedThreadId);
		}

		[TestMethod]
		public void RunInPublisherThreadCallsRunInPublisherThread()
		{
			TestSubscriber subscriber = new TestSubscriber();
			TestPublisher publisher = new TestPublisher();
			topic.AddPublication(publisher, "TestEvent", item, PublicationScope.WorkItem);
			topic.AddSubscription(subscriber, "TestEventHandler", item, ThreadOption.Publisher);

			publisher.FireTestEvent();

			Assert.AreEqual(publisher.ManagedThreadId, subscriber.ManagedThreadId);
		}


		[TestMethod]
		public void RunInBackgroundWorkerRunInBackgroundWorker()
		{
			AsyncTestSubscriber subscriber = new AsyncTestSubscriber();
			topic.AddSubscription(subscriber, "TestEventHandler", item, ThreadOption.Background);

			topic.Fire(this, EventArgs.Empty, item, PublicationScope.WorkItem);

			bool result = AsyncTestSubscriber.Wait.WaitOne(5000, true);

			Assert.IsTrue(result && (Thread.CurrentThread.ManagedThreadId != AsyncTestSubscriber.ManagedThreadId));
		}

		[TestMethod]
		public void RunInUserInterfaceThread()
		{
			BackgroundThreadPublisher publisher = new BackgroundThreadPublisher();

			UISubscriber subscriber = new UISubscriber();
			topic.AddSubscription(subscriber, "TestEventHandler", item, ThreadOption.UserInterface);

			publisher.CanFireTopic.Set();
			Application.Run();

			Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, subscriber.ManagedThreadId);
		}


		[TestMethod]
		public void RunInUserInterfaceThreadExceptionsAreReported()
		{
			BackgroundThreadPublisher publisher = new BackgroundThreadPublisher();

			UISubscriber subscriber = new UISubscriber();
			topic.AddSubscription(subscriber, "FailingHandler", item, ThreadOption.UserInterface);

			publisher.CanFireTopic.Set();
			Application.Run();

			Thread.Sleep(500);
			Assert.IsNotNull(publisher.ThrownException);
		}

		[TestMethod]
		public void RunInUserInterfaceThreadWithNoSyncronizationContextCallsSubscriber()
		{
			BackgroundThreadPublisher publisher = new BackgroundThreadPublisher();

			SynchronizationContext context = SynchronizationContext.Current;
			TestSubscriber subscriber = new TestSubscriber();
			topic.AddSubscription(subscriber, "TestEventHandler", item, ThreadOption.UserInterface);

			publisher.CanFireTopic.Set();

			subscriber.HandlerCalledSignal.WaitOne();
			Assert.IsTrue(subscriber.TestEventHandlerCalled);
		}

		class UISubscriber : Control
		{
			public int ManagedThreadId = -1;

			public void TestEventHandler(object sender, EventArgs e)
			{
				ManagedThreadId = Thread.CurrentThread.ManagedThreadId;
				Application.ExitThread();
			}

			public void FailingHandler(object sender, EventArgs e)
			{
				try
				{
					ManagedThreadId = Thread.CurrentThread.ManagedThreadId;
					throw new Exception("FailingHandler");
				}
				finally
				{
					Application.ExitThread();
				}
			}
		}

		class BackgroundThreadPublisher
		{
			public AutoResetEvent CanFireTopic = new AutoResetEvent(false);
			private Thread worker;
			public EventTopicException ThrownException = null;

			public BackgroundThreadPublisher()
			{
				worker = new Thread(new ThreadStart(Work));
				worker.Start();
			}

			public void Work()
			{
				CanFireTopic.WaitOne();
				try
				{
					topic.Fire(this, EventArgs.Empty, item, PublicationScope.WorkItem);
				}
				catch(EventTopicException ex)
				{
					ThrownException = ex;
					Application.ExitThread();
				}
			}
		}

		public class TestPublisher
		{
			public event EventHandler TestEvent;

			public int ManagedThreadId = -1;

			public void FireTestEvent()
			{
				ManagedThreadId = Thread.CurrentThread.ManagedThreadId;
				if (TestEvent != null)
				{
					TestEvent(this, EventArgs.Empty);
				}

			}
		}

		class TestSubscriber
		{
			public bool TestEventHandlerCalled = false;
			public int ManagedThreadId = -1;
			public AutoResetEvent HandlerCalledSignal = new AutoResetEvent(false);

			public void TestEventHandler(object sender, EventArgs e)
			{
				ManagedThreadId = Thread.CurrentThread.ManagedThreadId;
				TestEventHandlerCalled = true;
				HandlerCalledSignal.Set();
			}
		}

		class AsyncTestSubscriber
		{
			public static bool TestEventHandlerCalled = false;
			public static int ManagedThreadId = -1;
			public static AutoResetEvent Wait = new AutoResetEvent(false);

			public void TestEventHandler(object sender, EventArgs e)
			{
				ManagedThreadId = Thread.CurrentThread.ManagedThreadId;
				TestEventHandlerCalled = true;
				Wait.Set();
			}
		}

		class SeparatedThreadEventPublisher
		{
			private AutoResetEvent FireSignal = new AutoResetEvent(false);

			public SeparatedThreadEventPublisher()
			{
				Thread thread = new Thread(new ThreadStart(FiringTopicThread));
				thread.Start();
			}

			private void FiringTopicThread()
			{
				while (FireSignal.WaitOne())
				{
					topic.Fire(this, EventArgs.Empty, item, PublicationScope.WorkItem);
				}
			}
			

			public void Fire()
			{
				FireSignal.Set();
			}
		}
	}
}
