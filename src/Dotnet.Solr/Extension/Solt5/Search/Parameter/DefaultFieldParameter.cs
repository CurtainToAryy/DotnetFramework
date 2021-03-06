﻿using Newtonsoft.Json.Linq;
using Dotnet.Solr.Builder;
using Dotnet.Solr.Search;
using Dotnet.Solr.Search.Parameter;
using System;
using System.Linq.Expressions;

namespace Dotnet.Solr.Solr5.Search.Parameter
{
    public sealed class DefaultFieldParameter<TDocument> : IDefaultFieldParameter<TDocument>, ISearchItemExecution<JObject>
        where TDocument : Document
    {
        private JProperty _result;

        public DefaultFieldParameter(ExpressionBuilder<TDocument> expressionBuilder)
        {
            this.ExpressionBuilder = expressionBuilder;
        }

        public ExpressionBuilder<TDocument> ExpressionBuilder { get; set; }
        public Expression<Func<TDocument, object>> FieldExpression { get; set; }

        public void AddResultInContainer(JObject container)
        {
            var jObj = (JObject)container["params"] ?? new JObject();
            jObj.Add(this._result);
            container["params"] = jObj;
        }

        public void Execute()
        {
            var fieldName = this.ExpressionBuilder.GetFieldName(this.FieldExpression);
            this._result = new JProperty("df", fieldName);
        }
    }
}
