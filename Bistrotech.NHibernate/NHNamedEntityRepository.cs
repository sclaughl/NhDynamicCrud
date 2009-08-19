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

		protected ISession GetSession()
		{
			var session = sessionManager.OpenSession();
			var childSession = session.GetSession(EntityMode.Map);
			return childSession;
		}

		public IDictionary GetById(string entityName, object id)
		{
			var session = GetSession();

			//HACK
			int idAsInt;
			if (int.TryParse(id.ToString(), out idAsInt)) // id was an integer
				return (IDictionary)session.Get(entityName, idAsInt);

			return (IDictionary)session.Get(entityName, id);
		}

		public IList FindAll(string entityName)
		{
			var session = GetSession();
			return session.CreateCriteria(entityName).List();
		}

		public IList SortedFindAll(string entityName, string sort, bool isDescending)
		{
			var session = GetSession();
			var crit = session.CreateCriteria(entityName);
			crit.AddOrder(isDescending ? Order.Desc(sort) : Order.Asc(sort));
			return crit.List();
		}

		public virtual object Create(string entityName, IDictionary entity)
		{
			object insertedKey;
			var session = GetSession();
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

		public virtual void Update(string entityName, IDictionary entity)
		{
			var session = GetSession();
			var mergedEntity = Merge(session, entityName, entity);
			session.Update(entityName, mergedEntity);
			session.Flush();
		}

		public virtual void Delete(string entityName, object id)
		{
			var entity = GetById(entityName, id);

			var session = GetSession();
			session.Delete(entityName, entity);
			session.Flush();
		}

		// This method should probably be implemented in NH, via session.Merge(entityName, entity).
		//  see: http://groups.google.com/group/nhusers/browse_frm/thread/5e5a55452d6a7cf0
		private IDictionary Merge(ISession session, string entityName, IDictionary entity)
		{
			// HACK: In order to use Get or Load I would have to know the key field of the entity.
			//	NH already knows which field is the key, so it should have a method to do this (e.g. Merge()).
			var qbeMatches = session.CreateCriteria(entityName).Add(Example.Create(entity)).List();

			if (qbeMatches.Count != 1)
				throw new ApplicationException("Rudimentary Merge failed for this entity. Please contact an administrator.");

			var persistedEntity = (IDictionary)qbeMatches[0];
			foreach (var key in entity.Keys)
				persistedEntity[key] = entity[key];

			return persistedEntity;
		}
	}
}