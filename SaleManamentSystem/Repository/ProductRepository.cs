using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SaleManamentSystem.Models;

namespace SaleManamentSystem.Repository
{
    public class ProductRepository
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["SMSDbConn"].ConnectionString;

        public List<ProductEntity> GetAllProducts()
        {
            var products = new List<ProductEntity>();
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "SELECT ProductID, ProductName, Price FROM Products"; 
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new ProductEntity
                        {
                            ProductID = reader["ProductID"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"])
                        });
                    }
                }
            }
            return products;
        }

        public ProductEntity GetProductByID(string productID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "SELECT ProductID, ProductName, Price FROM Products WHERE ProductID = @ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", productID.ToUpper());
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ProductEntity
                        {
                            ProductID = reader["ProductID"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"])
                        };
                    }
                }
            }
            return null;
        }

        public bool AddNewProduct(ProductEntity product)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "INSERT INTO Products(ProductID, ProductName, Price) VALUES (@ID, @Name, @Price)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ID", product.ProductID.Trim().ToUpper());
                cmd.Parameters.AddWithValue("@Name", product.ProductName.Trim());
                cmd.Parameters.AddWithValue("@Price", product.Price);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateProduct(ProductEntity product)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "UPDATE Products SET ProductName = @Name, Price = @Price WHERE ProductID = @ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", product.ProductID);
                cmd.Parameters.AddWithValue("@Name", product.ProductName.Trim());
                cmd.Parameters.AddWithValue("@Price", product.Price);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool IsProductInInvoice(string productID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "SELECT COUNT(1) FROM InvoiceDetails WHERE ProductID = @ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", productID);
                conn.Open();
                int i = Convert.ToInt16(cmd.ExecuteScalar());
                return i > 0;
            }
        }

        public bool DeleteProduct(string productID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "DELETE FROM Products WHERE ProductID = @ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", productID);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

       
    }
}