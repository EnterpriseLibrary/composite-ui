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

using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Represents a policy for <see cref="PropertySetterStrategy"/>. The properties are
	/// indexed by the name of the property.
	/// </summary>
	public interface IPropertySetterPolicy : IBuilderPolicy
	{
		/// <summary>
		/// The property values to be set.
		/// </summary>
		Dictionary<string, IPropertySetterInfo> Properties { get; }
	}
}