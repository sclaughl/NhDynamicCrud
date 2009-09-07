using Bistrotech.NamedEntities;
using Castle.MonoRail.Framework;

namespace Bistrotech.DataMaintenanceApp.Contollers
{
	[Layout("default"), Rescue("general")]
	public class HomeController : SmartDispatcherController
	{
		private readonly DefinitionProvider definitionProvider;

		public HomeController(DefinitionProvider definitionProvider)
		{
			this.definitionProvider = definitionProvider;
		}

		public void Index()
		{
			var entityTypes = definitionProvider.Retrieve();
			PropertyBag["entityTypes"] = entityTypes;
		}
	}
}