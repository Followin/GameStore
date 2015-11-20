using System;

namespace GameStore.DAL.Static
{
    public static class KeyEncoder
    {
        /// <summary>
        /// Coefficient of hashing
        /// </summary>
        public const int Coefficient = 7;

        public static int GetNext(int maxId)
        {
            return maxId + Coefficient;
        }

        public static int GetNext(DatabaseTypes type)
        {
            return Coefficient + (int)type;
        }

        /// <summary>
        /// Gets encoded id
        /// </summary>
        /// <param name="id">Id to encode</param>
        /// <param name="databaseType">Database type enum</param>
        /// <returns>Encoded id</returns>
        public static int Encode(int id, DatabaseTypes databaseType)
        {
            return Coefficient*id + (int) databaseType;
        }

        /// <summary>
        /// Get database type by ecoded id
        /// </summary>
        /// <param name="id">Encoded id</param>
        /// <returns>Database type</returns>
        public static DatabaseTypes GetBase(int id)
        {
            return (DatabaseTypes) (id%Coefficient);
        }

        /// <summary>
        /// Get id by encoded one
        /// </summary>
        /// <param name="id">Encoded id</param>
        /// <returns>Original id</returns>
        public static int GetId(int id)
        {
            return id/Coefficient;
        }
    }
}
