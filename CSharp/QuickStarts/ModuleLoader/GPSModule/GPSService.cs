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

namespace GPSModule
{
    // Declare a service published in the module, along with
    // its contract registration.
    [Service(typeof(IGPSService))]
    public class GPService : IGPSService
    {
        public int GetLatitude()
        {
            return 42;
        }

        public int GetLongitude()
        {
            return 125;
        }
    }
}
