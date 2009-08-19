using System.Collections.Generic;

namespace Bistrotech.NamedEntities
{
	public class DefinitionProvider
	{
		private readonly List<EntityDefinition> definitions;

		public DefinitionProvider()
		{
			definitions = new List<EntityDefinition>();
		}

		public IEnumerable<EntityDefinition> Retrieve()
		{
			return definitions;
		}

		public void Add(EntityDefinition definition)
		{
			definitions.Add(definition);
		}

		public EntityDefinition Retrieve(string entityName)
		{
			return definitions.Find(def => def.EntityName == entityName);
		}
	}
}