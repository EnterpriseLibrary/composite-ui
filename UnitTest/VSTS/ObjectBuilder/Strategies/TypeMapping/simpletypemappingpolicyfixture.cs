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
	public class SimpleTypeMappingPolicyFixture
	{
		[TestMethod]
		public void PolicyReturnsGivenType()
		{
			TypeMappingPolicy policy = new TypeMappingPolicy(typeof (Foo), null);

			Assert.AreEqual(new DependencyResolutionLocatorKey(typeof (Foo), null), policy.Map(new DependencyResolutionLocatorKey()));
		}

		private class Foo
		{
		}
	}
}
