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
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// Implementation of <see cref="IBuilderStrategy"/> which creates objects.
	/// </summary>
	/// <remarks>
	/// <para>This strategy looks for policies in the context registered under the interface type
	/// <see cref="ICreationPolicy"/>. If it cannot find a policy on how to create the object,
	/// it will select the first constructor that returns from reflection, and re-runs the chain
	/// to create all the objects required to fulfill the constructor's parameters.</para>
	/// <para>If the Build method is passed an object via the existing parameter, then it
	/// will do nothing (since the object already exists). This allows this strategy to be
	/// in the chain when running dependency injection on existing objects, without fear that
	/// it will attempt to re-create the object.</para>
	/// </remarks>
	public class CreationStrategy : BuilderStrategy
	{
		/// <summary>
		/// Override of <see cref="IBuilderStrategy.BuildUp"/>. Creates the requested object.
		/// </summary>
		/// <param name="context">The build context.</param>
		/// <param name="typeToBuild">The type of object to be created.</param>
		/// <param name="existing">The existing object. If not null, this strategy does nothing.</param>
		/// <param name="idToBuild">The ID of the object to be created.</param>
		/// <returns>The created object</returns>
		public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			if (existing != null)
				BuildUpExistingObject(context, typeToBuild, existing, idToBuild);
			else
				existing = BuildUpNewObject(context, typeToBuild, existing, idToBuild);

			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}

		private void BuildUpExistingObject(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			RegisterObject(context, typeToBuild, existing, idToBuild);
		}

		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		private object BuildUpNewObject(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			ICreationPolicy policy = context.Policies.Get<ICreationPolicy>(typeToBuild, idToBuild);

			if (policy == null)
			{
				if (idToBuild == null)
					throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
						Properties.Resources.MissingPolicyUnnamed, typeToBuild));
				else
					throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
						Properties.Resources.MissingPolicyNamed, typeToBuild, idToBuild));
			}

			try
			{
				existing = FormatterServices.GetSafeUninitializedObject(typeToBuild);
			}
			catch (MemberAccessException exception)
			{
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Properties.Resources.CannotCreateInstanceOfType, typeToBuild), exception);
			}

			RegisterObject(context, typeToBuild, existing, idToBuild);
			InitializeObject(context, existing, idToBuild, policy);
			return existing;
		}

		private void RegisterObject(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			if (context.Locator != null)
			{
				ILifetimeContainer lifetime = context.Locator.Get<ILifetimeContainer>(typeof(ILifetimeContainer), SearchMode.Local);

				if (lifetime != null)
				{
					ISingletonPolicy singletonPolicy = context.Policies.Get<ISingletonPolicy>(typeToBuild, idToBuild);

					if (singletonPolicy != null && singletonPolicy.IsSingleton)
					{
						context.Locator.Add(new DependencyResolutionLocatorKey(typeToBuild, idToBuild), existing);
						lifetime.Add(existing);

						if (TraceEnabled(context))
							TraceBuildUp(context, typeToBuild, idToBuild, Properties.Resources.SingletonRegistered);
					}
				}
			}
		}

		private void InitializeObject(IBuilderContext context, object existing, string id, ICreationPolicy policy)
		{
			Type type = existing.GetType();
			ConstructorInfo constructor = policy.SelectConstructor(context, type, id);

			if (constructor == null)
			{
				if (type.IsValueType)
					return;
				throw new ArgumentException(Properties.Resources.NoAppropriateConstructor);
			}

			object[] parms = policy.GetParameters(context, type, id, constructor);

			MethodBase method = (MethodBase)constructor;
			Guard.ValidateMethodParameters(method, parms, existing.GetType());

			if (TraceEnabled(context))
				TraceBuildUp(context, type, id, Properties.Resources.CallingConstructor, ParametersToTypeList(parms));

			method.Invoke(existing, parms);
		}
	
		private void ValidateCtorParameters(MethodBase methodInfo, object[] parameters, Type typeBeingBuilt)
		{
			ParameterInfo[] paramInfos = methodInfo.GetParameters();
			for (int i = 0; i < paramInfos.Length; i++)
			{
				Guard.TypeIsAssignableFromType(paramInfos[i].ParameterType, parameters[i].GetType(), typeBeingBuilt);
			}
		}
	}
}