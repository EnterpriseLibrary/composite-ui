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
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using System.Globalization;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Indicates that an invalid combination of dependency injection attributes were used.
	/// </summary>
	[Serializable]
	public class InvalidAttributeException : Exception
	{
		/// <summary>
		/// Initializes the exception.
		/// </summary>
		public InvalidAttributeException()
		{
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
		/// <param name="message">Error Message</param>
		public InvalidAttributeException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
		/// <param name="message">Error Message</param>
		/// <param name="exception">Inner Exception</param>
		public InvalidAttributeException(string message, Exception exception)
			: base(message, exception)
		{
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="memberName"></param>
		public InvalidAttributeException(Type type, string memberName)
			: base(String.Format(CultureInfo.CurrentCulture, Properties.Resources.InvalidAttributeCombination, type, memberName))
		{
		}

		/// <summary>
		/// Initializes the exception.
		/// </summary>
		protected InvalidAttributeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
