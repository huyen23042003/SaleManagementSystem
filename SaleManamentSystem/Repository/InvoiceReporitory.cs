using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SaleManamentSystem.Models;

namespace SaleManamentSystem.Repository
{
    public class InvoiceReporitory
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["SMSDbConn"].ConnectionString;

        public List<InvoiceEntity> GetAllInvoices()
        {
            List<InvoiceEntity> invoices = new List<InvoiceEntity>();
            using (SqlConnection sqlConnection = new SqlConnection(this._connectionString))
            {
                String sql = "SELECT * FROM INVOICES";
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(sql, sqlConnection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        invoices.Add(new InvoiceEntity
                        {
                            InvoiceID = reader["InvoiceID"].ToString(),
                            CustomerID = reader["CustomerID"].ToString(),
                            InvoiceDate = Convert.ToDateTime(reader["InvoiceDate"]),
                            TotalPrice = Convert.ToDecimal(reader["TotalPrice"])

                        });
                    }
                }
            }
            return invoices;
        }

        public bool AddInvoice(InvoiceEntity invoice)
        {
            using (SqlConnection sqlConnection = new SqlConnection(this._connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection;
                cmd.CommandText = "INSERT INTO INVOICES VALUES (InvoiceId = @InvoiceID, CustomerID = @CustomerID, InvoiceDate = @InvoiceDate, TotalPrice = @TotalPrice)";

                sqlConnection.Open();
                return cmd.ExecuteNonQuery() > 0;

            }
        }
    }
}