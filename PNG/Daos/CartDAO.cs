using PNG.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PNG.Daos
{
    public class CartDAO
    {
        private static CartDAO instance;
        private CartDAO()
        {
        }

        public static CartDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CartDAO();
                }
                return instance;
            }
        }

        private string _connectionString = ConfigurationManager.ConnectionStrings["PNG"].ConnectionString;

        public bool AddNewCart(Cart cart, Account user)
        {
            bool check = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO tblOrder(email, totalPrice, paymentType, statusId, phone, address, name) VALUES(@email, @total, @payment, @statusId, @phone, @address, @name)";
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@total", cart.TotalPrice);
                command.Parameters.AddWithValue("@payment", "Cash");
                command.Parameters.AddWithValue("@statusId", 5);
                command.Parameters.AddWithValue("@phone", cart.Phone);
                command.Parameters.AddWithValue("@address", cart.Address);
                command.Parameters.AddWithValue("@name", cart.Name);
                try
                {
                    conn.Open();
                    check = command.ExecuteNonQuery() > 0;
                }
                catch (Exception e)
                {

                    Console.WriteLine(e);
                }
            }
            return check;
        }

        public string GetOrderID()
        {
            string id = string.Empty;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT TOP 1 orderId FROM tblOrder ORDER BY orderDate DESC";
                SqlCommand command = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            id = reader["orderID"].ToString();
                        }
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine(e);
                }
            }
            return id;
        }

        public void AddOrderDetail(Cart cart)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    foreach (Product item in cart.CartDetail.Values)
                    {
                        string sql = "INSERT INTO tblOrderDetail(orderId, productId, price, quantity) VALUES(@orderID, @productId, @price, @quantity)";
                        SqlCommand command = new SqlCommand(sql, conn);
                        command.Parameters.AddWithValue("@orderID", cart.CartID);
                        command.Parameters.AddWithValue("@productId", item.ProductID);
                        command.Parameters.AddWithValue("@price", item.Price);
                        command.Parameters.AddWithValue("@quantity", item.Quantity);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }   
            }
        }

        public List<Cart> GetHistory(Account user)
        {
            List<Cart> listHistory = new List<Cart>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM tblOrder WHERE email = @email AND (statusId = 5 OR statusId = 6)";
                    SqlCommand command = new SqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@email", user.Email);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string orderId = reader["orderId"].ToString();
                            float totalPrice = (float)Convert.ToDouble(reader["totalPrice"].ToString());
                            string payment = reader["paymentType"].ToString();
                            int statusId = Convert.ToInt32(reader["statusId"].ToString());
                            DateTime orderDate = (DateTime)reader["orderDate"];
                            string phone = reader["phone"].ToString();
                            string address = reader["address"].ToString();
                            string name = reader["name"].ToString();
                            Dictionary<string, Product> cartDetail = new Dictionary<string, Product>();

                            listHistory.Add(new Cart(orderId, null, totalPrice, payment, statusId, orderDate, phone, address, name, cartDetail));
                        }
                    }
                    reader.Close();
                    sql = "SELECT P.productId, P.productName, O.price, O.quantity FROM tblOrderDetail O, tblProduct P WHERE O.productId = P.productId AND orderId = @orderId";
                    foreach (Cart item in listHistory)
                    {
                        command = new SqlCommand(sql, conn);
                        command.Parameters.AddWithValue("@orderId", item.CartID);
                        reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string productId = reader["productId"].ToString();
                                string productName = reader["productName"].ToString();
                                int quantity = Convert.ToInt32(reader["quantity"].ToString());
                                float price = (float)Convert.ToDouble(reader["price"].ToString());
                                item.CartDetail[productId] = new Product(productId, productName, quantity, price);
                            }
                        }
                        reader.Close();
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return listHistory;
        }
    }
}