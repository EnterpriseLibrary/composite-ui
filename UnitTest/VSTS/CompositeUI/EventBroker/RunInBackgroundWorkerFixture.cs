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
using System.ComponentModel;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.Utility;
using System.Threading;

namespace Microsoft.Practices.CompositeUI.Tests.EventBroker
{

	[TestClass]
	public class RunInBackgroundWorkerFixture
	{
		private static EventTopic topic;
		private WorkItem workItem;

#if RELEASE
		private static int MillisecondsToWait = 1000;
#else
		private static int MillisecondsToWait = 60000;
#endif
		[TestInitialize]
		public void Setup()
		{
			topic = new EventTopic();
			workItem = new TestableRootWorkItem();
		}

		[TestMethod]
		public void SubscriberRequiringAsyncEventArgsReceivesWorkerAndEventArgs()
		{
			BackgroundSubscriber subscriber = new BackgroundSubscriber();
			EventArgs e = new EventArgs();
			topic.AddSubscription(subscriber, "AsyncEventHandler", workItem, ThreadOption.Background);

			topic.Fire(this, e, workItem, PublicationScope.WorkItem);

			subscriber.StartAsyncProcessSignal.Set();
			bool signaled = subscriber.FinishedSignal.WaitOne(MillisecondsToWait, true);

			Assert.IsTrue(signaled);
			Assert.IsNotNull(subscriber.ReceiverEventArgs);
			Assert.AreSame(e, subscriber.ReceiverEventArgs);
		}

		class BackgroundSubscriber
		{
			public AutoResetEvent FinishedSignal = new AutoResetEvent(false);


			public void TestEventHandler(object sender, EventArgs e)
			{
			}

			public void TestEventHandlerAsync(object sender, EventArgs e)
			{
				StartAsyncProcessSignal.WaitOne(MillisecondsToWait, true);
			}

			public EventArgs ReceiverEventArgs;

			public void AsyncEventHandler(object sender, EventArgs e)
			{
				StartAsyncProcessSignal.WaitOne(MillisecondsToWait, true);
				ReceiverEventArgs = e;
				FinishedSignal.Set();
			}

			public Exception ResultingException = null;
			public AutoResetEvent StartAsyncProcessSignal = new AutoResetEvent(false);
			public void FailingAsyncEventHandler(object sender, EventArgs e)
			{
				StartAsyncProcessSignal.WaitOne(MillisecondsToWait, true);
				ResultingException = new Exception("FailingAsyncEventHandler");
				throw ResultingException;
			}
		}
	}
}
