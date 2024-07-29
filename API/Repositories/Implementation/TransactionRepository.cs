
using API.DataAccess;
using API.Models;
using API.Repositories.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DateTime = System.DateTime;

namespace API.Repositories.Implementation
{
    public class TransactionRepository : ITransactionRepository
    {
        public readonly FirebaseContext _firebaseContext;

        public TransactionRepository(FirebaseContext firebaseContext)
        {
            _firebaseContext = firebaseContext;
        }
        public async Task<ResultDTO> AddData(Transaction transaction)
        {
            ResultDTO resultDTO = new ResultDTO();
            resultDTO.success = false;
            try
            {
                int count = await _firebaseContext.GetDataCount("USERS/"+transaction.XpCode+"/data");
                string num = ""+count+"~";
                if (count < 10)
                {
                    num = "0"+count+"~";
                }
                
                while(await _firebaseContext.checkMaxNumber("USERS/"+transaction.XpCode+"/data/"+num+"C") || await _firebaseContext.checkMaxNumber("USERS/"+transaction.XpCode+"/data/"+num+"D") )
                {
                    count++;
                    if (count < 10)
                    {
                        num = "0"+count+"~";
                    }
                    else
                    {
                        num = ""+count+"~";
                    }
                }

                Dictionary<string, string> map = new Dictionary<string, string>();
                string CD = "C";
                if(transaction.TransType.ToLower().Equals("debited"))
                {
                    CD = "D";
                }
                map.Add(num+CD,transaction.Amount+"~"+transaction.Account+"~"+"18/07/24 22:17");

                await _firebaseContext.AddDataAsync("USERS/"+transaction.XpCode+"/data",num+CD ,transaction.Amount+"~"+transaction.Account+"~"+transaction.Date+" 12:00");

                resultDTO.success = true;
                return resultDTO;
            }
            catch (Exception ex)
            {
                resultDTO.error = ex.Message;
                resultDTO.success = false;
                return resultDTO;
            }
        }

        public async Task<TotalCredDebt> ShowData(MonthTransaction monthTransaction)
        {
            TotalCredDebt showData = new TotalCredDebt();
            showData.ShowData = new List<AmountData>();
            showData.success = false;
            float CredCount = 0;
            float DebtCount = 0;
            try
            {
                if(monthTransaction.Month == null)
                {
                     DateTime currentDate = DateTime.Now;
        
                    string currentMonth = currentDate.Month.ToString("00"); // Format month as two digits
                    string currentYear = currentDate.ToString("yy"); // Format year as two digits
        
                    monthTransaction.Month = currentMonth+"/"+currentYear;
                }

                var data = _firebaseContext.ReadDataAsync("USERS/"+monthTransaction.XpCode+"/data").Result;

                string dbData = JsonConvert.SerializeObject(data);

                var arr = JObject.Parse(dbData);
                
                foreach(var dt in arr)
                {
                    string KeyValue = dt.Key;
                    string[] temp = KeyValue.Split("~");
                    string CDType = "Credited";
                    if(temp[1].Equals("D"))
                    {
                        CDType = "Debited";
                    }
                    KeyValue = dt.Value.ToString();
                    temp = KeyValue.Split("~");

                    string[] verifyCalendar = temp[2].Split();
                    string[] verifyMonth = verifyCalendar[0].Split("/");
                    verifyMonth[0] = verifyMonth[1]+"/"+verifyMonth[2];
                    if(verifyMonth[0].Equals(monthTransaction.Month)) 
                    {
                        showData.ShowData.Add(new AmountData
                        {
                            Account = temp[1],
                            Amount = float.Parse(temp[0]),
                            Type = CDType,
                            Date = temp[2]
                        });

                        if(CDType.Equals("Credited"))
                        {
                            CredCount = CredCount + float.Parse(temp[0]);
                        }
                        else
                        {
                            DebtCount = DebtCount + float.Parse(temp[0]);
                        }
                    }
                }

                showData.Cred = CredCount;
                showData.Debt = DebtCount;
                showData.Total = CredCount + DebtCount;
                showData.success = true;
                return showData;
            }
            catch (Exception ex)
            {
                showData.error = ex.Message;
                showData.success = false;
                return showData;
            }
        }

        public async Task<DailyTrend> ShowDailyTrend(MonthTransaction monthTransaction)
        {
            DailyTrend dailyTrend = new DailyTrend();
            dailyTrend.success = false;

            try
            {
                dailyTrend.Monday = new TotalCredDebt();
                dailyTrend.Tuesday = new TotalCredDebt();
                dailyTrend.Wednesday = new TotalCredDebt();
                dailyTrend.Thursday = new TotalCredDebt();
                dailyTrend.Friday = new TotalCredDebt();
                dailyTrend.Saturday = new TotalCredDebt();
                dailyTrend.Sunday = new TotalCredDebt();

                if(monthTransaction.Month == null)
                {
                     DateTime currentDate = DateTime.Now;
        
                    string currentMonth = currentDate.Month.ToString("00"); // Format month as two digits
                    string currentYear = currentDate.ToString("yy"); // Format year as two digits
        
                    monthTransaction.Month = currentMonth+"/"+currentYear;
                }

                var data = _firebaseContext.ReadDataAsync("USERS/"+monthTransaction.XpCode+"/data").Result;

                string dbData = JsonConvert.SerializeObject(data);

                var arr = JObject.Parse(dbData);

                foreach(var dt in arr)
                {
                    string KeyValue = dt.Key;
                    string[] temp = KeyValue.Split("~");
                    string CDType = "Credited";
                    if(temp[1].Equals("D"))
                    {
                        CDType = "Debited";
                    }
                    KeyValue = dt.Value.ToString();
                    temp = KeyValue.Split("~");

                    string[] verifyCalendar = temp[2].Split();
                    string[] verifyMonth = verifyCalendar[0].Split("/");
                    string monthYear = verifyMonth[1]+"/"+verifyMonth[2];
                    if(monthYear.Equals(monthTransaction.Month)) 
                    {
                        // Assuming "18/07/24" means July 24, 2018 (yy/MM/dd format)
                        DateTime givenDate = new DateTime(Convert.ToInt32(20+verifyMonth[2]), Convert.ToInt32(verifyMonth[1]), Convert.ToInt32(verifyMonth[0])); 

                        // Get the day of the week
                        DayOfWeek dayOfWeek = givenDate.DayOfWeek;

                        switch (dayOfWeek) {
                            case DayOfWeek.Monday:
                                    if(CDType.Equals("Credited"))
                                    {
                                        dailyTrend.Monday.Cred = (dailyTrend.Monday.Cred == null ? 0 : dailyTrend.Monday.Cred )+ float.Parse(temp[0]);
                                    }
                                    else
                                    {
                                        dailyTrend.Monday.Debt = (dailyTrend.Monday.Debt == null ? 0 : dailyTrend.Monday.Debt) + float.Parse(temp[0]);
                                    }
                                    dailyTrend.Monday.Total = (dailyTrend.Monday.Total == null ? 0 : dailyTrend.Monday.Total) + float.Parse(temp[0]);
                                    break;
                            case DayOfWeek.Tuesday:
                                    if(CDType.Equals("Credited"))
                                    {
                                        dailyTrend.Tuesday.Cred = (dailyTrend.Tuesday.Cred == null ? 0 : dailyTrend.Tuesday.Cred) + float.Parse(temp[0]);
                                    }
                                    else
                                    {
                                        dailyTrend.Tuesday.Debt = (dailyTrend.Tuesday.Debt == null ? 0 : dailyTrend.Tuesday.Debt) + float.Parse(temp[0]);
                                    }
                                    dailyTrend.Tuesday.Total = (dailyTrend.Tuesday.Total == null ? 0 : dailyTrend.Tuesday.Total) + float.Parse(temp[0]);
                                    break;
                            case DayOfWeek.Wednesday:
                                    if(CDType.Equals("Credited"))
                                    {
                                        dailyTrend.Wednesday.Cred = (dailyTrend.Wednesday.Cred == null ? 0 : dailyTrend.Wednesday.Cred) + float.Parse(temp[0]);
                                    }
                                    else
                                    {
                                        dailyTrend.Wednesday.Debt = (dailyTrend.Wednesday.Debt == null ? 0 : dailyTrend.Wednesday.Debt) + float.Parse(temp[0]);
                                    }
                                    dailyTrend.Wednesday.Total = (dailyTrend.Wednesday.Total == null ? 0 : dailyTrend.Wednesday.Total) + float.Parse(temp[0]);
                                    break;
                            case DayOfWeek.Thursday:
                                    if(CDType.Equals("Credited"))
                                    {
                                        dailyTrend.Thursday.Cred = (dailyTrend.Thursday.Cred == null ? 0 : dailyTrend.Thursday.Cred) + float.Parse(temp[0]);
                                    }
                                    else
                                    {
                                        dailyTrend.Thursday.Debt = (dailyTrend.Thursday.Debt == null ? 0 : dailyTrend.Thursday.Debt) + float.Parse(temp[0]);
                                    }
                                    dailyTrend.Thursday.Total = (dailyTrend.Thursday.Total == null ? 0 : dailyTrend.Thursday.Total) + float.Parse(temp[0]);
                                    break;
                            case DayOfWeek.Friday:
                                    if(CDType.Equals("Credited"))
                                    {
                                        dailyTrend.Friday.Cred = (dailyTrend.Friday.Cred == null ? 0 : dailyTrend.Friday.Cred) + float.Parse(temp[0]);
                                    }
                                    else
                                    {
                                        dailyTrend.Friday.Debt = (dailyTrend.Friday.Debt == null ? 0 : dailyTrend.Friday.Debt) + float.Parse(temp[0]);
                                    }
                                    dailyTrend.Friday.Total = (dailyTrend.Friday.Total == null ? 0 : dailyTrend.Friday.Total) + float.Parse(temp[0]);
                                    break;
                            case DayOfWeek.Saturday:
                                    if(CDType.Equals("Credited"))
                                    {
                                        dailyTrend.Saturday.Cred = (dailyTrend.Saturday.Cred == null ? 0 : dailyTrend.Saturday.Cred) + float.Parse(temp[0]);
                                    }
                                    else
                                    {
                                        dailyTrend.Saturday.Debt = (dailyTrend.Saturday.Debt == null ? 0 : dailyTrend.Saturday.Debt) + float.Parse(temp[0]);
                                    }
                                    dailyTrend.Saturday.Total = (dailyTrend.Saturday.Total == null ? 0 : dailyTrend.Saturday.Total) + float.Parse(temp[0]);
                                    break;
                            case DayOfWeek.Sunday:
                                    if(CDType.Equals("Credited"))
                                    {
                                        dailyTrend.Sunday.Cred = (dailyTrend.Sunday.Cred == null ? 0 : dailyTrend.Sunday.Cred) + float.Parse(temp[0]);
                                    }
                                    else
                                    {
                                        dailyTrend.Sunday.Debt = (dailyTrend.Sunday.Debt == null ? 0 : dailyTrend.Sunday.Debt) + float.Parse(temp[0]);
                                    }
                                    dailyTrend.Sunday.Total = (dailyTrend.Sunday.Total == null ? 0 : dailyTrend.Sunday.Total) + float.Parse(temp[0]);
                                    break;
                                }
                    }
                }
                dailyTrend.success = true;
                return dailyTrend;
            }
            catch (Exception ex)
            {
                dailyTrend.error = ex.Message;
                dailyTrend.success = false;
                return dailyTrend;
            }

        }

        public async Task<TotalCredDebt> ShowDayToDay(MonthTransaction monthTransaction)
        {
            TotalCredDebt dayToday = new TotalCredDebt();
            dayToday.success = false;

            try
            {
                float CredCount = 0;
                float DebtCount = 0;
                
                if(monthTransaction.Month == null)
                {
                     DateTime currentDate = DateTime.Now;
        
                    string currentDay = currentDate.ToString("dd/MM/yy");

                    monthTransaction.Month = currentDay;
                }

                var data = _firebaseContext.ReadDataAsync("USERS/"+monthTransaction.XpCode+"/data").Result;

                string dbData = JsonConvert.SerializeObject(data);

                var arr = JObject.Parse(dbData);
                
                foreach(var dt in arr)
                {
                    string KeyValue = dt.Key;
                    string[] temp = KeyValue.Split("~");
                    string CDType = "Credited";
                    if(temp[1].Equals("D"))
                    {
                        CDType = "Debited";
                    }
                    KeyValue = dt.Value.ToString();
                    temp = KeyValue.Split("~");

                    string[] verifyCalendar = temp[2].Split();

                    if(verifyCalendar[0].Equals(monthTransaction.Month)) 
                    {
                        if(CDType.Equals("Credited"))
                        {
                            CredCount = CredCount + float.Parse(temp[0]);
                        }
                        else
                        {
                            DebtCount = DebtCount + float.Parse(temp[0]);
                        }
                    }
                }

                dayToday.Cred = CredCount;
                dayToday.Debt = DebtCount;
                dayToday.Total = CredCount + DebtCount;

                dayToday.success = true;
                return dayToday;

            }
            catch (Exception ex)
            {
                dayToday.error = ex.Message;
                dayToday.success = false;
                return dayToday;
            }
        }

        public async Task<Dictionary<string,string>> GetLatestXpCode()
        {
            Dictionary<string,string> res = new Dictionary<string,string>();
            res.Add("code", await _firebaseContext.GetLatestXPCode());
            return res;
        }
    }
}