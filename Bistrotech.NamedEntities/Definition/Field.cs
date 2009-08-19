namespace Bistrotech.NamedEntities.Definition
{
	public class Field
	{
		public string Name { get; set; }
		public bool Selectable { get; set; }
		public int MaxLength { get; set; }
		public FieldViewSettings ViewSettings { get; set; }
		public object ExampleValue { get; set; }
		public DataType DataType { get; set; }

		public Field()
		{
			Selectable = false;
			ViewSettings = new FieldViewSettings
							{
								VisibleOnIndex = true,
								VisibleOnEdit = true,
								VisibleOnNew = true,
								EditableOnEdit = true,
								EditableOnNew = true
							};
		}
	}

	public enum DataType
	{
		String,
		Integer,
		DateTime
	}
}