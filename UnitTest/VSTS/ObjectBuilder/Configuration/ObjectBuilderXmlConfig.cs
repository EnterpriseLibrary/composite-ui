//===============================================================================
// Microsoft patterns & practices
// Object Builder Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Microsoft.Practices.ObjectBuilder
{
	public partial class ObjectBuilderXmlConfig : IBuilderConfigurator<BuilderStage>
	{
		private IBuilder<BuilderStage> builder;

		public static ObjectBuilderXmlConfig FromXml(string xml)
		{
			return ParseXmlConfiguration(xml);
		}

		public void ApplyConfiguration(IBuilder<BuilderStage> builder)
		{
			this.builder = builder;
			ProcessStrategies();

			if (buildrules != null)
			{
				foreach (BuildRule buildRule in buildrules)
				{
					Type buildType = Type.GetType(buildRule.type);

					ProcessMappedType(buildRule, buildType);
					ProcessConstructor(buildRule, buildType);
					ProcessProperties(buildRule, buildType);
					ProcessSingleton(buildRule, buildType);
				}
			}
		}

		private void ProcessStrategies()
		{
			if (strategies != null)
			{
				if (!strategies.includedefault)
					builder.Strategies.Clear();

				if (strategies.strategy != null)
				{
					foreach (Strategy strategy in strategies.strategy)
					{
						// TODO: Config file needs stage designation
						builder.Strategies.Add((IBuilderStrategy)Activator.CreateInstance(Type.GetType(strategy.type)),
												  BuilderStage.PostInitialization);
					}
				}
			}
		}

		private void ProcessMappedType(BuildRule buildRule, Type buildType)
		{
			if (buildRule.mappedtype == null)
				return;

			Type type = Type.GetType(buildRule.mappedtype.type);
			TypeMappingPolicy policy = new TypeMappingPolicy(type, buildRule.mappedtype.name);
			builder.Policies.Set<ITypeMappingPolicy>(policy, buildType, buildRule.name);
		}

		private void ProcessProperties(BuildRule buildRule, Type buildType)
		{
			if (buildRule.property == null)
				return;

			PropertySetterPolicy policy = new PropertySetterPolicy();

			foreach (Property prop in buildRule.property)
				policy.Properties.Add(prop.name, new PropertySetterInfo(prop.name, GetParameterFromConfigParam(prop.Item)));

			builder.Policies.Set<IPropertySetterPolicy>(policy, buildType, buildRule.name);
		}

		private void ProcessSingleton(BuildRule buildRule, Type buildType)
		{
			if (buildRule.mode == Mode.Singleton)
				builder.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), buildType, buildRule.name);
		}

		private void ProcessConstructor(BuildRule buildRule, Type buildType)
		{
			if (buildRule.constructorparams == null)
				return;

			ConstructorPolicy policy = new ConstructorPolicy();

			foreach (object param in buildRule.constructorparams.Items)
				policy.AddParameter(GetParameterFromConfigParam(param));

			builder.Policies.Set<ICreationPolicy>(policy, buildType, buildRule.name);
		}

		private static IParameter GetParameterFromConfigParam(object param)
		{
			if (param is ValueParam)
			{
				return ValueParamToParameter(param as ValueParam);
			}
			else if (param is RefParam)
			{
				return RefParamToParameter(param as RefParam);
			}
			else
			{
				throw new ArgumentException("param");
			}
		}

		private static IParameter RefParamToParameter(RefParam refParam)
		{
			Type paramType = Type.GetType(refParam.type);
			return new CreationParameter(paramType, refParam.name);
		}

		private static IParameter ValueParamToParameter(ValueParam valueParam)
		{
			Type paramType = Type.GetType(valueParam.type);
			ValueParameter p = new ValueParameter(paramType, Convert.ChangeType(valueParam.Value, paramType));
			return p;
		}

		private static ObjectBuilderXmlConfig ParseXmlConfiguration(string config)
		{
			XmlSerializer ser = new XmlSerializer(typeof (ObjectBuilderXmlConfig));
			StringReader stringReader = new StringReader(config);
			XmlSchema schema =
				XmlSchema.Read(
					Assembly.GetExecutingAssembly().GetManifestResourceStream(
						"Microsoft.Practices.ObjectBuilder.Tests.Configuration.ObjectBuilderXmlConfig.xsd"), null);
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ValidationType = ValidationType.Schema;
			settings.Schemas.Add(schema);
			XmlReader reader = XmlReader.Create(stringReader, settings);
			ObjectBuilderXmlConfig configData = (ObjectBuilderXmlConfig) ser.Deserialize(reader);
			return configData;
		}
	}
}