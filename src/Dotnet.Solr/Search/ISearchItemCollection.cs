﻿using Newtonsoft.Json;
using Dotnet.Solr.Search.Parameter;
using Dotnet.Solr.Search.Result;
using System;
using System.Collections.Generic;

namespace Dotnet.Solr.Search
{
    /// <summary>
    /// Parameter collection
    /// </summary>
    public interface ISearchItemCollection<TDocument>
        where TDocument : Document
    {
        /// <summary>
        /// Get all results in internal list
        /// </summary>
        /// <returns>Results in internal list</returns>
        List<ISearchResult<TDocument>> GetSearchResults();

        /// <summary>
        /// Get all parameters in internal list
        /// </summary>
        /// <returns>Parameters in internal list</returns>
        List<ISearchParameter> GetSearchParameters();

        /// <summary>
        /// Check if collection contains informed type
        /// </summary>
        /// <returns>True if contains informed type, otherwise false</returns>
        bool Contains<TSearchItem>()
            where TSearchItem : ISearchItem;

        /// <summary>
        /// Check if collection contains informed type
        /// </summary>
        /// <param name="searchItemType">Type to check</param>
        /// <returns>True if contains informed type, otherwise false</returns>
        bool Contains(Type searchItemType);

        /// <summary>
        /// Add item to collection
        /// </summary>
        /// <param name="item">Item to add in collection</param>
        /// <returns>Itself</returns>
        void Add(ISearchItem item);

        /// <summary>
        /// Add items to collection
        /// </summary>
        /// <param name="items">Items to add in collection</param>
        /// <returns>Itself</returns>
        void AddRange(IEnumerable<ISearchItem> items);

        /// <summary>
        /// Execute items and get query instructions
        /// </summary>
        /// <param name="requestHandler">Handler to use in SOLR request</param>
        /// <returns>Query instructions</returns>
        JsonReader Execute(string requestHandler);
    }
}
