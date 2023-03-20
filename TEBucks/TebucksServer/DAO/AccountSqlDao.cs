using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Principal;
using TEbucksServer.Models;

namespace TEbucksServer.DAO
{
    public class AccountSqlDao : IAccountDao
    {
        private readonly string connectionString;

        public AccountSqlDao(string _connectionString)
        {
            connectionString = _connectionString;
        }
        public Account GetBalance(int id)
        {
            Account account = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT * " +
                        "FROM account  "+
                        "WHERE user_id = @user_id;";

                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@user_id", id);
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        account = CreateAccountFromReader(reader);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return account;
        }
        public List<Transfer> GetListOfTransfer(int id)
        {
            List<Transfer> userTransfers = new List<Transfer>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    Transfer transfer = new Transfer();
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT * FROM transfer as t " +
                        "JOIN account as a ON a.account_id = t.account_from " +
                        "WHERE a.user_id = @user_id";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@user_id", id);
                    cmd.Connection = conn;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        userTransfers.Add(CreateTransferFromReader(reader));
                    }
                    conn.Close();
                }

            }
            catch (Exception)
            {

                throw;
            }
            return userTransfers;
        }
        public List<Transfer> GetTransfersByAccountId(string username)
        {
            List<Transfer> result = new List<Transfer>();

            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT transfer_id, transfer_type_id, transfer_status_id, CASE t.transfer_status_id WHEN 1 THEN 'send' WHEN 2 THEN 'approved' WHEN 3 THEN 'reject' ELSE 'unknown' END AS transfer_status, account_from, account_to, amount " +
                        "FROM transfer t JOIN account a ON t.account_from = a.account_id OR t.account_to = a.account_id " +
                        "JOIN tebucks_user te ON te.user_id = a.user_id WHERE te.username = @username;";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfer transfer = CreateTransferFromReader(reader);
                        result.Add(transfer);
                    }
                }
            }
            catch(Exception e)
            {
                throw;
            }
            return result;
        }
        public Account GetAccountById(int accountId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT account_id, user_id, balance FROM account WHERE account_id = @account_id;";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@account_id", accountId);
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Account account = CreateAccountFromReader(reader);
                        return account;
                    }
                }
            }
            catch
            {
                
            }
            return null;
        }
        public bool UpdateAccount(int accountid, decimal newBalance)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string sql = "UPDATE account SET balance = @balance WHERE account_id = @account_id";
                    cmd.CommandText = sql;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@balance", newBalance);
                    cmd.Parameters.AddWithValue("@account_id", accountid);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return (rowsAffected > 0);
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }
        private Account CreateAccountFromReader(SqlDataReader reader)
        {
            Account tempAccount = new Account();
            tempAccount.Account_Id = Convert.ToInt32(reader["account_id"]);
            tempAccount.User_Id = Convert.ToInt32(reader["user_id"]);
            tempAccount.Balance = Convert.ToDecimal(reader["balance"]);

            return tempAccount;
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
