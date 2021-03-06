﻿using Newtonsoft.Json.Linq;
using Dotnet.Solr.Builder;
using Dotnet.Solr.Search;
using Dotnet.Solr.Search.Parameter;
using Dotnet.Solr.Search.Parameter.Validation;
using Dotnet.Solr.Search.Query;
using Dotnet.Solr.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dotnet.Solr.Solr5.Search.Parameter
{
    [AllowMultipleInstances]
    [FieldMustBeIndexedTrue]
    public sealed class FacetSpatialParameter<TDocument> : IFacetSpatialParameter<TDocument>, ISearchItemExecution<JObject>
        where TDocument : Document
    {
        private JProperty _result;

        public FacetSpatialParameter(ExpressionBuilder<TDocument> expressionBuilder, ISolrExpressServiceProvider<TDocument> serviceProvider)
        {
            this.ExpressionBuilder = expressionBuilder;
            this.ServiceProvider = serviceProvider;
        }

        public string AliasName { get; set; }
        public GeoCoordinate CenterPoint { get; set; }
        public decimal Distance { get; set; }
        public string[] Excludes { get; set; }
        public ExpressionBuilder<TDocument> ExpressionBuilder { get; set; }
        public Expression<Func<TDocument, object>> FieldExpression { get; set; }
        public SpatialFunctionType FunctionType { get; set; }
        public int? Limit { get; set; }
        public int? Minimum { get; set; }
        public FacetSortType? SortType { get; set; }
        public ISolrExpressServiceProvider<TDocument> ServiceProvider { get; set; }
        public IList<IFacetParameter<TDocument>> Facets { get; set; }
        public SearchQuery<TDocument> Filter { get; set; }

        public void AddResultInContainer(JObject container)
        {
            var jObj = (JObject)container["facet"] ?? new JObject();
            jObj.Add(this._result);
            container["facet"] = jObj;
        }

        public void Execute()
        {
            var formule = ParameterUtil.GetSpatialFormule(
                this.ExpressionBuilder.GetFieldName(this.FieldExpression),
                this.FunctionType,
                this.CenterPoint,
                this.Distance);

            var array = new List<JProperty>
            {
                new JProperty("q", formule)
            };

            JProperty domain = null;
            if (this.Excludes?.Any() ?? false)
            {
                var excludeValue = new JObject(new JProperty("excludeTags", new JArray(this.Excludes)));
                domain = new JProperty("domain", excludeValue);
            }
            if (this.Filter != null)
            {
                var filter = new JProperty("filter", this.Filter.Execute());
                domain = domain ?? new JProperty("domain", new JObject());
                ((JObject)domain.Value).Add(filter);
            }
            if (domain != null)
            {
                array.Add(domain);
            }

            if (this.Minimum.HasValue)
            {
                array.Add(new JProperty("mincount", this.Minimum.Value));
            }

            if (this.SortType.HasValue)
            {
                ParameterUtil.GetFacetSort(this.SortType.Value, out string typeName, out string sortName);

                array.Add(new JProperty("sort", new JObject(new JProperty(typeName, sortName))));
            }

            this._result = new JProperty(this.AliasName, new JObject(new JProperty("query", new JObject(array.ToArray()))));
        }
    }
}
