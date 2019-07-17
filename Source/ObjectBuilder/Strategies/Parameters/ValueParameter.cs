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
	/// Implementation of <see cref="IParameter"/> which directly holds a value to
	/// be used for the parameter.
	/// </summary>
	/// <typeparam name="TValue">The type of the parameter value.</typeparam>
	public class ValueParameter<TValue> : KnownTypeParameter
	{
		private TValue value;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueParameter{T}"/> class.
		/// </summary>
		/// <param name="value">The value for the parameter.</param>
		public ValueParameter(TValue value)
			: base(typeof(TValue))
		{
			this.value = value;
		}

		/// <summary>
		/// Implementation of <see cref="IParameter.GetValue"/>.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <returns>The parameter's value.</returns>
		public override object GetValue(IBuilderContext context)
		{
			return value;
		}
	}

	/// <summary>
	/// Implementation of <see cref="IParameter"/> which directly holds a value to
	/// be used for the parameter.
	/// </summary>
	public class ValueParameter : KnownTypeParameter
	{
		private object value;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueParameter"/> class.
		/// </summary>
		/// <param name="valueType">The type of the parameter value.</param>
		/// <param name="value">The value for the parameter.</param>
		public ValueParameter(Type valueType, object value)
			: base(valueType)
		{
			this.value = value;
		}

		/// <summary>
		/// Implementation of <see cref="IParameter.GetValue"/>.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <returns>The parameter's value.</returns>
		public override object GetValue(IBuilderContext context)
		{
			return value;
		}
	}
}