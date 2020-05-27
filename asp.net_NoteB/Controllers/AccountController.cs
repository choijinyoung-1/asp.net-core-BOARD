using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net_NoteB.DataContext;
using asp.net_NoteB.Models;
using asp.net_NoteB.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace asp.net_NoteB.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 로그인
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            // ID,비밀번호 -필수
            if (ModelState.IsValid)
            {
                using (var db = new NoteDataContext())
                {
                    //Linq - 메서드 체이닝
                    // => : A Go to B
                    var user = db.Users.FirstOrDefault(u => u.UserId.Equals(model.UserId) && u.UserPassword.Equals(model.UserPassword));
                    if (user == null)
                    {
                        //로그인에 실패했을때
                        ModelState.AddModelError(string.Empty, "사용자 ID 혹은 비밀번호가 올바르지 않습니다.");
                    }
                    else
                    {
                        //로그인에 성공했을 때
                        //HttpContext.Session.SetInt32(key, value);
                        HttpContext.Session.SetInt32("USER_LOGIN_KEY", user.UserNo);
                        return RedirectToAction("Loginsuccess", "Home");    //로그인 성공 페이지로 이동
                    }
                }
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("USER_LOGIN_KEY");
            return RedirectToAction("Index", "Home"); 
        }
        /// <summary>
        /// 회원가입
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// 회원가입  전송
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new NoteDataContext())
                {
                    db.Users.Add(model);    // 메모리까지 올림
                    db.SaveChanges();       // Commit
                }
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
