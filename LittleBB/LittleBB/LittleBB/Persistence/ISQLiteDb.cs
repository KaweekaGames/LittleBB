using SQLite;

namespace LittleBB
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
