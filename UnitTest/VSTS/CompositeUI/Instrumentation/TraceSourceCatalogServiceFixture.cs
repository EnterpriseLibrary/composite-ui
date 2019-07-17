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
using System.Diagnostics;

namespace Microsoft.Practices.CompositeUI.Tests.Instrumentation
{
	[TestClass]
	public class TraceSourceCatalogServiceFixture
	{
		private static TraceSourceCatalogService catalog;

		[TestInitialize]
		public void SetUp()
		{
			catalog = new TraceSourceCatalogService();
		}

		[TestMethod]
		public void GetTraceSourceCreatesNewOne()
		{
			TraceSource ts = catalog.GetTraceSource("Foo");

			Assert.IsNotNull(ts);
			Assert.AreEqual("Foo", ts.Name);
		}

		[TestMethod]
		public void GetTraceSourceTwiceReturnsSame()
		{
			TraceSource ts1 = catalog.GetTraceSource("Foo");
			TraceSource ts2 = catalog.GetTraceSource("Foo");

			Assert.AreSame(ts1, ts2);

		}

		[TestMethod]
		public void AddingTraceFiresAddedEvent()
		{
			bool added = false;
			catalog.TraceSourceAdded += delegate { added = true; };

			TraceSource ts = catalog.GetTraceSource("Foo");

			Assert.IsTrue(added);
		}

		[TestMethod]
		public void TraceSourcesCollectionExposesAddedTraceSource()
		{
			TraceSource ts = catalog.GetTraceSource("Foo");

			Assert.AreEqual(1, catalog.TraceSources.Count);
		}

		[ExpectedException(typeof(NotSupportedException))]
		[TestMethod]
		public void TraceSourcesCollectionIsReadOnly()
		{
			catalog.TraceSources["Foo"] = new TraceSource("Foo");
		}

		[TestMethod]
		public void TraceSourceContainsDefaultListenerIfNoConfigForSwitch()
		{
			TraceSource source = new TraceSource("Foo");

			Assert.AreEqual(1, source.Listeners.Count);
			Assert.IsTrue(source.Listeners[0] is DefaultTraceListener);
		}

		[TestMethod]
		public void ServiceAddsTraceListenersToSource()
		{
			ConsoleTraceListener listener = new ConsoleTraceListener();
			Trace.Listeners.Add(listener);

			Assert.IsTrue(FindListener("Foo", listener), "Listener from Trace.Listener was not added to source");
		}

		[TestMethod]
		public void AddingSharedTraceSourceIsAddedToAllSources()
		{
			TraceSource source1 = catalog.GetTraceSource("Foo");
			TraceSource source2 = catalog.GetTraceSource("Bar");
			ConsoleTraceListener listener = new ConsoleTraceListener();

			catalog.AddSharedListener(listener);
			
			Assert.IsTrue(FindListener("Foo", listener));
			Assert.IsTrue(FindListener("Bar", listener));
		}

		[TestMethod]
		public void AddingSharedTraceSourceWithNameAddsToAllSources()
		{
			TraceSource source1 = catalog.GetTraceSource("Foo");
			TraceSource source2 = catalog.GetTraceSource("Bar");
			ConsoleTraceListener listener = new ConsoleTraceListener();

			catalog.AddSharedListener(listener, "Test");

			Assert.IsNotNull(source1.Listeners["Test"]);
			Assert.IsNotNull(source2.Listeners["Test"]);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddingSharedListenerWithNullThrows()
		{
			catalog.AddSharedListener(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddingSharedListenerWithNameNullListenerThrows()
		{
			catalog.AddSharedListener(null, "Test");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddingSharedListenerWithNameNullNameThrows()
		{
			catalog.AddSharedListener(new ConsoleTraceListener(), null);
		}

		private bool FindListener(string sourceName, TraceListener listener)
		{
			bool result = false;

			TraceSource ts = catalog.GetTraceSource("Bar");
			foreach (TraceListener l in ts.Listeners)
			{
				if (l == listener)
				{
					result = true;
					break;
				}
			}

			return result;
		}
	}
}
