using System.Collections.Generic;
using System.Net;
using EAuction.Products.Api.Repositories.Abstractions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace EAuction.Products.AzureFunction.API
{
    public class Product
    {
        private readonly ILogger _logger;
        private IProductRepository _productRepository;

        public Product(ILoggerFactory loggerFactory, IProductRepository productRepository)
        {
            _logger = loggerFactory.CreateLogger<Product>();
            _productRepository = productRepository;
        }

        [FunctionName("DeleteProduct")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "DeleteProduct/{productId}")] HttpRequestData req, string productId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var result = _productRepository.Delete(productId);

            var response = req.CreateResponse(HttpStatusCode.OK);
            if (!result.Result)
            { 
                response = req.CreateResponse(HttpStatusCode.NotFound);
            }
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            return response;
        }
    }
}
