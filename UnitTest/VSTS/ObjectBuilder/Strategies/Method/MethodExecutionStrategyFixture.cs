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
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder.Tests
{
	[TestClass]
	public class MethodExecutionStrategyFixture
	{
		#region Success Cases

		[TestMethod]
		public void StrategyCallsParameterlessMethod()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("ParameterlessMethod", new MethodCallInfo("ParameterlessMethod"));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof (MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);

			Assert.IsTrue(obj.ParameterlessWasCalled);
		}

		[TestMethod]
		public void StrategyDoesWorkBasedOnConcreteTypeInsteadOfPassedType()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("ParameterlessMethod", new MethodCallInfo("ParameterlessMethod"));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof(MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (object), obj, null);

			Assert.IsTrue(obj.ParameterlessWasCalled);
		}

		[TestMethod]
		public void StrategyCallsMethodWithDirectValues()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("IntMethod", new MethodCallInfo("IntMethod", 32));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof (MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);

			Assert.AreEqual(32, obj.IntValue);
		}

		[TestMethod]
		public void StrategyCallsMethodsUsingIParameterValues()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("IntMethod", new MethodCallInfo("IntMethod", new ValueParameter<int>(32)));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof (MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);

			Assert.AreEqual(32, obj.IntValue);
		}

		[TestMethod]
		public void CanCallMultiParameterMethods()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("MultiParamMethod", new MethodCallInfo("MultiParamMethod", 1.0, "foo"));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof (MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);

			Assert.AreEqual(1.0, obj.MultiDouble);
			Assert.AreEqual("foo", obj.MultiString);
		}

		[TestMethod]
		public void StrategyCallsMultipleMethodsAndCallsThemInOrder()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("ParameterlessMethod", new MethodCallInfo("ParameterlessMethod"));
			policy.Methods.Add("IntMethod", new MethodCallInfo("IntMethod", new ValueParameter<int>(32)));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof (MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);

			Assert.AreEqual(1, obj.CallOrderParameterless);
			Assert.AreEqual(2, obj.CallOrderInt);
		}

		#endregion

		#region Failure Cases

		[TestMethod]
		public void StrategyWithNoObjectDoesntThrow()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			ctx.InnerChain.Add(strategy);

			ctx.HeadOfChain.BuildUp(ctx, typeof (object), null, null);
		}

		[TestMethod]
		public void StrategyDoesNothingWithNoPolicy()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);

			Assert.IsFalse(obj.ParameterlessWasCalled);
		}

		[TestMethod]
		public void SettingPolicyForMissingMethodDoesntThrow()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("NonExistantMethod", new MethodCallInfo("NonExistantMethod"));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof (MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);
		}

		[TestMethod]
		public void SettingPolicyForWrongParameterCountDoesntThrow()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("ParameterlessMethod", new MethodCallInfo("ParameterlessMethod", 123));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof (MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);
		}

		[TestMethod]
		[ExpectedException(typeof(IncompatibleTypesException))]
		public void IncompatibleTypesThrows()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			MethodInfo mi = typeof(MockObject).GetMethod("IntMethod");
			policy.Methods.Add("IntMethod", new MethodCallInfo( mi, new ValueParameter<string>(String.Empty)));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof(MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof(MockObject), obj, null);
		}


		#endregion

		// ---------------------------------------------------------------------
		// Test List
		// ---------------------------------------------------------------------
		// TODO: Call method with non-void return values, and do something with the value
		// TODO: Testing with ref & out parameters
		// TODO: Statics

		#region Support Classes

		public interface IFoo
		{
		}

		public interface IBar
		{
		}

		public class FooBar : IFoo, IBar
		{
		}

		public class MockObject
		{
			private int currentOrder = 0;

			public bool ParameterlessWasCalled = false;
			public bool AmbiguousWasCalled = false;
			public int IntValue = 0;
			public int CallOrderParameterless = 0;
			public int CallOrderInt = 0;
			public double MultiDouble = 0.0;
			public string MultiString = null;

			public void ParameterlessMethod()
			{
				CallOrderParameterless = ++currentOrder;
				ParameterlessWasCalled = true;
			}

			public void IntMethod(int intValue)
			{
				CallOrderInt = ++currentOrder;
				IntValue = intValue;
			}

			public void AmbiguousMethod(IFoo foo)
			{
				AmbiguousWasCalled = true;
			}

			public void AmbiguousMethod(IBar bar)
			{
				AmbiguousWasCalled = true;
			}

			public void MultiParamMethod(double d, string s)
			{
				MultiDouble = d;
				MultiString = s;
			}
		}

		#endregion
	}
}
