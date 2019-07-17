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
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.WinForms;
using System.Windows.Forms;

namespace EventBrokerDemo
{
	class BrokerApplication : FormShellApplication<WorkItem, LaunchPadForm>
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			new BrokerApplication().Run();
		}

		protected override void BeforeShellCreated()
		{
			base.BeforeShellCreated();
			RootWorkItem.State["customers"] = new List<string>();
		}

		#region Unhandled Exception
		
		public override void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;

			if (ex != null)
			{
				MessageBox.Show(BuildExceptionString(ex));
			}
			else
			{
				MessageBox.Show("An Exception has occured, unable to get details");
			}

			Environment.Exit(0);
		}

		private string BuildExceptionString(Exception exception)
		{
			string errMessage = string.Empty;

			errMessage +=
				exception.Message + Environment.NewLine + exception.StackTrace;

			while (exception.InnerException != null)
			{
				errMessage += BuildInnerExceptionString(exception.InnerException);

				exception = exception.InnerException;
			}

			return errMessage;
		}

		private string BuildInnerExceptionString(Exception innerException)
		{
			string errMessage = string.Empty;

			errMessage += Environment.NewLine + " InnerException ";
			errMessage += Environment.NewLine + innerException.Message + Environment.NewLine + innerException.StackTrace;

			return errMessage;
		} 
		#endregion
	}
}