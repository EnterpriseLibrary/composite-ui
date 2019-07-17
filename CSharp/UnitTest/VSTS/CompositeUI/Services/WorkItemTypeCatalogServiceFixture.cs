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
using Microsoft.Practices.CompositeUI.Services;

namespace Microsoft.Practices.CompositeUI.Tests.Services
{
	[TestClass]
	public class WorkItemTypeCatalogServiceFixture
	{
		[TestMethod]
		public void CanRegisterWorkItem()
		{
			WorkItemTypeCatalogService svc = new WorkItemTypeCatalogService();

			svc.RegisterWorkItem<WorkItem>();

			Assert.AreEqual(1, svc.RegisteredWorkItemTypes.Count);
		}

		[TestMethod]
		public void CanCreateInstancesOfWorkItem()
		{
			bool created = false;
			WorkItem wi = new TestableRootWorkItem();
			WorkItemTypeCatalogService svc = wi.Services.AddNew<WorkItemTypeCatalogService, IWorkItemTypeCatalogService>();

			svc.RegisterWorkItem<WorkItem>();
			svc.CreateEachWorkItem<WorkItem>(wi, delegate(WorkItem item) { created = true; });

			Assert.IsTrue(created);
		}

		[TestMethod]
		public void CreatingEachWorkItemCheckAssigableRight()
		{
			bool created = false;
			WorkItem wi = new TestableRootWorkItem();
			WorkItemTypeCatalogService svc = wi.Services.AddNew<WorkItemTypeCatalogService, IWorkItemTypeCatalogService>();
			svc.RegisterWorkItem<MockWorkItem>();

			svc.CreateEachWorkItem<ITest>(wi, delegate(ITest item) { created = true; });

			Assert.IsTrue(created);
		}

		class MockWorkItem : WorkItem, ITest
		{
		}

		interface ITest
		{
		}

	}
}
