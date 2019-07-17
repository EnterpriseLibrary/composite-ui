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
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Encapsulates a property setter.
	/// </summary>
	public interface IPropertySetterInfo
	{
		/// <summary>
		/// Gets the value to be set into the property.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <param name="type">The type being built.</param>
		/// <param name="id">The ID being built.</param>
		/// <param name="propInfo">The property being set.</param>
		/// <returns>The value to be set into the property.</returns>
		object GetValue(IBuilderContext context, Type type, string id, PropertyInfo propInfo);

		/// <summary>
		/// Gets the property to be set.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <param name="type">The type being built.</param>
		/// <param name="id">The ID being built.</param>
		/// <returns>The property to be set; if the property cannot be found, returns null.</returns>
		PropertyInfo SelectProperty(IBuilderContext context, Type type, string id);
	}
}
