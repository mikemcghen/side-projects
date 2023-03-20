using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TEbucksServer.Models;

namespace TEbucksServer.DAO
{
    public class TransferStatusSqlDao : ITransferStatusDao 
    {
        private readonly string connectionString;

        public TransferStatusSqlDao(string _connectionString) 
        {
            this.connectionString = _connectionString;
        }

        public bool UpdateTransferStatus(int transferId, string transferStatus)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    string sql = "UPDATE transfer " +
                        "SET transfer_status_id = (SELECT transfer_status_id FROM transfer_status " +
                        "WHERE transfer_status_desc = @transfer_status_desc) WHERE transfer_id = @transfer_id";
;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@transfer_id", transferId);
                    command.Parameters.AddWithValue("@transfer_status_desc", transferStatus);
                    command.Connection = connection;

                    int rowsAffected = command.ExecuteNonQuery();
                    return (rowsAffected > 0);
                }
            }
            catch(Exception e)
            {
                throw;
            }
        }
        public int GetTransferStatusByName(string transferStatusName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT transfer_status_id FROM transfer_status WHERE transfer_status_desc = @transfer_status_desc";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@transfer_status_desc", transferStatusName);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return Convert.ToInt32(reader["transfer_status_id"]);
                    }
                    else
                    {
                        throw new Exception("Transfer Status not found");
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

    }
}
