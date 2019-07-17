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
using Microsoft.Practices.CompositeUI.Commands;
using System.Windows.Forms;

namespace Microsoft.Practices.CompositeUI.Tests.Commands
{
	[TestClass]
	public class EventCommandAdapterFixture
	{
		[TestMethod]
		public void CanCreateAdapterWithOneInvoker()
		{
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(invoker, "Event");

			Assert.AreEqual(1, adapter.Invokers.Count);
			Assert.AreSame("Event", adapter.Invokers[invoker][0]);
		}

		[TestMethod]
		public void CanAddInvoker()
		{
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(invoker, "Event");

			MockInvoker invoker2 = new MockInvoker();
			adapter.AddInvoker(invoker2, "Event");

			Assert.AreEqual(2, adapter.Invokers.Count);
		}


		[TestMethod]
		public void CanRemoveInvoker()
		{
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(invoker, "Event");
			MockInvoker invoker2 = new MockInvoker();
			adapter.AddInvoker(invoker2, "Event");

			adapter.RemoveInvoker(invoker2, "Event");

			Assert.AreEqual(1, adapter.Invokers.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CreatingWithWrongTypeThrows()
		{
			ToolStripMenuItem invoker = new ToolStripMenuItem();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>();
			adapter.AddInvoker(invoker, "Click");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddingWrongTypeInvokerThrows()
		{
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(invoker, "Event");

			adapter.AddInvoker(new ToolStripMenuItem(), "Click");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void RemovingWrongTypeInvokerThrows()
		{
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(invoker, "Event");
			ToolStripMenuItem invoker2 = new ToolStripMenuItem();

			adapter.RemoveInvoker(invoker2, "Event");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateingWithNullInvokerThrows()
		{
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(null, "Event");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreatingWithNullEventThrows()
		{
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(invoker, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddingWithNullInvokerThrows()
		{
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(invoker, "Event");
			adapter.AddInvoker(null, "Event");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddingWithNullEventThrows()
		{
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(invoker, "Event");
			adapter.AddInvoker(new MockInvoker(), null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemovingWithNullInvokerThrows()
		{
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(invoker, "Event");
			adapter.RemoveInvoker(null, "Event");
		}

		[TestMethod]
		public void RemovingNonRegisteredInvokerNoOps()
		{
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(invoker, "Event");
			adapter.RemoveInvoker(new MockInvoker(), "Event");

			Assert.AreEqual(1, adapter.Invokers.Count);
		}


		[TestMethod]
		public void InvokerIsWiredUp()
		{
			Command command = new Command();
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(invoker, "Event");
			command.AddCommandAdapter(adapter);
			MockListener listener = new MockListener();
			command.ExecuteAction += listener.CatchCommand;

			invoker.DoInvokeEvent();

			Assert.IsTrue(listener.CommandFired);
		}

		[TestMethod]
		public void InvokerIsUnwired()
		{
			Command command = new Command();
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(invoker, "Event");
			command.AddCommandAdapter(adapter);
			MockListener listener = new MockListener();
			command.ExecuteAction += listener.CatchCommand;

			adapter.RemoveInvoker(invoker, "Event");

			invoker.DoInvokeEvent();

			Assert.IsFalse(listener.CommandFired);
		}

		[TestMethod]
		public void CanAddMoreThanOneEventToAnInvokerObject()
		{
			Command command = new Command();

			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(invoker, "Event");
			adapter.AddInvoker(invoker, "Event2");

			Assert.IsTrue(invoker.EventIsHooked); 
			Assert.IsTrue(invoker.Event2IsHooked);
		}

		[TestMethod]
		public void CanRemoveOneInvokerEventFromAdapter()
		{
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>();

			adapter.AddInvoker(invoker, "Event");
			adapter.AddInvoker(invoker, "Event2");

			adapter.RemoveInvoker(invoker, "Event2");

			Assert.IsTrue(adapter.Invokers[invoker].Contains("Event"));
			Assert.IsFalse(adapter.Invokers[invoker].Contains("Event2"));
		}

		[TestMethod]
		public void CanTestIfInvokerIsContained()
		{
			MockInvoker invoker = new MockInvoker();
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>(invoker, "Event");
			MockInvokerB invokerB = new MockInvokerB();
			EventCommandAdapter<MockInvokerB> adapterB = new EventCommandAdapter<MockInvokerB>(invokerB, "Event");

			Assert.IsTrue(adapter.ContainsInvoker(invoker));
			Assert.IsTrue(adapterB.ContainsInvoker(invokerB));
			Assert.IsFalse(adapter.ContainsInvoker(invokerB));
			Assert.IsFalse(adapterB.ContainsInvoker(invoker));
		}

		[TestMethod]
		public void DisposeUnwiresAllInvokers()
		{
			EventCommandAdapter<MockInvoker> adapter = new EventCommandAdapter<MockInvoker>();
			MockInvoker invoker = new MockInvoker();
			adapter.AddInvoker(invoker, "Event");
			MockInvoker invokerB = new MockInvoker();
			adapter.AddInvoker(invokerB, "Event");
			adapter.AddInvoker(invokerB, "Event2");

			adapter.Dispose();

			Assert.AreEqual(0, adapter.Invokers.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(CommandException))]
		public void StaticInvokerThrows()
		{
			EventCommandAdapter<MockStaticInvoker> adapter = new EventCommandAdapter<MockStaticInvoker>();
			MockStaticInvoker invoker = new MockStaticInvoker();
			adapter.AddInvoker(invoker, "Event");
		}

		class MockListener
		{
			public bool CommandFired = false;

			public void CatchCommand(object sender, EventArgs args)
			{
				CommandFired = true;
			}
		}

		class MockStaticInvoker
		{
			public static event EventHandler Event;

			public static void DoInvokeEvent()
			{
				if (Event != null)
				{
					Event(null, EventArgs.Empty);
				}
			}
		}

		class MockInvokerB
		{
			public event EventHandler Event;

			public void DoInvokeEvent()
			{
				if (Event != null)
				{
					Event(this, EventArgs.Empty);
				}
			}
		}

		class MockInvoker
		{
			public event EventHandler Event;
			public event EventHandler Event2;

			public void DoInvokeEvent()
			{
				if (Event != null)
				{
					Event(this, EventArgs.Empty);
				}
			}

			public void DoInvokeEvent2()
			{
				if (Event2 != null)
				{
					Event2(this, EventArgs.Empty);
				}
			}

			public bool EventIsHooked
			{
				get { return Event != null; }
			}

			public bool Event2IsHooked
			{
				get { return Event2 != null; }
			}
		}

	}
}
