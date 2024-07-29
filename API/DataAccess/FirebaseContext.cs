using API.Models;
using Firebase.Database;
using Firebase.Database.Query;

namespace API.DataAccess
{
    public class FirebaseContext
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseContext()
        {
            // Initialize FirebaseClient with your database URL
            _firebaseClient = new FirebaseClient("https://xpense-android-default-rtdb.firebaseio.com/");
        }

        public async Task WriteDataAsync(string path, object data)
        {
            await _firebaseClient
                .Child(path)
                .PutAsync(data);
        }

        public async Task AddDataAsync(string path, string key, string data)
        {
            await _firebaseClient
                .Child(path)
                .Child(key)
                .PutAsync(data);
        }

        public async Task<object> ReadDataAsync(string path)
        {
            var result = await _firebaseClient
                .Child(path)
                .OnceSingleAsync<object>();
            return result;
        }

        public async Task<string> GetLatestXPCode()
        {
            var result = await _firebaseClient.Child("UserId").OnceSingleAsync<string>();
            return result;
        }

        public async Task AddUser(UserDetails user, string XPCode)
        {
            await _firebaseClient.Child("USERS/"+XPCode).PutAsync(user);
        }

        public async Task<bool> IsUserCreated(string xpCode)
        {
            var snapshot = await _firebaseClient
                .Child("USERS/"+xpCode)
                .OnceSingleAsync<object>();
            
            return snapshot != null;
        }

        public async Task UpdateUserId(string val)
        {
            await _firebaseClient.Child("UserId").PutAsync(val);
        }

        public async Task<int> GetDataCount(string path)
        {
            var result = await _firebaseClient
                .Child(path)
                .OnceAsync<object>();

            return result.Count();
        }

        public async Task<bool> checkMaxNumber(string val)
        {
            var snapshot = await _firebaseClient
                .Child(val)
                .OnceSingleAsync<object>();
            
            return snapshot != null;
        }

    }
}