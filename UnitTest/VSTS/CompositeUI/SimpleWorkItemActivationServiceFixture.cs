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

namespace Microsoft.Practices.CompositeUI.Tests
{
	[TestClass]
	public class SimpleWorkItemActivationServiceFixture
	{
		[TestMethod]
		public void ChecksThatWorkItemIsNotTerminatedBeforeChangingStatus()
		{
			WorkItem rootWorkItem = new TestableRootWorkItem();
			rootWorkItem.Services.AddNew<SimpleWorkItemActivationService, IWorkItemActivationService>();
			WorkItem w1 = rootWorkItem.WorkItems.AddNew<WorkItem>();
			WorkItem w2 = rootWorkItem.WorkItems.AddNew<WorkItem>();

			w2.Activate();
			w2.Terminate();
			w1.Activate();

			Assert.AreEqual(WorkItemStatus.Active, w1.Status);
			Assert.AreEqual(WorkItemStatus.Terminated, w2.Status);
		}
	}
}
