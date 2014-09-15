﻿using System.Runtime.Serialization;
using Transformalize.Libs.Newtonsoft.Json;
using Transformalize.Libs.Newtonsoft.Json.Converters;

namespace Transformalize.Libs.Nest.Enums
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum NumberType
	{
		[EnumMember(Value = "default")]
		Default,
		[EnumMember(Value = "float")]
		Float,
		[EnumMember(Value = "double")]
		Double,
		[EnumMember(Value = "integer")]
		Integer,
		[EnumMember(Value = "long")]
		Long,
		[EnumMember(Value = "short")]
		Short,
		[EnumMember(Value = "byte")]
		Byte
	}
}
