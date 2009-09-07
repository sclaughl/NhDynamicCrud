using System;
using System.Collections;
using Bistrotech.NamedEntities;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Exceptions;

namespace Bistrotech.NHibernate
{
	public class NHNamedEntityRepository : INamedEntityRepository
	{
		protected ISessionManager sessionManager;

		public NHNamedEntityRepository(ISessionManager sessionManager)
		{
			this.sessionManager = sessionManager;
		}

		public IDictionary GetById(string entityName, object id)
		{
			var session = sessionManager.OpenSession();
			return (IDictionary)session.Get(entityName, id);
		}

		public IList FindAll(string entityName)
		{
			var session = sessionManager.OpenSession();
			return session.CreateCriteria(entityName).List();
		}

		public IList SortedFindAll(string entityName, string sort, bool isDescending)
		{
			var session = sessionManager.OpenSession();
			var crit = session.CreateCriteria(entityName);
			crit.AddOrder(isDescending ? Order.Desc(sort) : Order.Asc(sort));
			return crit.List();
		}

		public virtual object Create(string entityName, IDictionary entity)
		{
			object insertedKey;
			var session = sessionManager.OpenSession();
			try
			{
				insertedKey = session.Save(entityName, entity);
				session.Flush();
			}
			catch (GenericADOException ex)
			{
				if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
					throw new DuplicateKeyException("A record with this value already exists.", ex);

				throw;
			}
			return insertedKey;
		}

		public void Update(string entityName, IDictionary entity)
		{
			throw new NotImplementedException();
		}

		public virtual void Update(string entityName, IDictionary entity, EntityDefinition definition)
		{
			var session = sessionManager.OpenSession();
			//var mergedEntity = session.Merge(entityName, entity); // doesn't work -- see below
			var mergedEntity = Merge(session, definition, entity);
			session.Update(entityName, mergedEntity);
			session.Flush();
		}

		public virtual void Delete(string entityName, object id)
		{
			var entity = GetById(entityName, id);

			var session = sessionManager.OpenSession();
			session.Delete(entityName, entity);
			session.Flush();
		}

		// HACK:
		// My opinion is that this method should be implemented in NH, via session.Merge(string entityName, IDictionary entity).
		//  see: http://groups.google.com/group/nhusers/browse_frm/thread/5e5a55452d6a7cf0
		private IDictionary Merge(ISession session, EntityDefinition entityDefinition, IDictionary updatedEntity)
		{
			// How to Merge:
			//		1) obtain the persisted entity having same ID as updatedEntity
			//		2) copy values from updatedEntity to persistedEntity
			//		3) return persistedEntity
			//
			//	NH already knows which field is the key, so it should have a Merge method that takes an IDictionary.
			var persistedEntity = (IDictionary)session.Get(entityDefinition.EntityName, updatedEntity[entityDefinition.Key.Name]);
			foreach (var key in updatedEntity.Keys)
				persistedEntity[key] = updatedEntity[key];

			return persistedEntity;
		}
	}
}