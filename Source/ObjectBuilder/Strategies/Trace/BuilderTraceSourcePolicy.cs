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
using System.Diagnostics;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// An implementation of <see cref="IBuilderTracePolicy"/> which logs the trace messages
	/// through a <see cref="TraceSource"/>.
	/// </summary>
	public class BuilderTraceSourcePolicy : IBuilderTracePolicy
	{
		TraceSource traceSource;

		/// <summary>
		/// Initializes a new instance of the <see cref="BuilderTraceSourcePolicy"/> class
		/// using the provided trace source.
		/// </summary>
		public BuilderTraceSourcePolicy(TraceSource traceSource)
		{
			this.traceSource = traceSource;
		}

		/// <summary>
		/// See <see cref="IBuilderTracePolicy.Trace"/> for more information.
		/// </summary>
		public void Trace(string format, params object[] args)
		{
			traceSource.TraceInformation(format, args);
		}
	}
}
