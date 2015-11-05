using System;

namespace GameStore.DAL.Static
{
    public static class KeyEncoder
    {
        public const Int32 Coefficient = 7;

        public static Int32 Encode(Int32 id, DatabaseTypes databaseType)
        {
            return Coefficient*id + (Int32) databaseType;
        }

        public static DatabaseTypes GetBase(Int32 id)
        {
            return (DatabaseTypes) (id%Coefficient);
        }

        public static Int32 GetId(Int32 id)
        {
            return id/Coefficient;
        }
    }
}
