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

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Represents an object that can configure a builder.
	/// </summary>
	/// <typeparam name="TStageEnum">The builder's stage enumeration</typeparam>
	public interface IBuilderConfigurator<TStageEnum>
	{
		/// <summary>
		/// Applies the configuration to the builder.
		/// </summary>
		/// <param name="builder">The builder to apply the configuration to.</param>
		void ApplyConfiguration(IBuilder<TStageEnum> builder);
	}
}