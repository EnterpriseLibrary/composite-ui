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
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Microsoft.Practices.CompositeUI.Services;

namespace Microsoft.Practices.CompositeUI.Tests
{
	[TestClass]
	public class FileStatePersistenceServiceTestFixture
	{
		private string stateID = "{0DBF9C49-5D03-4917-B49F-44EB31551E4F}";
		private string filename;
		private static WorkItem container;

		public FileStatePersistenceServiceTestFixture()
		{
			filename = stateID + ".state";
		}

		[TestInitialize]
		public void Setup()
		{
			container = new TestableRootWorkItem();
		}

		[TestCleanup]
		public void TearDown()
		{
			if (File.Exists(filename))
			{
				File.Delete(filename);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(StatePersistenceException))]
		public void InvalidCryptographySettingThrowsStatePersistenceException()
		{
			FileStatePersistenceService service = new FileStatePersistenceService();
			NameValueCollection settings = new NameValueCollection();
			settings["UseCryptography"] = "abc";

			service.Configure(settings);
		}


		[TestMethod]
		public void StateIsSerializable()
		{
			State state = new State(stateID);
			BinaryFormatter fmt = new BinaryFormatter();
			using (FileStream write = File.OpenWrite(filename))
			{
				fmt.Serialize(write, state);
			}
			Assert.IsTrue(File.Exists(filename));

			State restored = null;
			using (FileStream read = File.OpenRead(filename))
			{
				restored = (State)fmt.Deserialize(read);
			}
			Assert.AreEqual(state.ID, restored.ID);
		}


		[TestMethod]
		public void CanSaveState()
		{
			State state = new State(stateID);
			state["somekey"] = "somevalue";
			FileStatePersistenceService svc = new FileStatePersistenceService();
			svc.Save(state);
			Assert.IsTrue(File.Exists(filename));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CannotSaveNull()
		{
			FileStatePersistenceService svc = new FileStatePersistenceService();
			State state = null;
			svc.Save(state);
		}

		[TestMethod]
		public void CanLoadState()
		{
			CanSaveState();
			FileStatePersistenceService svc = new FileStatePersistenceService();
			State state = svc.Load(stateID);
			Assert.AreEqual(stateID, state.ID);
			Assert.AreEqual("somevalue", state["somekey"]);
		}

		[TestMethod]
		[ExpectedException(typeof(StatePersistenceException))]
		public void LoadingInexistentState()
		{
			FileStatePersistenceService svc = new FileStatePersistenceService();
			State state = svc.Load("junk");
		}

		[TestMethod]
		public void CanRemoveState()
		{
			FileStatePersistenceService svc = new FileStatePersistenceService();
			State state = new State(stateID);
			svc.Save(state);

			svc.Remove(stateID);
			Assert.IsFalse(File.Exists(filename));
		}

		[TestMethod]
		public void StateContainsSavedStates()
		{
			State state = new State(stateID);
			FileStatePersistenceService svc = new FileStatePersistenceService();
			Assert.IsFalse(svc.Contains(stateID), "The storage shouln't have the state persisted yet.");
			svc.Save(state);
			Assert.IsTrue(svc.Contains(stateID), "The storage should have the state persisted now.");
		}

		[TestMethod]
		[ExpectedException(typeof(StatePersistenceException))]
		public void ThrowsIfStateCannotBeSaved()
		{
			FileStatePersistenceService svc = new FileStatePersistenceService();
			State state = new State(stateID);
			using (StreamWriter sw = File.CreateText(filename))
			{
				svc.Save(state);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(StatePersistenceException))]
		public void ThrowsIfCannotRemove()
		{
			FileStatePersistenceService svc = new FileStatePersistenceService();
			State state = new State(stateID);
			svc.Save(state);
			using (StreamWriter sw = File.CreateText(filename))
			{
				svc.Remove(stateID);
			}
		}


		[TestMethod]
		[ExpectedException(typeof(StatePersistenceException))]
		public void CryptoFailsWhenCryptoNotAvailable()
		{
			WorkItem workItem = new TestableRootWorkItem();
			workItem.Services.Remove(typeof(ICryptographyService));

			FileStatePersistenceService perSvc = new FileStatePersistenceService();
			workItem.Services.Add(typeof(IStatePersistenceService), perSvc);
			NameValueCollection settings = new NameValueCollection();
			settings["UseCryptography"] = "True";
			perSvc.Configure(settings);
			perSvc.Save(new State("junk"));
		}

		[TestMethod]
		public void UsesCryptoToSave()
		{
			WorkItem host = container;
			ICryptographyService cryptoSvc = new DataProtectionCryptographyService();
			host.Services.Add(typeof(ICryptographyService), cryptoSvc);
			FileStatePersistenceService perSvc = new FileStatePersistenceService();
			host.Services.Add(typeof(IStatePersistenceService), perSvc);
			NameValueCollection settings = new NameValueCollection();
			settings["UseCryptography"] = "True";
			perSvc.Configure(settings);

			string id = Guid.NewGuid().ToString();
			State testState = new State(id);
			testState["someValue"] = "value";
			perSvc.Save(testState);


			byte[] stateData = null;
			string filename = string.Format(CultureInfo.InvariantCulture, "{0}.state", id);
			using (FileStream stream = File.OpenRead(filename))
			{
				byte[] cipherData = new byte[stream.Length];
				stream.Read(cipherData, 0, (int)stream.Length);
				stateData = ProtectedData.Unprotect(cipherData, null, DataProtectionScope.CurrentUser);
			}

			BinaryFormatter fmt = new BinaryFormatter();
			MemoryStream ms = new MemoryStream(stateData);
			State recovered = (State)fmt.Deserialize(ms);

			Assert.AreEqual(id, recovered.ID, "The state id is different.");
			Assert.AreEqual("value", recovered["someValue"]);

			if (File.Exists(filename))
			{
				File.Delete(filename);
			}
		}

		[TestMethod]
		public void UsesCryptoToLoad()
		{
			string id = Guid.NewGuid().ToString();
			State testState = new State(id);
			testState["someValue"] = "value";

			BinaryFormatter fmt = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			fmt.Serialize(ms, testState);
			byte[] cipherData = ProtectedData.Protect(ms.GetBuffer(), null, DataProtectionScope.CurrentUser);

			string filename = string.Format(CultureInfo.InvariantCulture, "{0}.state", id);
			using (FileStream stream = File.OpenWrite(filename))
			{
				stream.Write(cipherData, 0, cipherData.Length);
			}

			WorkItem host = container;
			ICryptographyService cryptoSvc = new DataProtectionCryptographyService();
			host.Services.Add(typeof(ICryptographyService), cryptoSvc);
			FileStatePersistenceService perSvc = new FileStatePersistenceService();
			host.Services.Add(typeof(IStatePersistenceService), perSvc);
			NameValueCollection settings = new NameValueCollection();
			settings["UseCryptography"] = "True";
			perSvc.Configure(settings);

			State recovered = perSvc.Load(id);

			Assert.AreEqual(id, recovered.ID, "The state id is different.");
			Assert.AreEqual("value", recovered["someValue"]);

			if (File.Exists(filename))
			{
				File.Delete(filename);
			}
		}

		[TestMethod]
		public void CanConstructWithFolder()
		{
			string basePath = Path.GetTempPath();
			string tempFile = Path.Combine(basePath,
										   String.Format(CultureInfo.InvariantCulture, "{0}.state", stateID));

			FileStatePersistenceService service = new FileStatePersistenceService(basePath);
			State st = new State(stateID);

			service.Save(st);

			Assert.AreEqual(basePath, service.BasePath);
			Assert.IsTrue(File.Exists(tempFile));

			File.Delete(tempFile);
		}
	}
}
