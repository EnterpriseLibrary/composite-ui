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
	public class BuilderConfigTests
	{
		private static string AssemblyName = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
		private static string FullNameISimpleObject = FullNameOf(typeof(ISimpleObject));
		private static string FullNameSimpleObject = FullNameOf(typeof(SimpleObject));
		private static string FullNameComplexObject = FullNameOf(typeof(ComplexObject));

		private static string FullNameOf(Type t)
		{
			return t.FullName + ", " + AssemblyName;
		}

		[TestMethod]
		public void CanCreateInstancesUsingConfigurationObject()
		{
			string config =
				@"<object-builder-config xmlns=""pag-object-builder"">
               <build-rules>
						<build-rule type=""" + FullNameSimpleObject + @""" mode=""Instance"">
							<constructor-params>
								<value-param type=""System.Int32"">12</value-param>
							</constructor-params>
						</build-rule>
					</build-rules>
				</object-builder-config>";

			Builder builder = new Builder(ObjectBuilderXmlConfig.FromXml(config));
			Locator locator = CreateLocator();

			SimpleObject m1 = builder.BuildUp<SimpleObject>(locator, null, null);
			SimpleObject m2 = builder.BuildUp<SimpleObject>(locator, null, null);

			Assert.IsNotNull(m1);
			Assert.IsNotNull(m2);
			Assert.AreEqual(12, m1.IntParam);
			Assert.AreEqual(12, m2.IntParam);
			Assert.IsTrue(m1 != m2);
		}

		[TestMethod]
		public void CanCreateInstances()
		{
			string config =
				@"<object-builder-config xmlns=""pag-object-builder"">
               <build-rules>
						<build-rule type=""" + FullNameSimpleObject + @""" mode=""Instance"">
							<constructor-params>
								<value-param type=""System.Int32"">12</value-param>
							</constructor-params>
						</build-rule>
					</build-rules>
				</object-builder-config>";

			Builder builder = new Builder(ObjectBuilderXmlConfig.FromXml(config));
			Locator locator = CreateLocator();

			SimpleObject m1 = builder.BuildUp<SimpleObject>(locator, null, null);
			SimpleObject m2 = builder.BuildUp<SimpleObject>(locator, null, null);

			Assert.IsNotNull(m1);
			Assert.IsNotNull(m2);
			Assert.AreEqual(12, m1.IntParam);
			Assert.AreEqual(12, m2.IntParam);
			Assert.IsTrue(m1 != m2);
		}

		[TestMethod]
		public void CanCreateSingleton()
		{
			string config =
				@"<object-builder-config xmlns=""pag-object-builder"">
               <build-rules>
						<build-rule type=""" + FullNameSimpleObject + @""" mode=""Singleton"">
							<constructor-params>
								<value-param type=""System.Int32"">12</value-param>
							</constructor-params>
						</build-rule>
					</build-rules>
				</object-builder-config>";

			Builder builder = new Builder(ObjectBuilderXmlConfig.FromXml(config));
			Locator locator = CreateLocator();

			SimpleObject m1 = builder.BuildUp<SimpleObject>(locator, null, null);
			SimpleObject m2 = builder.BuildUp<SimpleObject>(locator, null, null);

			Assert.AreSame(m1, m2);
		}

		[TestMethod]
		public void CreateComplexObject()
		{
			string config =
				@"<object-builder-config xmlns=""pag-object-builder"">
               <build-rules>
						<build-rule type=""" + FullNameSimpleObject + @""" mode=""Singleton"">
							<constructor-params>
								<value-param type=""System.Int32"">12</value-param>
							</constructor-params>
						</build-rule>
						<build-rule type=""" + FullNameComplexObject + @""" mode=""Singleton"">
							<constructor-params>
								<ref-param type=""" + FullNameSimpleObject + @""" />
							</constructor-params>
						</build-rule>
					</build-rules>
				</object-builder-config>";

			Builder builder = new Builder(ObjectBuilderXmlConfig.FromXml(config));
			Locator locator = CreateLocator();

			ComplexObject cm = builder.BuildUp<ComplexObject>(locator, null, null);
			SimpleObject m = builder.BuildUp<SimpleObject>(locator, null, null);

			Assert.AreSame(m, cm.SimpleObject);
			Assert.IsNotNull(cm);
			Assert.IsNotNull(cm.SimpleObject);
			Assert.AreEqual(12, cm.SimpleObject.IntParam);
		}

		[TestMethod]
		public void CanCreateNamedInstance()
		{
			string config =
				@"<object-builder-config xmlns=""pag-object-builder"">
               <build-rules>
						<build-rule name=""Object1"" type=""" + FullNameSimpleObject + @""" mode=""Instance"">
							<constructor-params>
								<value-param type=""System.Int32"">12</value-param>
							</constructor-params>
						</build-rule>
						<build-rule name=""Object2"" type=""" + FullNameSimpleObject + @""" mode=""Instance"">
							<constructor-params>
								<value-param type=""System.Int32"">32</value-param>
							</constructor-params>
						</build-rule>
					</build-rules>
				</object-builder-config>";

			Builder builder = new Builder(ObjectBuilderXmlConfig.FromXml(config));
			Locator locator = CreateLocator();

			SimpleObject m1 = builder.BuildUp<SimpleObject>(locator, "Object1", null);
			SimpleObject m2 = builder.BuildUp<SimpleObject>(locator, "Object2", null);

			Assert.IsNotNull(m1);
			Assert.IsNotNull(m2);
			Assert.AreEqual(12, m1.IntParam);
			Assert.AreEqual(32, m2.IntParam);
			Assert.IsTrue(m1 != m2);
		}

		[TestMethod]
		public void RefParamsCanAskForSpecificallyNamedObjects()
		{
			string config =
				@"<object-builder-config xmlns=""pag-object-builder"">
               <build-rules>
						<build-rule name=""Object1"" type=""" + FullNameSimpleObject + @""" mode=""Singleton"">
							<constructor-params>
								<value-param type=""System.Int32"">12</value-param>
							</constructor-params>
						</build-rule>
						<build-rule name=""Object2"" type=""" + FullNameSimpleObject + @""" mode=""Singleton"">
							<constructor-params>
								<value-param type=""System.Int32"">32</value-param>
							</constructor-params>
						</build-rule>
						<build-rule type=""" + FullNameComplexObject + @""" mode=""Singleton"">
							<constructor-params>
								<ref-param type=""" + FullNameSimpleObject + @""" name=""Object2"" />
							</constructor-params>
						</build-rule>
					</build-rules>
				</object-builder-config>";

			Builder builder = new Builder(ObjectBuilderXmlConfig.FromXml(config));
			Locator locator = CreateLocator();

			ComplexObject cm = builder.BuildUp<ComplexObject>(locator, null, null);
			SimpleObject sm = builder.BuildUp<SimpleObject>(locator, "Object2", null);

			Assert.IsNotNull(cm);
			Assert.IsNotNull(cm.SimpleObject);
			Assert.AreEqual(32, cm.SimpleObject.IntParam);
			Assert.AreSame(sm, cm.SimpleObject);
		}

		[TestMethod]
		public void CanInjectValuesIntoProperties()
		{
			string config =
				@"<object-builder-config xmlns=""pag-object-builder"">
               <build-rules>
						<build-rule type=""" + FullNameSimpleObject + @""" mode=""Instance"">
							<property name=""StringProperty"">
								<value-param type=""System.String"">Bar is here</value-param>
							</property>
						</build-rule>
					</build-rules>
				</object-builder-config>";

			Builder builder = new Builder(ObjectBuilderXmlConfig.FromXml(config));
			Locator locator = CreateLocator();

			SimpleObject sm = builder.BuildUp<SimpleObject>(locator, null, null);

			Assert.IsNotNull(sm);
			Assert.AreEqual("Bar is here", sm.StringProperty);
		}

		[TestMethod]
		public void CanInjectMultiplePropertiesIncludingCreatedObjects()
		{
			string config =
				@"<object-builder-config xmlns=""pag-object-builder"">
               <build-rules>
						<build-rule type=""" + FullNameSimpleObject + @""" mode=""Instance"">
							<constructor-params>
								<value-param type=""System.Int32"">15</value-param>
							</constructor-params>
						</build-rule>
						<build-rule type=""" + FullNameComplexObject + @""" mode=""Instance"">
							<property name=""StringProperty"">
								<value-param type=""System.String"">Bar is here</value-param>
							</property>
							<property name=""SimpleObject"">
								<ref-param type=""" + FullNameSimpleObject + @""" />
							</property>
						</build-rule>
					</build-rules>
				</object-builder-config>";

			Builder builder = new Builder(ObjectBuilderXmlConfig.FromXml(config));
			Locator locator = CreateLocator();

			ComplexObject co = builder.BuildUp<ComplexObject>(locator, null, null);

			Assert.IsNotNull(co);
			Assert.IsNotNull(co.SimpleObject);
			Assert.AreEqual("Bar is here", co.StringProperty);
			Assert.AreEqual(15, co.SimpleObject.IntParam);
		}

		[TestMethod]
		public void CanCreateConcreteObjectByAskingForInterface()
		{
			string config =
				@"<object-builder-config xmlns=""pag-object-builder"">
               <build-rules>
						<build-rule type=""" + FullNameISimpleObject + @""" mode=""Instance"">
							<mapped-type type=""" + FullNameSimpleObject + @""" />
						</build-rule>
					</build-rules>
				</object-builder-config>";

			Builder builder = new Builder(ObjectBuilderXmlConfig.FromXml(config));
			Locator locator = CreateLocator();

			ISimpleObject sm = builder.BuildUp<ISimpleObject>(locator, null, null);

			Assert.IsNotNull(sm);
			Assert.IsTrue(sm is SimpleObject);
		}

		[TestMethod]
		public void CanCreateNamedConcreteObjectByAskingForNamedInterface()
		{
			string config =
				@"<object-builder-config xmlns=""pag-object-builder"">
               <build-rules>
						<build-rule type=""" + FullNameSimpleObject + @""" name=""Foo"" mode=""Instance"">
							<constructor-params>
								<value-param type=""System.Int32"">12</value-param>
							</constructor-params>
						</build-rule>
						<build-rule name=""sm2"" type=""" + FullNameISimpleObject + @""" mode=""Instance"">
							<mapped-type type=""" + FullNameSimpleObject + @""" name=""Foo""/>
						</build-rule>
						<build-rule type=""" + FullNameISimpleObject + @""" mode=""Instance"">
							<mapped-type type=""" + FullNameSimpleObject + @"""/>
						</build-rule>
					</build-rules>
				</object-builder-config>";

			Builder builder = new Builder(ObjectBuilderXmlConfig.FromXml(config));
			Locator locator = CreateLocator();

			ISimpleObject sm1 = builder.BuildUp<ISimpleObject>(locator, null, null);
			ISimpleObject sm2 = builder.BuildUp<ISimpleObject>(locator, "sm2", null);

			Assert.IsNotNull(sm1);
			Assert.IsNotNull(sm2);
			Assert.IsTrue(sm1 is SimpleObject);
			Assert.IsTrue(sm2 is SimpleObject);
			Assert.AreEqual(0, ((SimpleObject)sm1).IntParam);
			Assert.AreEqual(12, ((SimpleObject)sm2).IntParam);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void BuilderWithNoStrategiesThrowsWhenBuilding()
		{
			string config =
				@"<object-builder-config xmlns=""pag-object-builder"">
					<strategies include-default=""false"" />
				</object-builder-config>";

			Builder builder = new Builder(ObjectBuilderXmlConfig.FromXml(config));
			Locator locator = CreateLocator();

			builder.BuildUp<object>(locator, null, null);
		}

		private Locator CreateLocator()
		{
			Locator locator = new Locator();
			LifetimeContainer lifetime = new LifetimeContainer();
			locator.Add(typeof(ILifetimeContainer), lifetime);
			return locator;
		}

		// TODO
		// - Is type mapping exclusive (no other rules)?
		// - Duplicate / conflicting build rules

		public interface ISimpleObject
		{
		}

		public class SimpleObject : ISimpleObject
		{
			public int IntParam;
			private string stringProperty;

			public string StringProperty
			{
				get { return stringProperty; }
				set { stringProperty = value; }
			}

			public SimpleObject(int foo)
			{
				IntParam = foo;
			}
		}

		public class ComplexObject
		{
			private SimpleObject simpleObject;
			private string stringProperty;

			public SimpleObject SimpleObject
			{
				get { return simpleObject; }
				set { simpleObject = value; }
			}

			public string StringProperty
			{
				get { return stringProperty; }
				set { stringProperty = value; }
			}

			public ComplexObject(SimpleObject monk)
			{
				SimpleObject = monk;
			}
		}
	}
}
