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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace Microsoft.Practices.CompositeUI.Tests
{
	[TestClass]
	public class StateFixture
	{
		[TestMethod]
		public void StateSerializable()
		{
			State st = new State();
			st["foo"] = "Foodle";
			st["info"] = new Info("thekey", DateTime.Now);

			MemoryStream mem = new MemoryStream();
			BinaryFormatter fmt = new BinaryFormatter();
			fmt.Serialize(mem, st);
		}

		[TestMethod]
		public void ChangingRootValue()
		{
			State s = new State();
			bool changed = false;
			s.StateChanged += delegate
			{
				changed = true;
			};

			s["Name"] = "kzu";
			Assert.IsTrue(changed);
		}

		[TestMethod]
		public void ChangingChildNoNotification()
		{
			State s = new State();
			Info info = new Info("key", new object());
			s["Complex"] = info;

			bool changed = false;
			s.StateChanged += delegate
			{
				changed = true;
			};

			// Change complex object.
			info.Key = "changed";
			Assert.IsFalse(changed);
		}

		[TestMethod]
		public void ChangingChildNotification()
		{
			State s = new State();
			InfoElement info = new InfoElement("key", new object());
			s["Complex"] = info;

			bool changed = false;
			s.StateChanged += delegate
			{
				changed = true;
			};

			// Change complex object.
			info.Key = "changed";
			Assert.IsTrue(changed);
		}

		[TestMethod]
		public void StateCanStoreNullValues()
		{
			State s = new State();
			s["nullvalue"] = null;
		}

		[TestMethod]
		public void StateChangedEventHasNewAndOldValues()
		{
			StateChangedEventArgs eventArgs = null;
			State s = new State();
			s.StateChanged += delegate(object sender, StateChangedEventArgs args)
			{
				eventArgs = args;
			};

			object obj1 = new object();
			s["Test"] = obj1;
			object obj2 = new object();
			s["Test"] = obj2;

			Assert.AreSame(obj2, eventArgs.NewValue);
			Assert.AreSame(obj1, eventArgs.OldValue);
		}
	}

	class InfoElement : StateElement
	{
		public InfoElement(string key, object value)
		{
			this.Key = key;
			this.Value = value;
		}

		public string Key
		{
			get { return (string)this["key"]; }
			set { this["key"] = value; }
		}

		public object Value
		{
			get { return this["value"]; }
			set { this["value"] = value; }
		}
	}

	[Serializable]
	class Info
	{
		public Info(string key, object value)
		{
			this.Key = key;
			this.Value = value;
		}
		public string Key;
		public object Value;
	}
}
