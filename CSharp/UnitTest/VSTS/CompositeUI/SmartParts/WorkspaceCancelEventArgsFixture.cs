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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Microsoft.Practices.CompositeUI.Tests.SmartParts
{
    [TestClass]
    public class WorkspaceCancelEventArgsFixture
    {
        private WorkspaceCancelEventArgs eventArgs;

        [TestInitialize]
        public void Setup()
        {
            eventArgs = new WorkspaceCancelEventArgs(new object());
        }

        [TestMethod]
        public void DefaultCancelValueIsFalse()
        {
            Assert.IsFalse(eventArgs.Cancel);
        }

        [TestMethod]
        public void SmartPartIsNotNull()
        {
            Assert.IsNotNull(eventArgs.SmartPart);
        }
    }
}
