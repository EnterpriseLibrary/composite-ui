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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI;

namespace GPSModule
{
	public partial class GPSView : UserControl
	{
		private IGPSService gpsService;

		[ServiceDependency]
		public IGPSService GPS
		{
			set { gpsService = value; }
		}

		private WorkItem workItem;

		[ServiceDependency]
		public WorkItem WorkItem
		{
			set { workItem = value; }
		}

		// Note that we didn't added a ServiceDependencyAttribute for the
		// IDistanceCalculatorService, because that way the service would be created
		// when this view is added to the work item. This is just for the purposes of this example.
		protected IDistanceCalculatorService CalcService
		{
			get { return workItem.Services.Get<IDistanceCalculatorService>(); }
		}

		public GPSView()
		{
			InitializeComponent();
		}

		private void cmdGetDistance_Click(object sender, EventArgs e)
		{
			txtDistance.Text = CalcService.ComputeDistance(gpsService.GetLatitude(), gpsService.GetLongitude()).ToString();
		}

		private void cmdGetLatitude_Click(object sender, EventArgs e)
		{
			txtLatitude.Text = gpsService.GetLatitude().ToString();
		}
	}
}
