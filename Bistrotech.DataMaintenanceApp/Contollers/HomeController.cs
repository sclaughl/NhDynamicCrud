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
			var tablesToMaintain = definitionProvider.Retrieve();
			PropertyBag["tables"] = tablesToMaintain;
		}
	}
}