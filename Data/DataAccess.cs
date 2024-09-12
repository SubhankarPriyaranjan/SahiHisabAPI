using Microsoft.EntityFrameworkCore;

namespace SahiHisabAPI.Data
{
    public class DataAccess : DbContext
        //DbContext is a collection of DB set its helps to communicate with Database 
    {
        public DataAccess(DbContextOptions<DataAccess> options):base(options) 
        {
        
        }

        // Here are the Model Class 
        //like this public Dbset<Model Name ...> Model NAme .. {get; set;}

    }
}
