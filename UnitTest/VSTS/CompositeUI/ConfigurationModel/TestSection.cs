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

using System;
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.CompositeUI.Configuration;

namespace Microsoft.Practices.CompositeUI.Tests.ConfigurationModel
{
	public class TestSection : ConfigurationSection
	{
		[ConfigurationProperty("services", IsRequired=true)]
		public TestServiceCollection Services
		{
			get { return (TestServiceCollection) this["services"]; }
		}

		[ConfigurationProperty("element", IsRequired = true)]
		public ElementWithAttributes Element
		{
			get { return (ElementWithAttributes) this["element"]; }
		}
	}

	public class ElementWithAttributes : ParametersElement
	{
		[ConfigurationProperty("name")]
		public string Name
		{
			get { return (string) this["name"]; }
		}

		[ConfigurationProperty("value")]
		public int Value
		{
			get { return (int) this["value"]; }
		}
	}

	[ConfigurationCollection(typeof (TestServiceElement))]
	public class TestServiceCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new TestServiceElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((TestServiceElement) element).ServiceType;
		}

		public TestServiceElement this[Type serviceType]
		{
			get { return (TestServiceElement) BaseGet(serviceType); }
			set
			{
				if (BaseGet(serviceType) != null)
				{
					BaseRemove(serviceType);
				}
				BaseAdd(value);
			}
		}

		public TestServiceElement this[int index]
		{
			get { return (TestServiceElement) BaseGet(index); }
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}
	}

	public class TestServiceElement : ConfigurationElement
	{
		[ConfigurationProperty("serviceType", IsKey=true, IsRequired=true)]
		[TypeConverter(typeof (TypeNameConverter))]
		public Type ServiceType
		{
			get { return (Type) this["serviceType"]; }
		}

		[ConfigurationProperty("instanceType", IsRequired = true)]
		[TypeConverter(typeof (TypeNameConverter))]
		public Type InstanceType
		{
			get { return (Type) this["instanceType"]; }
		}
	}
}