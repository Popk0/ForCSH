using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
//using System.Text;
//using System.Threading.Tasks;

namespace DataAccess.SqlCommandExtentions
{
    public static class SqlCommandExtentions
	{
		public static IEnumerable<TEntity> ReadMany<TEntity>(this SqlCommand command) where TEntity : new()
		{
			var result = ReadManyInternal<TEntity>(command);
			return result;
		}

		private static IEnumerable<TEntity> ReadManyInternal<TEntity>(SqlCommand command) where TEntity : new()
		{
			var reader = command.ExecuteReader();

			if (!reader.HasRows)
			{
				reader.Close();
				return Enumerable.Empty<TEntity>();
			}

			var entities = reader.ParseFromReaderInternal<TEntity>();
			reader.Close();

			return entities;
		}
        private static IEnumerable<TEntity> ParseFromReaderInternal<TEntity>(this SqlDataReader reader) where TEntity : new()
        {
            var entityType = typeof(TEntity);
            var entityProps = entityType.GetProperties();

            var entities = new List<TEntity>();            

            //object valueFromReader;

            while (reader.Read())
            {
                var entity = new TEntity();
                foreach (var entityPropInfo in entityProps)
                {                    
                    try
                    {
                        if (reader[entityPropInfo.Name] is DBNull)
                        {
                            entityPropInfo.SetValue(entity, null);
                            continue;
                        }
                        var valueFromReader = reader[entityPropInfo.Name];
                        entityPropInfo.SetValue(entity, valueFromReader);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }                    
                }
                entities.Add(entity);
            }
            return entities;
        }
    }
}
