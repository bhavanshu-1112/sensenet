﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SenseNet.ContentRepository.Storage.Data;

namespace SenseNet.ContentRepository.Storage.Search //UNDONE: namespace
{
    [System.Diagnostics.DebuggerDisplay("{Name} = {Value}")]
    public class NodeQueryParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public enum ChainOperator { And, Or }
    public enum OrderDirection { Asc, Desc }

    public enum IndexFieldType { String, Int, Long, Float, Double, DateTime }
    public enum IndexableDataType { String, Int, Long, Float, Double }
    public enum FieldInfoType { StringField, IntField, LongField, SingleField, DoubleField }

    public enum IndexingMode { Default, Analyzed, AnalyzedNoNorms, No, NotAnalyzed, NotAnalyzedNoNorms }
    public enum IndexStoringMode { Default, No, Yes }
    public enum IndexTermVector { Default, No, WithOffsets, WithPositions, WithPositionsOffsets, Yes }

    public enum IndexRebuildLevel { IndexOnly, DatabaseAndIndex };

    public interface IIndexableDocument
    {
        IEnumerable<IIndexableField> GetIndexableFields();
    }
    public interface IIndexableField
    {
        string Name { get; }
        bool IsInIndex { get; }
        bool IsBinaryField { get; }
        IEnumerable<IIndexFieldInfo> GetIndexFieldInfos(out string textExtract);
    }
    public interface IIndexValueConverter<T>
    {
        T GetBack(string fieldValue);
    }
    public interface IIndexValueConverter
    {
        object GetBack(string fieldValue);
    }


    public interface ISnField
    {
        string Name { get; }
        object GetData(bool localized = true);
    }


    public interface IQueryFieldValue
    {
        //internal bool IsPhrase { get; }
        //internal SnLucLexer.Token Token { get; }
        //internal double? FuzzyValue { get; set; }
        string StringValue { get; }
        object InputObject { get; }

        IndexableDataType Datatype { get; }
        Int32 IntValue { get; }
        Int64 LongValue { get; }
        Single SingleValue { get; }
        Double DoubleValue { get; }

        void Set(Int32 value);
        void Set(Int64 value);
        void Set(Single value);
        void Set(Double value);
        void Set(String value);
    }

    public interface IPerFieldIndexingInfo //UNDONE: Racionalize interface names: IPerFieldIndexingInfo and IIndexFieldInfo
    {
        string Analyzer { get; set; }
        IFieldIndexHandler IndexFieldHandler { get; set; }

        IndexingMode IndexingMode { get; set; }
        IndexStoringMode IndexStoringMode { get; set; }
        IndexTermVector TermVectorStoringMode { get; set; }

        bool IsInIndex { get; }

        Type FieldDataType { get; set; }
    }


    public interface IIndexFieldInfo //UNDONE: Racionalize interface names: IPerFieldIndexingInfo and IIndexFieldInfo
    {
        string Name { get; }
        string Value { get; }
        FieldInfoType Type { get; }
        IndexingMode Index { get; }
        IndexStoringMode Store { get; }
        IndexTermVector TermVector { get; }
    }
    public interface IFieldIndexHandler
    {
        /// <summary>For SnLucParser</summary>
        bool TryParseAndSet(IQueryFieldValue value);
        /// <summary>For LINQ</summary>
        void ConvertToTermValue(IQueryFieldValue value);

        string GetDefaultAnalyzerName();
        IEnumerable<string> GetParsableValues(ISnField field);
        int SortingType { get; }
        IndexFieldType IndexFieldType { get; }
        IPerFieldIndexingInfo OwnerIndexingInfo { get; set; }
        string GetSortFieldName(string fieldName);
        IEnumerable<IIndexFieldInfo> GetIndexFieldInfos(ISnField field, out string textExtract);
    }



    public interface IIndexDocumentProvider
    {
        object GetIndexDocumentInfo(Node node, bool skipBinaries, bool isNew, out bool hasBinary);
        object CompleteIndexDocumentInfo(Node node, object baseDocumentInfo);
    }
    public interface ISearchEngine
    {
        bool IndexingPaused { get; }
        void PauseIndexing();
        void ContinueIndexing();
        void WaitIfIndexingPaused();

        IIndexPopulator GetPopulator();

        IDictionary<string, Type> GetAnalyzers();

        void SetIndexingInfo(object indexingInfo);

        object DeserializeIndexDocumentInfo(byte[] IndexDocumentInfoBytes);
    }
    public class InternalSearchEngine : ISearchEngine
    {
        public static InternalSearchEngine Instance = new InternalSearchEngine();

        public bool IndexingPaused { get { return false; } }
        public void PauseIndexing()
        {
            // do nothing;
        }
        public void ContinueIndexing()
        {
            // do nothing;
        }
        public void WaitIfIndexingPaused()
        {
            // do nothing;
        }

        public IIndexPopulator GetPopulator()
        {
            return NullPopulator.Instance;
        }
        public IDictionary<string, Type> GetAnalyzers()
        {
            return null;
        }
        public void SetIndexingInfo(object indexingInfo)
        {
            // do nothing
        }
        public object DeserializeIndexDocumentInfo(byte[] IndexDocumentInfoBytes)
        {
            return null;
        }
    }

}