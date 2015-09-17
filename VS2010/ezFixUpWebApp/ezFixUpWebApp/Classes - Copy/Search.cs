/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ezFixUp.Data.LoveHitchParallel;

namespace ezFixUp.Data
{
    /// <summary>
    /// The base search results class
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [Serializable]
    public abstract class SearchResults<TKey, TValue>
    {
        /// <summary>
        /// Contains the results (usually only the IDs)
        /// </summary>
        protected TKey[] Results;

        /// <summary>
        /// Loads the result.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        protected abstract TValue LoadResult(TKey id);
        
        protected abstract TValue[] LoadResults(TKey[] ids);
        
        /// <summary>
        /// Gets the total pages.
        /// </summary>
        /// <param name="resultsPerPage">The results per page.</param>
        /// <returns></returns>
        public int GetTotalPages(int resultsPerPage)
        {
            if (Results == null) return 0;
            return (int) Math.Ceiling(
                             ((double) Results.Length/(double) resultsPerPage));
        }

        /// <summary>
        /// Use this method to get the search results
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <param name="resultsPerPage">The results per page.</param>
        /// <returns>Array of results</returns>
        public TValue[] GetPage(int Page, int resultsPerPage)
        {
            if (Results == null) return new TValue[0];

            int fromIndex, toIndex;

            fromIndex = (Page - 1)*resultsPerPage;
            toIndex = fromIndex + resultsPerPage - 1;

            if (fromIndex > Results.Length - 1)
            {
                fromIndex = 0;
                toIndex = fromIndex + resultsPerPage - 1;
            }

            if (toIndex > Results.Length - 1)
                toIndex = Results.Length - 1;

            var results = new List<TValue>();

            for (int i = fromIndex; i <= toIndex; i++)
            {
                TValue value = LoadResult(Results[i]);

                if (ReferenceEquals(value, null))
                    continue;

                results.Add(value);
            }

            return results.ToArray();
        }
        /// <summary>
        /// Use this method to get the search results
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <param name="resultsPerPage">The results per page.</param>
        /// <returns>Array of results</returns>
        public TValue[] GetPageBulk(int Page, int resultsPerPage)
        {
            if (Results == null) return new TValue[0];

            int fromIndex, toIndex;

            fromIndex = (Page - 1) * resultsPerPage;
            toIndex = fromIndex + resultsPerPage - 1;

            if (fromIndex > Results.Length - 1)
            {
                fromIndex = 0;
                toIndex = fromIndex + resultsPerPage - 1;
            }

            if (toIndex > Results.Length - 1)
                toIndex = Results.Length - 1;

            var results = LoadResults(Results.ToList().GetRange(fromIndex, toIndex-fromIndex+1).ToArray());
            return results == null ? new TValue[0] : results.ToArray();
        }
    }
}