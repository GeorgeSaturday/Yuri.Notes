using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Yuri.Notes.DB;

namespace Yuri.Notes.Web
{
    public class BaseRepository<T> : IEntityRepository<T> where T : class, IEntity
    {
        protected string connectionString;
        protected IEnumerable<PropertyInfo> typeProperties;

        public BaseRepository(string connectionString)
        {
            this.connectionString = connectionString;

            var type = typeof(T);

            typeProperties = type.GetProperties();
        }

        public virtual T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public virtual void Delete(long id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                    $"DELETE FROM {TableName} WHERE Id = {id};",
                    connection);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Получить все сущности
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll()
        {
            var result = new List<T>();

            var fields = string.Join(",", typeProperties.Select(p => p.Name));

            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                    $"SELECT {fields} FROM {TableName};",
                    connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var entity = Create();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var column = reader.GetName(i);

                            var prop = typeProperties.FirstOrDefault(p => p.Name == column);

                            if (prop != null)
                            {
                                prop.SetValue(entity, reader[i]);
                            }
                        }

                        result.Add(entity);
                    }
                }
                reader.Close();
            }

            return result;
        }

        public string TableName { get; set; }

        /// <summary>
        /// Загрузить по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Load(long id)
        {
            var result = Create();

            var fields = string.Join(",", typeProperties.Select(p => p.Name));

            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                    $"SELECT {fields} FROM {TableName} WHERE Id = {id};",
                    connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var column = reader.GetName(i);

                            var prop = typeProperties.FirstOrDefault(p => p.Name == column);

                            if (prop != null)
                            {
                                prop.SetValue(result, reader[i]);
                            }
                        }

                    }
                }
                else
                {
                    result = null;
                }
                reader.Close();
            }

            return result;
        }

        protected string GetSqlValue(PropertyInfo propInfo, object obj)
        {
            var propValue = propInfo.GetValue(obj);
            var str = "";

            if (propValue == null)
            {
                str = "NULL";
            }
            else if (propValue is string)
            {
                str = $"N'{propValue}'";
            }
            else if (propValue is double)
            {
                var doubleValue = (double)propValue;
                str = $"{doubleValue.ToString(CultureInfo.InvariantCulture)}";
            }
            else if (propValue is DateTime)
            {
                var doubleValue = (DateTime)propValue;
                str = $"N'{doubleValue.ToString(CultureInfo.InvariantCulture)}'";
            }
            else if (propValue.GetType().IsEnum)
            {
                var enumValue = (int)propValue;
                str = $"{enumValue}";
            }
            else
            {
                str = $"{propValue}";
            }

            return str;
        }

        public virtual void Save(T entity)
        {

            if (entity.Id > 0)
            {
                var fields = new List<string>();
                foreach (var prop in typeProperties)
                {
                    fields.Add($"[{prop.Name}] = {GetSqlValue(prop, entity)}");
                }

                if (fields.Any())
                {
                    var setSQL = string.Join(",", fields);

                    using (var connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(
                            $"UPDATE {TableName} {setSQL} WHERE Id = {entity.Id};",
                            connection);
                        connection.Open();

                        if (command.ExecuteNonQuery() == 1) {
                            // all good
                        } else
                        {
                            // ошибка
                        }
                    }
                }
            }
            else
            {
                var columnValues = new List<string>();
                var columnNames = new List<string>();

                foreach (var prop in typeProperties)
                {
                    if (prop.Name == "Id")
                    {
                        continue;
                    }

                    columnNames.Add($"[{prop.Name}]");
                    columnValues.Add($"{GetSqlValue(prop, entity)}");
                }

                if (columnNames.Any())
                {
                    var columnNamesSQL = string.Join(",", columnNames);
                    var columnValuesSQL = string.Join(",", columnValues);

                    using (var connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(
                            $"INSERT INTO {TableName} ({columnNamesSQL}) VALUES ({columnValuesSQL});",
                            connection);
                        connection.Open();

                        if (command.ExecuteNonQuery() == 1)
                        {
                            // all good
                        }
                        else
                        {
                            // ошибка
                        }
                    }
                }



            }
        }
    }
}
