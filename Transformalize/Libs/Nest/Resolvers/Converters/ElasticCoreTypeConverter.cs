﻿using System;
using System.Collections.Generic;
using Transformalize.Libs.Newtonsoft.Json;
using Transformalize.Libs.Newtonsoft.Json.Linq;
using Transformalize.Libs.Nest.Domain.Mapping.Types;
using Transformalize.Libs.Nest.Domain.Marker;

namespace Transformalize.Libs.Nest.Resolvers.Converters
{

	public class ElasticCoreTypeConverter : JsonConverter
	{
		public override bool CanWrite
		{
			get { return true; }
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			new DictionaryKeysAreNotPropertyNamesJsonConverter().WriteJson(writer, value, serializer);
		}

		private IElasticCoreType GetTypeFromJObject(JObject po, JsonSerializer serializer)
		{
			JToken typeToken;
			serializer.TypeNameHandling = TypeNameHandling.None;
			if (po.TryGetValue("type", out typeToken))
			{
				var type = typeToken.Value<string>().ToLowerInvariant();
				switch (type)
				{
					case "string":
						return serializer.Deserialize(po.CreateReader(), typeof(StringMapping)) as StringMapping;
					case "float":
					case "double":
					case "byte":
					case "short":
					case "integer":
					case "long":
						return serializer.Deserialize(po.CreateReader(), typeof(NumberMapping)) as NumberMapping;
					case "date":
						return serializer.Deserialize(po.CreateReader(), typeof(DateMapping)) as DateMapping;
					case "boolean":
						return serializer.Deserialize(po.CreateReader(), typeof(BooleanMapping)) as BooleanMapping;
					case "binary":
						return serializer.Deserialize(po.CreateReader(), typeof(BinaryMapping)) as BinaryMapping;
				}
			}
			return null;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
										JsonSerializer serializer)
		{
			var r = new Dictionary<PropertyNameMarker, IElasticCoreType>();

			JObject o = JObject.Load(reader);

			foreach (var p in o.Properties())
			{
				var name = p.Name;
				var po = p.First as JObject;
				if (po == null)
					continue;

				var esType = this.GetTypeFromJObject(po, serializer);
				if (esType == null)
					continue;

				r.Add(name, esType);

			}
			return r;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(IDictionary<string, IElasticCoreType>);
		}

	}
}