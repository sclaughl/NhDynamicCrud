using System;
using System.IO;
using System.Reflection;
using System.Web;
using Bistrotech.NamedEntities;
using Bistrotech.NHibernate;
using Castle.Facilities.NHibernateIntegration;
using Castle.MicroKernel.Registration;
using Castle.MonoRail.Framework;
using Castle.MonoRail.Framework.Configuration;
using Castle.MonoRail.Framework.Internal;
using Castle.MonoRail.Framework.Views.NVelocity;
using Castle.MonoRail.WindsorExtension;
using Castle.Windsor;
using Component = Castle.MicroKernel.Registration.Component;

namespace Bistrotech.DataMaintenanceApp
{
	public class Global : HttpApplication, IMonoRailConfigurationEvents, IContainerAccessor
	{
		private static IWindsorContainer container;

		protected void Application_Start(object sender, EventArgs e)
		{
			container = new WindsorContainer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Windsor.config"));

			container.AddFacility<MonoRailFacility>();
			container.Register(AllTypes.Of<IController>().FromAssembly(Assembly.GetExecutingAssembly()));

			container.Register(
				Component.For<DefinitionProvider>(),
				Component.For<DefinitionCreator>(),
				Component.For<PersistentClassTranslator>()
			);

			container.AddComponent<IConfigurationContributor, EntityDefinitionContributor>();
			container.AddComponent<INamedEntityRepository, NHNamedEntityRepository>();

			container.Resolve<IConfigurationContributor>();
			//container.Resolve<ITranslateFrom<PersistentClass>>();
		}

		protected void Application_End(object sender, EventArgs e)
		{
			container.Dispose();
		}

		public void Configure(IMonoRailConfiguration configuration)
		{
			configuration.ViewEngineConfig.ViewPathRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views");
			configuration.ViewEngineConfig.ViewEngines.Add(new ViewEngineInfo(typeof(NVelocityViewEngine), false));
		}

		public IWindsorContainer Container
		{
			get { return container; }
		}
	}
}