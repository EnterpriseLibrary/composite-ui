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

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Represents a single parameter used for constructor and method calls, and
	/// property setting.
	/// </summary>
	public interface IParameter
	{
		/// <summary>
		/// Gets the type of the parameter value.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <returns>The parameter's type.</returns>
		Type GetParameterType(IBuilderContext context);

		/// <summary>
		/// Gets the parameter value.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <returns>The parameter's value.</returns>
		object GetValue(IBuilderContext context);
	}
}