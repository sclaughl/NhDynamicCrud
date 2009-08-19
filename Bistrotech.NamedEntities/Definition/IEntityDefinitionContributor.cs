using System.IO;

namespace Asd.Core.NamedEntities.Definition
{
	public interface IEntityDefinitionContributor
	{
		void Process(EntityDefinition entityDefinition, FileInfo file);
	}
}