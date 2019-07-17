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
	public class LookupParameterFixture
	{
		[TestMethod]
		public void ConstructorPolicyCanUseLookupToFindAnObject()
		{
			MockBuilderContext ctx = new MockBuilderContext();
			ctx.InnerLocator.Add("foo", 11);

			LookupParameter param = new LookupParameter("foo");

			Assert.AreEqual(11, param.GetValue(ctx));
			Assert.AreSame(typeof (int), param.GetParameterType(ctx));
		}
	}
}
