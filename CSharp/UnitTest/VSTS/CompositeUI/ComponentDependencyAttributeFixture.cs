//===============================================================================
// Microsoft patterns & practices
// CompositeUI Application Block
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
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.CompositeUI.Tests
{
	[TestClass]
	public class ComponentDependencyAttributeFixture
	{
		private static Locator locator;
		private static Builder builder;
		private static LifetimeContainer container;

		[TestInitialize]
		public void SetUp()
		{
			locator = new Locator();
			container = new LifetimeContainer();
			locator.Add(typeof(ILifetimeContainer), container);
			builder = new Builder();
			builder.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
		}

		[TestMethod]
		[ExpectedException(typeof(DependencyMissingException))]
		public void AttributeDefaultsToNotCreatingAndRequired()
		{
			MockDefaultDependency obj = builder.BuildUp<MockDefaultDependency>(locator, "foo", null);
		}

		[TestMethod]
		public void AttributeDefaultsToNotCreatingWithOptional()
		{
			MockOptionalDependencyDefaults obj = builder.BuildUp<MockOptionalDependencyDefaults>(locator, "foo", null);

			Assert.IsNull(obj.Dependency);
		}

		[TestMethod]
		public void DependencyGetsCreatedIfSpecifiedOnAttribute()
		{
			MockRequiredDependencyCreate obj = builder.BuildUp<MockRequiredDependencyCreate>(locator, "foo", null);

			Assert.IsNotNull(obj.Dependency, "Dependency not injected");
		}

		[TestMethod]
		public void DependencyGetsCreatedAndReusedIfSpecifiedOnAttribute()
		{
			MockRequiredDependencyCreate obj1 = builder.BuildUp<MockRequiredDependencyCreate>(locator, "foo", null);
			MockRequiredDependencyCreate obj2 = builder.BuildUp<MockRequiredDependencyCreate>(locator, "bar", null);

			// 2 objects + 1 dependency
			Assert.AreEqual(3, container.Count);

			Assert.IsNotNull(obj1.Dependency, "Dependency not injected");
			Assert.IsNotNull(obj2.Dependency, "Dependency not injected");
			Assert.AreSame(obj1.Dependency, obj2.Dependency, "Instance not reused");
		}

		[TestMethod]
		public void DependencyOfExplicitTypeIsCreatedIfSpecified()
		{
			MockRequiredDependencyCreateWithType obj = builder.BuildUp<MockRequiredDependencyCreateWithType>(locator, "foo", null);

			Assert.IsNotNull(obj.Dependency, "Dependency not injected");
			Assert.IsTrue(obj.Dependency is Dependency, "Dependency is not of the specified type");
		}

		[TestMethod]
		public void NullDependencyDoesNotThrow()
		{
			MockOptionalDependencyDefaults obj = new MockOptionalDependencyDefaults();
			
			builder.BuildUp(locator, typeof(MockOptionalDependencyDefaults), "foo", obj);

			Assert.IsNull(obj.Dependency);
		}

		#region Helper classes

		class MockDefaultDependency
		{
			private object dependency;

			[ComponentDependency("dependency")]
			public object Dependency
			{
				get { return dependency; }
				set { dependency = value; }
			}
		}

		class MockOptionalDependencyDefaults
		{
			private object dependency;

			[ComponentDependency("dependency", Required = false)]
			public object Dependency
			{
				get { return dependency; }
				set { dependency = value; }
			}
		}

		class MockRequiredDependencyCreate
		{
			private object dependency;

			[ComponentDependency("dependency", CreateIfNotFound = true)]
			public object Dependency
			{
				get { return dependency; }
				set { dependency = value; }
			}
		}

		class MockRequiredDependencyCreateWithType
		{
			private object dependency;

			[ComponentDependency("dependency", CreateIfNotFound = true, Type = typeof(Dependency))]
			public object Dependency
			{
				get { return dependency; }
				set { dependency = value; }
			}
		}

		class Dependency { }

		#endregion
	}
}
