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
	// These "modes" describe the classed of behavior provided by DI.
	// 1. I need a new X. Don'typeToBuild reuse any existing ones.
	// 2. I need the unnamed X. Create it if it doesn'typeToBuild exist, else return the existing one.
	// 3. I need the X named Y. Create it if it doesn'typeToBuild exist, else return the existing one.
	// 4. I want the unnamed X. Return null if it doesn'typeToBuild exist.
	// 5. I want the X named Y. Return null if it doesn'typeToBuild exist.

	[TestClass]
	public class PropertyReflectionStrategyFixture
	{
		// Invalid attribute combination

		[TestMethod]
		[ExpectedException(typeof(InvalidAttributeException))]
		public void SpecifyingCreateNewAndDependencyThrows()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof(MockInvalidDualAttributes), null, null);
		}

		// Existing policy

		[TestMethod]
		public void PropertyReflectionWillNotOverwriteAPreExistingPolicyForAProperty()
		{
			MockBuilderContext context = CreateContext();
			PropertySetterPolicy policy = new PropertySetterPolicy();
			policy.Properties.Add("Foo", new PropertySetterInfo("Foo", new ValueParameter(typeof(object), 12)));
			context.Policies.Set<IPropertySetterPolicy>(policy, typeof(MockRequiresNewObject), null);

			MockRequiresNewObject obj = (MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, null);

			Assert.AreEqual(12, obj.Foo);
		}

		// Non creatable stuff

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowsIfConcreteTypeToCreateCannotBeCreated()
		{
			MockBuilderContext context = CreateContext();
			context.HeadOfChain.BuildUp(context, typeof(MockDependsOnInterface), null, null);
		}

		// Mode 1

		[TestMethod]
		public void CreateNewAttributeAlwaysCreatesNewObject()
		{
			MockBuilderContext context;

			context = CreateContext();
			MockRequiresNewObject depending1 = (MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, null);

			context = CreateContext();
			MockRequiresNewObject depending2 = (MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, null);

			Assert.IsNotNull(depending1);
			Assert.IsNotNull(depending2);
			Assert.IsNotNull(depending1.Foo);
			Assert.IsNotNull(depending2.Foo);
			Assert.IsFalse(depending1.Foo == depending2.Foo);
		}

		[TestMethod]
		public void NamedAndUnnamedObjectsInLocatorDontGetUsedForCreateNew()
		{
			MockBuilderContext context;
			object unnamed = new object();
			object named = new object();

			context = CreateContext();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), null), unnamed);
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), "Foo"), named);
			MockRequiresNewObject depending1 = (MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, null);

			context = CreateContext();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), null), unnamed);
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), "Foo"), named);
			MockRequiresNewObject depending2 = (MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, null);

			Assert.IsFalse(depending1.Foo == unnamed);
			Assert.IsFalse(depending1.Foo == unnamed);
			Assert.IsFalse(depending2.Foo == named);
			Assert.IsFalse(depending2.Foo == named);
		}

		// Mode 2

		[TestMethod]
		public void CanInjectExistingUnnamedObjectIntoProperty()
		{
			// Mode 2, with an existing object
			MockBuilderContext context = CreateContext();
			object dependent = new object();
			context.InnerLocator.Add(new DependencyResolutionLocatorKey(typeof(object), null), dependent);

			object depending = context.HeadOfChain.BuildUp(context, typeof(MockDependingObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockDependingObject);
			Assert.AreSame(dependent, ((MockDependingObject)depending).InjectedObject);
		}

		[TestMethod]
		public void InjectionCreatingNewUnnamedObjectWillOnlyCreateOnce()
		{
			// Mode 2, both flavors
			MockBuilderContext context;

			context = CreateContext();
			MockDependingObject depending1 = (MockDependingObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingObject), null, null);

			context = CreateContext(context.Locator);
			MockDependingObject depending2 = (MockDependingObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingObject), null, null);

			Assert.AreSame(depending1.InjectedObject, depending2.InjectedObject);
		}

		[TestMethod]
		public void InjectionCreatesNewObjectIfNotExisting()
		{
			// Mode 2, no existing object
			MockBuilderContext context = CreateContext();

			object depending = context.HeadOfChain.BuildUp(context, typeof(MockDependingObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockDependingObject);
			Assert.IsNotNull(((MockDependingObject)depending).InjectedObject);
		}

		[TestMethod]
		public void CanInjectNewInstanceWithExplicitTypeIfNotExisting()
		{
			// Mode 2, explicit type
			MockBuilderContext context = CreateContext();

			MockDependsOnIFoo depending = (MockDependsOnIFoo)context.HeadOfChain.BuildUp(
				context, typeof(MockDependsOnIFoo), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNotNull(depending.Foo);
		}

		// Mode 3

		[TestMethod]
		public void CanInjectExistingNamedObjectIntoProperty()
		{
			// Mode 3, with an existing object
			MockBuilderContext context = CreateContext();
			object dependent = new object();
			context.InnerLocator.Add(new DependencyResolutionLocatorKey(typeof(object), "Foo"), dependent);

			object depending = context.HeadOfChain.BuildUp(context, typeof(MockDependingNamedObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockDependingNamedObject);
			Assert.AreSame(dependent, ((MockDependingNamedObject)depending).InjectedObject);
		}

		[TestMethod]
		public void InjectionCreatingNewNamedObjectWillOnlyCreateOnce()
		{
			// Mode 3, both flavors
			MockBuilderContext context;

			context = CreateContext();
			MockDependingNamedObject depending1 = (MockDependingNamedObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingNamedObject), null, null);

			context = CreateContext(context.Locator);
			MockDependingNamedObject depending2 = (MockDependingNamedObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingNamedObject), null, null);

			Assert.AreSame(depending1.InjectedObject, depending2.InjectedObject);
		}

		[TestMethod]
		public void InjectionCreatesNewNamedObjectIfNotExisting()
		{
			// Mode 3, no existing object
			MockBuilderContext context = CreateContext();

			MockDependingNamedObject depending = (MockDependingNamedObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingNamedObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNotNull(depending.InjectedObject);
		}

		[TestMethod]
		public void CanInjectNewNamedInstanceWithExplicitTypeIfNotExisting()
		{
			// Mode 3, explicit type
			MockBuilderContext context = CreateContext();

			MockDependsOnNamedIFoo depending = (MockDependsOnNamedIFoo)context.HeadOfChain.BuildUp(context, typeof(MockDependsOnNamedIFoo), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNotNull(depending.Foo);
		}

		// Mode 2 & 3 together

		[TestMethod]
		public void NamedAndUnnamedObjectsDontCollide()
		{
			MockBuilderContext context = CreateContext();
			object dependent = new object();
			context.InnerLocator.Add(new DependencyResolutionLocatorKey(typeof(object), null), dependent);

			MockDependingNamedObject depending = (MockDependingNamedObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingNamedObject), null, null);

			Assert.IsFalse(Object.ReferenceEquals(dependent, depending.InjectedObject));
		}

		// Mode 4

		[TestMethod]
		public void PropertyIsNullIfUnnamedNotExists()
		{
			// Mode 4, no object provided
			MockBuilderContext context = CreateContext();

			MockOptionalDependingObject depending = (MockOptionalDependingObject)context.HeadOfChain.BuildUp(context, typeof(MockOptionalDependingObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNull(depending.InjectedObject);
		}

		[TestMethod]
		public void CanInjectExistingUnnamedObjectIntoOptionalDependentProperty()
		{
			// Mode 4, with an existing object
			MockBuilderContext context = CreateContext();
			object dependent = new object();
			context.InnerLocator.Add(new DependencyResolutionLocatorKey(typeof(object), null), dependent);

			object depending = context.HeadOfChain.BuildUp(context, typeof(MockOptionalDependingObject), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockOptionalDependingObject);
			Assert.AreSame(dependent, ((MockOptionalDependingObject)depending).InjectedObject);
		}

		// Mode 5

		[TestMethod]
		public void PropertyIsNullIfNamedNotExists()
		{
			// Mode 5, no object provided
			MockBuilderContext context = CreateContext();

			MockOptionalDependingObjectWithName depending = (MockOptionalDependingObjectWithName)context.HeadOfChain.BuildUp(context, typeof(MockOptionalDependingObjectWithName), null, null);

			Assert.IsNotNull(depending);
			Assert.IsNull(depending.InjectedObject);
		}

		[TestMethod]
		public void CanInjectExistingNamedObjectIntoOptionalDependentProperty()
		{
			// Mode 5, with an existing object
			MockBuilderContext context = CreateContext();
			object dependent = new object();
			context.InnerLocator.Add(new DependencyResolutionLocatorKey(typeof(object), "Foo"), dependent);

			object depending = context.HeadOfChain.BuildUp(context, typeof(MockOptionalDependingObjectWithName), null, null);

			Assert.IsNotNull(depending);
			Assert.IsTrue(depending is MockOptionalDependingObjectWithName);
			Assert.AreSame(dependent, ((MockOptionalDependingObjectWithName)depending).InjectedObject);
		}

		// NotPresentBehavior.Throw Tests

		[TestMethod]
		[ExpectedException(typeof(DependencyMissingException))]
		public void StrategyThrowsIfObjectNotPresent()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof(ThrowingMockObject), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(DependencyMissingException))]
		public void StrategyThrowsIfNamedObjectNotPresent()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof(NamedThrowingMockObject), null, null);
		}

		// SearchMode Tests

		[TestMethod]
		public void CanSearchDependencyUp()
		{
			Locator parent = new Locator();
			parent.Add(new DependencyResolutionLocatorKey(typeof(int), null), 25);
			Locator child = new Locator(parent);
			MockBuilderContext context = CreateContext(child);

			context.HeadOfChain.BuildUp(context, typeof(SearchUpMockObject), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(DependencyMissingException))]
		public void LocalSearchFailsIfDependencyIsOnlyUpstream()
		{
			Locator parent = new Locator();
			parent.Add(new DependencyResolutionLocatorKey(typeof(int), null), 25);
			Locator child = new Locator(parent);
			MockBuilderContext context = CreateContext(child);

			context.HeadOfChain.BuildUp(context, typeof(SearchLocalMockObject), null, null);
		}

		[TestMethod]
		public void LocalSearchGetsLocalIfDependencyIsAlsoUpstream()
		{
			Locator parent = new Locator();
			parent.Add(new DependencyResolutionLocatorKey(typeof(int), null), 25);
			Locator child = new Locator(parent);
			child.Add(new DependencyResolutionLocatorKey(typeof(int), null), 15);
			MockBuilderContext context = CreateContext(child);

			SearchLocalMockObject obj = (SearchLocalMockObject)context.HeadOfChain.BuildUp(context, typeof(SearchLocalMockObject), null, null);

			Assert.AreEqual(15, obj.Value);
		}

		// Helpers

		private MockBuilderContext CreateContext()
		{
			return CreateContext(new Locator());
		}

		private MockBuilderContext CreateContext(IReadWriteLocator locator)
		{
			MockBuilderContext result = new MockBuilderContext(locator);
			result.InnerChain.Add(new SingletonStrategy());
			result.InnerChain.Add(new PropertyReflectionStrategy());
			result.InnerChain.Add(new CreationStrategy());
			result.InnerChain.Add(new PropertySetterStrategy());
			result.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			result.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			return result;
		}

		#region Mock Classes

		public class SearchUpMockObject
		{
			private int value;

			[Dependency(SearchMode = SearchMode.Up, NotPresentBehavior = NotPresentBehavior.Throw)]
			public int Value
			{
				get { return value; }
				set { this.value = value; }
			}
		}

		public class SearchLocalMockObject
		{
			private int value;

			[Dependency(SearchMode = SearchMode.Local, NotPresentBehavior = NotPresentBehavior.Throw)]
			public int Value
			{
				get { return value; }
				set { this.value = value; }
			}
		}

		public class ThrowingMockObject
		{
			[Dependency(NotPresentBehavior = NotPresentBehavior.Throw)]
			public object InjectedObject
			{
				set { }
			}
		}

		public class NamedThrowingMockObject
		{
			[Dependency(Name = "Foo", NotPresentBehavior = NotPresentBehavior.Throw)]
			public object InjectedObject
			{
				set { }
			}
		}

		public class MockInvalidDualAttributes
		{
			private int value;

			[CreateNew]
			[Dependency]
			public int Value
			{
				get { return value; }
				set { this.value = value; }
			}

		}

		private interface ISomeInterface
		{
		}

		private  class MockDependsOnInterface
		{
			private ISomeInterface value;

			[Dependency]
			public ISomeInterface Value
			{
				get { return value; }
				set { this.value = value; }
			}
		}

		public class MockDependingObject
		{
			private object injectedObject;

			[Dependency]
			public virtual object InjectedObject
			{
				get { return injectedObject; }
				set { injectedObject = value; }
			}
		}

		public class MockDependingObjectDerived : MockDependingObject
		{
			public override object InjectedObject
			{
				get
				{
					return base.InjectedObject;
				}
				set
				{
					base.InjectedObject = value;
				}
			}
		}

		public class MockOptionalDependingObject
		{
			private object injectedObject;

			[Dependency(NotPresentBehavior = NotPresentBehavior.ReturnNull)]
			public object InjectedObject
			{
				get { return injectedObject; }
				set { injectedObject = value; }
			}
		}

		public class MockOptionalDependingObjectWithName
		{
			private object injectedObject;

			[Dependency(Name = "Foo", NotPresentBehavior = NotPresentBehavior.ReturnNull)]
			public object InjectedObject
			{
				get { return injectedObject; }
				set { injectedObject = value; }
			}
		}

		public class MockDependingNamedObject
		{
			private object injectedObject;

			[Dependency(Name = "Foo")]
			public object InjectedObject
			{
				get { return injectedObject; }
				set { injectedObject = value; }
			}
		}

		public class MockDependsOnIFoo
		{
			private IFoo foo;

			[Dependency(CreateType = typeof(Foo))]
			public IFoo Foo
			{
				get { return foo; }
				set { foo = value; }
			}
		}

		public class MockDependsOnNamedIFoo
		{
			private IFoo foo;

			[Dependency(Name = "Foo", CreateType = typeof(Foo))]
			public IFoo Foo
			{
				get { return foo; }
				set { foo = value; }
			}
		}

		public class MockRequiresNewObject
		{
			private object foo;

			[CreateNew]
			public virtual object Foo
			{
				get { return foo; }
				set { foo = value; }
			}
		}

		public class MockRequiresNewObjectDerived : MockRequiresNewObject
		{
			public override object Foo
			{
				get
				{
					return base.Foo;
				}
				set
				{
					base.Foo = value;
				}
			}
		}

		public interface IFoo { }
		public class Foo : IFoo { }

		#endregion
	}
}
