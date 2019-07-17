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

namespace ModuleExposingServices
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

    [Service(AddOnDemand = true)]
    public class OnDemandService
    {
        public static bool ServiceCreated = false;

        public OnDemandService()
        {
            ServiceCreated = true;
        }
    }

    [Service(AddOnDemand = true)]
    public class OnDemandService2
    {
        public static bool ServiceCreated = false;

        public OnDemandService2()
        {
            ServiceCreated = true;
        }
    }
}
