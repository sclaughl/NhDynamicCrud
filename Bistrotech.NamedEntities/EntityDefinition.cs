using System.Collections.Generic;
using Bistrotech.NamedEntities.Definition;

namespace Bistrotech.NamedEntities
{
	public class EntityDefinition
	{
		private readonly List<Field> fields;

		public EntityDefinition()
		{
			fields = new List<Field>();
		}

		public string EntityName { get; set; }
		public Field Key { get; set; }

		/// <summary>
		/// Fields collection does not include the key.
		/// </summary>
		public IList<Field> Fields { get { return fields; } }
	}
}