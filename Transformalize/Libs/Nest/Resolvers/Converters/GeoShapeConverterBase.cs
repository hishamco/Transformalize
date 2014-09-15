﻿using Transformalize.Libs.Newtonsoft.Json;
using Transformalize.Libs.Newtonsoft.Json.Linq;

namespace Transformalize.Libs.Nest.Resolvers.Converters
{
	public abstract class GeoShapeConverterBase : JsonConverter
	{
		public virtual T GetCoordinates<T>(JToken shape)
		{
			var coordinates = shape["coordinates"];
			if (coordinates != null)
				return coordinates.ToObject<T>();
			return default(T);
		}
	}
}
