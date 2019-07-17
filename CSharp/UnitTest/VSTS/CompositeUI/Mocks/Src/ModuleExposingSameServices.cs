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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;

namespace ModuleExposingSameServices
{
    public class TestModule : ModuleInit
    {
    }

    [Service]
    public class SimpleService
    {
    }

    public interface ITestService
    {
    }

    [Service(typeof(ITestService))]
    public class TestService : ITestService
    {
    }

    [Service]
    public class NonDefaultCtorService
    {
        public NonDefaultCtorService(string someArg)
        {

        }
    }

    [Service]
    public abstract class AbstractService
    {
    }
}
