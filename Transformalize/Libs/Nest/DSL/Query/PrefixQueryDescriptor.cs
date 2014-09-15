﻿using Transformalize.Libs.Newtonsoft.Json;
using Transformalize.Libs.Newtonsoft.Json.Converters;
using Transformalize.Libs.Nest.Domain.Marker;
using Transformalize.Libs.Nest.DSL.Query.Behaviour;
using Transformalize.Libs.Nest.Enums;

namespace Transformalize.Libs.Nest.DSL.Query
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public interface IPrefixQuery : ITermQuery
	{
		[JsonProperty(PropertyName = "rewrite")]
		[JsonConverter(typeof (StringEnumConverter))]
		RewriteMultiTerm? Rewrite { get; set; }
	}

	public class PrefixQuery : PlainQuery, IPrefixQuery
	{
		protected override void WrapInContainer(IQueryContainer container)
		{
			container.Prefix = this;
		}

		bool IQuery.IsConditionless { get { return false; } }
		PropertyPathMarker IFieldNameQuery.GetFieldName()
		{
			return this.Field;
		}

		void IFieldNameQuery.SetFieldName(string fieldName)
		{
			this.Field = fieldName;
		}

		public PropertyPathMarker Field { get; set; }
		public object Value { get; set; }
		public double? Boost { get; set; }
		public RewriteMultiTerm? Rewrite { get; set; }
	}

	public class PrefixQueryDescriptor<T> : TermQueryDescriptorBase<PrefixQueryDescriptor<T>, T>, 
		IPrefixQuery where T : class
	{
		RewriteMultiTerm? IPrefixQuery.Rewrite { get; set; }

		public PrefixQueryDescriptor<T> Rewrite(RewriteMultiTerm rewrite)
		{
			((IPrefixQuery)this).Rewrite = rewrite;
			return this;
		}
		
	}
}
