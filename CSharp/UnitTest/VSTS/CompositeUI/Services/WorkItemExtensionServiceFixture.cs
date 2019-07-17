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
using Microsoft.Practices.CompositeUI.Services;

namespace Microsoft.Practices.CompositeUI.Tests.Services
{
	[TestClass]
	public class WorkItemExtensionServiceFixture
	{
		[TestMethod]
		public void Bug1713()
		{
			WorkItemExtensionService service = new WorkItemExtensionService();
			WorkItem wi = new TestableRootWorkItem();

			service.RegisterExtension(typeof(MyWorkItem), typeof(MockExtension));
			service.RegisterExtension(typeof(MyWorkItem), typeof(MockExtension2));

			service.RegisterExtension(typeof(MyWorkItem2), typeof(MockExtension));
			service.RegisterExtension(typeof(MyWorkItem2), typeof(MockExtension2));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullWorkItemTypeThrows()
		{
			WorkItemExtensionService service = new WorkItemExtensionService();

			service.RegisterExtension(null, typeof(MockExtension));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullExtensionTypeThrows()
		{
			WorkItemExtensionService service = new WorkItemExtensionService();

			service.RegisterExtension(typeof(WorkItem), null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullRootExtensionTypeThrows()
		{
			WorkItemExtensionService service = new WorkItemExtensionService();

			service.RegisterRootExtension(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowsIfExtensionTypeIsNotIWorkItemExtension()
		{
			WorkItemExtensionService service = new WorkItemExtensionService();

			service.RegisterExtension(typeof(WorkItem), typeof(object));
		}

		[TestMethod]
		public void CanRegisterExtensionType()
		{
			WorkItemExtensionService service = new WorkItemExtensionService();

			service.RegisterExtension(typeof(WorkItem), typeof(MockExtension));

			Assert.AreEqual(1, service.RegisteredExtensions.Count);
			Assert.IsTrue(service.RegisteredExtensions.ContainsKey(typeof(WorkItem)));
		}

		[TestMethod]
		public void CanInitializeExtensionsForWorkItem()
		{
			WorkItemExtensionService service = new WorkItemExtensionService();
			WorkItem wi = new TestableRootWorkItem();
			service.RegisterExtension(typeof(WorkItem), typeof(MockExtension));

			service.InitializeExtensions(wi);

			Assert.AreEqual(true, MockExtension.Initialized);
		}

		[TestMethod]
		public void CreatingWorkItemInitializesExtensions()
		{
			WorkItem wi = new TestableRootWorkItem();
			WorkItemExtensionService svc = wi.Services.AddNew<WorkItemExtensionService, IWorkItemExtensionService>();
			svc.RegisterExtension(typeof(WorkItem), typeof(MockExtension));

			wi.Items.AddNew<WorkItem>();

			Assert.AreEqual(true, MockExtension.Initialized);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void RegisteringExtensionTwiceThrows()
		{
			WorkItemExtensionService service = new WorkItemExtensionService();
			WorkItem wi = new TestableRootWorkItem();
			service.RegisterExtension(typeof(WorkItem), typeof(MockExtension));

			service.RegisterExtension(typeof(WorkItem), typeof(MockExtension));
		}

        [TestMethod]
        public void CanRegisterExtensionForDifferentWorkItems()
        {
            WorkItemExtensionService service = new WorkItemExtensionService();
			WorkItem wi = new TestableRootWorkItem();
			service.RegisterExtension(typeof(MyWorkItem), typeof(MockExtension));
			service.RegisterExtension(typeof(MyWorkItem2), typeof(MockExtension));

            Assert.IsTrue(service.RegisteredExtensions.ContainsKey(typeof(MyWorkItem)));
            Assert.IsTrue(service.RegisteredExtensions.ContainsKey(typeof(MyWorkItem2)));
        }

		#region Helper classes

		private class MyWorkItem : WorkItem { }

		private class MyWorkItem2 : MyWorkItem { }

		private class MockExtension : IWorkItemExtension
		{
			public static bool Initialized = false;

			public void Initialize(WorkItem workItem)
			{
				Initialized = true;
			}
		}

		private class MockExtension2 : IWorkItemExtension
		{
			public void Initialize(WorkItem workItem)
			{
			}
		}

		#endregion
	}
}
