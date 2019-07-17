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
using System.Reflection;
using System.Globalization;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Implementation of <see cref="IBuilderStrategy"/> which sets property values.
	/// </summary>
	/// <remarks>
	/// This strategy looks for policies in the context registered under the interface type
	/// <see cref="IPropertySetterPolicy"/>, and sets the property values. If no policy is
	/// found, the no property values are set.
	/// </remarks>
	public class PropertySetterStrategy : BuilderStrategy
	{
		/// <summary>
		/// Implementation of <see cref="IBuilderStrategy.BuildUp"/>. Sets the property values.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <param name="typeToBuild">The type being built.</param>
		/// <param name="existing">The object on which to inject property values.</param>
		/// <param name="idToBuild">The ID of the object being built.</param>
		/// <returns>The built object.</returns>
		public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			if (existing != null)
				InjectProperties(context, existing, idToBuild);

			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}

		private void InjectProperties(IBuilderContext context, object obj, string id)
		{
			if (obj == null)
				return;

			Type type = obj.GetType();
			IPropertySetterPolicy policy = context.Policies.Get<IPropertySetterPolicy>(type, id);

			if (policy == null)
				return;

			foreach (IPropertySetterInfo propSetterInfo in policy.Properties.Values)
			{
				PropertyInfo propInfo = propSetterInfo.SelectProperty(context, type, id);

				if (propInfo != null)
				{
					if (propInfo.CanWrite)
					{
						object value = propSetterInfo.GetValue(context, type, id, propInfo);

						if( value != null )
							Guard.TypeIsAssignableFromType(propInfo.PropertyType, value.GetType(), obj.GetType());

						if (TraceEnabled(context))
							TraceBuildUp(context, type, id, Properties.Resources.CallingProperty, propInfo.Name, propInfo.PropertyType.Name);

						propInfo.SetValue(obj, value, null);
					}
					else
					{
						throw new ArgumentException(String.Format(
							CultureInfo.CurrentCulture,
							Properties.Resources.CannotInjectReadOnlyProperty,
							type, propInfo.Name));
					}
				}
			}
		}
	}
}