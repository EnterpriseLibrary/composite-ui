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
	public class TypeMappingStrategyFixture
	{
		[TestMethod]
		public void CanMapInterfacesToConcreteTypes()
		{
			MockBuilderContext ctx = new MockBuilderContext();
			TypeMappingStrategy strategy = new TypeMappingStrategy();
			ctx.Policies.Set<ITypeMappingPolicy>(new TypeMappingPolicy(typeof(SalesFoo), null), typeof(IFoo), "sales");
			ctx.Policies.Set<ITypeMappingPolicy>(new TypeMappingPolicy(typeof(Foo), null), typeof(IFoo), "marketing");
			ctx.InnerChain.Add(strategy);

			MockStrategy mock = new MockStrategy();
			ctx.InnerChain.Add(mock);

			strategy.BuildUp<IFoo>(ctx, null, "sales");

			Assert.IsTrue(mock.WasRun);
			Assert.AreEqual(typeof(SalesFoo), mock.IncomingType);

			mock.WasRun = false;
			mock.IncomingType = null;

			strategy.BuildUp<IFoo>(ctx, null, "marketing");

			Assert.IsTrue(mock.WasRun);
			Assert.AreEqual(typeof(Foo), mock.IncomingType);
		}

		[TestMethod]
		[ExpectedException(typeof(IncompatibleTypesException))]
		public void IncompatibleTypes()
		{
			MockBuilderContext ctx = new MockBuilderContext();
			TypeMappingStrategy strategy = new TypeMappingStrategy();
			ctx.Policies.Set<ITypeMappingPolicy>(new TypeMappingPolicy(typeof(object), null), typeof(IFoo), "sales");
			ctx.InnerChain.Add(strategy);

			strategy.BuildUp<IFoo>(ctx, null, "sales");
		}


		private class MockStrategy : BuilderStrategy
		{
			public bool WasRun = false;
			public Type IncomingType = null;

			public override object BuildUp(IBuilderContext context, Type t, object existing, string id)
			{
				WasRun = true;
				IncomingType = t;
				return null;
			}
		}

		private interface IFoo
		{
		}

		private class Foo : IFoo
		{
		}

		private class SalesFoo : IFoo
		{
		}
	}
}
