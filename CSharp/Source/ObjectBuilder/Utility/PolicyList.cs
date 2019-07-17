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

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// A custom collection wrapper over <see cref="IBuilderPolicy"/> objects.
	/// </summary>
	public class PolicyList
	{
		private Dictionary<BuilderPolicyKey, IBuilderPolicy> policies = new Dictionary<BuilderPolicyKey, IBuilderPolicy>();
		private object lockObject = new object();

		/// <summary>
		/// Initializes a new instance of the <see cref="PolicyList"/> class using the
		/// provided (optional) policies to copy.
		/// </summary>
		/// <param name="policiesToCopy">The policies to be copied into the policy list.</param>
		public PolicyList(params PolicyList[] policiesToCopy)
		{
			if (policiesToCopy != null)
				foreach (PolicyList policyList in policiesToCopy)
					AddPolicies(policyList);
		}

		/// <summary>
		/// Returns the number of policies in the list.
		/// </summary>
		public int Count
		{
			get
			{
				lock (lockObject)
				{
					return policies.Count;
				}
			}
		}

		/// <summary>
		/// Adds a bundle of policies into the policy list. Any policies in this list will override
		/// policies that are already in the policy list.
		/// </summary>
		/// <param name="policiesToCopy">The policies to be copied.</param>
		public void AddPolicies(PolicyList policiesToCopy)
		{
			lock (lockObject)
			{
				if (policiesToCopy != null)
					foreach (KeyValuePair<BuilderPolicyKey, IBuilderPolicy> kvp in policiesToCopy.policies)
						policies[kvp.Key] = kvp.Value;
			}
		}

		/// <summary>
		/// Removes an individual policy.
		/// </summary>
		/// <typeparam name="TPolicyInterface">The type the policy was registered as.</typeparam>
		/// <param name="typePolicyAppliesTo">The type the policy applies to.</param>
		/// <param name="idPolicyAppliesTo">The ID the policy applies to.</param>
		public void Clear<TPolicyInterface>(Type typePolicyAppliesTo, string idPolicyAppliesTo)
		{
			Clear(typeof(TPolicyInterface), typePolicyAppliesTo, idPolicyAppliesTo);
		}

		/// <summary>
		/// Removes an individual policy.
		/// </summary>
		/// <param name="policyInterface">The type the policy was registered as.</param>
		/// <param name="typePolicyAppliesTo">The type the policy applies to.</param>
		/// <param name="idPolicyAppliesTo">The ID the policy applies to.</param>
		public void Clear(Type policyInterface, Type typePolicyAppliesTo, string idPolicyAppliesTo)
		{
			lock (lockObject)
			{
				policies.Remove(new BuilderPolicyKey(policyInterface, typePolicyAppliesTo, idPolicyAppliesTo));
			}
		}

		/// <summary>
		/// Removes all policies from the list.
		/// </summary>
		public void ClearAll()
		{
			lock (lockObject)
			{
				policies.Clear();
			}
		}

		/// <summary>
		/// Removes a default policy.
		/// </summary>
		/// <typeparam name="TPolicyInterface">The type the policy was registered as.</typeparam>
		public void ClearDefault<TPolicyInterface>()
		{
			ClearDefault(typeof(TPolicyInterface));
		}

		/// <summary>
		/// Removes a default policy.
		/// </summary>
		/// <param name="policyInterface">The type the policy was registered as.</param>
		public void ClearDefault(Type policyInterface)
		{
			Clear(policyInterface, null, null);
		}

		/// <summary>
		/// Gets an individual policy.
		/// </summary>
		/// <typeparam name="TPolicyInterface">The interface the policy is registered under.</typeparam>
		/// <param name="typePolicyAppliesTo">The type the policy applies to.</param>
		/// <param name="idPolicyAppliesTo">The ID the policy applies to.</param>
		/// <returns>The policy in the list, if present; returns null otherwise.</returns>
		public TPolicyInterface Get<TPolicyInterface>(Type typePolicyAppliesTo, string idPolicyAppliesTo)
			where TPolicyInterface : IBuilderPolicy
		{
			return (TPolicyInterface)Get(typeof(TPolicyInterface), typePolicyAppliesTo, idPolicyAppliesTo);
		}

		/// <summary>
		/// Gets an individual policy.
		/// </summary>
		/// <param name="policyInterface">The interface the policy is registered under.</param>
		/// <param name="typePolicyAppliesTo">The type the policy applies to.</param>
		/// <param name="idPolicyAppliesTo">The ID the policy applies to.</param>
		/// <returns>The policy in the list, if present; returns null otherwise.</returns>
		public IBuilderPolicy Get(Type policyInterface, Type typePolicyAppliesTo, string idPolicyAppliesTo)
		{
			BuilderPolicyKey key = new BuilderPolicyKey(policyInterface, typePolicyAppliesTo, idPolicyAppliesTo);
			lock (lockObject)
			{
				IBuilderPolicy policy;

				if (policies.TryGetValue(key, out policy))
					return policy;

				BuilderPolicyKey defaultKey = new BuilderPolicyKey(policyInterface, null, null);
				if (policies.TryGetValue(defaultKey, out policy))
					return policy;

				return null;
			}
		}

		/// <summary>
		/// Sets an individual policy.
		/// </summary>
		/// <typeparam name="TPolicyInterface">The interface to register the policy under.</typeparam>
		/// <param name="policy">The policy to be registered.</param>
		/// <param name="typePolicyAppliesTo">The type the policy applies to.</param>
		/// <param name="idPolicyAppliesTo">The ID the policy applies to.</param>
		public void Set<TPolicyInterface>(TPolicyInterface policy, Type typePolicyAppliesTo, string idPolicyAppliesTo)
			where TPolicyInterface : IBuilderPolicy
		{
			Set(typeof(TPolicyInterface), policy, typePolicyAppliesTo, idPolicyAppliesTo);
		}

		/// <summary>
		/// Sets an individual policy.
		/// </summary>
		/// <param name="policyInterface">The interface to register the policy under.</param>
		/// <param name="policy">The policy to be registered.</param>
		/// <param name="typePolicyAppliesTo">The type the policy applies to.</param>
		/// <param name="idPolicyAppliesTo">The ID the policy applies to.</param>
		public void Set(Type policyInterface, IBuilderPolicy policy, Type typePolicyAppliesTo, string idPolicyAppliesTo)
		{
			BuilderPolicyKey key = new BuilderPolicyKey(policyInterface, typePolicyAppliesTo, idPolicyAppliesTo);
			lock (lockObject)
			{
				policies[key] = policy;
			}
		}

		/// <summary>
		/// Sets a default policy. When checking for a policy, if no specific individual policy
		/// is available, the default will be used.
		/// </summary>
		/// <typeparam name="TPolicyInterface">The interface to register the policy under.</typeparam>
		/// <param name="policy">The default policy to be registered.</param>
		public void SetDefault<TPolicyInterface>(TPolicyInterface policy)
			where TPolicyInterface : IBuilderPolicy
		{
			SetDefault(typeof(TPolicyInterface), policy);
		}

		/// <summary>
		/// Sets a default policy. When checking for a policy, if no specific individual policy
		/// is available, the default will be used.
		/// </summary>
		/// <param name="policyInterface">The interface to register the policy under.</param>
		/// <param name="policy">The default policy to be registered.</param>
		public void SetDefault(Type policyInterface, IBuilderPolicy policy)
		{
			Set(policyInterface, policy, null, null);
		}
	}
}