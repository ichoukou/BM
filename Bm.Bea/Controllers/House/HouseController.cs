﻿using System.Linq;
using System.Web.Http;
using System.Web.Razor.Generator;
using Bm.Extensions;
using Bm.Models.Common;
using Bm.Models.Dp;
using Bm.Modules.Helper;
using Bm.Services.Dp;

namespace Bm.Controllers.House
{

    public class HouseController : BaseApiController
    {

        /// <summary>
        /// 获取所有房源信息
        /// </summary>
        /// <returns></returns>
        [Route("api/get_house_all")]
        public IHttpActionResult GetHouseAll()
        {
            var service = new ProjectService();
            var r = service.GetAllHouse();
            return Ok(r);
        }
        /// <summary>
        /// 获取我的房源信息
        /// </summary>
        /// <returns></returns>
        [Route("api/get_house_my")]
        public IHttpActionResult GetHouseMy()
        {
            //var m = Request.GetQueryString("m");
            var u = Request.GetQueryString("u");

            var service = new HouseBrokerRefService();
            var r = service.GetHouseByBrokerNo(u);
            return Ok(r);
        }
        /// <summary>
        /// 根据编号获取房源信息
        /// </summary>
        /// <returns></returns>
        [Route("api/get_house_detail")]
        public IHttpActionResult GetHouseById(string id)
        {
            var mr = new MessageRecorder<Project>();
            if (id.IsNullOrEmpty()) return Ok(mr.Error("楼盘编号无效"));

            var service = new ProjectService();
            var r = service.GetByNo(id);

            return Ok(mr.SetValue(r));
        }
    }
}
