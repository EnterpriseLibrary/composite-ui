
* What Is It Intended to Show?
This QuickStart shows the usage of the EventBroker feature of CAB. In 
particular, it shows how to declare event publications and event subscriptions, 
using both Global and WorkItem publication scopes. It also shows in a very 
simple manner how to work with background threads.

* How Do I Use It?
Build and run the EventBrokerQuickStart WinForm application. It shows the 
LaunchPad form used to execute the samples.
Start adding a customer to the customer list displayed on the left,
by entering some text on the "Customer ID" box and clicking the "Add 
Customer" button.
Then you can show a global and a local customer list using the "Show List" button.
You should see the customer already added on the global list. Using this form
you can add customers to the global list or to the local list. Try
adding a few customers to the local list, and the click the "Process
Local" button.

* What Should I Look At? 
You should pay attention at how event publications and subscriptions are declared.
Special attention should be paid to the background thread example (accessed by 
the "Process Local" button on the CustomerListView Form), and the different
subscriptions using ThreadOption enumeration.
For the tracing features, take a look to the App.config and the TraceTextBox 
control.

NOTE: If you open the LaunchPadForm in the designer view without having compiled
the EventBrokerQuickStart project, you'll get a design-time error:
"Could not find type 'EventBrokerDemo.TraceTextBox'. Please make sure that 
the assembly that contains this type is referenced. If this type is a part of 
your development project, make sure that the project has been successfully 
built."
This error will not appear after you compile the EventBrokerQuickStart and
reopen the LaunchPadForm.

For more information about the EventBroker QuickStart, see the QuickStarts section 
in the application block documentation.
