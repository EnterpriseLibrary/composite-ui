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
	public class ConstructorPolicyFixture
	{
		[TestMethod]
		public void GetConstructorReturnsTheCorrectOneWhenParamsPassedThruAddParameter()
		{
			ConstructorPolicy policy = new ConstructorPolicy();

			policy.AddParameter(new ValueParameter<int>(5));
			ConstructorInfo actual = policy.SelectConstructor(new MockBuilderContext(), typeof(MockObject), null);
			ConstructorInfo expected = typeof(MockObject).GetConstructors()[1];

			Assert.AreSame(expected, actual);
		}

		[TestMethod]
		public void GetConstructorReturnsTheCorrectOneWhenParamsPassedThruCtor()
		{
			ConstructorPolicy policy = new ConstructorPolicy(new ValueParameter<int>(5));

			ConstructorInfo actual = policy.SelectConstructor(new MockBuilderContext(), typeof(MockObject), null);
			ConstructorInfo expected = typeof(MockObject).GetConstructors()[1];

			Assert.AreSame(expected, actual);
		}

		private class MockObject
		{
			public MockObject()
			{
			}

			public MockObject(int val)
			{
			}
		}
	}
}
