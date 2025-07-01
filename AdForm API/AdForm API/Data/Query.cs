using AdForm_API.AdFormDB;
using AdForm_API.Models;
using AdForm_API.Services;
using System.Fabric.Query;

namespace AdForm_API.Data
{
    public class Query
    {
        public PostResponse PostProduct(AdFormService adFormService, string name, decimal price)
        {
            PostResponse response = adFormService.PostProduct(name, price);
            return response;
        }
    }
}