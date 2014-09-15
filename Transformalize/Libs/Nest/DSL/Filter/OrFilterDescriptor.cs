﻿using System.Collections.Generic;
using System.Linq;
using Transformalize.Libs.Newtonsoft.Json;
using Transformalize.Libs.Nest.Extensions;
using Transformalize.Libs.Nest.Resolvers.Converters;

namespace Transformalize.Libs.Nest.DSL.Filter
{
	[JsonConverter(typeof(ReadAsTypeConverter<OrFilterDescriptor>))]
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public interface IOrFilter : IFilter
	{
		[JsonProperty("filters",
			ItemConverterType = typeof(CompositeJsonConverter<ReadAsTypeConverter<FilterContainer>, CustomJsonConverter>))]
		IEnumerable<IFilterContainer> Filters { get; set; }
	}

	public class OrFilter : PlainFilter, IOrFilter
	{
		protected internal override void WrapInContainer(IFilterContainer container)
		{
			container.Or = this;
		}

		public IEnumerable<IFilterContainer> Filters { get; set; }
	}

	public class OrFilterDescriptor : FilterBase, IOrFilter
	{

		IEnumerable<IFilterContainer> IOrFilter.Filters { get; set; }

		bool IFilter.IsConditionless
		{
			get
			{
				var af = ((IOrFilter)this);
				return !af.Filters.HasAny() || af.Filters.All(f=>f.IsConditionless);
			} 
		}
	}
}
