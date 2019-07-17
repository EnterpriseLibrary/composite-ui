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
	/// Implementation of <see cref="IParameter"/> which clones the provided value through
	/// ICloneable. If the value does not implement ICloneable, then the original value
	/// is returned instead.
	/// </summary>
	public class CloneParameter : IParameter
	{
		private IParameter param;

		/// <summary>
		/// Initializes a new instance of the <see cref="CloneParameter"/> class using the
		/// provided parameter to be cloned.
		/// </summary>
		/// <param name="param">The parameter to be cloned.</param>
		public CloneParameter(IParameter param)
		{
			this.param = param;
		}

		/// <summary>
		/// See <see cref="IParameter.GetParameterType"/> for more information.
		/// </summary>
		public Type GetParameterType(IBuilderContext context)
		{
			return param.GetParameterType(context);
		}

		/// <summary>
		/// See <see cref="IParameter.GetValue"/> for more information.
		/// </summary>
		public object GetValue(IBuilderContext context)
		{
			object val = param.GetValue(context);

			if (val is ICloneable)
				val = ((ICloneable)val).Clone();

			return val;
		}
	}
}