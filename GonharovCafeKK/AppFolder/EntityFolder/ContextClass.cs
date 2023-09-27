using System.Data.Entity;

namespace GonharovCafeKK.AppFolder.EntityFolder
{
    public partial class DBEntities : DbContext
    {
        private static DBEntities context;

        public static DBEntities GetContext()
        {
            if(context == null)
            {
                context = new DBEntities();
            }
            return context;
        }
    }
}
