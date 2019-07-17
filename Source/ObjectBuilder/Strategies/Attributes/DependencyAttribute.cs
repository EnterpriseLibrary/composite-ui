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
	/// Attribute applied to properties and constructor parameters, to describe when the dependency
	/// injection system should inject an object.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public sealed class DependencyAttribute : ParameterAttribute
	{
		private string name;
		private Type createType;
		private NotPresentBehavior notPresentBehavior = NotPresentBehavior.CreateNew;
		private SearchMode searchMode;

		/// <summary>
		/// Initializes a new instance of the <see cref="DependencyAttribute"/> class.
		/// </summary>
		public DependencyAttribute()
		{
		}

		/// <summary>
		/// The name of the object to inject. Optional.
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		/// <summary>
		/// The type to be created, when <see cref="DependencyAttribute.NotPresentBehavior"/> is set
		/// to <see cref="Microsoft.Practices.ObjectBuilder.NotPresentBehavior.CreateNew"/>
		/// and an existing object cannot be found. Optional.
		/// </summary>
		public Type CreateType
		{
			get { return createType; }
			set { createType = value; }
		}

		/// <summary>
		/// Specifies how the dependency will be searched in the locator.
		/// </summary>
		public SearchMode SearchMode
		{
			get { return searchMode; }
			set { searchMode = value; }
		}


		/// <summary>
		/// The behavior when the object isn't found. Defaults to 
		/// <see cref="Microsoft.Practices.ObjectBuilder.NotPresentBehavior.CreateNew"/>.
		/// </summary>
		public NotPresentBehavior NotPresentBehavior
		{
			get { return notPresentBehavior; }
			set { notPresentBehavior = value; }
		}

		/// <summary>
		/// See <see cref="ParameterAttribute.CreateParameter"/> for more information.
		/// </summary>
		public override IParameter CreateParameter(Type annotatedMemberType)
		{
			return new DependencyParameter(annotatedMemberType, name, createType, notPresentBehavior, searchMode);
		}
	}
}
