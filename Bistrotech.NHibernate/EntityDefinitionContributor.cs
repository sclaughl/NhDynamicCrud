using System;
using System.Collections.Generic;
using System.IO;
using Bistrotech.NamedEntities;
using Castle.Facilities.NHibernateIntegration;
using NHibernate.Cfg;

namespace Bistrotech.NHibernate
{
	public class EntityDefinitionContributor : IConfigurationContributor
	{
		private readonly DefinitionCreator definitionCreator;
		private readonly DefinitionProvider definitionProvider;

		public EntityDefinitionContributor(DefinitionCreator definitionCreator, DefinitionProvider definitionProvider)
		{
			this.definitionCreator = definitionCreator;
			this.definitionProvider = definitionProvider;
		}

		public void Process(string name, Configuration nhConfig)
		{
			foreach (var file in GetMappingFiles())
			{
				nhConfig.AddFile(file);
				// HBM.XML file must be named to match entity-name.
				var entityName = file.Name.Replace(".hbm.xml", string.Empty);
				var nhMappingInfo = nhConfig.GetClassMapping(entityName);
				var entityDefinition = definitionCreator.CreateFrom(nhMappingInfo);
				definitionProvider.Add(entityDefinition);
			}
		}

		private IEnumerable<FileInfo> GetMappingFiles()
		{
			var mappingDir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mappings"));
			return mappingDir.GetFiles("*.hbm.xml");
		}
	}
}