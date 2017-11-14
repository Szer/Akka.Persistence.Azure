﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Akka.Persistence.AzureTable
{
    public static class AzureTableExtensions
    {
        /// <summary>
        /// Initiates an asynchronous operation to execute a query and return all the results.
        /// </summary>
        /// <param name="table">Microsoft.WindowsAzure.Storage.Table.CloudTable representing the table</param>
        /// <param name="tableQuery">A Microsoft.WindowsAzure.Storage.Table.TableQuery representing the query to execute.</param>
        /// <param name="ct">A System.Threading.CancellationToken to observe while waiting for a task to complete.</param>
        public static async Task<IEnumerable<TElement>> ExecuteAsync<TElement>(
            this CloudTable table,
            TableQuery<TElement> tableQuery,
            CancellationToken ct) where TElement : ITableEntity, new()
        {
            var nextQuery = tableQuery;
            var continuationToken = default(TableContinuationToken);
            var results = new List<TElement>();

            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
                //Execute the next query segment async.
                //var queryResult = await nextQuery.ExecuteSegmentedAsync(continuationToken, ct);

                //Set exact results list capacity with result count.
                results.Capacity += queryResult.Results.Count;

                //Add segment results to results list.
                results.AddRange(queryResult.Results);

                continuationToken = queryResult.ContinuationToken;

                //Continuation token is not null, more records to load.
                if (continuationToken != null && tableQuery.TakeCount.HasValue)
                {
                    //Query has a take count, calculate the remaining number of items to load.
                    var itemsToLoad = tableQuery.TakeCount.Value - results.Count;

                    //If more items to load, update query take count, or else set next query to null.
                    nextQuery = itemsToLoad > 0
                        ? tableQuery.Take(itemsToLoad)
                        : null;
                }
            } while (continuationToken != null && nextQuery != null && !ct.IsCancellationRequested);

            return results;
        }
    }
}