﻿using Bm.Services.Dp;
using System.Web.Mvc;
using Bm.Models.Dp;
using Bm.Modules.Helper;
using System;
using System.ComponentModel;
using System.Web.Routing;
using Bm.Extensions;

namespace Bm.Areas.Biz.Controllers
{
    [DisplayName("置业顾问")]
    public sealed class PropertyAdvisorController : BaseAuthController
    {
        private PropertyAdvisorService _service;
        
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _service = new PropertyAdvisorService(CurrAccountNo);
        }

        // GET: Biz/Project
        public ActionResult Index()
        {
            var models = _service.GetAll();
            return View(models);
        }

        // GET: Biz/Project/Details/5
        public ActionResult Details(long[] ids)
        {
            if (ids.IsNullOrEmpty())
            {
                FlashWarn("请选择一条数据");
                return RedirectToAction("Index");
            }
            var list = _service.GetByIds(ids);
            return View(list);
        }

        // GET: Biz/Project/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Biz/Project/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var model = new PropertyAdvisor();
                TryUpdateModel(model, collection);
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = "SYSTEM";
                if (!ModelState.IsValid)
                {
                    FlashError("数据验证未通过，请检查是否存在为空的必填项");
                    return View(model);
                }
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = "SYSTEM";

                var service = new PropertyAdvisorAccountService();
                var r = service.Create(model.Name, model.MobileNo, model.Title.Equals("案场经理"), model.ProjectNo);
                if (r.HasError)
                {
                    FlashMessage(r);
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: Biz/Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var model = _service.GetById(id.Value);
                return View(model);
            }
            FlashWarn("没有指定ID");
            return RedirectToAction("Index");
        }

        // POST: Biz/Project/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = new PropertyAdvisor();
            TryUpdateModel(model, collection);
            if (!ModelState.IsValid)
            {
                FlashError("数据验证未通过，请检查是否存在为空的必填项");
                return View(model);
            }
            model.UpdatedBy = "SYSTEM";
            model.UpdatedAt = DateTime.Now;

            var service = new PropertyAdvisorAccountService();
            var r1 = service.Remove(model.No, model.ProjectNo);
            if (r1.HasError)
            {
                FlashMessage(r1);
                return View(model);
            }
            var r2 = service.Create(model.Name, model.MobileNo, model.Title.Equals("案场经理"), model.ProjectNo);
            if (r2.HasError)
            {
                FlashMessage(r2);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        // GET: Biz/Project/Delete/5
        public ActionResult Delete(long[] ids)
        {
            if (ids.IsNullOrEmpty())
            {
                FlashWarn("请选择一条数据");
                return RedirectToAction("Index");
            }
            var list = _service.GetByIds(ids);
            return View(list);
        }

        // POST: Biz/Project/Delete/5
        [HttpPost]
        public ActionResult Delete(FormCollection collection, long[] ids)
        {
            if (ids.IsNullOrEmpty())
            {
                FlashWarn("请选择一条数据");
                return View();
            }
            var list = _service.GetByIds(ids);
            try
            {

                var service = new PropertyAdvisorAccountService();
                foreach (var advisor in list)
                {
                    var r1 = service.Remove(advisor.No, advisor.ProjectNo);
                    if (r1.HasError)
                    {
                        FlashMessage(r1);
                        return View(list);
                    }
                }
                FlashSuccess("删除成功");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                FlashError("删除失败");
                return View(list);
            }
        }
    }
}
