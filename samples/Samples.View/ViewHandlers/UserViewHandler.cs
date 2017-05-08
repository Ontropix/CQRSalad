using System.Data;
using System.Threading.Tasks;
using CQRSalad.Dispatching;
using Dapper;
using Kutcha.Core;
using Npgsql;
using Samples.Domain.User;
using Samples.ViewModel.Views;

namespace Samples.ViewModel.ViewHandlers
{
    [DispatcherHandler]
    [DispatchingPriority(Priority.High)]
    public sealed class UserViewHandler
    {
        private readonly IKutchaStore<UserView> _store;

        public UserViewHandler(IKutchaStore<UserView> store)
        {
            _store = store;
        }

        public async Task Apply(UserCreated evnt)
        {
            await _store.InsertAsync(new UserView
            {
                Id = evnt.UserId,
                Email = evnt.Email,
            });
        }
    }

    //[DispatcherHandler]
    //[DispatchingPriority(Priority.Normal)]
    //public sealed class UsersViewHandlerPostgre
    //{
    //    private readonly string _connectionString = "User ID=postgres;Password=7799;Host=localhost;Port=5432;Database=cqrsalad;Pooling=true;";

    //    public async Task Apply(UserCreated evnt)
    //    {
    //        using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
    //        {
    //            dbConnection.Open();
    //            int result = await dbConnection.ExecuteAsync(
    //                "INSERT INTO users (id, email) " +
    //                "VALUES (@Id, @Email)", 
    //                new
    //                {
    //                    Id = evnt.UserId,
    //                    Email = evnt.Email
    //                });

    //        }
    //    }
    //}
}
