﻿using System.Collections.Generic;
using Transformalize.Libs.Newtonsoft.Json;
using Transformalize.Libs.Nest.Domain.Geometry;
using Transformalize.Libs.Nest.Domain.Marker;
using Transformalize.Libs.Nest.Extensions;

namespace Transformalize.Libs.Nest.DSL.Filter
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public interface IGeoShapeCircleFilter : IGeoShapeBaseFilter
	{
		[JsonProperty("shape")]
		ICircleGeoShape Shape { get; set; }
	}

	public class GeoShapeCircleFilter : PlainFilter, IGeoShapeCircleFilter
	{
		protected internal override void WrapInContainer(IFilterContainer container)
		{
			container.GeoShape = this;
		}

		public PropertyPathMarker Field { get; set; }

		public ICircleGeoShape Shape { get; set; }
	}

	public class GeoShapeCircleFilterDescriptor : FilterBase, IGeoShapeCircleFilter
	{
		IGeoShapeCircleFilter Self { get { return this; } }

		bool IFilter.IsConditionless
		{
			get
			{
				return this.Self.Shape == null || !this.Self.Shape.Coordinates.HasAny();
			}
		}

		PropertyPathMarker IFieldNameFilter.Field { get; set; }
		ICircleGeoShape IGeoShapeCircleFilter.Shape { get; set; }

		public GeoShapeCircleFilterDescriptor Coordinates(IEnumerable<double> coordinates)
		{
			if (this.Self.Shape == null)
				this.Self.Shape = new CircleGeoShape();
			this.Self.Shape.Coordinates = coordinates;
			return this;
		}

		public GeoShapeCircleFilterDescriptor Radius(string radius)
		{
			if (this.Self.Shape == null)
				this.Self.Shape = new CircleGeoShape();
			this.Self.Shape.Radius = radius;
			return this;
		}
	}

}
