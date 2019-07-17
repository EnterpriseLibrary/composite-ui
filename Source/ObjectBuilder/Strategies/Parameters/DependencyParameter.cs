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
	/// Parameter that performs value retrieval depending on dependency injection attributes.
	/// </summary>
	public class DependencyParameter : KnownTypeParameter
	{
		private string name;
		private Type createType;
		private NotPresentBehavior notPresentBehavior;
		private SearchMode searchMode;

		/// <summary>
		/// Initializes a new instance of the <see cref="DependencyParameter"/> class using the
		/// provided parameter type, name, creation type, not present behavior, and search mode.
		/// </summary>
		public DependencyParameter(Type parameterType, string name,
			 Type createType, NotPresentBehavior notPresentBehavior, SearchMode searchMode)
			: base(parameterType)
		{
			this.name = name;
			this.createType = createType;
			this.notPresentBehavior = notPresentBehavior;
			this.searchMode = searchMode;
		}

		/// <summary>
		/// See <see cref="IParameter.GetValue"/> for more information.
		/// </summary>
		public override object GetValue(IBuilderContext context)
		{
			return new DependencyResolver(context).Resolve(
				 base.type, createType, name, notPresentBehavior, searchMode);
		}
	}
}
