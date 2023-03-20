using System;
using System.Data.SqlClient;
using System.Security.Cryptography.Xml;
using TEbucksServer.Models;

namespace TEbucksServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string _connectString)
        {
            connectionString = _connectString;
        }
        public Transfer GetTransferByTransferId(int transfer_id)
        {
            Transfer transfer = null;
            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "select transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount from transfer where transfer_id = @transferId";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@transferId", transfer_id);
                    cmd.Connection = conn;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        transfer = CreateTransferFromReader(reader);
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }
            return transfer;
        }

        public Transfer InsertTransfer(Transfer transfer)
        {
            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "insert into transfer (account_from, account_to, amount, transfer_type_id, transfer_status_id) " +
                        "values (@account_from, @account_to, @amount, @transfer_type_id, @transfer_status_id)";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@account_from", transfer.Account_From);
                    cmd.Parameters.AddWithValue("@account_to", transfer.Account_To);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);
                    cmd.Parameters.AddWithValue("@transfer_type_id", transfer.Transfer_Type_Id);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transfer.Transfer_Status_Id);
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();


                    cmd = new SqlCommand("select @@IDENTITY", conn);
                    int newId = Convert.ToInt32(cmd.ExecuteScalar());

                    transfer.Transfer_Id = newId;
                    return transfer;
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }
        public bool UpdateTransfer(Transfer transfer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "UPDATE transfer SET transfer_type_id = @transferTypeId, transfer_status_id = @transferStatusId, " +
                        "account_from = @accountFrom, account_to = @accountTo, amount = @amount WHERE transfer_id = @transferId";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@transferId", transfer.Transfer_Id);
                    cmd.Parameters.AddWithValue("@transferTypeId", transfer.Transfer_Type_Id);
                    cmd.Parameters.AddWithValue("@transferStatusId", transfer.Transfer_Status_Id);
                    cmd.Parameters.AddWithValue("@accountFrom", transfer.Account_From);
                    cmd.Parameters.AddWithValue("@accountTo", transfer.Account_To);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    int rowsUpdated = cmd.ExecuteNonQuery();

                    return rowsUpdated == 1;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public string GetAccountToUsername(int accountId)
        {
            
            string username = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "select username from tebucks_user as tu join account as a on tu.user_id = a.user_id where account_id = @accountId";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@accountId", accountId);
                    cmd.Connection = conn;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        username = reader.GetString(0);
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            return username;
        }
        private Transfer CreateTransferFromReader(SqlDataReader reader)
        {
            Transfer tempTransfer = new Transfer();
            tempTransfer.Transfer_Id = Convert.ToInt32(reader["transfer_id"]);
            tempTransfer.Transfer_Type_Id = Convert.ToInt32(reader["transfer_type_id"]);
            tempTransfer.Transfer_Status_Id = Convert.ToInt32(reader["transfer_status_id"]);
            tempTransfer.Account_From = Convert.ToInt32(reader["account_from"]);
            tempTransfer.Account_To = Convert.ToInt32(reader["account_to"]);
            tempTransfer.Amount = Convert.ToDecimal(reader["amount"]);

            return tempTransfer;
        }
    }
}
