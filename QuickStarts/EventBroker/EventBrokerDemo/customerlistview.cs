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
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.EventBroker;

namespace EventBrokerDemo
{
    public partial class CustomerListView : Form
    {
        private List<string> customers;

        [State("customers")]
        public List<string> Customers
        {
            set { customers = value; }
        }

        private CustomerListController controller;

        [CreateNew]
        public CustomerListController Controller
        {
            set { controller = value; }
        }

        public CustomerListView()
        {
            InitializeComponent();
        }

        // Once sited, we can access the injected dependencies
        // so we can add our handlers to the state change events
        protected override void OnLoad(EventArgs e)
        {
            controller.ParentState.StateChanged += new EventHandler<StateChangedEventArgs>(ParentState_StateChanged);
            controller.State.StateChanged += new EventHandler<StateChangedEventArgs>(State_StateChanged);
            lstGlobalCustomers.DataSource = this.controller.ParentState["customers"];
        }

        // These two EventSubscriptions handle the background process start and stop, updating the
        // UI accordingly by enabling or disabling the Cancel button.

        [EventSubscription("topic://EventBrokerQuickStart/ProcessCompleted", Thread = ThreadOption.UserInterface)]
        public void OnControllerProcessCompleted(object sender, EventArgs args)
        {
            btnCancelProcess.Enabled = false;
        }

        [EventSubscription("topic://EventBrokerQuickStart/StartProcess", Thread = ThreadOption.UserInterface)]
        public void OnControllerStartProcess(object sender, DictionaryEventArgs args)
        {
            btnCancelProcess.Enabled = true;
        }

        // Here we handle the UI elements events and call the appropiate
        // controller methods.

        private void btnAddLocalCustomer_Click(object sender, EventArgs e)
        {
            if (txtCustomerId.Text.Length > 0)
            {
                this.controller.AddLocalCustomer(txtCustomerId.Text);
                txtCustomerId.Text = "";
            }
            txtCustomerId.Focus();
        }

        private void btnAddGlobalCustomer_Click(object sender, EventArgs e)
        {
            if (txtCustomerId.Text.Length > 0)
            {
                this.controller.AddGlobalCustomer(txtCustomerId.Text);
                txtCustomerId.Text = "";
            }
            txtCustomerId.Focus();
        }

        private void btnProcessLocal_Click(object sender, EventArgs e)
        {
            this.controller.ProcessLocalCustomers();
        }

        private void btnCancelProcess_Click(object sender, EventArgs e)
        {
            this.controller.CancelProcess();
        }

        // These are our handlers to the state change events
        // where we update the view accordingly.

        void ParentState_StateChanged(object sender, StateChangedEventArgs e)
        {
            lstGlobalCustomers.DataSource = null;
            lstGlobalCustomers.DataSource = this.controller.ParentState["customers"];
        }

        void State_StateChanged(object sender, StateChangedEventArgs e)
        {
            lstLocalCustomers.DataSource = null;
            lstLocalCustomers.DataSource = this.customers;
        }
    }
}