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
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Implementation of <see cref="IReflectionMemberInfo{T}"/> suitable for direct calls
	/// to the <see cref="MethodBase"/> class.
	/// </summary>
	/// <typeparam name="TMemberInfo"></typeparam>
	public class ReflectionMemberInfo<TMemberInfo> : IReflectionMemberInfo<TMemberInfo>
		where TMemberInfo : MethodBase
	{
		private TMemberInfo memberInfo;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReflectionMemberInfo{T}"/> class.
		/// </summary>
		/// <param name="memberInfo">The member used to satisfy the interface calls.</param>
		public ReflectionMemberInfo(TMemberInfo memberInfo)
		{
			this.memberInfo = memberInfo;
		}

		/// <summary>
		/// See <see cref="IReflectionMemberInfo{T}.MemberInfo"/> for more information.
		/// </summary>
		public TMemberInfo MemberInfo
		{
			get { return memberInfo; }
		}

		/// <summary>
		/// See <see cref="IReflectionMemberInfo{T}.Name"/> for more information.
		/// </summary>
		public string Name
		{
			get { return memberInfo.Name; }
		}

		/// <summary>
		/// See <see cref="IReflectionMemberInfo{T}.GetCustomAttributes"/> for more information.
		/// </summary>
		public object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return memberInfo.GetCustomAttributes(attributeType, inherit);
		}

		/// <summary>
		/// See <see cref="IReflectionMemberInfo{T}.GetParameters"/> for more information.
		/// </summary>
		public ParameterInfo[] GetParameters()
		{
			return memberInfo.GetParameters();
		}
	}
}
