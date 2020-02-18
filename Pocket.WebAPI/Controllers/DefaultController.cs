using Pocket.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Pocket.WebAPI.Controllers
{
    public class DefaultController : ApiController
    {
        PocketDb db;

        public DefaultController()
        {
            db = new PocketDb();
            db.Configuration.ProxyCreationEnabled = false;
        }
       

        [Route("getallproducts")]
        [HttpGet]
        public IHttpActionResult GetAllProducts()
        {
          

            try
            {
                var products = (from PROD in db.Products
                                select PROD).ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {
                var errMsg = ex.Message;
                return InternalServerError();
            }

            
        }

        [Route("getproduct/{Id}")]
        [HttpGet]
        public IHttpActionResult GetProduct(long Id)
        {
           

            try
            {
                var product = (from PROD in db.Products
                                where PROD.Id == Id
                                select PROD).Single();
                return Ok(product);
            }
            catch (Exception ex)
            {
                var errMsg = ex.Message;
                return InternalServerError();
            }


        }

       
        [Route("getproductPrice/{Id}")]
        [HttpGet]
        public IHttpActionResult GetProductPrice(long Id)
        {


            try
            {
                var ProductPrice = (from PROD in db.Products
                               where PROD.Id == Id
                               select PROD).Single().Price;
                return Ok(ProductPrice);
            }

            catch (Exception ex)
            {
                var errMsg = ex.Message;
                return InternalServerError();
            }

        }

        [Route("saveordermaster")]
        [HttpPost]
        public IHttpActionResult SaveOrderMaster(OrderMaster orderMaster)
        {
            try
            {
                orderMaster.CreatedDate = DateTime.Now;
                orderMaster.ModifiedDate = DateTime.Now;

                int count = orderMaster.OrderDetails.Count;
                var OrderDetails = orderMaster.OrderDetails.ToArray();

                for (int i = 0; i < count; i++)
                {
                    OrderDetails[i].CreatedDate = DateTime.Now;
                    OrderDetails[i].ModifiedDate = DateTime.Now;

                }

                db.OrderMasters.Add(orderMaster);
                db.SaveChanges();

                return Ok(true);
            }
            catch (Exception ex)
            {
                var errMsg = ex.Message;

                return Content(HttpStatusCode.InternalServerError, false);
            }
        }

    }
}
