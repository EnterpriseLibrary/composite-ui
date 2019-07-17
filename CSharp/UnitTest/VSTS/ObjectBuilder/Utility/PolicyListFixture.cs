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
	public class PolicyListFixture
	{
		[TestMethod]
		public void CanAddPolicyToBagAndRetrieveIt()
		{
			PolicyList list = new PolicyList();

			list.Set<IBuilderPolicy>(new MockPolicy(), typeof(object), null);

			Assert.IsNotNull(list.Get<IBuilderPolicy>(typeof(object), null));
		}

		[TestMethod]
		public void CanAddMultiplePoliciesToBagAndRetrieveThem()
		{
			PolicyList list = new PolicyList();
			MockPolicy policy1 = new MockPolicy();
			MockPolicy policy2 = new MockPolicy();

			list.Set<IBuilderPolicy>(policy1, typeof(object), "1");
			list.Set<IBuilderPolicy>(policy2, typeof(string), "2");

			Assert.AreSame(policy1, list.Get<IBuilderPolicy>(typeof(object), "1"));
			Assert.AreSame(policy2, list.Get<IBuilderPolicy>(typeof(string), "2"));
		}

		[TestMethod]
		public void SetOverwritesExistingPolicy()
		{
			PolicyList list = new PolicyList();
			MockPolicy policy1 = new MockPolicy();
			MockPolicy policy2 = new MockPolicy();

			list.Set<IBuilderPolicy>(policy1, typeof(string), "1");
			list.Set<IBuilderPolicy>(policy2, typeof(string), "1");

			Assert.AreSame(policy2, list.Get<IBuilderPolicy>(typeof(string), "1"));
		}

		[TestMethod]
		public void CanClearPolicy()
		{
			PolicyList list = new PolicyList();
			MockPolicy policy = new MockPolicy();

			list.Set<IBuilderPolicy>(policy, typeof(string), "1");
			list.Clear<IBuilderPolicy>(typeof(string), "1");

			Assert.IsNull(list.Get<IBuilderPolicy>(typeof(string), "1"));
		}

		[TestMethod]
		public void CanClearAllPolicies()
		{
			PolicyList list = new PolicyList();
			list.Set<IBuilderPolicy>(new MockPolicy(), typeof(object), null);
			list.Set<IBuilderPolicy>(new MockPolicy(), typeof(object), "1");

			list.ClearAll();

			Assert.AreEqual(0, list.Count);
		}

		[TestMethod]
		public void DefaultPolicyUsedWhenSpecificPolicyIsntAvailable()
		{
			PolicyList list = new PolicyList();
			MockPolicy defaultPolicy = new MockPolicy();

			list.SetDefault<IBuilderPolicy>(defaultPolicy);

			Assert.AreSame(defaultPolicy, list.Get<IBuilderPolicy>(typeof(object), null));
		}

		[TestMethod]
		public void SpecificPolicyOverridesDefaultPolicy()
		{
			PolicyList list = new PolicyList();
			MockPolicy defaultPolicy = new MockPolicy();
			MockPolicy specificPolicy = new MockPolicy();

			list.Set<IBuilderPolicy>(specificPolicy, typeof(object), null);
			list.SetDefault<IBuilderPolicy>(defaultPolicy);

			Assert.AreSame(specificPolicy, list.Get<IBuilderPolicy>(typeof(object), null));
		}

		[TestMethod]
		public void CanClearDefaultPolicy()
		{
			PolicyList list = new PolicyList();
			MockPolicy defaultPolicy = new MockPolicy();
			list.SetDefault<IBuilderPolicy>(defaultPolicy);

			list.ClearDefault<IBuilderPolicy>();

			Assert.IsNull(list.Get<IBuilderPolicy>(typeof(object), null));
		}

		private class MockPolicy : IBuilderPolicy
		{
		}
	}
}
