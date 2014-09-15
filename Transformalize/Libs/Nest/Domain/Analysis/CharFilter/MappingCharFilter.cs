﻿using System.Collections.Generic;
using Transformalize.Libs.Newtonsoft.Json;

namespace Transformalize.Libs.Nest.Domain.Analysis.CharFilter
{
	/// <summary>
	/// A char filter of type mapping replacing characters of an analyzed text with given mapping.
	/// </summary>
	public class MappingCharFilter : CharFilterBase
	{
		public MappingCharFilter()
			: base("mapping")
		{

		}
		[JsonProperty("mappings")]
		public IEnumerable<string> Mappings { get; set; }

		[JsonProperty("mappings_path")]
		public string MappingsPath { get; set; }
	}

}