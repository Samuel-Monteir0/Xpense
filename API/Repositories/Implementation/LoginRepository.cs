
using API.DataAccess;
using API.Models;
using API.Repositories.Interface;

namespace API.Repositories.Implementation
{
    public class LoginRepository : ILoginRepository
    {
        public readonly FirebaseContext _firebaseContext;

        public LoginRepository(FirebaseContext firebaseContext)
        {
            _firebaseContext = firebaseContext;
        }

        public async Task<CurrentUser> LoginUser(Users user)
        {
            var currUser = new CurrentUser();
            currUser.username = user.UserName;
            currUser.email = user.Email;
            currUser.xpCode = user.XPCode;
            currUser.result = false;

            try
            {
                bool userPresent = await _firebaseContext.IsUserCreated(user.XPCode);

                if(userPresent)
                {
                    var dbXWD = _firebaseContext.ReadDataAsync("USERS/" + user.XPCode + "/xwd").Result;
                    if(user.XWD.Trim().Equals(dbXWD) && checkUserName(user))
                    {
                        currUser.result = true;
                        if(user.UserName.Contains("@") && user.UserName.Contains("."))
                        {
                            currUser.email = user.UserName;
                            currUser.username = _firebaseContext.ReadDataAsync("USERS/" + user.XPCode + "/username").Result.ToString();
                        }
                        else
                        {
                            currUser.username = user.UserName;
                            currUser.email = _firebaseContext.ReadDataAsync("USERS/" + user.XPCode + "/email").Result.ToString();
                        }
                    }
                    else
                    {
                        currUser.error = "Username/password is incorrect.";
                        currUser.result = false;
                    }
                }
                else
                {
                    currUser.error = "Invalid XP-Code";
                    currUser.result = false;
                }

                return currUser;
            }
            catch (Exception ex)
            {
                currUser.error = ex.Message;
                currUser.result = false;
                return currUser;
            }
        }
    
        bool checkUserName(Users user)
        {
            try
            {
                string dbName = "";
                if(user.UserName.Contains("@") && user.UserName.Contains("."))
                {
                    dbName = _firebaseContext.ReadDataAsync("USERS/" + user.XPCode + "/email").Result.ToString();
                }
                else
                {
                    dbName = _firebaseContext.ReadDataAsync("USERS/" + user.XPCode + "/username").Result.ToString();
                }

                if(dbName.Equals(user.UserName.Trim()))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}