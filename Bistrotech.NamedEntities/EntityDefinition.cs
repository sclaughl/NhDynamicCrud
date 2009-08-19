using System;
using System.Collections;
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
		public KeyType KeyType { get; set; }

		/// <summary>
		/// Fields collection does not include the key.
		/// </summary>
		public IList<Field> Fields { get { return fields; } }
		public bool Undeletable { get; set; }

		public bool KeyIsAssignable
		{ get { return KeyType == KeyType.Assigned; } }

		public Field GetFieldByName(string fieldName)
		{
			return fieldName == Key.Name ? Key : fields.Find(f => f.Name == fieldName);
		}

		public IDictionary ApplyTypesToEntity(IDictionary entity)
		{
			var typed = new Hashtable();

			foreach (string key in entity.Keys)
				typed[key] = ApplyType(entity[key].ToString(), GetFieldByName(key));

			return typed;
		}

		private object ApplyType(string value, Field field)
		{
			if (string.IsNullOrEmpty(value))
				return null;

			switch (field.DataType)
			{
				case (DataType.Integer):
					return int.Parse(value);

				case (DataType.DateTime):
					return DateTime.Parse(value);

				default:
					return value;
			}
		}
	}
}