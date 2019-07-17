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
	public class WorkItemEventTopicCollectionFixture
	{
		private static WorkItem root;
		
		[TestInitialize]
		public void Setup()
		{
			root = new TestableRootWorkItem();
		}


		[TestMethod]
		public void CollectionIsEmpty()
		{
			Assert.AreEqual(0, root.EventTopics.Count);
		}


		[TestMethod]
		public void CanAddEventTopic()
		{
			EventTopic topic = root.EventTopics.AddNew<EventTopic>("test");

			Assert.AreEqual(1, root.EventTopics.Count);
			Assert.AreSame(topic, root.EventTopics.Get("test"));
		}

		[TestMethod]
		public void TopicAddedToChildIsAccessibleInParent()
		{
			WorkItem child = root.WorkItems.AddNew<WorkItem>();

            EventTopic topic = child.EventTopics.AddNew<EventTopic>("test");

			Assert.AreEqual(1, root.EventTopics.Count);
			Assert.AreSame(topic, root.EventTopics.Get("test"));			
		}

	}
}
