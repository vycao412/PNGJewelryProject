using PNG.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PNG.Daos
{
    public class AccountDAO
    {
        private static AccountDAO instance;
        private AccountDAO()
        {
        }

        public static AccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountDAO();
                }
                return instance;
            }
        }

        private string _connectionString = ConfigurationManager.ConnectionStrings["PNG"].ConnectionString;

        public Account CheckLogin(Account user)
        {
            Account account = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT roleId, userName FROM tblAccount WHERE email = @email AND password = @password AND statusID = 1";
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@password", user.Password);
                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            string name = reader["userName"].ToString();
                            int roleId = Convert.ToInt32(reader["roleId"].ToString());
                            account = new Account(user.Email, name, roleId, 1);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return account;
        }

        public int GetRoleId(string email)
        {
            int roleId = -1;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT roleId FROM tblAccount WHERE email = @email AND statusID = 1";
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@email", email);
                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {                          
                             roleId = Convert.ToInt32(reader["roleId"].ToString());                        
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return roleId;
        }

        public bool AddNewAccount(Account account)
        {
            bool check = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO tblAccount(email, userName, password, statusID, roleId) VALUES(@email, @name, @pass, @statusId, @roleId)";
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@email", account.Email);
                command.Parameters.AddWithValue("@pass", account.Password);
                command.Parameters.AddWithValue("@name", account.UserName);
                command.Parameters.AddWithValue("@statusId", 1);
                command.Parameters.AddWithValue("@roleId", 2);
                try
                {
                    conn.Open();
                    check = command.ExecuteNonQuery() > 0;
                   
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            return check;
        }
    }
}