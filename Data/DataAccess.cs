using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SahiHisabAPI.Data
{
    public class DataAccess : IDisposable
    {
        private readonly string? _connectionString;  // Mark as nullable
        private SqlConnection? _connection;  // Mark as nullable
        private SqlTransaction? _transaction;  // Mark as nullable
        private bool _isTransaction;

        // Constructor accepts IConfiguration to inject the configuration
        public DataAccess(IConfiguration configuration)
        {
            // Use "DefaultConnection" from appsettings.json and handle potential null value
            _connectionString = configuration.GetConnectionString("Environment")
                ?? throw new InvalidOperationException("Connection string not found.");  // Throw exception if null

            _connection = new SqlConnection(_connectionString);  // Initialize connection
        }

        // Methods for data access operations
        public DataSet GetDataSet(string sqlStr)
        {
            EnsureConnectionOpen();
            var ds = new DataSet();
            using (var cmd = new SqlCommand(sqlStr, _connection))
            {
                cmd.CommandTimeout = 800;
                var da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            return ds;
        }

        public DataSet GetDataSet(string storedProcedureName, SqlParameter[] parameters)
        {
            EnsureConnectionOpen();
            var ds = new DataSet();
            using (var cmd = new SqlCommand(storedProcedureName, _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 800;
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                var da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            return ds;
        }

        public object ExecuteScalar(string sqlStr)
        {
            EnsureConnectionOpen();
            using (var cmd = new SqlCommand(sqlStr, _connection))
            {
                cmd.CommandType = CommandType.Text;
                return cmd.ExecuteScalar();
            }
        }

        public object ExecuteScalar(string storedProcedureName, SqlParameter[] parameters)
        {
            EnsureConnectionOpen();
            using (var cmd = new SqlCommand(storedProcedureName, _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        public int ExecuteNonQuery(string query)
        {
            EnsureConnectionOpen();
            using (var cmd = new SqlCommand(query, _connection))
            {
                cmd.CommandType = CommandType.Text;
                return cmd.ExecuteNonQuery();
            }
        }

        public int ExecuteNonQuery(string storedProcedureName, SqlParameter[] parameters)
        {
            EnsureConnectionOpen();
            using (var cmd = new SqlCommand(storedProcedureName, _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                if (_isTransaction)
                    cmd.Transaction = _transaction;
                return cmd.ExecuteNonQuery();
            }
        }

        public void BeginTransaction()
        {
            // Ensure _connection is not null before proceeding
            if (_connection == null)
            {
                throw new InvalidOperationException("Database connection has not been initialized.");
            }

            EnsureConnectionOpen();
            _transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);
            _isTransaction = true;
        }


        public void EndTransaction(bool commit)
        {
            if (_transaction != null && _isTransaction)
            {
                if (commit)
                    _transaction.Commit();
                else
                    _transaction.Rollback();
            }

            CloseConnection();
            _isTransaction = false;
        }

        private void EnsureConnectionOpen()
        {
            if (_connection?.State == ConnectionState.Closed)
                _connection.Open();
        }

        private void CloseConnection()
        {
            if (_connection?.State == ConnectionState.Open)
                _connection.Close();
        }

        public void Dispose()
        {
            CloseConnection();
            _connection?.Dispose();
        }
    }
}
