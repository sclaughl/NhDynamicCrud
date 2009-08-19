using Bistrotech.NamedEntities;
using Bistrotech.NamedEntities.Definition;
using NHibernate.Mapping;
using NHibernate.Type;

namespace Bistrotech.NHibernate
{
	public class TranslateFromPersistentClass : ITranslateFrom<PersistentClass>
	{
		public EntityDefinition TranslateFrom(PersistentClass nhMapping)
		{
			var def = new EntityDefinition { EntityName = nhMapping.EntityName };

			var keyType = KeyType.Assigned;
			var keyVisibleOnIndex = true;

			var keyAsSimpleValue = nhMapping.Key as SimpleValue;
			if (keyAsSimpleValue != null && keyAsSimpleValue.IdentifierGeneratorStrategy == "native")
			{
				keyType = KeyType.SqlServerIdentity;
				keyVisibleOnIndex = false;
			}

			def.KeyType = keyType;

			// assume one key column per table, so break out of the foreach after one iteration
			var keyName = nhMapping.IdentifierProperty.Name;
			var keyDataType = ToDataType(nhMapping.IdentifierProperty.Type);
			foreach (Column keyCol in nhMapping.Key.ColumnIterator)
			{
				var key = new Field
				{
					Name = keyName,
					MaxLength = keyCol.Length,
					DataType = keyDataType
				};

				def.Key = key;
				def.Key.ViewSettings.VisibleOnIndex = keyVisibleOnIndex;
				break;
			}

			foreach (var property in nhMapping.PropertyIterator)
			{
				var maxlength = 0;
				var dataType = ToDataType(property.Type);

				// assume one column per property
				foreach (Column c in property.ColumnIterator)
				{
					maxlength = c.Length;
					break;
				}

				def.Fields.Add(new Field
				{
					Name = property.Name,
					MaxLength = maxlength,
					DataType = dataType
				});
			}

			return def;
		}

		private DataType ToDataType(IType nhDataType)
		{
			var type = nhDataType.GetType();

			if (type == typeof(Int16Type)
					|| type == typeof(Int32Type))
				return DataType.Integer;

			if (type == typeof(DateTimeType)
					|| type == typeof(DateTime2Type)
					|| type == typeof(DateType))
				return DataType.DateTime;

			return DataType.String;
		}



	}
}