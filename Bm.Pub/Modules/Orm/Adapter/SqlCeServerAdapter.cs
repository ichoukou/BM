using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;

namespace Bm.Modules.Orm.Adapter
{
    public partial class SqlCeServerAdapter : ISqlAdapter
    {
        public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
        {
            var cmd = $"insert into {tableName} ({columnList}) values ({parameterList})";
            connection.Execute(cmd, entityToInsert, transaction, commandTimeout);
            var r = connection.Query("select @@IDENTITY id", transaction: transaction, commandTimeout: commandTimeout).ToList();

            if (r.First().id == null) return 0;
            var id = (int)r.First().id;

            var propertyInfos = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
            if (!propertyInfos.Any()) return id;

            var idProperty = propertyInfos.First();
            idProperty.SetValue(entityToInsert, Convert.ChangeType(id, idProperty.PropertyType), null);

            return id;
        }

        public void AppendColumnName(StringBuilder sb, string columnName)
        {
            sb.AppendFormat("[{0}]", columnName);
        }

        public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
        {
            sb.AppendFormat("[{0}] = @{1}", columnName, columnName);
        }

        public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName,
            string columnSql, string valuesSql, object entityToInsert)
        {
            throw new NotImplementedException();
            var cmd = $"INSERT INTO {tableName} ({columnSql}) VALUES ({valuesSql})";
            connection.Execute(cmd, entityToInsert, transaction, commandTimeout);

            // REVIEW
            var r = connection.Query("SELECT LAST_INSERT_ID() id", transaction: transaction, commandTimeout: commandTimeout);
            var id = r.First().id;
            if (id == null) return 0;

            return Convert.ToInt32(id);
        }
    }
}