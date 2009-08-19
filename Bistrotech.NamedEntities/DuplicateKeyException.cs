using System;

namespace Bistrotech.NamedEntities
{
	public class DuplicateKeyException : ApplicationException
	{
		public DuplicateKeyException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}