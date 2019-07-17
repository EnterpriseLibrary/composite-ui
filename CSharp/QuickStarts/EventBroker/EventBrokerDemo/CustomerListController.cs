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
using System.Threading;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.Utility;

namespace EventBrokerDemo
{
	[DesignerCategory("Code")]
	public class CustomerListController : Controller
	{
		// Declares a WorkItem scoped event that signals the starting of the background processing
		[EventPublication("topic://EventBrokerQuickStart/StartProcess", PublicationScope.WorkItem)]
		public event EventHandler<DictionaryEventArgs> StartProcess;

		// Declares a WorkItem scoped event that signals the completion of the background processing
		[EventPublication("topic://EventBrokerQuickStart/ProcessCompleted", PublicationScope.WorkItem)]
		public event EventHandler ProcessCompleted;

		// Declares a WorkItem scoped event that reports progress on the background processing
		[EventPublication("topic://EventBrokerQuickStart/ProgressChanged", PublicationScope.WorkItem)]
		public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

		// Declares a global scoped event that signals that a customer was added to the global list
		[EventPublication("topic://EventBrokerQuickStart/CustomerAdded")]
		public event EventHandler<DataEventArgs<string>> GlobalCustomerAdded;

		private bool processing = false;
		private bool cancelled = false;

		private List<string> customers;

		[State("customers")]
		public List<string> Customers
		{
			set { customers = value; }
		}

		// This method adds a customer to the local work item state.
		public void AddLocalCustomer(string customerId)
		{
			customers.Add(customerId);
			State.RaiseStateChanged("customers", customerId);
		}

		// This method just fires the GlobalCustomerAdded event, which is associated
		// to the "topic://EventBrokerQuickStart/CustomerAdded" event topic with
		// global scope.
		public void AddGlobalCustomer(string customerId)
		{
			if (GlobalCustomerAdded != null)
			{
				GlobalCustomerAdded(this, new DataEventArgs<string>(customerId));
			}
		}

		// Starts the asynchronous processing of the customers added to the local work item.
		// The process simply moves one customer after anothe to the parent work item, which is
		// the application default work item.
		public void ProcessLocalCustomers()
		{
			// Only start the process if we're not processing
			// and if we have someone interested in doing the job 
			// (in this case, this very same class).
			if (!processing && StartProcess != null)
			{
				processing = true;
				cancelled = false;

				// Prepare the arguments to the processor
				DictionaryEventArgs args = new DictionaryEventArgs();
				List<string> toProcess = new List<string>(customers);
				args.Data["customers"] = toProcess;

				// Fire the StartProcess event (meaning, fire the
				// "topic://EventBrokerQuickStart/StartProcess" topic.

				if (StartProcess!=null)
				{
					StartProcess(this, args);
				}

				// Clear the local customer list
				customers.Clear();

				// Raise the local state change event, so the UI can update itself
				State.RaiseStateChanged("customers", null);

				// Create a progress view that consumes some progress events
				// and show that progress
				this.WorkItem.Items.AddNew<ProgressView>().Show();
			}
		}

		// This is a background subscription that will be called in another thread.
		[EventSubscription("topic://EventBrokerQuickStart/StartProcess", Thread = ThreadOption.Background)]
		public void StartProcessHandler(object sender, DictionaryEventArgs args)
		{
			// Get the lis of customers to process
			List<string> customers = (List<string>) args.Data["customers"];

			// Process the customer list
			for (int i = 0; i < customers.Count; i++)
			{
				if (cancelled)
				{
					break;
				}

				// Report progress, and along with it send the customer we're processing
				OnProgressChanged((i + 1) * (100 / customers.Count), customers[i]);

				// Simulate some time consuming proccess.
				Thread.Sleep(1000);
			}
			OnProcessCompleted();
		}

		private void OnProcessCompleted()
		{
			if (ProcessCompleted != null)
			{
				ProcessCompleted(this, EventArgs.Empty);
			}
		}

		private void OnProgressChanged(int progress, string data)
		{
			if (ProgressChanged != null)
			{
				ProgressChangedEventArgs args = new ProgressChangedEventArgs(progress, data);
				ProgressChanged(this, args);
			}
		}

		//// Handles the ProgressChanged event
		[EventSubscription("topic://EventBrokerQuickStart/ProgressChanged", Thread = ThreadOption.UserInterface)]
		public void ProgressChangedHandler(object sender, ProgressChangedEventArgs args)
		{
			this.AddGlobalCustomer((string)args.UserState);
		}


		// Gets the BackgroundWorker (if all) and send it a cancel message.
		// This method shows how to access the event catalog service and
		// how access an event topic and its background workers
		public void CancelProcess()
		{
			if (processing)
			{
				cancelled = true;
			}
		}

		//// Handles the ProcessCompleted event
		[EventSubscription("topic://EventBrokerQuickStart/ProcessCompleted", Thread = ThreadOption.UserInterface)]
		public void ProcessCompletedHandler(object sender, EventArgs args)
		{
			cancelled = false;
			processing = false;
		}

		// Here we publish our and our parent's state
		// to make easy for the view to perform the UI updates

		public new State State
		{
			get { return base.State; }
		}

		public State ParentState
		{
			get { return this.WorkItem.Parent.State; }
		}

	}
}