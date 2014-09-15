﻿using System;
using System.Collections.Generic;
using System.Linq;
using Transformalize.Libs.Newtonsoft.Json;
using Transformalize.Libs.Elasticsearch.Net.Domain;
using Transformalize.Libs.Elasticsearch.Net.Domain.RequestParameters;
using Transformalize.Libs.Nest.Domain.Connection;
using Transformalize.Libs.Nest.Domain.Paths;
using Transformalize.Libs.Nest.DSL.Paths;
using Transformalize.Libs.Nest.Extensions;

namespace Transformalize.Libs.Nest.DSL
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public interface IClusterStateRequest : IIndicesOptionalPath<ClusterStateRequestParameters>
	{
		IEnumerable<ClusterStateMetric> Metrics { get; set; }
	}

	internal static class ClusterStatePathInfo
	{
		public static void Update(ElasticsearchPathInfo<ClusterStateRequestParameters> pathInfo, IClusterStateRequest request)
		{
			pathInfo.HttpMethod = PathInfoHttpMethod.GET;
			if (request.Metrics != null)
				pathInfo.Metric = request.Metrics.Cast<Enum>().GetStringValue();
		}
	}
	
	public partial class ClusterStateRequest : IndicesOptionalPathBase<ClusterStateRequestParameters>, IClusterStateRequest
	{
		public IEnumerable<ClusterStateMetric> Metrics { get; set; }

		protected override void UpdatePathInfo(IConnectionSettingsValues settings, ElasticsearchPathInfo<ClusterStateRequestParameters> pathInfo)
		{
			ClusterStatePathInfo.Update(pathInfo, this);
		}

	}


	public partial class ClusterStateDescriptor : IndicesOptionalPathDescriptor<ClusterStateDescriptor, ClusterStateRequestParameters>, IClusterStateRequest
	{
		private IClusterStateRequest Self { get { return this; } }

		IEnumerable<ClusterStateMetric> IClusterStateRequest.Metrics { get; set; }
		public ClusterStateDescriptor Metrics(params ClusterStateMetric[] metrics)
		{
			Self.Metrics = metrics;
			return this;
		}

		protected override void UpdatePathInfo(IConnectionSettingsValues settings, ElasticsearchPathInfo<ClusterStateRequestParameters> pathInfo)
		{
			ClusterStatePathInfo.Update(pathInfo, this);
		}
	}
}
