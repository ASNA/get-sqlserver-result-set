using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace SqlServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            string sql = @"SELECT SUM([cmcustnox]), SUM([jan]) as Jan, SUM([feb]) as Feb, SUM([mar]) as Mar FROM cmsales";
            string databaseName = "testsql";
            int fieldCount = ResultSets.CountStringOccurrences(sql.ToLower(), "sum");

            decimal[] results = new decimal[fieldCount];

            try
            {
                results = (new ResultSets()).GetResults(databaseName, sql, fieldCount);
                int i = 0;
                foreach (decimal result in results)
                {
                    Console.WriteLine("{0} = {1}", i++, result);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
         }
    }

    public class ResultSets
    {
        public decimal[] GetResults(string databaseName, string sql, int fieldCount)
        {
            string x = ConfigurationManager.ConnectionStrings["sqlserverconnection"].ConnectionString;

            string connectionString = String.Format(ConfigurationManager.ConnectionStrings["sqlserverconnection"].ConnectionString, databaseName);

            decimal[] results = new decimal[fieldCount]; 

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                if (dr.FieldCount != fieldCount)
                                {
                                    throw new ArgumentException("SQL field count is not correct");
                                }
                                for (int i = 0; i < dr.FieldCount; i++)
                                {
                                    results[i] = dr.GetFieldValue<decimal>(i);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return results;
        }

        static public int CountStringOccurrences(string text, string pattern)
        {
            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }
    }
}
    

