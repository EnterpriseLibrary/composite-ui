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
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder.Tests
{
	[TestClass]
	public class WeakRefDictionaryFixture
	{
		[TestMethod]
		public void CanRegisterObjectAndFindItByID()
		{
			object o = new object();
			WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();

			dict.Add("foo", o);
			Assert.IsNotNull(dict["foo"]);
			Assert.AreSame(o, dict["foo"]);
		}

		[TestMethod]
		public void CanRegisterTwoObjectsAndGetThemBoth()
		{
			object o1 = new object();
			object o2 = new object();
			WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();

			dict.Add("foo1", o1);
			dict.Add("foo2", o2);

			Assert.AreSame(o1, dict["foo1"]);
			Assert.AreSame(o2, dict["foo2"]);
		}

		[TestMethod]
		public void KeyCanBeOfArbitraryType()
		{
			object oKey = new object();
			object oVal = new object();
			WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();

			dict.Add(oKey, oVal);

			Assert.AreSame(oVal, dict[oKey]);
		}

		[TestMethod]
		public void CanAddSameObjectTwiceWithDifferentKeys()
		{
			object o = new object();
			WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();

			dict.Add("foo1", o);
			dict.Add("foo2", o);

			Assert.AreSame(dict["foo1"], dict["foo2"]);
		}

		[TestMethod]
		[ExpectedException(typeof(KeyNotFoundException))]
		public void AskingForAKeyThatDoesntExistThrows()
		{
			WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();
			object unused = dict["foo"];
		}

		[TestMethod]
		[ExpectedException(typeof(KeyNotFoundException))]
		public void CanRemoveAnObjectThatWasAlreadyAdded()
		{
			object o = new object();
			WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();

			dict.Add("foo", o);
			dict.Remove("foo");
			object unused = dict["foo"];
		}

		[TestMethod]
		public void RemovingAKeyOfOneObjectDoesNotAffectOtherKeysForSameObject()
		{
			object o = new object();
			WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();

			dict.Add("foo1", o);
			dict.Add("foo2", o);
			dict.Remove("foo1");

			Assert.AreSame(o, dict["foo2"]);
		}

		[TestMethod]
		public void RemovingAKeyDoesNotAffectOtherKeys()
		{
			object o1 = new object();
			object o2 = new object();
			WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();

			dict.Add("foo1", o1);
			dict.Add("foo2", o2);
			dict.Remove("foo1");

			Assert.AreSame(o2, dict["foo2"]);
		}

		[TestMethod]
		public void RemovingANonExistantKeyDoesntThrow()
		{
			WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();
			dict.Remove("foo1");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddingToSameKeyTwiceAlwaysThrows()
		{
			object o = new object();
			WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();

			dict.Add("foo1", o);
			dict.Add("foo1", o);
		}

		[TestMethod]
		[ExpectedException(typeof(KeyNotFoundException))]
		public void RegistrationDoesNotPreventGarbageCollection()
        {
            WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();
            RegistrationDoesNotPreventGarbageCollection_TestHelper(dict);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            object unused = dict["foo"];
        }

        private static void RegistrationDoesNotPreventGarbageCollection_TestHelper(WeakRefDictionary<object, object> dict)
        {
            dict.Add("foo", new object());
        }

        [TestMethod]
		public void NullIsAValidValue()
		{
			WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();
			dict.Add("foo", null);
			Assert.IsNull(dict["foo"]);
		}

		[TestMethod]
		public void CanFindOutIfContainsAKey()
		{
			WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();

			dict.Add("foo", null);
			Assert.IsTrue(dict.ContainsKey("foo"));
			Assert.IsFalse(dict.ContainsKey("foo2"));
		}

		[TestMethod]
		public void CanEnumerate()
		{
			object o1 = new object();
			object o2 = new object();
			WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();

			dict.Add("foo1", o1);
			dict.Add("foo2", o2);

			foreach (KeyValuePair<object, object> kvp in dict)
			{
				Assert.IsNotNull(kvp);
				Assert.IsNotNull(kvp.Key);
				Assert.IsNotNull(kvp.Value);
			}
		}

		[TestMethod]
		public void CountReturnsNumberOfKeysWithLiveValues()
        {
            WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();
            CountReturnsNumberOfKeysWithLiveValues_TestHelper(out object o, dict);

            o = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.AreEqual(0, dict.Count);
        }

        private static void CountReturnsNumberOfKeysWithLiveValues_TestHelper(out object o, WeakRefDictionary<object, object> dict)
        {
            o = new object();
            dict.Add("foo1", o);
            dict.Add("foo2", o);

            Assert.AreEqual(2, dict.Count);
        }

        [TestMethod]
		public void CanAddItemAfterPreviousItemIsCollected()
        {
            WeakRefDictionary<object, object> dict = new WeakRefDictionary<object, object>();
            CanAddItemAfterPreviousItemIsCollected_TestHelper(dict);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            dict.Add("foo", new object());
        }

        private static void CanAddItemAfterPreviousItemIsCollected_TestHelper(WeakRefDictionary<object, object> dict)
        {
            dict.Add("foo", new object());
        }
    }
}
