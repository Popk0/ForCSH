using System;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using DataAccess;
using Models.SearchModels;
using System.Xml.Serialization;
using Models;
using System.Collections.Generic;

namespace Pattern
{
    public class Program
    {        
        public static void Proc(string TargetPath, string fileName, string command)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Northwind"];

            var regionsRepository = new RegionsRepository(connectionString.ConnectionString);

            var searchCriteria = new SearchCriteria
            {
                RegionID = 1,
            };            

            var result = regionsRepository.GetRegions(searchCriteria, command);                        

            XmlSerializer formatter = new XmlSerializer(result.GetType());

            using (FileStream fs = new FileStream(Path.Combine(TargetPath, fileName), FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, result);               
            }
           
        }

    }
}