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

using Microsoft.Practices.CompositeUI.Commands;
using System;

namespace Microsoft.Practices.CompositeUI.Tests.Commands
{
	[TestClass]
	public class CommandHandlerAttributeFixture
	{
		[TestMethod]
		public void CanCreateAttribute()
		{
			CommandHandlerAttribute attr = new CommandHandlerAttribute("TestCommand");

			Assert.IsNotNull(attr);
			Assert.AreEqual("TestCommand", attr.CommandName);
		}


		[TestMethod]
		public void AddingObjectWithoutCommandHandlerAttributeDoesNotCreateCommand()
		{
			TestableRootWorkItem item = new TestableRootWorkItem();
			int cmdCount = item.Commands.Count;
			
			object cmdHandler = item.Items.AddNew<object>();

			Assert.AreEqual(cmdCount, item.Commands.Count);
		}

		[TestMethod]
		public void SingleCommandHandlerAttributeAddsOneCommand()
		{
			TestableRootWorkItem item = new TestableRootWorkItem();
			int cmdCount = item.Commands.Count;

			SingleTestCommandHandler cmdHandler = item.Items.AddNew<SingleTestCommandHandler>();

			Assert.AreEqual(cmdCount + 1, item.Commands.Count);
		}

		[TestMethod]
		public void SingleCommandHandlerAttributeIsCalledOnce()
		{
			TestableRootWorkItem item = new TestableRootWorkItem();
			SingleTestCommandHandler cmdHandler = item.Items.AddNew<SingleTestCommandHandler>();
			
			item.Commands["TestCommand"].Execute();

			Assert.AreEqual(1, cmdHandler.CommandHandlerCalledCount);
		}


		[TestMethod]
		public void MultipleEqualAttributesRegisterCommandOnlyOnce()
		{
			TestableRootWorkItem item = new TestableRootWorkItem();
			int cmdCount = item.Commands.Count;

			TwoTestCommandHandler cmdHandler = item.Items.AddNew<TwoTestCommandHandler>();

			Assert.AreEqual(cmdCount + 1, item.Commands.Count);
		}


		[TestMethod]
		public void MultipleEqualAttributesRegisterHandlerWithCommandOnlyOnce()
		{
			TestableRootWorkItem item = new TestableRootWorkItem();
			TwoTestCommandHandler cmdHandler = item.Items.AddNew<TwoTestCommandHandler>();

			item.Commands["TestCommand"].Execute();

			Assert.AreEqual(1, cmdHandler.CommandHandlerCalledCount);
		}

		[TestMethod]
		[ExpectedException(typeof(CommandException))]
		public void StaticHandlerThrows()
		{
			TestableRootWorkItem item = new TestableRootWorkItem();

			SingleStaticTestCommandHandler cmdHandler = item.Items.AddNew<SingleStaticTestCommandHandler>();
		}

		class SingleStaticTestCommandHandler
		{
			public int CommandHandlerCalledCount = 0;

			[CommandHandler("TestCommand")]
			public static void TestCommandHandler(object sender, EventArgs e)
			{
			}
		}

		class SingleTestCommandHandler
		{
			public int CommandHandlerCalledCount = 0;

			[CommandHandler("TestCommand")]
			public void TestCommandHandler(object sender, EventArgs e)
			{
				CommandHandlerCalledCount++;
			}
		}

		class TwoTestCommandHandler
		{
			public int CommandHandlerCalledCount = 0;
			
			[CommandHandler("TestCommand")]
			[CommandHandler("TestCommand")]
			public void TestCommandHandler(object sender, EventArgs e)
			{
				CommandHandlerCalledCount++;
			}

		}
	}
}
