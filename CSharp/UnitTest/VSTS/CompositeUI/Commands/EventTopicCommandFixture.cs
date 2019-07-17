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
using Microsoft.Practices.CompositeUI.Commands;

namespace Microsoft.Practices.CompositeUI.Tests.Commands
{
	[TestClass]
	public class EventTopicCommandFixture
	{
		private static WorkItem workItem;
		private static MockTopic topic;

		[TestInitialize]
		public void Setup()
		{
			workItem = new TestableRootWorkItem();
			topic = new MockTopic();
			workItem.EventTopics.Add(topic, topic.Name);
		}


		[TestMethod]
		public void CommandExecutionFiresEventTopic()
		{
            topic = workItem.EventTopics.AddNew<MockTopic>("topic://EventTopicCommand/Test");
            EventTopicCommand cmd = workItem.Commands.AddNew<EventTopicCommand>("Test");
            cmd.Execute();

            Assert.IsTrue(topic.FireCalled);
		}


		class MockTopic : EventTopic
		{
			public bool FireCalled = false;

            public MockTopic() : base()
            {
            }

			public override void Fire(object sender, EventArgs e, WorkItem workItem, PublicationScope scope)
			{
				FireCalled = true;
			}
		}
	}
}
