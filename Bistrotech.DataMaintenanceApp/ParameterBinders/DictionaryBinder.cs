using System;
using System.Collections;
using System.Collections.Specialized;
using Castle.Components.Binder;

namespace Bistrotech.DataMaintenanceApp.ParameterBinders
{
	internal class DictionaryBinder
	{
		public IDictionary BindObject(Type targetType, string prefix, CompositeNode root)
		{
			if (typeof(IDictionary).IsAssignableFrom(targetType))
			{
				var dict = new HybridDictionary();

				// traverse from root and populate dict
				var entityRoot = root.GetChildNode(prefix);

				if (entityRoot.NodeType != NodeType.Composite)
					throw new ArgumentException(string.Format("CompositeNode with prefix '{0}' could not be found.", prefix));

				foreach (var node in ((CompositeNode)entityRoot).ChildNodes)
				{
					var leaf = node as LeafNode;
					if (leaf == null)
						throw new InvalidOperationException("only supporting one level for now");

					dict[node.Name] = leaf.Value;
				}
				return dict;
			}
			throw new ArgumentException(string.Format("Type {0} is not an IDictionary.", targetType));
		}
	}
}