using API.DataAccess;
using API.Models;
using API.Repositories.Interface;

namespace API.Repositories.Implementation
{
    public class SignUpRepository : ISignUpRepository
    {
        public readonly FirebaseContext _firebaseContext;

        public SignUpRepository(FirebaseContext firebaseContext)
        {
            _firebaseContext = firebaseContext;
        }

        public async Task<ResultDTO> CreateUser(Users user)
        {
            ResultDTO resultDTO = new ResultDTO();
            resultDTO.success = false;

            try
            {
                var xpCode = await _firebaseContext.GetLatestXPCode();
                if(xpCode.Trim().Equals(user.XPCode.Trim()))
                {
                    bool userPresent = await _firebaseContext.IsUserCreated(user.XPCode);

                    if(!userPresent)
                    {
                        if(user.XWD.Equals(user.ConfirmXWD) && user.Email.Contains('@') && user.Email.Contains('.'))
                        {
                            UserDetails userDetails = new UserDetails();
                            userDetails.username = user.UserName.Trim();
                            userDetails.xwd = user.XWD.Trim();
                            userDetails.email = user.Email.Trim();
                            userDetails.premium = user.Premium;

                            await _firebaseContext.AddUser(userDetails, user.XPCode);

                            resultDTO.success = await _firebaseContext.IsUserCreated(user.XPCode);

                            if(resultDTO.success)
                            {
                                string[] userID = user.XPCode.Split("-");
                                int code = Convert.ToInt32(userID[1]) + 1;
                                userID[0] = "xp-00"+code;
                                await _firebaseContext.UpdateUserId(userID[0]);

                                var newXpCode = await _firebaseContext.GetLatestXPCode();
                                if(newXpCode.Trim().Equals(userID[0].Trim()))
                                {
                                    resultDTO.success = true;
                                }
                                else
                                {
                                    resultDTO.error = "Database error.";
                                    resultDTO.success = false;
                                }
                            }
                            else
                            {
                                resultDTO.error = "Unable to create user, try again later.";
                                resultDTO.success = false;
                            }
                        }
                        else
                        {
                            resultDTO.error = "Invalid email address.";
                            resultDTO.success = false;
                        }
                    }
                    else
                    {
                        resultDTO.success = false;
                        resultDTO.error = user.XPCode+" Already Present.";
                    }
                }
                else
                {
                    resultDTO.success = false;
                    resultDTO.error = "Invalid XP-Code.";
                }

                return resultDTO;
            }
            catch (Exception ex)
            {
                resultDTO.success = false;
                resultDTO.error = ex.Message;
                return resultDTO;
            }

        }
    }
}