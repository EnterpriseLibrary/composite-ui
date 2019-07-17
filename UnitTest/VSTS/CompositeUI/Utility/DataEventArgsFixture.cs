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

namespace Microsoft.Practices.CompositeUI.Utility.Tests
{
    [TestClass]
    public class DataEventArgsFixture
    {
        [TestMethod]
        public void IsCreatable()
        {
            DataEventArgs<int> e = new DataEventArgs<int>(32);
            Assert.IsNotNull(e, "Cannot create an instance of ApplicationEventArgs");
        }

        [TestMethod]
        public void CanPassData()
        {
            DataEventArgs<int> e = new DataEventArgs<int>(32);
            Assert.AreEqual(32, e.Data);
        }

        [TestMethod]
        public void IsEventArgs()
        {
            Assert.IsTrue(typeof(EventArgs).IsAssignableFrom(typeof(DataEventArgs<int>)), "ApplicationEventArgs is not a subtype of EventArgs");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsIfDataIsNull()
        {
            DataEventArgs<object> data = new DataEventArgs<object>(null);
        }
    }
}
