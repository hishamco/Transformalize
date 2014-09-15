﻿using System.Collections.Generic;
using Transformalize.Libs.Newtonsoft.Json;
using Transformalize.Libs.Nest.Domain.Hit;
using Transformalize.Libs.Nest.Domain.Status;
using Transformalize.Libs.Nest.Resolvers.Converters;

namespace Transformalize.Libs.Nest.Domain.Responses
{
	public interface IStatusResponse : IResponse
	{
		ShardsMetaData Shards { get; }
		Dictionary<string, IndexStatus> Indices { get; }
	}

	[JsonObject]
	public class StatusResponse : BaseResponse, IStatusResponse
	{


		[JsonProperty(PropertyName = "_shards")]
		public ShardsMetaData Shards { get; internal set; }

		[JsonProperty("indices")]
		[JsonConverter(typeof(DictionaryKeysAreNotPropertyNamesJsonConverter))]
		public Dictionary<string, IndexStatus> Indices { get; internal set; }

	}
}
