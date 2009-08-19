namespace Bistrotech.NamedEntities
{
	public interface ITranslateFrom<T>
	{
		EntityDefinition TranslateFrom(T entityInfo);
	}
}