//===============================================================================
// Microsoft patterns & practices
// CompositeUI Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#endif

using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using Microsoft.Practices.CompositeUI.Services;

namespace Microsoft.Practices.CompositeUI.Tests.Services
{
	[TestClass]
	public class DataProtectionCryptographyServiceFixture
	{
		[TestMethod]
		public void DataProtectionCryptographyServiceIsAvailable()
		{
			DataProtectionCryptographyService svc = new DataProtectionCryptographyService();
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentNullException))]
		public void EncryptFailsIfNullData()
		{
			DataProtectionCryptographyService svc = new DataProtectionCryptographyService();
			svc.EncryptSymmetric(null);
		}


		[TestMethod]
		[ExpectedException(typeof (ArgumentNullException))]
		public void DecryptFailsIfNullData()
		{
			DataProtectionCryptographyService svc = new DataProtectionCryptographyService();
			svc.DecryptSymmetric(null);
		}

		[TestMethod]
		public void CanEncryptData()
		{
			byte[] userData = {0, 1, 2, 3, 4, 1, 2, 3, 4};
			DataProtectionCryptographyService svc = new DataProtectionCryptographyService();
			byte[] result = svc.EncryptSymmetric(userData);

			byte[] restored = ProtectedData.Unprotect(result, null, DataProtectionScope.CurrentUser);

			Assert.AreEqual(9, restored.Length, "Restored data is not correct.");
			for (int i = 0; i < userData.Length; i++)
			{
				Assert.AreEqual(userData[i], restored[i], "The encryption failed.");
			}
		}

		[TestMethod]
		public void CanDecryptData()
		{
			byte[] userData = {0, 1, 2, 3, 4, 1, 2, 3, 4};
			byte[] cipherData = ProtectedData.Protect(userData, null, DataProtectionScope.CurrentUser);

			DataProtectionCryptographyService svc = new DataProtectionCryptographyService();
			byte[] restored = svc.DecryptSymmetric(cipherData);

			Assert.AreEqual(9, restored.Length, "Restored data is not correct.");
			for (int i = 0; i < userData.Length; i++)
			{
				Assert.AreEqual(userData[i], restored[i], "The decryption failed.");
			}
		}

		[TestMethod]
		public void ServiceCanBeConfigured()
		{
			NameValueCollection settings = new NameValueCollection();
			DataProtectionCryptographyService svc = new DataProtectionCryptographyService();
			svc.Configure(settings);
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentNullException))]
		public void ThrowsIfConfiguredWithNullSettings()
		{
			DataProtectionCryptographyService svc = new DataProtectionCryptographyService();
			svc.Configure(null);
		}

		[TestMethod]
		public void EncryptsUsingEntropy()
		{
			byte[] userData = {0, 1, 2, 3, 4, 1, 2, 3, 4};
			byte[] entropy = {1, 2, 3, 4};
			NameValueCollection settings = new NameValueCollection();
			settings["Entropy"] = Convert.ToBase64String(entropy);
			DataProtectionCryptographyService svc = new DataProtectionCryptographyService();
			svc.Configure(settings);

			byte[] cipherData = svc.EncryptSymmetric(userData);

			byte[] recovered = ProtectedData.Unprotect(cipherData, entropy, DataProtectionScope.CurrentUser);
			Assert.AreEqual(Convert.ToBase64String(userData), Convert.ToBase64String(recovered));
		}

		[TestMethod]
		public void DecryptUsingEntropy()
		{
			byte[] userData = {0, 1, 2, 3, 4, 1, 2, 3, 4};
			byte[] entropy = {1, 2, 3, 4};
			NameValueCollection settings = new NameValueCollection();
			settings["Entropy"] = Convert.ToBase64String(entropy);
			DataProtectionCryptographyService svc = new DataProtectionCryptographyService();
			svc.Configure(settings);
			byte[] cipherData = ProtectedData.Protect(userData, entropy, DataProtectionScope.CurrentUser);

			byte[] recovered = svc.DecryptSymmetric(cipherData);

			Assert.AreEqual(Convert.ToBase64String(userData), Convert.ToBase64String(recovered));
		}

		[TestMethod]
		public void ServiceClearsSettingsAfterConfigured()
		{
			byte[] entropy = { 1, 2, 3, 4 };
			NameValueCollection settings = new NameValueCollection();
			settings["Entropy"] = Convert.ToBase64String(entropy);
			DataProtectionCryptographyService svc = new DataProtectionCryptographyService();
			svc.Configure(settings);

			Assert.AreEqual(0, settings.Count);
		}
	}
}
