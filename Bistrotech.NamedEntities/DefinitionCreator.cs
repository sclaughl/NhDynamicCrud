namespace Bistrotech.NamedEntities
{
	public class DefinitionCreator<T>
	{
		private readonly ITranslateFrom<T> translator;

		public DefinitionCreator(ITranslateFrom<T> translator)
		{
			this.translator = translator;
		}

		public EntityDefinition CreateFrom(T entityInfo)
		{
			return translator.TranslateFrom(entityInfo);
		}
	}
}