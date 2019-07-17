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
	/// Represents a tracing policy for ObjectBuilder.
	/// </summary>
	public interface IBuilderTracePolicy : IBuilderPolicy
	{
		/// <summary>
		/// Trace a message.
		/// </summary>
		/// <param name="format">Message format.</param>
		/// <param name="args">Message arguments.</param>
		void Trace(string format, params object[] args);
	}
}
