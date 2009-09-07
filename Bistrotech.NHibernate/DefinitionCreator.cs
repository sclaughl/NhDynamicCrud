using Bistrotech.NamedEntities;
using NHibernate.Mapping;

namespace Bistrotech.NHibernate
{
	public class DefinitionCreator
	{
		private readonly PersistentClassTranslator translator;

		public DefinitionCreator(PersistentClassTranslator translator)
		{
			this.translator = translator;
		}

		public EntityDefinition CreateFrom(PersistentClass entityInfo)
		{
			return translator.TranslateFrom(entityInfo);
		}
	}
}