﻿using System;
using System.Collections.Generic;
using System.Linq;
using Transformalize.Libs.Newtonsoft.Json;
using Transformalize.Libs.Newtonsoft.Json.Linq;
using Transformalize.Libs.Elasticsearch.Net.Domain.Response;
using Transformalize.Libs.Nest.Domain.Analysis.Analyzers;
using Transformalize.Libs.Nest.Domain.Mapping.Types;
using Transformalize.Libs.Nest.Domain.Settings;
using Transformalize.Libs.Nest.Domain.Similarity;
using Transformalize.Libs.Nest.Exception;
using Transformalize.Libs.Nest.Extensions;

namespace Transformalize.Libs.Nest.Resolvers.Converters
{
	public class IndexSettingsConverter : JsonConverter
	{
		private void WriteSettingObject(JsonWriter writer, JObject obj)
		{
			writer.WriteStartObject();
			foreach (var property in obj.Children<JProperty>())
			{
				writer.WritePropertyName(property.Name);
				if (property.Value is JObject)
					this.WriteSettingObject(writer, property.Value as JObject);
				else
					writer.WriteValue(property.Value);
			}
			writer.WriteEndObject();

		}
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var indexSettings = (IndexSettings)value;

			writer.WriteStartObject();

			WriteSettings(writer, serializer, indexSettings);
			
			WriteMappings(writer, serializer, indexSettings);
			
			WriteWarmers(writer, serializer, indexSettings);
			
			WriteAliases(writer, serializer, indexSettings);
			
			writer.WriteEndObject();
		}

		private void WriteSettings(JsonWriter writer, JsonSerializer serializer, IndexSettings indexSettings)
		{
			writer.WritePropertyName("settings");
			writer.WriteStartObject();
			WriteIndexSettings(writer, serializer, indexSettings);
			writer.WriteEndObject();
		}

		private static void WriteAliases(JsonWriter writer, JsonSerializer serializer, IndexSettings indexSettings)
		{
			if (indexSettings.Aliases == null || indexSettings.Aliases.Count <= 0) return;
			writer.WritePropertyName("aliases");
			serializer.Serialize(writer, indexSettings.Aliases);
		}
		
		private static void WriteWarmers(JsonWriter writer, JsonSerializer serializer, IndexSettings indexSettings)
		{
			if (indexSettings.Warmers.Count <= 0) return;
			writer.WritePropertyName("warmers");
			serializer.Serialize(writer, indexSettings.Warmers);
		}

		private static void WriteMappings(JsonWriter writer, JsonSerializer serializer, IndexSettings indexSettings)
		{
			if (indexSettings.Mappings.Count <= 0) return;
			var contract = serializer.ContractResolver as SettingsContractResolver;
			if (contract == null || contract.ConnectionSettings == null) return;
			
			writer.WritePropertyName("mappings");
			serializer.Serialize(
				writer,
				indexSettings.Mappings.ToDictionary(m =>
				{
					var name = contract.Infer.PropertyName(m.Name);
					if (name.IsNullOrEmpty())
						throw new DslException("{0} should have a name!".F(m.GetType()));
					return name;
				})
			);
		}

		private void WriteIndexSettings(JsonWriter writer, JsonSerializer serializer, IndexSettings indexSettings)
		{
			writer.WritePropertyName("index");
			writer.WriteStartObject();
			if (indexSettings.Settings.HasAny())
			{
				foreach (var kv in indexSettings.Settings)
				{
					writer.WritePropertyName(kv.Key);
					if (kv.Value is JObject)
						this.WriteSettingObject(writer, kv.Value as JObject);
					else
						writer.WriteValue(kv.Value);
				}
			}

			if (
				indexSettings.Analysis.Analyzers.Count > 0
				|| indexSettings.Analysis.TokenFilters.Count > 0
				|| indexSettings.Analysis.Tokenizers.Count > 0
				|| indexSettings.Analysis.CharFilters.Count > 0
				)
			{
				writer.WritePropertyName("analysis");
				serializer.Serialize(writer, indexSettings.Analysis);
			}

			if (
				indexSettings.Similarity.CustomSimilarities.Count > 0
				|| !string.IsNullOrEmpty(indexSettings.Similarity.Default)
				)
			{
				writer.WritePropertyName("similarity");
				serializer.Serialize(writer, indexSettings.Similarity);
			}

			writer.WriteEndObject();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
										JsonSerializer serializer)
		{
			JObject o = JObject.Load(reader);
			var result = new IndexSettings();
			var dictionary = new Dictionary<string, object>();
			serializer.Populate(o.CreateReader(), dictionary);
			result.Settings = dictionary;
			result.AsExpando = DynamicDictionary.Create(dictionary);
			foreach (var rootProperty in o.Children<JProperty>())
			{
				if (rootProperty.Name.Equals("analysis", StringComparison.InvariantCultureIgnoreCase))
				{
					result.Analysis = serializer.Deserialize<AnalysisSettings>(rootProperty.Value.CreateReader());
					result.Settings.Remove(rootProperty.Name);
				}
				else if (rootProperty.Name.Equals("warmers", StringComparison.InvariantCultureIgnoreCase))
				{
					foreach (var jWarmer in rootProperty.Value.Children<JProperty>())
					{
						result.Warmers[jWarmer.Name] = serializer.Deserialize<WarmerMapping>(jWarmer.Value.CreateReader());
					}
					result.Settings.Remove(rootProperty.Name);
				}
				else if (rootProperty.Name.Equals("similarity", StringComparison.InvariantCultureIgnoreCase))
				{
					result.Similarity = serializer.Deserialize<SimilaritySettings>(rootProperty.Value.CreateReader());
					result.Settings.Remove(rootProperty.Name);
				}
			}
			return result;
		}

		private static Type _type = typeof(IndexSettings);
		public override bool CanConvert(Type objectType)
		{
			return objectType == _type;
		}
	}
}