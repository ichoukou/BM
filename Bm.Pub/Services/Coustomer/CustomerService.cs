﻿using Bm.Models.Common;
using Bm.Models.Dp;
using Bm.Modules.Orm;
using Bm.Modules.Orm.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bm.Services.Common
{
   public sealed  class CustomerService : RepoService<Customer>
    {
        public CustomerService(string accountNo) : base(accountNo)
        {
        }
        public override Customer GetById(long id)
        {
            using (var conn = ConnectionManager.Open())
            {
                var model = conn.Get<Customer>(id);

                if (model != null)
                {
                    var query = new Criteria<Customer>()
                        .Where(m => m.No, Op.Eq, model.No)
                        .Asc(m => m.Id);
                    model.Customers = conn.Query(query);
                };
                return model;
            }
        }
        public MessageRecorder<IList<Customer>> GetAllCustomer()
        {
            var r = new MessageRecorder<IList<Customer>>();
            var CustomerAll = GetAll();

            if (!CustomerAll.Any())
            {
                return r.Error("没有客户信息");
            }
            return r.SetValue(CustomerAll);
        }
    }
    }