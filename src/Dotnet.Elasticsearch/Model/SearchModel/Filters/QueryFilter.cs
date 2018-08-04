﻿using Dotnet.Elasticsearch.Utils;

namespace Dotnet.Elasticsearch.Model.SearchModel.Filters
{
	/// <summary>
	/// Wraps any query to be used as a filter. Can be placed within queries that accept a filter.
	/// 
	/// Keep in mind that once you wrap a query as a filter, it loses query features like highlighting and scoring because these are not features supported by filters.
	/// 
	/// The result of the filter is not cached by default. The _cache can be set to true to cache the result of the filter. 
	/// This is handy when the same query is used on several (many) other queries. Note, the process of caching the first execution is higher when not caching 
	/// (since it needs to satisfy different queries).
	/// </summary>
	public class QueryFilter : IFilter
	{
		private readonly IQuery _query;

		public QueryFilter(IQuery query)
		{
			_query = query;
		}

		public void WriteJson(ElasticsearchCrudJsonWriter elasticsearchCrudJsonWriter)
		{
			elasticsearchCrudJsonWriter.JsonWriter.WritePropertyName("fquery");
			elasticsearchCrudJsonWriter.JsonWriter.WriteStartObject();

			elasticsearchCrudJsonWriter.JsonWriter.WritePropertyName("query");
			elasticsearchCrudJsonWriter.JsonWriter.WriteStartObject();

			_query.WriteJson(elasticsearchCrudJsonWriter);
			elasticsearchCrudJsonWriter.JsonWriter.WriteEndObject();
			elasticsearchCrudJsonWriter.JsonWriter.WriteEndObject();
		}
	}
}
