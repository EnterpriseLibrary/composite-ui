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
using System.Diagnostics;

namespace Microsoft.Practices.CompositeUI.Tests
{
	[TestClass]
	public class StateAttributeFixture
	{
		private WorkItem workItem;

		[TestInitialize]
		public void FixtureSetup()
		{
			workItem = new TestableRootWorkItem();
		}

		[TestMethod]
		public void StateIsAssignedMatchingType()
		{
			workItem.State["stringValue1"] = "value1";
			workItem.State["intValue1"] = 1;

			MockObject target = workItem.Items.AddNew<MockObject>();

			Assert.AreEqual("value1", target.StringProp);
			Assert.AreEqual(1, target.IntProp);
		}

		class MockObject
		{
			private string stringProp;

			[State]
			public string StringProp
			{
				get { return stringProp; }
				set { stringProp = value; }
			}

			private int intProp;

			[State]
			public int IntProp
			{
				get { return intProp; }
				set { intProp = value; }
			}

		}
	}
}
