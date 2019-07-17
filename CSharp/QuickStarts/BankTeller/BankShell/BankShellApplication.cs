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
using System.Windows.Forms;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.UIElements;
using Microsoft.Practices.CompositeUI.WinForms;
using Microsoft.Practices.CompositeUI.Commands;
using Microsoft.Practices.CompositeUI.Services;
using BankTellerCommon;

namespace BankShell
{
	public class BankShellApplication : FormShellApplication<WorkItem, BankShellForm>
	{
		[STAThread]
		public static void Main()
		{
			new BankShellApplication().Run();
		}

		// This method is called just after your shell has been created (the root work item
		// also exists). Here, you might want to:
		//   - Attach UIElementManagers
		//   - Register the form with a name.
		//   - Register additional workspaces (e.g. a named WindowWorkspace)
		protected override void AfterShellCreated()
		{
			base.AfterShellCreated();

			ToolStripMenuItem fileItem = (ToolStripMenuItem)Shell.MainMenuStrip.Items["File"];

			RootWorkItem.UIExtensionSites.RegisterSite(UIExtensionConstants.MAINMENU, Shell.MainMenuStrip);
			RootWorkItem.UIExtensionSites.RegisterSite(UIExtensionConstants.MAINSTATUS, Shell.mainStatusStrip);
			RootWorkItem.UIExtensionSites.RegisterSite(UIExtensionConstants.FILE, fileItem);
			RootWorkItem.UIExtensionSites.RegisterSite(UIExtensionConstants.FILEDROPDOWN, fileItem.DropDownItems);

			// Load the menu structure from App.config
			UIElementBuilder.LoadFromConfig(RootWorkItem);
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

			errMessage += exception.Message + Environment.NewLine + exception.StackTrace;

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
