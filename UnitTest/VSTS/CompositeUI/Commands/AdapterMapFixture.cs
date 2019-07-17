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
using System.Collections.ObjectModel;
using Microsoft.Practices.CompositeUI.Commands;
using System.Windows.Forms;

namespace Microsoft.Practices.CompositeUI.Tests.Commands
{
	[TestClass]
	public class AdapterMapFixture
	{
		private static WorkItem container;
		private static ICommandAdapterMapService mapSvc;

		[TestInitialize]
		public void Setup()
		{
			container = new TestableRootWorkItem();
			mapSvc = container.Services.Get<ICommandAdapterMapService>(true);
		}

		[TestMethod]
		public void CanRegisterAnAdapter()
		{
			mapSvc.Register(typeof (object), typeof (MockAdapter));

			CommandAdapter ad = mapSvc.CreateAdapter(typeof (object));

			Assert.AreSame(typeof (MockAdapter), ad.GetType());
		}

		[TestMethod]
		[ExpectedException(typeof (AdapterMapServiceException))]
		public void ThrowsWhenRegisteringANonAdapter()
		{
			mapSvc.Register(typeof (object), typeof (object));
		}


		[TestMethod]
		public void UnregisterAdapter()
		{
			mapSvc.Register(typeof (object), typeof (MockAdapter));
			mapSvc.UnRegister(typeof (object));

			CommandAdapter ad = mapSvc.CreateAdapter(typeof (object));

			Assert.IsNull(ad);
		}

		[TestMethod]
		public void AddingInvokerToCommandCreatesAdapter()
		{
			mapSvc.Register(typeof (MockInvoker), typeof (MockAdapter));
			Command cmd = new Command();
			container.Items.Add(cmd);

			MockInvoker inv = new MockInvoker();
			cmd.AddInvoker(inv, "Event");

			ReadOnlyCollection<MockAdapter> list = cmd.FindAdapters<MockAdapter>();

			Assert.AreEqual(1, list.Count);
		}

		[TestMethod]
		public void AddedInvokerFiresTheCommand()
		{
			mapSvc.Register(typeof(MockInvoker), typeof(MockAdapter));
			Command cmd = new Command();
			bool called = false;
			cmd.ExecuteAction += delegate(object sender, EventArgs e)
			{
				called = true;
			};

			container.Items.Add(cmd);

			MockInvoker invoker = new MockInvoker();
			cmd.AddInvoker(invoker, "Event");

			invoker.DoInvoke();

			Assert.IsTrue(called);
		}

		[TestMethod]
		public void RemovedInvokerDoesNotFiresTheCommand()
		{
			mapSvc.Register(typeof(MockInvoker), typeof(MockAdapter));
			Command cmd = new Command();
			container.Items.Add(cmd);
			bool called = false;
			cmd.ExecuteAction += delegate(object sender, EventArgs e)
			{
				called = true;
			};

			MockInvoker invoker = new MockInvoker();
			cmd.AddInvoker(invoker, "Event");
			cmd.RemoveInvoker(invoker, "Event");

			invoker.DoInvoke();

			Assert.IsFalse(called);

		}

		[TestMethod]
		public void CanFindAdapterForAssignableType()
		{
			mapSvc.Register(typeof(MockInvoker), typeof(MockAdapter));

			CommandAdapter adapter = mapSvc.CreateAdapter(typeof(AnotherMockInvoker));

			Assert.IsNotNull(adapter);
			Assert.AreEqual(typeof(MockAdapter), adapter.GetType());
		}

		class MockInvoker
		{
			public event EventHandler Event;

			public void DoInvoke()
			{
				if (Event != null)
				{
					Event(this, EventArgs.Empty);
				}
			}
		}

		class AnotherMockInvoker : MockInvoker
		{
		}

		class MockAdapter : CommandAdapter
		{
			private MockInvoker invoker;

			public override void AddInvoker(object invoker, string evenName)
			{
				this.invoker = (MockInvoker)invoker;
				this.invoker.Event += InvokerEventHandler;

			}

			public override void RemoveInvoker(object invoker, string eventName)
			{
				if (this.invoker == invoker)
				{
					this.invoker.Event -= InvokerEventHandler;
				}
			}

			private void InvokerEventHandler(object sender, EventArgs e)
			{
				base.FireCommand();
			}

			public override bool ContainsInvoker(object invoker)
			{
				return true;
			}

			public override int InvokerCount
			{
				get { return 0; }
			}
		}
	}
}
