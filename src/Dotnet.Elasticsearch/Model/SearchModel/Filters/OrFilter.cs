﻿using System.Collections.Generic;
using Dotnet.Elasticsearch.Utils;

namespace Dotnet.Elasticsearch.Model.SearchModel.Filters
{
	/// <summary>
	/// A filter that matches documents using the OR boolean operator on other filters. Can be placed within queries that accept a filter.
	/// </summary>
	public class OrFilter : IFilter
	{
		private readonly List<IFilter> _or;

		public OrFilter(List<IFilter> or)
		{
			_or = or;
		}

		public void WriteJson(ElasticsearchCrudJsonWriter elasticsearchCrudJsonWriter)
		{
			elasticsearchCrudJsonWriter.JsonWriter.WritePropertyName("or");
			elasticsearchCrudJsonWriter.JsonWriter.WriteStartObject();

			WriteOrFilterList(elasticsearchCrudJsonWriter);

			elasticsearchCrudJsonWriter.JsonWriter.WriteEndObject();
		}

		private void WriteOrFilterList(ElasticsearchCrudJsonWriter elasticsearchCrudJsonWriter)
		{
			elasticsearchCrudJsonWriter.JsonWriter.WritePropertyName("filters");
			elasticsearchCrudJsonWriter.JsonWriter.WriteStartArray();

			foreach (var or in _or)
			{
				elasticsearchCrudJsonWriter.JsonWriter.WriteStartObject();
				or.WriteJson(elasticsearchCrudJsonWriter);
				elasticsearchCrudJsonWriter.JsonWriter.WriteEndObject();
			}

			elasticsearchCrudJsonWriter.JsonWriter.WriteEndArray();
		}
	}
}