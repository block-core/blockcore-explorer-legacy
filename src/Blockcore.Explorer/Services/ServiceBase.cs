using System;
using RestSharp;

namespace Blockcore.Explorer.Services
{
   public class ServiceBase
   {
      private readonly RestClient client;

      public ServiceBase(string baseUrl)
      {
         // RestClient is suppose to be able to be re-used. If not, we should move this into the Execute method.
         client = new RestClient(baseUrl);
      }

      public T Execute<T>(RestRequest request) where T : new()
      {
         IRestResponse<T> response = client.Execute<T>(request);

         if (response.ErrorException != null)
         {
            const string message = "Error retrieving response. Check inner details for more info.";
            var blockIndexServiceException = new ApplicationException(message, response.ErrorException);
            throw blockIndexServiceException;
         }

         return response.Data;
      }

      public string Execute(RestRequest request)
      {
         IRestResponse response = client.Execute(request);

         if (response.ErrorException != null)
         {
            const string message = "Error retrieving response. Check inner details for more info.";
            var blockIndexServiceException = new ApplicationException(message, response.ErrorException);
            throw blockIndexServiceException;
         }

         return response.Content;
      }

      protected RestRequest GetRequest(string resource)
      {
         var request = new RestRequest();
         request.AddQueryParameter("api-version", "1.0");
         request.Method = Method.GET;
         request.Resource = resource;
         return request;
      }

   }
}
