﻿using Transformalize.Libs.Newtonsoft.Json;
using Transformalize.Libs.Nest.Resolvers.Converters;

namespace Transformalize.Libs.Nest.DSL.Filter
{
	[JsonConverter(typeof(ReadAsTypeConverter<NotFilterDescriptor>))]
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public interface INotFilter : IFilter
	{
		[JsonProperty("filter")]
		[JsonConverter(typeof(CompositeJsonConverter<ReadAsTypeConverter<FilterContainer>, CustomJsonConverter>))]
		IFilterContainer Filter { get; set; }
	}
	
	public class NotFilter : PlainFilter, INotFilter
	{
		protected internal override void WrapInContainer(IFilterContainer container)
		{
			container.Not = this;
		}

		public IFilterContainer Filter { get; set; }
	}

	public class NotFilterDescriptor : FilterBase, INotFilter
	{

		IFilterContainer INotFilter.Filter { get; set; }

		bool IFilter.IsConditionless
		{
			get
			{
				var nf = ((INotFilter)this);
				return nf.Filter == null || nf.Filter.IsConditionless;
			} 
		}
	}
}
