using System.Collections;
using System.Collections.Specialized;
using Bistrotech.NamedEntities;
using Castle.MonoRail.Framework;

namespace Bistrotech.DataMaintenanceApp.Contollers
{
	[Layout("default"), Rescue("general")]
	public class MaintenanceController : SmartDispatcherController
	{
		private readonly INamedEntityRepository namedEntityRepository;
		private readonly DefinitionProvider definitionProvider;

		public MaintenanceController(INamedEntityRepository namedEntityRepository, DefinitionProvider definitionProvider)
		{
			this.namedEntityRepository = namedEntityRepository;
			this.definitionProvider = definitionProvider;
		}

		public void Index(string entityName)
		{
			var entities = namedEntityRepository.FindAll(entityName);
			PropertyBag["entities"] = entities;
		}

		public void New(string entityName)
		{
			PropertyBag["entityDefinition"] = definitionProvider.Retrieve(entityName);
		}

		public void Create(string entityName, IDictionary entity)
		{
			namedEntityRepository.Create(entityName, entity);
			RedirectToIndex(entityName);
		}

		public void Edit(string entityName, object id)
		{
			PropertyBag["entityDefinition"] = definitionProvider.Retrieve(entityName);
			PropertyBag["entity"] = namedEntityRepository.GetById(entityName, id);
		}

		public void Update(string entityName, IDictionary entity)
		{
			namedEntityRepository.Update(entityName, entity);
			RedirectToIndex(entityName);
		}

		public void Delete(string entityName, object id)
		{
			namedEntityRepository.Delete(entityName, id);
			RedirectToIndex(entityName);
		}

		private void RedirectToIndex(string entityName)
		{
			RedirectToAction("index", new NameValueCollection { { "entityName", entityName } });
		}
	}
}