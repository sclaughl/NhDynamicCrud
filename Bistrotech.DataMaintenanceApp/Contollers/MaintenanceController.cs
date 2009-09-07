using System.Collections;
using System.Collections.Specialized;
using Bistrotech.DataMaintenanceApp.ParameterBinders;
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

			// following needed to build edit/delete links
			var definition = definitionProvider.Retrieve(entityName);
			PropertyBag["entityName"] = entityName;
			PropertyBag["idName"] = definition.Key.Name;
		}

		public void New(string entityName)
		{
			PropertyBag["entityDefinition"] = definitionProvider.Retrieve(entityName);
		}

		public void Create(string entityName, [DictBind("entity")] IDictionary entity)
		{
			namedEntityRepository.Create(entityName, entity);
			RedirectToIndex(entityName);
		}

		public void Edit(string entityName, int id)
		{
			PropertyBag["entityDefinition"] = definitionProvider.Retrieve(entityName);
			PropertyBag["entity"] = namedEntityRepository.GetById(entityName, id);
		}

		public void Update(string entityName, [DictBind("entity")] IDictionary entity)
		{
			// HACK: key is always int
			var entityDefinition = definitionProvider.Retrieve(entityName);
			entity[entityDefinition.Key.Name] = int.Parse(entity[entityDefinition.Key.Name].ToString());

			namedEntityRepository.Update(entityName, entity, entityDefinition);
			RedirectToIndex(entityName);
		}

		public void Delete(string entityName, int id)
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