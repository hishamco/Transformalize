﻿using System;
using System.Threading.Tasks;
using Transformalize.Libs.Elasticsearch.Net.Domain.RequestParameters;
using Transformalize.Libs.Nest.Domain.Responses;
using Transformalize.Libs.Nest.DSL;

namespace Transformalize.Libs.Nest
{
	public partial class ElasticClient
	{
		/// <inheritdoc />
		public IGlobalStatsResponse IndicesStats(Func<IndicesStatsDescriptor, IndicesStatsDescriptor> selector = null)
		{
			selector = selector ?? (s => s);
			return this.Dispatch<IndicesStatsDescriptor, IndicesStatsRequestParameters, GlobalStatsResponse>(
				selector,
				(p, d) => this.RawDispatch.IndicesStatsDispatch<GlobalStatsResponse>(p)
			);
		}

		/// <inheritdoc />
		public IGlobalStatsResponse IndicesStats(IIndicesStatsRequest statsRequest)
		{
			return this.Dispatch<IIndicesStatsRequest, IndicesStatsRequestParameters, GlobalStatsResponse>(
				statsRequest,
				(p, d) => this.RawDispatch.IndicesStatsDispatch<GlobalStatsResponse>(p)
			);
		}

		/// <inheritdoc />
		public Task<IGlobalStatsResponse> IndicesStatsAsync(Func<IndicesStatsDescriptor, IndicesStatsDescriptor> selector = null)
		{
			selector = selector ?? (s => s);
			return this.DispatchAsync<IndicesStatsDescriptor, IndicesStatsRequestParameters, GlobalStatsResponse, IGlobalStatsResponse>(
				selector,
				(p, d) => this.RawDispatch.IndicesStatsDispatchAsync<GlobalStatsResponse>(p)
			);
		}

		/// <inheritdoc />
		public Task<IGlobalStatsResponse> IndicesStatsAsync(IIndicesStatsRequest statsRequest)
		{
			return this.DispatchAsync<IIndicesStatsRequest, IndicesStatsRequestParameters, GlobalStatsResponse, IGlobalStatsResponse>(
				statsRequest,
				(p, d) => this.RawDispatch.IndicesStatsDispatchAsync<GlobalStatsResponse>(p)
			);
		}

	}
}