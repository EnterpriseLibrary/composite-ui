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
//===============================================================================

namespace Microsoft.Practices.ObjectBuilder.Tests
{
	//[TestClass]
	//public class LifecycleWorkItemFixture
	//{
	//   [TestMethod]
	//   public void CanInjectDependenciesFromStrategyChainQuickAndDirty()
	//   {
	//      DependencyInjectionContainer2 container = new DependencyInjectionContainer2();
	//      container.AddService(typeof(ITraceSourceCatalogService), new TraceSourceCatalogService());
	//      container.AddService(typeof(IService), new MyService());

	//      Component comp = new Component();
	//      container.Add(comp, "Foo");

	//      ClassWithDependencies obj = container.AddNew<ClassWithDependencies>();

	//      Assert.IsNotNull(obj.service);
	//      Assert.AreSame(comp, obj.component);
	//      Assert.IsNotNull(obj.source);
	//      Assert.AreEqual("MySource", obj.source.Name);
	//   }

	//   private interface IService { }
	//   private class MyService : IService { }

	//   private class ClassWithDependencies
	//   {
	//      [ServiceDependency]
	//      public IService service = null;

	//      [ComponentDependency("Foo")]
	//      public Component component = null;

	//      [TraceSource("MySource")]
	//      public TraceSource source = null;
	//   }

	//   private class DependencyInjectionContainer2 : WorkItem, ILifetimeContainer
	//   {
	//      ICompositionStrategyContext compositionContext;

	//      public DependencyInjectionContainer2()
	//      {
	//         compositionContext = new DirtyCompositionContext(this);
	//      }

	//      public override object GetService(Type serviceType)
	//      {
	//         if (serviceType == typeof(IComponentChangeService) ||
	//            serviceType == typeof(IServiceChangeService))
	//         {
	//            // This will turn off all the monitors as they query for these 
	//            // services in order to hook to the events.
	//            return null;
	//         }

	//         return base.GetService(serviceType);
	//      }

	//      protected override void OnComponentAdded(object sender, IComponent component)
	//      {
	//         base.OnComponentAdded(sender, component);
	//         // Will be null while the base class is adding the old monitors and built-in services.
	//         compositionContext.StrategyChain.Run(compositionContext, component.GetType(), component, component.Site.Name);
	//      }

	//      #region Composition classes

	//      private class DirtyCompositionContext : ICompositionStrategyContext, IServiceProvider
	//      {
	//         ICompositionStrategyChain chain;
	//         IReadWriteLocator locator = new Locator();
	//         DependencyInjectionContainer2 container;

	//         public DirtyCompositionContext(DependencyInjectionContainer2 container)
	//         {
	//            this.container = container;
	//            chain = new CompositionStrategyChain();

	//            // Setup our strategies in the chain.
	//            chain.Add(new TraceSourceDependencyInjectionStrategy());
	//            chain.Add(new ServiceDependencyInjectionStrategy());
	//            chain.Add(new ComponentDependencyInjectionStrategy());
	//         }

	//         #region ICompositionStrategyContext Members

	//         public ICompositionStrategyChain StrategyChain
	//         {
	//            get { return chain; }
	//         }

	//         public IReadWriteLocator Locator
	//         {
	//            get { return locator; }
	//         }

	//         public ILifetimeContainer LifetimeContainer
	//         {
	//            get { return container; }
	//         }

	//         #endregion

	//         #region IServiceProvider Members

	//         public object GetService(Type serviceType)
	//         {
	//            return container.GetService(serviceType);
	//         }

	//         #endregion
	//      }

	//      private class TraceSourceDependencyInjectionStrategy : AbstractCompositionStrategy
	//      {
	//         public override object Run(ICompositionStrategyContext context, Type t, object existing, string id)
	//         {
	//            DependencyInspector di = new DependencyInspector((IServiceProvider)context);
	//            BindingFlags flags = System.Reflection.BindingFlags.Instance | BindingFlags.Static | System.Reflection.BindingFlags.Public;
	//            di.InjectDependencies(
	//               ComponentAdapter.Wrap(existing),
	//               typeof(TraceSourceAttribute), OnDependencyFound, flags);

	//            return RunNext(context, t, existing, id);
	//         }

	//         #region Copy-pasted from TraceSourceMonitor

	//         private object OnDependencyFound(object sender, DependencyMemberEventArgs e)
	//         {
	//            TraceSourceAttribute attrib = (TraceSourceAttribute)e.DependencyAttribute;
	//            ITraceSourceCatalogService svc = (ITraceSourceCatalogService)
	//               e.InspectedComponent.Site.GetService(typeof(ITraceSourceCatalogService));

	//            if (svc != null)
	//               return svc.GetTraceSource(attrib.SourceName);
	//            else
	//               return new NullTraceSource();
	//         }

	//         #endregion
	//      }

	//      private class ServiceDependencyInjectionStrategy : AbstractCompositionStrategy
	//      {
	//         public override object Run(ICompositionStrategyContext context, Type t, object existing, string id)
	//         {
	//            new DependencyInspector((IServiceProvider)context.LifetimeContainer).InjectDependencies(
	//               ComponentAdapter.Wrap(existing), typeof(ServiceDependencyAttribute), OnDependencyCallback);
	//            return RunNext(context, t, existing, id);
	//         }

	//         #region Copy-pasted from ServiceDependencyMonitor

	//         /// <devdoc>
	//         /// Retrieves the service for the dependency.
	//         /// </devdoc>
	//         private object OnDependencyCallback(object sender, DependencyMemberEventArgs e)
	//         {
	//            if (e.InspectedComponent.Site == null)
	//            {
	//               //TODO: throw new InvalidOperationException(Properties.Resources.General_ComponentNotSited);
	//               throw new InvalidOperationException();
	//            }

	//            ServiceDependencyAttribute dependencyAttribute = (ServiceDependencyAttribute)e.DependencyAttribute;
	//            Type serviceType;

	//            if (dependencyAttribute.ServiceType != null)
	//            {
	//               serviceType = dependencyAttribute.ServiceType;
	//            }
	//            else
	//            {
	//               serviceType = e.MemberType;
	//            }

	//            object serviceInstance = e.InspectedComponent.Site.GetService(serviceType);
	//            // Always check compatibility of types.
	//            if (serviceInstance != null && !e.MemberType.IsAssignableFrom(serviceInstance.GetType()))
	//            {
	//               //TODO: throw new InvalidOperationException(String.Format(
	//               //        CultureInfo.CurrentCulture,
	//               //        Properties.Resources.ServiceDependencyMonitor_InvalidServiceType,
	//               //        serviceType, e.MemberName,
	//               //        ComponentInfo.GetComponentName(e.InspectedComponent)));
	//               throw new InvalidOperationException();
	//            }
	//            if (serviceInstance == null)
	//            {
	//               // The exception type is shared with the ServiceHelper class.
	//               throw new ServiceMissingException(serviceType, e.InspectedComponent);
	//            }

	//            return serviceInstance;
	//         }

	//         #endregion
	//      }

	//      private class ComponentDependencyInjectionStrategy : AbstractCompositionStrategy
	//      {
	//         public override object Run(ICompositionStrategyContext context, Type t, object existing, string id)
	//         {
	//            new DependencyInspector((IServiceProvider)context.LifetimeContainer).InjectDependencies(
	//               ComponentAdapter.Wrap(existing), typeof(ComponentDependencyAttribute), OnDependencyCallback);

	//            return RunNext(context, t, existing, id);
	//         }

	//         #region Copy-pasted from ComponentDependencyMonitor

	//         /// <devdoc>
	//         /// Gets an existing component or creates a new one, depending on the configuration of the 
	//         /// dependencyAttribute received.
	//         /// </devdoc>
	//         private object OnDependencyCallback(object sender, DependencyMemberEventArgs e)
	//         {
	//            if (e.InspectedComponent.Site == null)
	//            {
	//               // TODO: throw new InvalidOperationException(Properties.Resources.General_ComponentNotSited);
	//               throw new InvalidOperationException();
	//            }

	//            Type determinedDependencyType;
	//            ComponentDependencyAttribute dependencyAttribute = (ComponentDependencyAttribute)e.DependencyAttribute;

	//            // Determine target component type
	//            if (dependencyAttribute.ComponentType != null)
	//            {
	//               determinedDependencyType = dependencyAttribute.ComponentType;
	//            }
	//            else
	//            {
	//               determinedDependencyType = e.MemberType;
	//            }

	//            object dependencyInstance = null;

	//            // If a name is specified, no search for matching type will be performed.
	//            if (dependencyAttribute.ComponentName != null)
	//            {
	//               ReadOnlyDictionary<string, object> items = ((WorkItem)e.InspectedComponent.Site.Container).Items;
	//               if (items.ContainsKey(dependencyAttribute.ComponentName))
	//               {
	//                  dependencyInstance = items[dependencyAttribute.ComponentName];
	//               }
	//            }
	//            else
	//            {
	//               // Search for the first with a (loose) matching type. 
	//               IEnumerable collection = ((WorkItem)e.InspectedComponent.Site.Container).Items.Values;

	//               foreach (object candidate in collection)
	//               {
	//                  object realCandidate = ComponentAdapter.Unwrap(candidate);

	//                  // Don't assign to self.
	//                  if (candidate != e.InspectedComponent && determinedDependencyType.IsAssignableFrom(realCandidate.GetType()))
	//                  {
	//                     dependencyInstance = realCandidate;
	//                     break;
	//                  }
	//               }
	//            }

	//            // On-demand creation of the dependent component.
	//            if (dependencyInstance == null && dependencyAttribute.CreateInstance)
	//            {
	//               if (determinedDependencyType.GetConstructor(Type.EmptyTypes) == null)
	//               {
	//                  // TODO: throw new InvalidOperationException(String.Format(
	//                  //    CultureInfo.CurrentCulture,
	//                  //    Properties.Resources.ComponentDependencyMonitor_ComponentNoConstructor,
	//                  //    determinedDependencyType, e.MemberName,
	//                  //    ComponentInfo.GetComponentName(e.InspectedComponent)));
	//                  throw new InvalidOperationException();
	//               }
	//               dependencyInstance = Activator.CreateInstance(determinedDependencyType);

	//               // Site an adapter for the dependency if necessary. The name can be null for unnamed components.
	//               ((WorkItem)e.InspectedComponent.Site.Container).Add(dependencyInstance, dependencyAttribute.ComponentName);
	//            }

	//            if (dependencyInstance == null)
	//            {
	//               // TODO: throw new InvalidOperationException(String.Format(
	//               //    CultureInfo.CurrentCulture,
	//               //    Properties.Resources.ComponentDependencyMonitor_MissingDependency,
	//               //    e.MemberName, ComponentInfo.GetComponentName(e.InspectedComponent),
	//               //    dependencyAttribute));
	//               throw new InvalidOperationException();
	//            }

	//            return dependencyInstance;
	//         }

	//         #endregion
	//      }

	//      #endregion
	//   }
	//}
}
