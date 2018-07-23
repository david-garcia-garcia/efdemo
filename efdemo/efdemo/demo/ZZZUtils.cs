using ETG.SABENTISpro.Application.Core.Module.Database;
using NewRelic.Api.Agent;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Xml;
using System.Xml.Linq;
using Z.BulkOperations;
using Z.EntityFramework.Extensions;
using Z.EntityFramework.Plus;

namespace ETG.SABENTISpro.Application.Core.Kernel
{
    /// <summary>
    /// Tools to cleanup ORM internal caches
    /// </summary>
    public static class ZzzUtils
    {
        /// <summary>
        /// The locker
        /// </summary>
        public static object Locker = new object();

        /// <summary>
        /// Flush internal caches for ZZZ and EF. Do this every time the mappings change.
        /// </summary>
        /// <param name="connectionString"></param>
        [Trace]
        public static void ClearZzzAndEfCaches(string connectionString)
        {
            lock (Locker)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (var ctx = new DbContext(connection, false))
                    {
                        ctx.RefreshModel();
                    }
                }

                ClearEfCaches();
            }
        }

        /// <summary>
        /// Clears internal and external EF caches
        /// </summary>
        [Trace]
        public static void ClearEfCaches()
        {
            MetadataWorkspace.ClearCache();
            InformationSchemaManager.ClearInformationSchemaTable();
            EntityFrameworkManager.Cache = new StaticObjectCache();
            QueryCacheManager.Cache = new StaticObjectCache();
            MemoryCache.Default.Flush();

            ClearEfInternalCaches();
        }

        /// <summary>
        /// Clear entity framework's internal caches
        /// </summary>
        [Trace]
        public static void ClearEfInternalCaches()
        {
            var assembly = typeof(DbContext).Assembly;

            var typeLazyInternalContext = assembly.GetType("System.Data.Entity.Internal.LazyInternalContext");

            IDictionary initializedDatabases = (IDictionary)typeLazyInternalContext.GetField("InitializedDatabases", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            initializedDatabases?.Clear();

            IDictionary cachedModels = (IDictionary)typeLazyInternalContext.GetField("_cachedModels", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            cachedModels?.Clear();

            var typeMetadataWorkspace = typeof(MetadataWorkspace);

            IDictionary cachedWorkspaces = (IDictionary)typeMetadataWorkspace.GetField("_cachedWorkspaces", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            cachedWorkspaces?.Clear();

            var typeOfDbCompiledModel = typeof(DbCompiledModel);

            IDictionary contextConstructors = (IDictionary)typeOfDbCompiledModel.GetField("_contextConstructors", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            contextConstructors?.Clear();

            var typeOfObjectContext = typeof(ObjectContext);

            IDictionary contextTypesWithViewCacheInitialized = (IDictionary)typeOfObjectContext.GetField("_contextTypesWithViewCacheInitialized", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            contextTypesWithViewCacheInitialized?.Clear();

            var typeOfDbProviderServices = typeof(DbProviderServices);

            IDictionary spatialServices = (IDictionary)typeOfDbProviderServices.GetField("_spatialServices", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            spatialServices?.Clear();

            IDictionary executionStrategyFactories = (IDictionary)typeOfDbProviderServices.GetField("_executionStrategyFactories", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            executionStrategyFactories?.Clear();

            var typeOfDbHelpers = assembly.GetType("System.Data.Entity.Internal.DbHelpers");

            IDictionary propertyTypes = (IDictionary)typeOfDbHelpers.GetField("_propertyTypes", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            propertyTypes?.Clear();

            IDictionary propertySetters = (IDictionary)typeOfDbHelpers.GetField("_propertySetters", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            propertySetters?.Clear();

            IDictionary propertyGetters = (IDictionary)typeOfDbHelpers.GetField("_propertyGetters", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            propertyGetters?.Clear();

            var typeOfDbSetDiscoveryService = assembly.GetType("System.Data.Entity.Internal.DbSetDiscoveryService");

            IDictionary objectSetInitializers = (IDictionary)typeOfDbSetDiscoveryService.GetField("_objectSetInitializers", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            objectSetInitializers?.Clear();

            var typeOfInternalCollectionEntry = assembly.GetType("System.Data.Entity.Internal.InternalCollectionEntry");

            IDictionary entryFactories = (IDictionary)typeOfInternalCollectionEntry.GetField("_entryFactories", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            entryFactories?.Clear();

            var typeOfObjectContextTypeCache = assembly.GetType("System.Data.Entity.Internal.ObjectContextTypeCache");

            IDictionary typeCache = (IDictionary)typeOfObjectContextTypeCache.GetField("_typeCache", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            typeCache?.Clear();

            var typeOfinternalContext = assembly.GetType("System.Data.Entity.Internal.InternalContext");

            IDictionary entityFactories = (IDictionary)typeOfinternalContext.GetField("_entityFactories", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            entityFactories?.Clear();

            IDictionary queryExecutors = (IDictionary)typeOfinternalContext.GetField("_queryExecutors", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            queryExecutors?.Clear();

            // IDictionary asyncQueryExecutors = (IDictionary)typeOfinternalContext.GetField("_asyncQueryExecutors", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            // asyncQueryExecutors?.Clear();

            IDictionary setFactories = (IDictionary)typeOfinternalContext.GetField("_setFactories", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            setFactories?.Clear();

            var typeOfDbQueryVisitor = assembly.GetType("System.Data.Entity.Internal.Linq.DbQueryVisitor");

            IDictionary wrapperFactories = (IDictionary)typeOfDbQueryVisitor.GetField("_wrapperFactories", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            wrapperFactories?.Clear();

            var typeOfMetadataPropertyCollection = assembly.GetType("System.Data.Entity.Core.Metadata.Edm.MetadataPropertyCollection");

            object itemTypeMemoizer = (object)typeOfMetadataPropertyCollection.GetField("_itemTypeMemoizer", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            ClearMemoizer(itemTypeMemoizer);
        }

        /// <summary>
        /// Clear internal cache of Memoizer object
        /// </summary>
        [Trace]
        public static void ClearMemoizer(object memoizer)
        {
            var typeOfMemoizer = memoizer.GetType();
            IDictionary resultCache = (IDictionary)typeOfMemoizer.GetField("_resultCache", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance)?.GetValue(memoizer);
            resultCache?.Clear();
        }

        /// <summary>
        /// Get the EDMX model for a DBContext. Used for debugging the context definition
        /// state when it fails...
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [Trace]
        public static string GetModelXDocument(DbContext context)
        {
            if (context == null)
            {
                return string.Empty;
            }

            XDocument doc;
            using (var memoryStream = new MemoryStream())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, new XmlWriterSettings { Indent = true }))
                {
                    EdmxWriter.WriteEdmx(context, xmlWriter);
                }

                memoryStream.Position = 0;

                doc = XDocument.Load(memoryStream);
            }

            return doc.ToString();
        }

        /// <summary>
        /// Get the EDMX model for a DBContext. Used for debugging the context definition
        /// state when it fails...
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Trace]
        public static string GetModelXDocument(DbModel model)
        {
            if (model == null)
            {
                return string.Empty;
            }

            XDocument doc;
            using (var memoryStream = new MemoryStream())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, new XmlWriterSettings { Indent = true }))
                {
                    EdmxWriter.WriteEdmx(model, xmlWriter);
                }

                memoryStream.Position = 0;

                doc = XDocument.Load(memoryStream);
            }

            return doc.ToString();
        }

        /// <summary>
        /// Flush all items in the memory cache
        /// </summary>
        /// <param name="cache"></param>
        public static void Flush(this MemoryCache cache)
        {
            List<string> cacheKeys = cache.Select(kvp => kvp.Key).ToList();
            foreach (string cacheKey in cacheKeys)
            {
                cache.Remove(cacheKey);
            }
        }
    }
}
