using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SaleManamentSystem.Models;

namespace SaleManamentSystem.Repository
{
    public class CustomerRepository
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["SMSDbConn"].ConnectionString;

        public List<CustomerEntity> GetAllCustomers()
        {
            var Customers = new List<CustomerEntity>();
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "SELECT * FROM Customers";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customers.Add(new CustomerEntity
                        {
                            CustomerID = reader["CustomerID"].ToString(),
                            CustomerName = reader["CustomerName"].ToString(),
                            Phone = reader["Phone"].ToString(),
                        });
                    }
                }
            }
            return Customers;
        }

        public CustomerEntity GetCustomerByID(string CustomerID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "SELECT * FROM Customers WHERE CustomerID = @ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", CustomerID);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new CustomerEntity
                        {
                            CustomerID = reader["CustomerID"].ToString(),
                            CustomerName = reader["CustomerName"].ToString(),
                            Phone = reader["Phone"].ToString(),
                        };
                    }
                }
            }
            return null;
        }

        public bool AddNewCustomer(CustomerEntity Customer)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "INSERT INTO Customers(CustomerID, CustomerName, Phone) VALUES (@ID, @Name, @Phone)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", Customer.CustomerID.Trim().ToUpper());
                cmd.Parameters.AddWithValue("@Name", Customer.CustomerName.Trim());
                cmd.Parameters.AddWithValue("@Phone", Customer.Phone);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateCustomer(CustomerEntity Customer)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "UPDATE Customers SET CustomerName = @Name, Phone = @Phone WHERE CustomerID = @ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", Customer.CustomerID);
                cmd.Parameters.AddWithValue("@Name", Customer.CustomerName.Trim());
                cmd.Parameters.AddWithValue("@Phone", Customer.Phone);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool IsCustomerInInvoice(string CustomerID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "SELECT COUNT(1) FROM Invoices WHERE CustomerID = @ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", CustomerID);
                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public bool DeleteCustomer(string CustomerID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "DELETE FROM Customers WHERE CustomerID = @ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", CustomerID);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}