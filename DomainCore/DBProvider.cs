using DomainCore.Context;

namespace DomainCore
{
    public static class DBProvider
    {
        private static JournalDBContext dBContext = null;
        public static JournalDBContext Context
        {
            get
            {
                if (dBContext == null)
                    dBContext = new JournalDBContext();
                return dBContext;
            }
        }
    }
}
