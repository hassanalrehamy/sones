﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lucene.Net;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.Index;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Util;
using Lucene.Net.Documents;

namespace sones.Plugins.Index.LuceneIdx
{
    public class LuceneIndex
    {
        private String _IndexId;
        private IndexWriter _IndexWriter;
        private Lucene.Net.Store.Directory _IndexDirectory;

        private const int _hitsPerPage = 10;

        #region Constructor

        /// <summary>
        /// Index constructor, in-memory version
        /// </summary>
		/// 
		/// <param name="myLocation">The Solr URL (i.e. "http://localhost:8983/solr").</param>
		/// 
		/// <exception cref="System.ArgumentNullException">
		///		myLocation is NULL.
		/// </exception>
        public LuceneIndex(String myIndexId)
        {
            _IndexId = myIndexId;

            _IndexDirectory = new RAMDirectory();

            Analyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);

            _IndexWriter = new IndexWriter(_IndexDirectory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
        }

        /// <summary>
        /// Index constructor, persistent version with path
        /// </summary>
        /// 
        /// <param name="myLocation">The Solr URL (i.e. "http://localhost:8983/solr").</param>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///		myLocation is NULL.
        /// </exception>
        public LuceneIndex(String myIndexId, String myPath)
        {
            _IndexId = myIndexId;

            _IndexDirectory = new SimpleFSDirectory(new DirectoryInfo(myPath));
            _IndexDirectory.CreateOutput(myPath);
            _IndexDirectory.OpenInput(myPath);

            Analyzer analyzer = new
            StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);

            _IndexWriter = new IndexWriter(_IndexDirectory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
        }

        #endregion

        /// <summary>
        /// Adds the specified entry to the index.
        /// </summary>
        /// 
        /// <param name="myEntry">The entry to be added.</param>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///		myEntry is NULL.
        /// </exception>
        public void AddEntry(LuceneEntry myEntry)
        {
            Document doc = new Document();

            Field id =
              new Field("id",
              myEntry.Id,
              Field.Store.YES,
              Field.Index.ANALYZED,
              Field.TermVector.YES);

            doc.Add(id);

            Field indexId =
              new Field("indexId",
              myEntry.IndexId,
              Field.Store.YES,
              Field.Index.ANALYZED,
              Field.TermVector.YES);

            doc.Add(indexId);

            NumericField vertexId =
              new NumericField("vertexId",
              Field.Store.YES,
              false);

            vertexId.SetLongValue(myEntry.VertexId);

            doc.Add(vertexId);

            if (myEntry.PropertyId != null)
            {

                NumericField propertyId =
                  new NumericField("propertyId",
                  Field.Store.YES,
                  false);
             
                propertyId.SetLongValue((long)myEntry.PropertyId);

                doc.Add(propertyId);
            }

            Field text =
              new Field("text",
              myEntry.Text,
              Field.Store.YES,
              Field.Index.ANALYZED,
              Field.TermVector.YES);

            doc.Add(text);

            _IndexWriter.AddDocument(doc);
            _IndexWriter.Commit();
        }

        /// <summary>
        /// Deletes an entry that matches the specified LuceneEntry.
        /// </summary>
        /// 
        /// <param name="myEntry">The entry to be deleted.</param>
        /// 
        /// <returns>
        /// 0 (zero) if the operation succeeded; otherwise a value other than 0 (zero).
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///		myEntry is NULL.
        /// </exception>
        public Int32 DeleteEntry(LuceneEntry myEntry)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes all entries that match the specified Lucene query.
        /// </summary>
        /// 
        /// <param name="myLuceneQuery">The Lucene query.</param>
        /// <param name="select">A predicate which takes a LuceneEntry and returns whether a LuceneEntry should be taken into account when deleting entries.
        ///						 If this parameter is NULL, no Lucene entry is ignored.</param>
        /// 
        /// <returns>
        /// 0 (zero) if the operation succeeded; otherwise a value other than 0 (zero).
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///		myLuceneQuery is NULL.
        /// </exception>
        public Int32 DeleteEntry(String myLuceneQuery, Predicate<LuceneEntry> select = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks whether the Lucene index has entries matching the specified Lucene query.
        /// </summary>
        /// 
        /// <param name="myQuery">The query string.</param>
        /// <param name="select">A predicate which takes a LuceneEntry and returns whether a LuceneEntry should be taken into account when looking for Lucene entries.
        ///						 If this parameter is NULL, no Lucene entry is ignored.</param>
        /// 
        /// <returns>
        /// true, if there are matching entries; otherwise false.
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///		myQuery is NULL.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///		myQuery is an empty string or contains only whitespace.
        /// </exception>
        public Boolean HasEntry(String myQuery, Predicate<LuceneEntry> select = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all entries matching the specified Lucene query and (optionally) inner Lucene query.
        /// </summary>
        /// 
        /// <param name="myQuery">The query string.</param>
        /// <param name="myInnerQuery">The optional inner query string (to prefilter entries).</param>
        /// 
        /// <returns>
        /// A collection containing all matching Lucene entries; or an empty collection if no entries are matching the query string.
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///		myQuery is NULL.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///		myQuery is an empty string or contains only whitespace.
        /// </exception>
        public IEnumerable<LuceneEntry> GetEntries(String myQuery, String myInnerQuery = null)
        {
            var queryparser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "text", new StandardAnalyzer());
            Query outerquery = null;
            Query innerquery = null;
            Query query = null;
            try
            {
                outerquery = queryparser.Parse(myQuery);
                if (myInnerQuery != null)
                {
                    innerquery = queryparser.Parse(myInnerQuery);
                    BooleanQuery boolquery = new BooleanQuery();
                    boolquery.Add(outerquery, BooleanClause.Occur.MUST);
                    boolquery.Add(innerquery, BooleanClause.Occur.MUST);
                    query = (Query)boolquery;
                }
                else
                {
                    query = outerquery;
                }
            }
            catch (ParseException)
            {
                yield break;
            }

            var _IndexSearcher = new IndexSearcher(_IndexDirectory, true);
            TopScoreDocCollector collector = TopScoreDocCollector.create(_hitsPerPage, true);

            _IndexSearcher.Search(query, collector);
            
            foreach (var hit in collector.TopDocs().scoreDocs)
            {
                Document cur = _IndexSearcher.Doc(hit.doc);

                var entry = new LuceneEntry(
                                    cur.GetField("indexId").StringValue(),
                                    Convert.ToInt64(cur.GetField("vertexId").StringValue()),
                                    cur.GetField("text").StringValue()
                                    );

                yield return entry;
            }

            _IndexSearcher.Close();
        }

        /// <summary>
        /// Gets all entries matching the specified Lucene query and inner field query.
        /// </summary>
        /// 
        /// <param name="myQuery">The query string.</param>
        /// <param name="myInnerQuery">The inner field query string (to prefilter entries).</param>
        /// <param name="myInnerField">Name of the field to use for inner field query.</param>
        /// 
        /// <returns>
        /// A collection containing all matching Lucene entries; or an empty collection if no entries are matching the query string.
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///		myQuery is NULL.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///		myQuery is an empty string or contains only whitespace.
        /// </exception>
        public IEnumerable<LuceneEntry> GetEntriesInnerByField(String myQuery, String myInnerQuery, String myInnerField)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all Lucene index keys (an index key is represented by LuceneEntry.Text).
        /// </summary>
        /// 
        /// <param name="select">A predicate which takes a LuceneEntry and returns whether a LuceneEntry should be taken into account when looking for keys.
        ///						 If this parameter is NULL, no Lucene entry is ignored.</param>
        /// 
        /// <returns>
        /// A collection with all keys; or an empty list if no entries are within the index
        /// </returns>
        public IEnumerable<String> GetKeys(Predicate<LuceneEntry> select = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all distinct values (values are the vertex IDs).
        /// </summary>
        /// 
        /// <param name="select">A predicate which takes a LuceneEntry and returns whether a LuceneEntry should be taken into account when looking for vertex ids.
        ///						 If this parameter is NULL, no Lucene entry is ignored.</param>
        /// 
        /// <returns>
        /// A collection containing a single set of Int64 values, representing the distinct vertex ids within the Lucene index;
        /// or a collection containing an empty set, if no entries are within the index.
        /// </returns>
        /// 
        /// <dev_doc>
        /// TODO: the return value should be a simple IEnumerable(Of long)
        /// </dev_doc>
        public IEnumerable<ISet<long>> GetValues(Predicate<LuceneEntry> select = null)
        {
            throw new NotImplementedException();
        }
    }
}