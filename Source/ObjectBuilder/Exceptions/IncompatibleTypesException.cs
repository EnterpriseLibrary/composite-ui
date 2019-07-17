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
using System.Runtime.Serialization;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Indicates that a dependency could not be resolved because the resolved type is
	/// not compatible with the injected type.
	/// </summary>
	[Serializable]
	public class IncompatibleTypesException : Exception
	{
		/// <summary>
		/// Initializes the exception.
		/// </summary>
		public IncompatibleTypesException()
		{
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
		/// <param name="message">Error Message</param>
		public IncompatibleTypesException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
		/// <param name="message">Error Message</param>
		/// <param name="exception">Inner Exception</param>
		public IncompatibleTypesException(string message, Exception exception)
			: base(message, exception)
		{
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
		protected IncompatibleTypesException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
