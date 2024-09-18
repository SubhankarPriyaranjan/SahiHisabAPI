using System;
using System.Data.SqlClient;
using SahiHisabAPI.Data;
using SahiHisabAPI.Model;

namespace SahiHisabAPI.SahiHisabDA
{
    public class AccountDA : IAccountDA
    {
        // Use DataAccess to handle database operations
        private readonly DataAccess _dbContext;

        // Constructor to inject DataAccess (e.g., via Dependency Injection)
        public AccountDA(DataAccess dbContext)
        {
            _dbContext = dbContext;
        }

        // Method to register a new account
        public int RegisterAccount(Register obj)
        {
            try
            {
                // Initialize Parameters
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@Email", obj.Email);
                param[1] = new SqlParameter("@Phone", obj.Phone);
                param[2] = new SqlParameter("@FirstName", obj.FirstName);
                param[3] = new SqlParameter("@LastName", obj.LastName);
                param[4] = new SqlParameter("@Password", obj.Password);
                param[5] = new SqlParameter("@ConfirmPass", obj.ConfirmPassword);

                // Execute the stored procedure to insert into the Register table
                int result = _dbContext.ExecuteNonQuery("SP_RegisterAccount", param);
                // The stored procedure returns the Organization ID (orgId)
                return result; 

            }
            catch (Exception e)
            {
                // Handle the exception (logging can be added here)
                Console.WriteLine($"Error occurred: {e.Message}");

                // Return -1 or another value to indicate failure
                return -1;
            }
        }
    }
}
