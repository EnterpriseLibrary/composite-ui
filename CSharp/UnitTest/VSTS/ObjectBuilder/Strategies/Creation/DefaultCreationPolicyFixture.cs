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
	public class DefaultCreationPolicyFixture
	{
		[TestMethod]
		public void CanReturnObjectWithEmptyConstructor()
		{
			MockBuilderContext ctx = CreateContext();

			object result = ctx.HeadOfChain.BuildUp(ctx, typeof(object), null, null);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void CanCreateObjectWithParameterizedConstructor()
		{
			MockBuilderContext ctx = CreateContext();

			CtorObject result = (CtorObject)ctx.HeadOfChain.BuildUp(ctx, typeof(CtorObject), null, null);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Foo);
		}

		[TestMethod]
		public void DependencyChainIsFollowed()
		{
			MockBuilderContext ctx = CreateContext();

			CascadingCtorObject result = (CascadingCtorObject)ctx.HeadOfChain.BuildUp(ctx, typeof(CascadingCtorObject), null, null);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.InnerCtorObject);
			Assert.IsNotNull(result.InnerCtorObject.Foo);
		}

		[TestMethod]
		public void PassingAnExistingObjectReturnsThatObject()
		{
			MockBuilderContext ctx = CreateContext();

			object existing = new object();
			object result = ctx.HeadOfChain.BuildUp(ctx, typeof(object), existing, null);

			Assert.AreSame(existing, result);
		}

		[TestMethod]
		public void PassingAnExistingObjectRunsLaterStrategies()
		{
			MockBuilderContext ctx = CreateContext();
			MockStrategy mockStrategy = new MockStrategy();
			ctx.InnerChain.Add(mockStrategy);

			object existing = new object();
			object result = ctx.HeadOfChain.BuildUp(ctx, typeof(object), existing, null);

			Assert.IsTrue(mockStrategy.WasRun);
		}

		[TestMethod]
		public void MultiParameterCtorWorks()
		{
			MockBuilderContext ctx = CreateContext();

			MultiParamCtorObject result = (MultiParamCtorObject)ctx.HeadOfChain.BuildUp(ctx, typeof(MultiParamCtorObject), null, null);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.O1);
			Assert.IsNotNull(result.O2);
		}

		[TestMethod]
		public void PureValueTypeCanBeConstructed()
		{
			MockBuilderContext ctx = CreateContext();

			Assert.AreEqual(0, ctx.HeadOfChain.BuildUp(ctx, typeof(int), null, null));
		}

		[TestMethod]
		public void ConstructorWithValueTypeWorks()
		{
			MockBuilderContext ctx = CreateContext();

			CtorValueTypeObject result = (CtorValueTypeObject)ctx.HeadOfChain.BuildUp(ctx, typeof(CtorValueTypeObject), null, null);

			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.IntValue);
		}

		private MockBuilderContext CreateContext()
		{
			MockBuilderContext result = new MockBuilderContext();
			result.InnerChain.Add(new CreationStrategy());
			result.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			return result;
		}

		private class CtorObject
		{
			public object Foo;

			public CtorObject(object foo)
			{
				Foo = foo;
			}
		}

		private class CascadingCtorObject
		{
			public CtorObject InnerCtorObject;

			public CascadingCtorObject(CtorObject ctorObject)
			{
				InnerCtorObject = ctorObject;
			}
		}

		private class MultiParamCtorObject
		{
			public object O1;
			public object O2;

			public MultiParamCtorObject(object o1, object o2)
			{
				O1 = o1;
				O2 = o2;
			}
		}

		private class CtorValueTypeObject
		{
			public int IntValue;

			public CtorValueTypeObject(int i)
			{
				IntValue = i;
			}
		}

		private struct CtorValueType
		{
			public object ObjectValue;

			public CtorValueType(object o)
			{
				ObjectValue = o;
			}
		}

		private class MockStrategy : BuilderStrategy
		{
			public bool WasRun = false;

			public override object BuildUp(IBuilderContext context, Type t, object existing, string id)
			{
				WasRun = true;
				return existing;
			}
		}
	}
}
