﻿using System.Web.Http;
using Bm.Extensions;
using Bm.Models.Common;
using System;
using Bm.Modules.Helper;
using Bm.Services.Common;
using Bm.Models.Dp;
using System.Collections.Generic;
using System.Linq;
using Bm.Services.Dp;

namespace Bm.Controllers.Customers
{
    public class CustomerController : BaseApiController
    {

        [Route("api/base_customer_details")]
        public IHttpActionResult GetAll()
        {
            var m = Request.GetQueryString("m");

            var mr = new MessageRecorder<bool>();
            //TODO 更新本周推客数量、活跃客户数、成交金额
            var now = DateTime.Now;
            var a = Ok(mr, n => new
            {
                TuikeAmount = "张三",
                ActiveCustomer = now.Hour * 100 + +now.Minute * 2 + now.Second,
                TotalSales = now.Hour * 200 + +now.Minute * 50 + now.Second * 10,
                AlertCount = now.Minute % 10
            });
            return a;
        }
        public IHttpActionResult GetCustomer()
        {
            var service = new CustomerService();
            var r = service.GetAllCustomer();
            return Ok(r);
        }
        [HttpGet]
        [Route("api/base_customer_create")]
        public IHttpActionResult Create()
        {
            var m = Request.GetQueryString("m");
            var c = Request.GetQueryString("c");
            var s = Request.GetQueryString("s");
            var d = Request.GetQueryString("d");
            var e = Request.GetQueryString("e");
            var x = Request.GetQueryString("x");
            var u = Request.GetQueryString("u");
            var at = DateTime.Now; 
            var cust = new Customer()
            { No = e, Name = m, Gender = s, Mobile = c, Level = d, RegAt = at, Remark = x, BrokerNo = u,CreatedAt= at,CreatedBy=u};
            var service = new CustomerService();
            var r = service.Create(cust);
            if (r.HasError)
                return Ok(r);
            var dic = new Dictionary<string, object>
            {
                {"HasError", r.HasError},
                {"Errors", r.Errors },
                {"Result", r.Value}
            };
            return Ok(dic);

        }
    }
}
