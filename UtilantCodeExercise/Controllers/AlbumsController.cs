using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UtilantCodeExercise.Helpers;
using UtilantCodeExercise.Models;

namespace UtilantCodeExercise.Controllers
{
    public class AlbumsController : Controller
    {
        // GET: Albums
        public ActionResult Index(int? page, string titleFilter, string userNameFilter)
        {
            int pageSize = 12;
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            string json = DataRetriever.GetJsonData("albums");
            IQueryable<Album> albums = JsonConvert.DeserializeObject<List<Album>>(json).AsQueryable();

            ViewBag.TitleFilter = titleFilter;
            ViewBag.UserNameFilter = userNameFilter;

            if (!string.IsNullOrEmpty(titleFilter))
            {
                albums = albums.Where(a => a.title == titleFilter);
            }
            if (!string.IsNullOrEmpty(userNameFilter))
            {
                // Since we aren't loading the full user data for the album until we have limited the results to the data showing on the page until later, we have to pull the user id for the username
                json = DataRetriever.GetJsonData("users?name=" + userNameFilter);
                User user = JsonConvert.DeserializeObject<List<User>>(json).FirstOrDefault();

                if (user != null)
                {
                    albums = albums.Where(a => a.userId == user.id);
                }
                else
                {
                    albums = albums.Where(a => a.userId == -1);
                }
            }

            if (albums.Count() < (pageIndex * 12))
            {
                pageIndex = 1;
            }

            IPagedList<Album> pagedData = albums.ToPagedList(pageIndex, pageSize);

            foreach (Album a in pagedData)
            {
                a.GetAlbumData();
            }

            return View(pagedData);
        }

        public ActionResult GetAlbumDetails(int id)
        {
            string json = DataRetriever.GetJsonData("albums?id=" + id);
            Album album = JsonConvert.DeserializeObject<List<Album>>(json).FirstOrDefault();
            if (album != null)
            {
                album.GetAlbumData();
            }

            return PartialView("~/Views/Albums/DisplayTemplates/AlbumDetails.cshtml", album);
        }

        public ActionResult GetUserDetails(int userId)
        {
            string json = DataRetriever.GetJsonData("users?id=" + userId);
            User user = JsonConvert.DeserializeObject<List<User>>(json).FirstOrDefault();
            if (user != null)
            {
                user.GetPostData();
            }

            return PartialView("~/Views/Albums/DisplayTemplates/UserDetails.cshtml", user);
        }
    }
}