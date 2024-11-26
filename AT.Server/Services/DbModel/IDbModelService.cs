namespace AT.Server.Services.DbModel
{
    public interface IDbModelService
    {
        bool ModelExists<T>(int id) where T : class;
    }
}
