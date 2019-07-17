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

namespace Microsoft.Practices.CompositeUI.Tests.Commands
{
	[TestClass]
	public class CommandAdapterFixture
	{
		private static MockAdapter adapter;

		[TestInitialize]
		public void SetUp()
		{
			adapter = new MockAdapter();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfBindNullCommand()
		{
			adapter.BindCommand(null);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ThrowsIfBindTwice()
		{
			adapter.BindCommand(new Command());
			adapter.BindCommand(new Command());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfUnbindNullCommand()
		{
			adapter.UnbindCommand(null);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ThrowsIfUnbindToNotBoundCommand()
		{
			adapter.UnbindCommand(new Command());
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ThrowsIfUnbindWithDifferentCommand()
		{
			adapter.BindCommand(new Command());
			adapter.UnbindCommand(new Command());
		}

		[TestMethod]
		public void CanBindAgainIfUnbound()
		{
			Command cmd = new Command();
			adapter.BindCommand(cmd);
			adapter.UnbindCommand(cmd);

			adapter.BindCommand(new Command());
		}

		class MockAdapter : CommandAdapter
		{
			public override void AddInvoker(object invoker, string eventName)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public override void RemoveInvoker(object invoker, string eventName)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public override int InvokerCount
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public override bool ContainsInvoker(object invoker)
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}
	}
}
