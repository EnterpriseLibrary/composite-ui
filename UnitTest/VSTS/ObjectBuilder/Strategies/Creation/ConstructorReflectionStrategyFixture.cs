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
	public class ConstructorReflectionStrategyFixture
	{
		// Value type creation

		[TestMethod]
		public void CanCreateValueTypesWithConstructorInjectionStrategyInPlace()
		{
			MockBuilderContext context = CreateContext();

			Assert.AreEqual(0, context.HeadOfChain.BuildUp(context, typeof(int), null, null));
		}

		// Invalid attribute combination

		[TestMethod]
		[ExpectedException(typeof(InvalidAttributeException))]
		public void SpecifyingMultipleConstructorsThrows()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof(MockInvalidDualConstructorAttributes), null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidAttributeException))]
		public void SpecifyingCreateNewAndDependencyThrows()
		{
			MockBuilderContext context = CreateContext();

			context.HeadOfChain.BuildUp(context, typeof(MockInvalidDualParameterAttributes), null, null);
		}

		// Default behavior

		[TestMethod]
		public void DefaultBehaviorIsMode2ForUndecoratedParameter()
		{
			MockBuilderContext context = CreateContext();

			MockUndecoratedObject obj1 = (MockUndecoratedObject)context.HeadOfChain.BuildUp(context, typeof(MockUndecoratedObject), null, null);
			MockUndecoratedObject obj2 = (MockUndecoratedObject)context.HeadOfChain.BuildUp(context, typeof(MockUndecoratedObject), null, null);

			Assert.AreSame(obj1.Foo, obj2.Foo);
		}

		[TestMethod]
		public void WhenSingleConstructorIsPresentDecorationIsntRequired()
		{
			MockBuilderContext context = CreateContext();

			MockUndecoratedConstructorObject obj1 = (MockUndecoratedConstructorObject)context.HeadOfChain.BuildUp(context, typeof(MockUndecoratedConstructorObject), null, null);
			MockUndecoratedConstructorObject obj2 = (MockUndecoratedConstructorObject)context.HeadOfChain.BuildUp(context, typeof(MockUndecoratedConstructorObject), null, null);

			Assert.IsNotNull(obj1.Foo);
			Assert.AreSame(obj1.Foo, obj2.Foo);
		}

		// Mode 1

		[TestMethod]
		public void CreateNewAttributeAlwaysCreatesNewObject()
		{
			MockBuilderContext context = CreateContext();

			MockRequiresNewObject depending1 = (MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, "Foo");
			MockRequiresNewObject depending2 = (MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, "Bar");

			Assert.IsNotNull(depending1);
			Assert.IsNotNull(depending2);
			Assert.IsNotNull(depending1.Foo);
			Assert.IsNotNull(depending2.Foo);
			Assert.IsFalse(Object.ReferenceEquals(depending1.Foo, depending2.Foo));
		}

		[TestMethod]
		public void NamedAndUnnamedObjectsInLocatorDontGetUsedForCreateNew()
		{
			MockBuilderContext context = CreateContext();
			object unnamed = new object();
			object named = new object();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), null), unnamed);
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), "Foo"), named);

			MockRequiresNewObject depending1 = (MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, null);
			MockRequiresNewObject depending2 = (MockRequiresNewObject)context.HeadOfChain.BuildUp(context, typeof(MockRequiresNewObject), null, null);

			Assert.IsFalse(depending1.Foo == unnamed);
			Assert.IsFalse(depending2.Foo == unnamed);
			Assert.IsFalse(depending1.Foo == named);
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
			MockBuilderContext context = CreateContext();

			MockDependingObject depending1 = (MockDependingObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingObject), null, null);
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
			MockBuilderContext context = CreateContext();

			MockDependingNamedObject depending1 = (MockDependingNamedObject)context.HeadOfChain.BuildUp(context, typeof(MockDependingNamedObject), null, null);
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

		private MockBuilderContext CreateContext(Locator locator)
		{
			MockBuilderContext result = new MockBuilderContext(locator);
			result.InnerChain.Add(new SingletonStrategy());
			result.InnerChain.Add(new ConstructorReflectionStrategy());
			result.InnerChain.Add(new CreationStrategy());
			result.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			result.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			return result;
		}

		public class SearchUpMockObject
		{
			public int Value;

			public SearchUpMockObject(
				[Dependency(SearchMode = SearchMode.Up, NotPresentBehavior = NotPresentBehavior.Throw)]
				int value)
			{
				this.Value = value;
			}
		}

		public class SearchLocalMockObject
		{
			public int Value;

			public SearchLocalMockObject(
				[Dependency(SearchMode = SearchMode.Local, NotPresentBehavior = NotPresentBehavior.Throw)]
				int value
				)
			{
				this.Value = value;
			}
		}

		public class ThrowingMockObject
		{
			[InjectionConstructor]
			public ThrowingMockObject([Dependency(NotPresentBehavior = NotPresentBehavior.Throw)] object foo)
			{
			}
		}

		public class NamedThrowingMockObject
		{
			[InjectionConstructor]
			public NamedThrowingMockObject([Dependency(Name = "Foo", NotPresentBehavior = NotPresentBehavior.Throw)] object foo)
			{
			}
		}

		public class MockDependingObject
		{
			private object injectedObject;

			public MockDependingObject([Dependency] object injectedObject)
			{
				this.injectedObject = injectedObject;
			}

			public virtual object InjectedObject
			{
				get { return injectedObject; }
				set { injectedObject = value; }
			}
		}

		public class MockOptionalDependingObject
		{
			private object injectedObject;

			public MockOptionalDependingObject
				(
					[Dependency(NotPresentBehavior = NotPresentBehavior.ReturnNull)] object injectedObject
				)
			{
				this.injectedObject = injectedObject;
			}

			public object InjectedObject
			{
				get { return injectedObject; }
				set { injectedObject = value; }
			}
		}

		public class MockOptionalDependingObjectWithName
		{
			private object injectedObject;

			public MockOptionalDependingObjectWithName
				(
					[Dependency(Name = "Foo", NotPresentBehavior = NotPresentBehavior.ReturnNull)] object injectedObject
				)
			{
				this.injectedObject = injectedObject;
			}

			public object InjectedObject
			{
				get { return injectedObject; }
				set { injectedObject = value; }
			}
		}

		public class MockDependingNamedObject
		{
			private object injectedObject;

			public MockDependingNamedObject([Dependency(Name = "Foo")] object injectedObject)
			{
				this.injectedObject = injectedObject;
			}

			public object InjectedObject
			{
				get { return injectedObject; }
				set { injectedObject = value; }
			}
		}

		public class MockDependsOnIFoo
		{
			private IFoo foo;

			public MockDependsOnIFoo([Dependency(CreateType = typeof(Foo))] IFoo foo)
			{
				this.foo = foo;
			}

			public IFoo Foo
			{
				get { return foo; }
				set { foo = value; }
			}
		}

		public class MockDependsOnNamedIFoo
		{
			private IFoo foo;

			public MockDependsOnNamedIFoo([Dependency(Name = "Foo", CreateType = typeof(Foo))] IFoo foo)
			{
				this.foo = foo;
			}

			public IFoo Foo
			{
				get { return foo; }
				set { foo = value; }
			}
		}

		public class MockRequiresNewObject
		{
			private object foo;

			public MockRequiresNewObject([CreateNew] object foo)
			{
				this.foo = foo;
			}

			public virtual object Foo
			{
				get { return foo; }
				set { foo = value; }
			}
		}

		public interface IFoo { }
		public class Foo : IFoo { }

		class MockInvalidDualParameterAttributes
		{
			[InjectionConstructor]
			public MockInvalidDualParameterAttributes([CreateNew][Dependency]object obj)
			{
			}
		}

		class MockInvalidDualConstructorAttributes
		{
			[InjectionConstructor]
			public MockInvalidDualConstructorAttributes(object obj)
			{
			}

			[InjectionConstructor]
			public MockInvalidDualConstructorAttributes(int i)
			{
			}
		}

		class MockUndecoratedObject
		{
			public object Foo;

			[InjectionConstructor]
			public MockUndecoratedObject(object foo)
			{
				Foo = foo;
			}
		}

		class MockUndecoratedConstructorObject
		{
			public object Foo;

			public MockUndecoratedConstructorObject(object foo)
			{
				Foo = foo;
			}
		}
	}
}
