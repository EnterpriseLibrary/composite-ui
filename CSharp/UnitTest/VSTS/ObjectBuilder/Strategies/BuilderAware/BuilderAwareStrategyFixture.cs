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
	public class BuilderAwareStrategyFixture
	{
		[TestMethod]
		public void BuildIgnoresClassWithoutInterface()
		{
			BuilderAwareStrategy strategy = new BuilderAwareStrategy();
			MockBuilderContext context = new MockBuilderContext();
			IgnorantObject obj = new IgnorantObject();

			context.InnerChain.Add(strategy);

			context.HeadOfChain.BuildUp(context, typeof(IgnorantObject), obj, null);

			Assert.IsFalse(obj.OnAssembledCalled);
			Assert.IsFalse(obj.OnDisassemblingCalled);
		}

		[TestMethod]
		public void UnbuildIgnoresClassWithoutInterface()
		{
			BuilderAwareStrategy strategy = new BuilderAwareStrategy();
			MockBuilderContext context = new MockBuilderContext();
			IgnorantObject obj = new IgnorantObject();

			context.InnerChain.Add(strategy);

			context.HeadOfChain.TearDown(context, obj);

			Assert.IsFalse(obj.OnAssembledCalled);
			Assert.IsFalse(obj.OnDisassemblingCalled);
		}

		[TestMethod]
		public void BuildCallsClassWithInterface()
		{
			BuilderAwareStrategy strategy = new BuilderAwareStrategy();
			MockBuilderContext context = new MockBuilderContext();
			AwareObject obj = new AwareObject();

			context.InnerChain.Add(strategy);

			context.HeadOfChain.BuildUp(context, typeof(AwareObject), obj, "foo");

			Assert.IsTrue(obj.OnAssembledCalled);
			Assert.IsFalse(obj.OnDisassemblingCalled);
			Assert.AreEqual("foo", obj.AssembledID);
		}

		[TestMethod]
		public void UnbuildCallsClassWithInterface()
		{
			BuilderAwareStrategy strategy = new BuilderAwareStrategy();
			MockBuilderContext context = new MockBuilderContext();
			AwareObject obj = new AwareObject();

			context.InnerChain.Add(strategy);

			context.HeadOfChain.TearDown(context, obj);

			Assert.IsFalse(obj.OnAssembledCalled);
			Assert.IsTrue(obj.OnDisassemblingCalled);
		}

		[TestMethod]
		public void BuildChecksConcreteTypeAndNotRequestedType()
		{
			BuilderAwareStrategy strategy = new BuilderAwareStrategy();
			MockBuilderContext context = new MockBuilderContext();
			AwareObject obj = new AwareObject();

			context.InnerChain.Add(strategy);

			context.HeadOfChain.BuildUp(context, typeof(IgnorantObject), obj, null);

			Assert.IsTrue(obj.OnAssembledCalled);
			Assert.IsFalse(obj.OnDisassemblingCalled);
		}

		class IgnorantObject
		{
			public bool OnAssembledCalled = false;
			public bool OnDisassemblingCalled = false;
			public string AssembledID = null;

			public void OnBuiltUp(string id)
			{
				OnAssembledCalled = true;
				AssembledID = id;
			}

			public void OnTearingDown()
			{
				OnDisassemblingCalled = true;
			}
		}

		class AwareObject : IgnorantObject, IBuilderAware
		{
		}
	}
}
