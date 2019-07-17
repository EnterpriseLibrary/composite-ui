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
	/// Implementation of <see cref="IParameter"/> which looks up the parameter value
	/// in the build context locator.
	/// </summary>
	public class LookupParameter : IParameter
	{
		private object key;

		/// <summary>
		/// Initializes a new instance of the <see cref="LookupParameter"/> class.
		/// </summary>
		/// <param name="key">The key to look the object up with.</param>
		public LookupParameter(object key)
		{
			this.key = key;
		}

		/// <summary>
		/// Implementation of <see cref="IParameter.GetParameterType"/>.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <returns>The parameter's type.</returns>
		public Type GetParameterType(IBuilderContext context)
		{
			return GetValue(context).GetType();
		}

		/// <summary>
		/// Implementation of <see cref="IParameter.GetValue"/>.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <returns>The parameter's value.</returns>
		public object GetValue(IBuilderContext context)
		{
			return context.Locator.Get(key);
		}
	}
}