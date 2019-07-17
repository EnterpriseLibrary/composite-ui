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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Microsoft.Practices.CompositeUI.Services;

namespace Microsoft.Practices.CompositeUI.Tests
{
	[TestClass]
	public class StreamStatePersistenceServiceTestFixture
	{
		private string stateID = "DummyStateID";

		[TestMethod]
		[ExpectedException(typeof(StatePersistenceException))]
		public void InvalidCryptographySettingThrowsStatePersistenceException()
		{
			MemoryStreamPersistence service = new MemoryStreamPersistence();
			NameValueCollection settings = new NameValueCollection();
			settings["UseCryptography"] = "abc";

			service.Configure(settings);
		}

		[TestMethod]
		public void CanSaveAndLoadState()
		{
			State state = new State(stateID);
			state["somekey"] = "somevalue";
			MemoryStreamPersistence svc = new MemoryStreamPersistence();
			svc.Save(state);

			State state2 = svc.Load(stateID);

			Assert.AreEqual(state["somekey"], state2["somekey"]);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CannotSaveNull()
		{
			MemoryStreamPersistence svc = new MemoryStreamPersistence();
			svc.Save(null);
		}

		[TestMethod]
		public void DoesNotDisposeStreamIfNotToldTo()
		{
			MemoryStreamPersistence svc = new MemoryStreamPersistence();
			State state = new State(stateID);
			svc.Save(state);

			Assert.IsTrue(svc.Stream.CanSeek);
		}

		[TestMethod]
		[ExpectedException(typeof(StatePersistenceException))]
		public void LoadingInexistentState()
		{
			NonExisingPersistence svc = new NonExisingPersistence();
			State state = svc.Load("junk");
		}

		[TestMethod]
		[ExpectedException(typeof(StatePersistenceException))]
		public void ThrowsIfStreamNull()
		{
			NullStreamPersistence svc = new NullStreamPersistence();
			svc.Load(stateID);
		}

		[TestMethod]
		public void SaveStateRemovesPreviousState()
		{
			RemovesPersistence svc = new RemovesPersistence();

			svc.Save(new State(stateID));

			Assert.IsTrue(svc.RemoveCalled);
		}

		[TestMethod]
		[ExpectedException(typeof(StatePersistenceException))]
		public void ThrowsIfStateCannotBeSaved()
		{
			LockedFilePersistence svc = new LockedFilePersistence();
			State state = new State(stateID);
			svc.Save(state);
		}

		[TestMethod]
		[ExpectedException(typeof(StatePersistenceException))]
		public void ThrowsIfCannotRemove()
		{
			LockedFilePersistence svc = new LockedFilePersistence();
			svc.Remove(stateID);
		}

		[TestMethod]
		[ExpectedException(typeof(StatePersistenceException))]
		public void CryptoFailsWhenCryptoNotAvailable()
		{
			MemoryStreamPersistence svc = new MemoryStreamPersistence();
			NameValueCollection settings = new NameValueCollection();
			settings["UseCryptography"] = "True";
			svc.Configure(settings);
			svc.Save(new State(stateID));
		}

		[TestMethod]
		public void UsesCryptoToSave()
		{
			WorkItem container = new TestableRootWorkItem();
			DataProtectionCryptographyService cryptoSvc = new DataProtectionCryptographyService();
			MemoryStreamPersistence perSvc = new MemoryStreamPersistence();
			container.Services.Add(typeof(ICryptographyService), cryptoSvc);
			container.Services.Add(typeof(IStatePersistenceService), perSvc);
			NameValueCollection settings = new NameValueCollection();
			settings["UseCryptography"] = "True";
			perSvc.Configure(settings);

			Guid id = Guid.NewGuid();
			State testState = new State(id.ToString());
			testState["someValue"] = "value";
			perSvc.Save(testState);

			byte[] stateData = null;
			perSvc.Stream.Position = 0;
			Stream stream = perSvc.Stream;
			byte[] cipherData = new byte[stream.Length];
			stream.Read(cipherData, 0, (int)stream.Length);
			stateData = ProtectedData.Unprotect(cipherData, null, DataProtectionScope.CurrentUser);

			BinaryFormatter fmt = new BinaryFormatter();
			MemoryStream ms = new MemoryStream(stateData);
			State recovered = (State)fmt.Deserialize(ms);

			Assert.AreEqual(id.ToString(), recovered.ID, "The state id is different.");
			Assert.AreEqual("value", recovered["someValue"]);
		}

		[TestMethod]
		public void UsesCryptoToLoad()
		{
			string id = "id";
			State testState = new State(id);
			testState["someValue"] = "value";

			BinaryFormatter fmt = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			fmt.Serialize(ms, testState);
			byte[] cipherData = ProtectedData.Protect(ms.GetBuffer(), null, DataProtectionScope.CurrentUser);

			MemoryStream stream = new MemoryStream();
			stream.Write(cipherData, 0, cipherData.Length);

			WorkItem host = new TestableRootWorkItem();
			DataProtectionCryptographyService cryptoSvc = new DataProtectionCryptographyService();
			MemoryStreamPersistence perSvc = new MemoryStreamPersistence();
			perSvc.Stream = stream;

			host.Services.Add(typeof(ICryptographyService), cryptoSvc);
			host.Services.Add(typeof(IStatePersistenceService), perSvc);
			NameValueCollection settings = new NameValueCollection();
			settings["UseCryptography"] = "True";
			perSvc.Configure(settings);

			State recovered = perSvc.Load(id);

			Assert.AreEqual(id, recovered.ID, "The state id is different.");
			Assert.AreEqual("value", recovered["someValue"]);
		}

		#region Supporting Classes

		private class MemoryStreamPersistence : StreamStatePersistenceService
		{
			public MemoryStream Stream = new MemoryStream();

			public override void RemoveStream(string id)
			{
			}

			public override bool Contains(string id)
			{
				return true;
			}

			protected override Stream GetStream(string id)
			{
				Stream.Position = 0;
				return Stream;
			}

			protected override Stream GetStream(string id, out bool shouldDispose)
			{
				shouldDispose = false;
				return GetStream(id);
			}
		}

		private class NonExisingPersistence : StreamStatePersistenceService
		{
			public override void RemoveStream(string id)
			{
			}

			public override bool Contains(string id)
			{
				return false;
			}

			protected override Stream GetStream(string id)
			{
				return new MemoryStream();
			}
		}

		private class NullStreamPersistence : StreamStatePersistenceService
		{
			public override void RemoveStream(string id)
			{
			}

			public override bool Contains(string id)
			{
				return false;
				;
			}

			protected override Stream GetStream(string id)
			{
				return null;
			}
		}

		private class RemovesPersistence : StreamStatePersistenceService
		{
			public bool RemoveCalled = false;

			public override void RemoveStream(string id)
			{
				RemoveCalled = true;
			}

			public override bool Contains(string id)
			{
				return true;
			}

			protected override Stream GetStream(string id)
			{
				return new MemoryStream();
			}
		}

		private class LockedFilePersistence : StreamStatePersistenceService
		{
			public override void RemoveStream(string id)
			{
				string file = Path.GetTempFileName();
				Stream locked = File.OpenWrite(file);

				File.Delete(file);
			}

			public override bool Contains(string id)
			{
				return false;
			}

			protected override Stream GetStream(string id)
			{
				string file = Path.GetTempFileName();
				Stream locked = File.OpenWrite(file);

				return File.OpenRead(file);
			}
		}

		#endregion
	}
}
