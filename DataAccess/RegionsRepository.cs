using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.SearchModels;
using DataAccess.SqlCommandExtentions;
using System.Xml.Serialization;

namespace DataAccess
{
    public class RegionsRepository
    {
        private readonly string _connection;
        private SqlConnection _sqlConnection;

        public RegionsRepository(string connection)
        {
            _connection = connection ?? throw new ArgumentOutOfRangeException(nameof(connection), " can not to be null");
            _sqlConnection = new SqlConnection(_connection);
        }
        
        public SearchResult<Region> GetRegions(SearchCriteria searchCriteria, string _command)
        {
            try
            {
                _sqlConnection.Open();

                var command = new SqlCommand(_command, _sqlConnection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RegionID", searchCriteria.RegionID);               

                var entities = command.ReadMany<Region>();

                var searchResult = new SearchResult<Region>
                {
                    Entities = entities as List<Region>,                    
                };
                return searchResult;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
    }
}


