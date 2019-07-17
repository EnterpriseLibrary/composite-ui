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
	public class SingletonStrategyFixture
	{
		[TestMethod]
		public void CreatingASingletonTwiceReturnsSameInstance()
		{
			MockBuilderContext ctx = BuildContext();
			ctx.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), typeof(Something), null);

			Something i1 = (Something)ctx.HeadOfChain.BuildUp(ctx, typeof(Something), null, null);
			Something i2 = (Something)ctx.HeadOfChain.BuildUp(ctx, typeof(Something), null, null);

			Assert.AreSame(i1, i2);
		}

		[TestMethod]
		public void SingletonsCanBeBasedOnTypeAndID()
		{
			MockBuilderContext ctx = BuildContext();
			ctx.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), typeof(Something), "magickey");

			Something i1a = (Something)ctx.HeadOfChain.BuildUp(ctx, typeof(Something), null, "magickey");
			Something i1b = (Something)ctx.HeadOfChain.BuildUp(ctx, typeof(Something), null, "magickey");
			Something i2 = (Something)ctx.HeadOfChain.BuildUp(ctx, typeof(Something), null, null);
			Something i3 = (Something)ctx.HeadOfChain.BuildUp(ctx, typeof(Something), null, null);

			Assert.AreSame(i1a, i1b);
			Assert.IsTrue(i1a != i2);
			Assert.IsTrue(i2 != i3);
		}

		private static MockBuilderContext BuildContext()
		{
			MockBuilderContext ctx = new MockBuilderContext();

			ctx.InnerChain.Add(new SingletonStrategy());
			ctx.InnerChain.Add(new CreationStrategy());

			ctx.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());

			return ctx;
		}

		private class Something
		{
		}
	}
}
