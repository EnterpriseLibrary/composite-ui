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
using Microsoft.Practices.CompositeUI.Commands;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.CompositeUI.Utility;

namespace Microsoft.Practices.CompositeUI.Tests.Commands
{
	[TestClass]
	public class CommandFixture
	{
		private static Command command;
		private static bool executed;

		[TestInitialize]
		public void SetUp()
		{
			command = new Command();
			executed = false;
		}

		private static void ActionHandler(object sender, EventArgs e)
		{
			executed = true;
		}

		[TestMethod]
		public void CommandIsNotNull()
		{
			Assert.IsNotNull(command, "The command was not created.");
		}

		[TestMethod]
		public void CommandExposesExecuteHandler()
		{
			command.ExecuteAction += ActionHandler;
			command.Execute();

			Assert.IsTrue(executed);
		}

		[TestMethod]
		public void CanRegisterAdapter()
		{
			MockAdapter adapter = new MockAdapter();
			command.AddCommandAdapter(adapter);

			Assert.AreEqual(1, command.Adapters.Count);
			Assert.AreSame(adapter, command.Adapters[0]);
		}

		[TestMethod]
		public void CanRemoveAdapter()
		{
			MockAdapter adapter = new MockAdapter();
			command.AddCommandAdapter(adapter);
			command.RemoveCommandAdapter(adapter);

			Assert.AreEqual(0, command.Adapters.Count);
			Assert.IsFalse(command.Adapters.Contains(adapter));
		}

		[TestMethod]
		public void AdapterIsNotifiedAboutCommandChanges()
		{
			MockAdapter adapter = new MockAdapter();
			command.AddCommandAdapter(adapter);
			Assert.AreEqual(0, adapter.OnChangedCalled);

			command.Status = CommandStatus.Disabled;

			Assert.AreEqual(1, adapter.OnChangedCalled);
		}

		[TestMethod]
		public void CommandIsTheSender()
		{
			command.ExecuteAction += delegate(object sender, EventArgs e)
			{
				Assert.AreSame(command, sender);
			};
			MockAdapter adapter = new MockAdapter();
			command.AddCommandAdapter(adapter);
			adapter.Fire();
		}

		[TestMethod]
		public void CanRemoveInvokerWithMultipleAdapters()
		{
			Command command = new Command();
			MockInvokerA invokerA = new MockInvokerA();
			MockInvokerB invokerB = new MockInvokerB();
			EventCommandAdapter<MockInvokerA> adapterA = new EventCommandAdapter<MockInvokerA>(invokerA, "Event");
			EventCommandAdapter<MockInvokerB> adapterB = new EventCommandAdapter<MockInvokerB>(invokerB, "Event");

			command.AddCommandAdapter(adapterA);
			command.AddCommandAdapter(adapterB);

			command.RemoveInvoker(invokerA, "Event");

			Assert.AreEqual(1, adapterB.Invokers.Count);
			Assert.AreEqual(0, adapterA.Invokers.Count);
		}

		[TestMethod]
		public void RemovingAllInvokersFromAdapterRemovesAdapterFromCommand()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			ICommandAdapterMapService svc = workItem.Services.Get<ICommandAdapterMapService>();
			svc.Register(typeof(MockInvokerA), typeof(MockAdapter));

			Command command = new Command();
			workItem.Commands.Add(command);

			MockInvokerA invoker = new MockInvokerA();
			command.AddInvoker(invoker, "Event");
			Assert.AreEqual(1, command.Adapters.Count);

			command.RemoveInvoker(invoker, "Event");
			Assert.AreEqual(0, command.Adapters.Count);
		}


		[TestMethod]
		public void RemovingCommandFromWorkItemUnWiresTheCommandHandler()
		{
			TestableRootWorkItem workItem = new TestableRootWorkItem();
			Assert.AreEqual(0, workItem.Commands.Count);

			MockHandler handler = workItem.Items.AddNew<MockHandler>();
			Assert.AreEqual(1, workItem.Commands.Count);

			Command command = workItem.Commands.Get("TestCommand");
			Assert.IsNotNull(command);

			workItem.Items.Remove(handler);

			command.Execute();
			Assert.IsFalse(handler.TestCommandHandlerCalled);
		}

		[TestMethod]
		public void EnabledCommandFiresExecutedEvent()
		{
			bool executed = false;
			command.ExecuteAction += delegate { executed = true; };

			command.Status = CommandStatus.Enabled;
			command.Execute();

			Assert.IsTrue(executed);
		}

		[TestMethod]
		public void DisabledCommandDoesNotFireExecutedEvent()
		{
			bool executed = false;
			command.ExecuteAction += delegate { executed = true; };

			command.Status = CommandStatus.Disabled;
			command.Execute();

			Assert.IsFalse(executed);
		}

		[TestMethod]
		public void UnavailableCommandDoesNotFireExecutedEvent()
		{
			bool executed = false;
			command.ExecuteAction += delegate { executed = true; };

			command.Status = CommandStatus.Unavailable;
			command.Execute();

			Assert.IsFalse(executed);
		}

		[TestMethod]
		public void InvokerDoesNotCauseExecuteOnDisabledCommand()
		{
			MockInvokerA invoker = new MockInvokerA();
			bool executed = false;
			command.ExecuteAction += delegate { executed = true; };
			EventCommandAdapter<MockInvokerA> adapter = new EventCommandAdapter<MockInvokerA>(invoker, "Event");
			command.AddCommandAdapter(adapter);

			command.Status = CommandStatus.Disabled;
			invoker.DoInvokeEvent();

			Assert.IsFalse(executed);
		}

		[TestMethod]
		public void InvokerDoesNotCauseExecuteOnUnavailableCommand()
		{
			MockInvokerA invoker = new MockInvokerA();
			bool executed = false;
			command.ExecuteAction += delegate { executed = true; };
			EventCommandAdapter<MockInvokerA> adapter = new EventCommandAdapter<MockInvokerA>(invoker, "Event");
			command.AddCommandAdapter(adapter);

			command.Status = CommandStatus.Unavailable;
			invoker.DoInvokeEvent();

			Assert.IsFalse(executed);
		}


		[TestMethod]
		public void AddingCommandWithNameCreatesCommandWithName()
		{
			WorkItem workItem = new TestableRootWorkItem();
			Command cmd = workItem.Commands["TestCommand"];
		}


		[TestMethod]
		public void CommandAddedWithANameGetsItsNameThroughBuilder()
		{
			WorkItem workItem = new TestableRootWorkItem();
			Command cmd = workItem.Commands.AddNew<Command>("TestCommand");

			Assert.AreEqual("TestCommand", cmd.Name);
		}

		[TestMethod]
		public void DisposingCommandRemovesAllOfItsAdapters()
		{
			WorkItem wi = new TestableRootWorkItem();
			ICommandAdapterMapService svc = wi.Services.Get<ICommandAdapterMapService>();
			svc.Register(typeof(MockInvokerA), typeof(MockAdapter));
			Command cmd = wi.Commands.AddNew<Command>();
			cmd.AddInvoker(new MockInvokerA(), "Event");
			cmd.AddInvoker(new MockInvokerA(), "Event");
			cmd.AddInvoker(new MockInvokerA(), "Event");
			cmd.AddInvoker(new MockInvokerA(), "Event");

			Assert.AreEqual(4, cmd.Adapters.Count);
			cmd.Dispose();
			Assert.AreEqual(0, cmd.Adapters.Count);
		}

		[TestMethod]
		public void StatusDefaultsToEnabled()
		{
			Assert.AreEqual(CommandStatus.Enabled, command.Status);
		}

		[TestMethod]
		public void ChangedEventFiredWhenStatusChanges()
		{
			bool changedFired = false;
			command.Changed += delegate { changedFired = true; };

			command.Status = CommandStatus.Disabled;

			Assert.IsTrue(changedFired);
		}

		[TestMethod]
		public void ChangedEventDoesNotFireIfAssignedStatusIsSame()
		{
			bool changedFired = false;
			command.Changed += delegate { changedFired = true; };

			command.Status = CommandStatus.Enabled;

			Assert.IsFalse(changedFired);
		}

		[TestMethod]
		public void AdapterCratedByCommandIsDisposedWithCommand()
		{
			WorkItem wi = new TestableRootWorkItem();
			ICommandAdapterMapService svc = wi.Services.Get<ICommandAdapterMapService>();
			svc.Register(typeof(Control), typeof(MockControlAdapter));

			Command cmd = wi.Commands.AddNew<Command>();

			Control invoker = new Control();
			cmd.AddInvoker(invoker, "GotFocus");

			MockControlAdapter adapter = (MockControlAdapter)cmd.Adapters[0];
			cmd.Dispose();
			Assert.IsTrue(adapter.IsDisposed);
		}

		[TestMethod]
		public void CommandDoesNotReuseAdpatersOnAddInvoker()
		{
			WorkItem wi = new TestableRootWorkItem();
			ICommandAdapterMapService svc = wi.Services.Get<ICommandAdapterMapService>();
			svc.Register(typeof(Control), typeof(MockControlAdapter));

			Command cmd = wi.Commands.AddNew<Command>();

			Control invoker = new Control();
			cmd.AddInvoker(invoker, "GotFocus");
			cmd.AddInvoker(invoker, "Click");

			Assert.AreEqual(2, cmd.Adapters.Count);
		}

		[TestMethod]
		public void AdapterCreatedByCommandIsDisposedWhenInvokerRemoved()
		{
			WorkItem wi = new TestableRootWorkItem();
			ICommandAdapterMapService svc = wi.Services.Get<ICommandAdapterMapService>();
			svc.Register(typeof(Control), typeof(MockControlAdapter));

			Command cmd = wi.Commands.AddNew<Command>();

			Control invoker = new Control();
			cmd.AddInvoker(invoker, "GotFocus");

			MockControlAdapter adapter = (MockControlAdapter)cmd.Adapters[0];

			cmd.RemoveInvoker(invoker, "GotFocus");

			Assert.IsTrue(adapter.IsDisposed);
		}

		[TestMethod]
		public void AdapterCreatedByCommandIsRemovedWhenInvokerRemoved()
		{
			WorkItem wi = new TestableRootWorkItem();
			ICommandAdapterMapService svc = wi.Services.Get<ICommandAdapterMapService>();
			svc.Register(typeof(Control), typeof(MockControlAdapter));

			Command cmd = wi.Commands.AddNew<Command>();

			Control invoker = new Control();
			cmd.AddInvoker(invoker, "GotFocus");

			MockControlAdapter adapter = (MockControlAdapter)cmd.Adapters[0];

			cmd.RemoveInvoker(invoker, "GotFocus");

			adapter.IsDisposed = false;

			cmd.Dispose();
			// Should not be disposed again as it shouldn't be contained at all in the command anymore.
			Assert.IsFalse(adapter.IsDisposed);
		}

		[TestMethod]
		public void AdapterCreatedByCommandIsRemovedWhenLastInvokerRemoved()
		{
			WorkItem wi = new TestableRootWorkItem();
			ICommandAdapterMapService svc = wi.Services.Get<ICommandAdapterMapService>();
			svc.Register(typeof(Control), typeof(MockControlAdapter));

			Command cmd = wi.Commands.AddNew<Command>();

			Control invoker = new Control();
			cmd.AddInvoker(invoker, "GotFocus");

			MockControlAdapter adapter = (MockControlAdapter)cmd.Adapters[0];
			adapter.AddInvoker(invoker, "Click");

			cmd.RemoveInvoker(invoker, "GotFocus");

			Assert.IsFalse(adapter.IsDisposed);

			cmd.RemoveInvoker(invoker, "Click");
			
			Assert.IsTrue(adapter.IsDisposed);

			adapter.IsDisposed = false;

			cmd.Dispose();
			// Should not be disposed again as it shouldn't be contained at all in the command anymore.
			Assert.IsFalse(adapter.IsDisposed);
		}

		[TestMethod]
		public void UserAddedAdapterIsNotDisposedWithCommand()
		{

			Control invoker = new Control();
			MockControlAdapter adapter = new MockControlAdapter(invoker, "GotFocus");

			command.AddCommandAdapter(adapter);

			command.Dispose();

			Assert.IsFalse(adapter.IsDisposed);
		}

		//[TestMethod]
		//public void RemoveDisposesCommandAndUnplugsCommandAdapters()
		//{
		//    TestableRootWorkItem parent = new TestableRootWorkItem();
		//    Command cmd = parent.Commands["TestCommand"];
		//    ICommandAdapterMapService mapService = parent.Services.Get<ICommandAdapterMapService>();
		//    mapService.Register(typeof(MockInvokerA), typeof(EventCommandAdapter<MockInvokerA>));

		//    MockInvokerA invoker = new MockInvokerA();
		//    cmd.AddInvoker(invoker, "Event");
		//    Assert.AreEqual(1, parent.Commands["TestCommand"].Adapters.Count);

		//    InstanceHandlerClass instance = new InstanceHandlerClass();
		//    parent.Items.Add(instance);

		//    invoker.DoInvokeEvent();
		//    Assert.AreEqual(1, instance.counter);


		//    parent.Commands.Remove(parent.Commands["TestCommand"]);
		//    GC.Collect();
		//    GC.WaitForPendingFinalizers();

		//    invoker.DoInvokeEvent();
		//    Assert.AreEqual(0, parent.Commands["TestCommand"].Adapters.Count);

		//    // Returns 2
		//    Assert.AreEqual(1, instance.counter);
		//}

		[TestMethod]
		public void AddingCommandToCommandsCollectionFiresAddeddEvent()
		{
			WorkItem item = new TestableRootWorkItem();
			bool addedCalled = false;

			item.Commands.Added += delegate(object sender, DataEventArgs<Command> args)
			{
				addedCalled = true;
			};
			Command cmd = item.Commands.AddNew<Command>();

			Assert.IsTrue(addedCalled);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void RemovingCommandFromItemsCollectionThrows()
		{
			WorkItem item = new TestableRootWorkItem();
			Command cmd = item.Commands.AddNew<Command>();
			item.Items.Remove(cmd);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void RemovingCommandFromCommandsCollectionThrows()
		{
			WorkItem item = new TestableRootWorkItem();
			Command cmd = item.Commands.AddNew<Command>();
			item.Commands.Remove(cmd);
		}

		[TestMethod]
		public void AddingCommandSetsItsName()
		{
			TestableRootWorkItem wi = new TestableRootWorkItem();
			Command cmd = wi.Commands["TestCommand1"];
			Assert.AreEqual("TestCommand1", cmd.Name);

			cmd = wi.Commands.AddNew<Command>("TestCommand2");
			Assert.AreEqual("TestCommand2", cmd.Name);
		}

		#region Helper classes

		class MockCommand : Command
		{
			public TraceSource GetTraceSource()
			{
				return base.TraceSource;
			}
		}

		class InstanceHandlerClass
		{
			public bool HandlerCalled = false;
			public int counter = 0;

			[CommandHandler("TestCommand")]
			public void InstanceHandler(object sender, EventArgs e)
			{
				HandlerCalled = true;
				counter++;
			}
		}

		class MockHandler
		{
			public bool TestCommandHandlerCalled = false;

			[CommandHandler("TestCommand")]
			public void TestCommandHandler(object sender, EventArgs e)
			{
				TestCommandHandlerCalled = true;
			}
		}

		class MockInvokerA
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

		class MockAdapter : CommandAdapter
		{
			public bool DisposeCalled = false;

			public MockAdapter()
			{
			}

			public void Fire()
			{
				base.FireCommand();
			}

			public int OnChangedCalled = 0;

			protected override void OnCommandChanged(Command command)
			{
				OnChangedCalled++;
			}

			public int AddInvokerCalled = 0;
			public override void AddInvoker(object invoker, string evenName)
			{
				AddInvokerCalled++;
			}

			public override void RemoveInvoker(object invoker, string eventName)
			{
				AddInvokerCalled--;
			}

			public override bool ContainsInvoker(object invoker)
			{
				return true;
			}

			public override int InvokerCount
			{
				get { return AddInvokerCalled; }
			}

			protected override void Dispose(bool disposing)
			{
				DisposeCalled = true;
			}
		}

		class MockControlAdapter : EventCommandAdapter<Control>
		{
			public bool IsDisposed;

			public MockControlAdapter()
				: base()
			{
			}

			public MockControlAdapter(Control invoker, string eventName)
				: base(invoker, eventName)
			{

			}

			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
				IsDisposed = true;
			}
		}

		#endregion
	}
}
