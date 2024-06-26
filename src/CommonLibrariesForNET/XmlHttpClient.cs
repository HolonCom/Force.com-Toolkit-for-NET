﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Salesforce.Common.Internals;
using Salesforce.Common.Models.Json;

namespace Salesforce.Common
{
    public class XmlHttpClient : BaseHttpClient, IXmlHttpClient
    {
        public XmlHttpClient(string instanceUrl, string apiVersion, string accessToken, HttpClient httpClient, bool callerWillDisposeHttpClient = false)
            : base(instanceUrl, apiVersion, "application/xml", httpClient, callerWillDisposeHttpClient)
        {
            if (ApiVersion.StartsWith("v", StringComparison.OrdinalIgnoreCase))
            {
                ApiVersion = ApiVersion.Substring(1);
            }
            HttpClient.DefaultRequestHeaders.Add("X-SFDC-Session", accessToken);
        }

        // GET

        public async Task<T> HttpGetAsync<T>(string urlSuffix)
        {
            var url = Common.FormatUrl(urlSuffix, InstanceUrl, ApiVersion);
            return await HttpGetAsync<T>(url).ConfigureAwait(false);
        }

        public async Task<T> HttpGetAsync<T>(Uri uri)
        {
            try
            {
                var response = await HttpGetAsync(uri).ConfigureAwait(false);
                return DeserializeXmlString<T>(response);
            }
            catch (BaseHttpClientException e)
            {
                throw ParseForceException(e.Message);
            }
        }

        // POST

        public async Task<T> HttpPostAsync<T>(object inputObject, string urlSuffix)
        {
            var url = Common.FormatUrl(urlSuffix, InstanceUrl, ApiVersion);
            return await HttpPostAsync<T>(inputObject, url).ConfigureAwait(false);
        }

        public async Task<T> HttpPostAsync<T>(object inputObject, Uri uri)
        {
            var postBody = SerializeXmlObject(inputObject);
            try
            {
                var response = await HttpPostAsync(postBody, uri).ConfigureAwait(false);
                return DeserializeXmlString<T>(response);
            }
            catch (BaseHttpClientException e)
            {
                throw ParseForceException(e.Message);
            }
        }

        // HELPER METHODS

        private static ForceException ParseForceException(string responseMessage)
        {
            try
            {
                var errorResponse = DeserializeXmlString<ErrorResponse>(responseMessage);
                return new ForceException(errorResponse.ErrorCode, errorResponse.Message);
            }
            catch (Exception)
            {
                return new ForceException("unknown", responseMessage);
            }
        }

        private static string SerializeXmlObject(object inputObject)
        {
            var xmlSerializer = XmlSerializerCache.Instance.Value.GetSerializer(inputObject.GetType());
            var stringWriter = new StringWriter();
            string result;
            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlSerializer.Serialize(writer, inputObject);
                result = stringWriter.ToString();
            }
            stringWriter.Dispose();
            return result;
        }

        private static T DeserializeXmlString<T>(string inputString)
        {
            var serializer = XmlSerializerCache.Instance.Value.GetSerializer<T>();
            T result;
            using (TextReader reader = new StringReader(inputString))
            {
                result = (T)serializer.Deserialize(reader);
            }
            return result;
        }
    }
}