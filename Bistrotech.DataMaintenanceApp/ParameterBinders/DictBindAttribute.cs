using System;
using System.Collections;
using System.Reflection;
using Castle.MonoRail.Framework;

namespace Bistrotech.DataMaintenanceApp.ParameterBinders
{
	public class DictBindAttribute : Attribute, IParameterBinder
	{
		private readonly string prefix;

		public DictBindAttribute(string prefix)
		{
			this.prefix = prefix;
		}

		public int CalculateParamPoints(IEngineContext context, IController controller, IControllerContext controllerContext, ParameterInfo parameterInfo)
		{
			return 10;
		}

		public object Bind(IEngineContext context, IController controller, IControllerContext controllerContext, ParameterInfo parameterInfo)
		{
			if (!typeof(IDictionary).IsAssignableFrom(parameterInfo.ParameterType))
				throw new ArgumentException("The parameter upon which this attribute is placed must be an IDictionary.");

			var binder = new DictionaryBinder();
			var node = context.Request.ObtainParamsNode(ParamStore.Form);
			var instance = binder.BindObject(parameterInfo.ParameterType, prefix, node);
			return instance;
		}
	}
}