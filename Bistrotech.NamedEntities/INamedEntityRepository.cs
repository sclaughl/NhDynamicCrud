using System.Collections;

namespace Bistrotech.NamedEntities
{
	public interface INamedEntityRepository
	{
		IDictionary GetById(string entityName, object id);
		IList FindAll(string entityName);
		object Create(string entityName, IDictionary entity);
		void Update(string entityName, IDictionary entity);
		void Update(string entityName, IDictionary entity, EntityDefinition definition); // this should go away
		void Delete(string entityName, object id);
	}
}