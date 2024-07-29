using Microsoft.AspNetCore.Mvc;
using API.DataAccess;
using API.BusinessLayer.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class XpenseController : ControllerBase
    {
        private readonly FirebaseContext _firebaseContext;
        private readonly ISignUp _signUp;
        private readonly ILogin _login;
        private readonly IAddTransaction _addTransaction;
        private readonly IShowTransaction _showTransaction;
        private readonly IDailyTrendService _dailyTrendService;
        private readonly IDayToDay _dayToDay;
        private readonly ITokenService _tokenService;

        public XpenseController(FirebaseContext firebaseContext, ISignUp signUp, ILogin login, IAddTransaction addTransaction, IShowTransaction showTransaction, IDailyTrendService dailyTrendService, IDayToDay dayToDay, ITokenService tokenService)
        {
            _firebaseContext = firebaseContext;
            _signUp = signUp;
            _login = login;
            _addTransaction = addTransaction;
            _showTransaction = showTransaction;
            _dailyTrendService = dailyTrendService;
            _dayToDay = dayToDay;
            _tokenService = tokenService;
        }

        // [HttpPost("write")]
        // public async Task<IActionResult> Write([FromBody] DataModel data)
        // {
        //     await _firebaseContext.WriteDataAsync("path/to/data", data);
        //     return Ok();
        // }
        [Authorize]
        [HttpGet("read")]
        public async Task<IActionResult> Read()
        {
            var data = await _firebaseContext.ReadDataAsync("USERS/xp-0017");
            return Ok(data);
        }

        [HttpPost("signup")]
        public async Task<ActionResult<ResultDTO>> SignUp(Users user)
        {
            var data = await _signUp.CreateUser(user);
            return Ok(data);
        }

        [HttpPost("login")]
        public async Task<ActionResult<CurrentUser>> Login(Users user)
        {
            var data = await _login.LoginUser(user);
            data.token = _tokenService.CreateToken(user);
            return Ok(data);
        }

        [Authorize]
        [HttpPost("addData")]
        public async Task<ActionResult<ResultDTO>> AddData(Transaction transaction)
        {
            var data = await _addTransaction.AddData(transaction);
            return Ok(data);
        }

        [Authorize]
        [HttpPost("showData")]
        public async Task<ActionResult<TotalCredDebt>> ShowData(MonthTransaction monthTransaction)
        {
            var data = await _showTransaction.ShowData(monthTransaction);
            return Ok(data);
        }

        [Authorize]
        [HttpPost("dailyTrend")]
        public async Task<ActionResult<DailyTrend>> ShowDailyTrend(MonthTransaction monthTransaction)
        {
            var data = await _dailyTrendService.ShowDailyTrend(monthTransaction);
            return Ok(data);
        }

        [Authorize]
        [HttpPost("dayToday")]
        public async Task<ActionResult<TotalCredDebt>> ShowDayToDay(MonthTransaction monthTransaction)
        {
            var data = await _dayToDay.ShowDayToDay(monthTransaction);
            return Ok(data);
        }

        [HttpGet("getLatestXP")]
        public async Task<ActionResult<Dictionary<string,string>>> GetLatestXpCode()
        {
            var data = await _showTransaction.GetLatestXpCode();
            return Ok(data);
        }

    }
}