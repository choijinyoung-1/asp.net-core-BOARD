using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net_NoteB.DataContext;
using asp.net_NoteB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp.net_NoteB.Controllers
{
    public class NoteController : Controller
    {
        /// <summary>
        /// 게시판 리스트
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                //로그인이 안된 상태
                return RedirectToAction("Login", "Account");
            }

            using (var db = new NoteDataContext())
            {
                var list = db.Notes.ToList();
                return View(list);
            }
        }
        /// <summary>
        /// 게시판 상세
        /// </summary>
        /// <param name="noteNo"></param>
        /// <returns></returns>
        public IActionResult Detail(int noteNo)
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                //로그인이 안된 상태
                return RedirectToAction("Login", "Account");
            }
            using (var db = new NoteDataContext())
            {
                var note = db.Notes.FirstOrDefault(n => n.NoteNo.Equals(noteNo));
                HttpContext.Session.SetInt32("NOTE_KEY", note.NoteNo);
                return View(note);
            }
        }

        /// <summary>
        /// 게시물 추가
        /// </summary>
        /// <returns></returns>
        public IActionResult Add()
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                //로그인이 안된 상태
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Add(Note model)
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                //로그인이 안된 상태
                return RedirectToAction("Login", "Account");
            }
            model.UserNo = int.Parse(HttpContext.Session.GetInt32("USER_LOGIN_KEY").ToString());
            if (ModelState.IsValid)
            {
                using (var db = new NoteDataContext())
                {
                    db.Notes.Add(model);

                    if (db.SaveChanges() > 0)
                    {
                        return RedirectToAction("Index", "Note");
                    }
                }
                ModelState.AddModelError(string.Empty, "게시물을 저장할 수 없습니다.");
            }
            return View(model);
        }
        /// <summary>
        /// 게시물 수정
        /// </summary>
        /// <returns></returns>
        public IActionResult Edit(Note model)
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                //로그인이 안된 상태
                return RedirectToAction("Login", "Account");
            }
            int noteNo = int.Parse(HttpContext.Session.GetInt32("NOTE_KEY").ToString());

            using (var db = new NoteDataContext())
            {
                var note = db.Notes.FirstOrDefault(n => n.NoteNo.Equals(noteNo));
                return View(note);
            }


        }
        [HttpPost]
        public IActionResult EditS(Note model)
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                //로그인이 안된 상태
                return RedirectToAction("Login", "Account");
            }

            model.UserNo = int.Parse(HttpContext.Session.GetInt32("USER_LOGIN_KEY").ToString());
            using (var db = new NoteDataContext())
            {
                var note = db.Notes.FirstOrDefault(n => n.NoteNo.Equals(model.NoteNo));
                if (model.UserNo == note.UserNo)
                {
                    db.Entry(model).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        return RedirectToAction("Index", "Note");
                    }
                }
                else
                {
                    return RedirectToAction("EditError", "Note");
                }
            }
            return View();
        }
        /// <summary>
        /// 게시물 삭제
        /// </summary>
        /// <returns></returns>

        public IActionResult Delete(Note model)
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                //로그인이 안된 상태
                return RedirectToAction("Login", "Account");
            }

            model.NoteNo = int.Parse(HttpContext.Session.GetInt32("NOTE_KEY").ToString());
            using (var db = new NoteDataContext())
            {
                //Note note = db.Notes.Find(model);
                db.Notes.Remove(model);

                if (db.SaveChanges() > 0)
                {
                    return RedirectToAction("Index", "Note");
                }
            }
            return View(model);
        }
    }
}
