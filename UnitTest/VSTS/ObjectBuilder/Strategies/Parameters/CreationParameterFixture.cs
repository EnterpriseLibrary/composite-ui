//===============================================================================
// Microsoft patterns & practices
// Object Builder Application Block
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

namespace Microsoft.Practices.ObjectBuilder.Tests
{
	[TestClass]
	public class CreationParameterFixture
	{
		[TestMethod]
		public void CreationParameterUsesStrategyChainToCreateObjects()
		{
			MockBuilderContext ctx = new MockBuilderContext();
			NullStrategy strategy = new NullStrategy();
			ctx.InnerChain.Add(strategy);

			CreationParameter param = new CreationParameter(typeof (object));
			param.GetValue(ctx);

			Assert.IsTrue(strategy.WasCalled);
			Assert.AreEqual(typeof (object), strategy.TypeRequested);
		}

		[TestMethod]
		public void CreationParameterCanCreateObjectsOfAGivenID()
		{
			MockBuilderContext ctx = new MockBuilderContext();
			NullStrategy strategy = new NullStrategy();
			ctx.InnerChain.Add(strategy);

			CreationParameter param = new CreationParameter(typeof (object), "foo");
			param.GetValue(ctx);

			Assert.AreEqual("foo", strategy.IDRequested);
		}

		private class NullStrategy : BuilderStrategy
		{
			public bool WasCalled = false;
			public Type TypeRequested = null;
			public object IDRequested = null;

			public override object BuildUp(IBuilderContext context, Type t, object existing, string id)
			{
				WasCalled = true;
				TypeRequested = t;
				IDRequested = id;

				return null;
			}
		}
	}
}
