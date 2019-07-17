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
using Microsoft.Practices.ObjectBuilder;
using System.Diagnostics;

namespace Microsoft.Practices.CompositeUI.Tests.Instrumentation
{
	[TestClass]
	public class TraceSourceAttributeFixture
	{
		[TestMethod]
		public void AttributeGetsTraceSourceFromCatalogIfAvailable()
		{
			TraceSourceCatalogService catalog = new TraceSourceCatalogService();
			Locator locator = new Locator();
			Builder builder = new Builder();
			locator.Add(new DependencyResolutionLocatorKey(typeof(ITraceSourceCatalogService), null), catalog);

			MockTracedClass mock = builder.BuildUp<MockTracedClass>(locator, null, null);

			Assert.AreEqual(1, catalog.TraceSources.Count);
			Assert.IsNotNull(mock.TraceSource);
		}

		[TestMethod]
		public void AttributeInjectsNullIfNoCatalogAvailable()
		{
			Locator locator = new Locator();
			Builder builder = new Builder();

			MockTracedClass mock = builder.BuildUp<MockTracedClass>(locator, null, null);

			Assert.IsNull(mock.TraceSource);
		}

		#region Helper classes

		class MockTracedClass
		{
			private TraceSource traceSource;

			[TraceSource("Foo")]
			public TraceSource TraceSource
			{
				get { return traceSource; }
				set { traceSource = value; }
			}
		}

		#endregion
	}
}
