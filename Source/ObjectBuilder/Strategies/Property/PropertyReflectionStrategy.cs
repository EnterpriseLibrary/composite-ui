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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Strategy that describes property injection via attributes.
	/// </summary>
	/// <remarks>
	/// This strategy processes any injection attribute that inherits from <see cref="ParameterAttribute"/>, 
	/// thus providing a generic strategy that easily allows for extension through new injection attributes and 
	/// their corresponding <see cref="IParameter"/> implementations (if necessary) to retrieve values.
	/// </remarks>
	public class PropertyReflectionStrategy : ReflectionStrategy<PropertyInfo>
	{
		/// <summary>
		/// See <see cref="ReflectionStrategy{T}.GetMembers"/> for more information.
		/// </summary>
		protected override IEnumerable<IReflectionMemberInfo<PropertyInfo>> GetMembers(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			foreach (PropertyInfo propInfo in typeToBuild.GetProperties())
				yield return new PropertyReflectionMemberInfo(propInfo);
		}

		/// <summary>
		/// See <see cref="ReflectionStrategy{T}.AddParametersToPolicy"/> for more information.
		/// </summary>
		protected override void AddParametersToPolicy(IBuilderContext context, Type typeToBuild, string idToBuild, IReflectionMemberInfo<PropertyInfo> member, IEnumerable<IParameter> parameters)
		{
			PropertySetterPolicy result = context.Policies.Get<IPropertySetterPolicy>(typeToBuild, idToBuild) as PropertySetterPolicy;

			if (result == null)
			{
				result = new PropertySetterPolicy();
				context.Policies.Set<IPropertySetterPolicy>(result, typeToBuild, idToBuild);
			}

			foreach (IParameter parameter in parameters)
				if (!result.Properties.ContainsKey(member.Name))
					result.Properties.Add(member.Name, new PropertySetterInfo(member.MemberInfo, parameter));
		}

		/// <summary>
		/// See <see cref="ReflectionStrategy{T}.MemberRequiresProcessing"/> for more information.
		/// </summary>
		protected override bool MemberRequiresProcessing(IReflectionMemberInfo<PropertyInfo> member)
		{
			return (member.GetCustomAttributes(typeof(ParameterAttribute), true).Length > 0);
		}

		private class PropertyReflectionMemberInfo : IReflectionMemberInfo<PropertyInfo>
		{
			PropertyInfo prop;

			public PropertyReflectionMemberInfo(PropertyInfo prop)
			{
				this.prop = prop;
			}

			public PropertyInfo MemberInfo
			{
				get { return prop; }
			}

			public string Name
			{
				get { return prop.Name; }
			}

			public object[] GetCustomAttributes(Type attributeType, bool inherit)
			{
				return prop.GetCustomAttributes(attributeType, inherit);
			}

			public ParameterInfo[] GetParameters()
			{
				return new ParameterInfo[] { new CustomPropertyParameterInfo(prop) };
			}
		}

		private class CustomPropertyParameterInfo : ParameterInfo
		{
			PropertyInfo prop;

			public CustomPropertyParameterInfo(PropertyInfo prop)
			{
				this.prop = prop;
			}

			public override object[] GetCustomAttributes(Type attributeType, bool inherit)
			{
				return prop.GetCustomAttributes(attributeType, inherit);
			}

			public override Type ParameterType
			{
				get { return prop.PropertyType; }
			}
		}
	}
}
