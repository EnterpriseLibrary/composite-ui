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
	/// An implementation helper class for <see cref="IParameter"/> which can be used
	/// when you know the type of the parameter value ahead of time.
	/// </summary>
	public abstract class KnownTypeParameter : IParameter
	{
		/// <summary>
		/// The parmeter type.
		/// </summary>
		protected Type type;

		/// <summary>
		/// Initializes a new instance of the <see cref="KnownTypeParameter"/> class
		/// using the given type.
		/// </summary>
		/// <param name="type">The parameter type.</param>
		protected KnownTypeParameter(Type type)
		{
			this.type = type;
		}

		/// <summary>
		/// See <see cref="IParameter.GetParameterType"/> for more information.
		/// </summary>
		public Type GetParameterType(IBuilderContext context)
		{
			return type;
		}

		/// <summary>
		/// Abstract for <see cref="IParameter.GetValue"/>. Derived classes are required
		/// to provide the implemenation for providing the value itself.
		/// </summary>
		/// <param name="context">The builder context.</param>
		/// <returns>The parameter value.</returns>
		public abstract object GetValue(IBuilderContext context);
	}
}