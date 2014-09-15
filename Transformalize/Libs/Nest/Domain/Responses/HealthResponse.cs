﻿using System.Collections.Generic;
using Transformalize.Libs.Newtonsoft.Json;
using Transformalize.Libs.Nest.Domain.Stats;
using Transformalize.Libs.Nest.Resolvers.Converters;

namespace Transformalize.Libs.Nest.Domain.Responses
{
	public interface IHealthResponse : IResponse
	{
		string ClusterName { get; }
		string Status { get; }
		bool TimedOut { get; }
		int NumberOfNodes { get; }
		int NumberOfDataNodes { get; }
		int ActivePrimaryShards { get; }
		int ActiveShards { get; }
		int RelocatingShards { get; }
		int InitializingShards { get; }
		int UnassignedShards { get; }
		Dictionary<string, IndexHealthStats> Indices { get; }
	}

	[JsonObject]
	public class HealthResponse : BaseResponse, IHealthResponse
	{
		public HealthResponse()
		{
			this.IsValid = true;
		}

		[JsonProperty(PropertyName = "cluster_name")]
		public string ClusterName { get; internal set; }
		[JsonProperty(PropertyName = "status")]
		public string Status { get; internal set; }
		[JsonProperty(PropertyName = "timed_out")]
		public bool TimedOut { get; internal set; }

		[JsonProperty(PropertyName = "number_of_nodes")]
		public int NumberOfNodes { get; internal set; }
		[JsonProperty(PropertyName = "number_of_data_nodes")]
		public int NumberOfDataNodes { get; internal set; }

		[JsonProperty(PropertyName = "active_primary_shards")]
		public int ActivePrimaryShards { get; internal set; }
		[JsonProperty(PropertyName = "active_shards")]
		public int ActiveShards { get; internal set; }
		[JsonProperty(PropertyName = "relocating_shards")]
		public int RelocatingShards { get; internal set; }
		[JsonProperty(PropertyName = "initializing_shards")]
		public int InitializingShards { get; internal set; }
		[JsonProperty(PropertyName = "unassigned_shards")]
		public int UnassignedShards { get; internal set; }

		[JsonProperty(PropertyName = "indices")]
		[JsonConverter(typeof(DictionaryKeysAreNotPropertyNamesJsonConverter))]
		public Dictionary<string, IndexHealthStats> Indices { get; set; }
	}
}
