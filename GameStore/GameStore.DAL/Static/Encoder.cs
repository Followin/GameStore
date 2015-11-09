using System;

namespace GameStore.DAL.Static
{
    public static class KeyEncoder
    {
        /// <summary>
        /// Coefficient of hashing
        /// </summary>
        public const Int32 Coefficient = 7;

        /// <summary>
        /// Gets encoded id
        /// </summary>
        /// <param name="id">Id to encode</param>
        /// <param name="databaseType">Database type enum</param>
        /// <returns>Encoded id</returns>
        public static Int32 Encode(Int32 id, DatabaseTypes databaseType)
        {
            return Coefficient*id + (Int32) databaseType;
        }

        /// <summary>
        /// Get database type by ecoded id
        /// </summary>
        /// <param name="id">Encoded id</param>
        /// <returns>Database type</returns>
        public static DatabaseTypes GetBase(Int32 id)
        {
            return (DatabaseTypes) (id%Coefficient);
        }

        /// <summary>
        /// Get id by encoded one
        /// </summary>
        /// <param name="id">Encoded id</param>
        /// <returns>Original id</returns>
        public static Int32 GetId(Int32 id)
        {
            return id/Coefficient;
        }
    }
}
