using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class TeamMemberController : Controller
    {
        private AppEntities db = new AppEntities();
        private ITeamMemberService _teamMemberService;
        Message _message = new Message();

        private readonly string subPath = @"~/Content/Upload/images";

        public TeamMemberController()
        {
            _teamMemberService = new TeamMemberService(db);
        }

        // GET: Designations/Index
        [AppAuthorization]
        public ActionResult Index(string currentFilter, string searchString, int? page = 1, int? NoOfRows = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.page = page;
            ViewBag.CurrentFilter = searchString;
            ViewBag.NoOfRows = NoOfRows;
            return View(_teamMemberService.GetPageList(page.Value, NoOfRows.Value, searchString));

        }
        //public ActionResult Index()
        //{
        //    var list = _designationService.GetAll();
        //    return View(list.ToList());
        //}

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamMember model = _teamMemberService.GetDetails(id ?? 0);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Departments/Create
        [AppAuthorization]
        public ActionResult Create()
        {

            return View(new TeamMember());
        }
        #region POST Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public ActionResult Create(TeamMember model)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var hasFile = Request.Files[0];
                    if (hasFile != null && hasFile.ContentLength > 0)
                    {
                        model.ImagePath = UploadImage(Request);
                    }
                }
                
                if (_teamMemberService.Add(model))
                {
                    //Saved Successfully;
                    _message.save(this);
                    return RedirectToAction("Index");
                }
            }
            //No Data Saved
            _message.warning(this, "Invalid Data!");
            return View(model);
        }
        #endregion

        // GET: Departments/Edit/5
        [AppAuthorization]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamMember model = _teamMemberService.GetDetails(id ?? 0);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public ActionResult Edit(TeamMember model)
        {
            if (ModelState.IsValid)
            {
                #region Delete previously uploaded image if there is any
                if (Request.Files.Count > 0)
                {
                    var hasFile = Request.Files[0];
                    if (hasFile != null && hasFile.ContentLength > 0)
                    {
                        bool exists = Directory.Exists(Server.MapPath(subPath));
                        if (exists)
                        {
                            if (System.IO.File.Exists(model.ImagePath))
                            {
                                System.IO.File.Delete(model.ImagePath);
                            }
                        }


                        model.ImagePath = UploadImage(Request);
                    }


                }
                #endregion

                if (_teamMemberService.Update(model))
                {
                    _message.update(this);
                    //Updaed Successfully;
                    return RedirectToAction("Index");
                }
            }
            //No Data Updated

            return View(model);
        }

        // GET: Departments/Delete/5
        [AppAuthorization]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (_teamMemberService.Delete(id ?? 0))
            {
                _message.delete(this);
                //Deleted Successfully;
            }
            else
            {
                _message.custom(this, "Can't delete item!");
                //No Data Deleted
            }
            return RedirectToAction("Index");
        }

        #region Image Upload
        private string UploadImage(HttpRequestBase httpRequest)
        {
            string filePath = "";
            if (httpRequest.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    string fileExtension = System.IO.Path.GetExtension(Request.Files["image"].FileName);
                    if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg")
                    {
                        var fileExt = Path.GetExtension(file.FileName);

                        bool exists = Directory.Exists(Server.MapPath(subPath));
                        if (!exists)
                            Directory.CreateDirectory(Server.MapPath(subPath));

                        var path = Path.Combine(Server.MapPath(subPath), Path.GetFileName(file.FileName) + fileExt);
                        file.SaveAs(path);
                        filePath = subPath + "/" + Path.GetFileName(file.FileName) + fileExt;
                    }
                }
            }
            return filePath;
        }
        #endregion
    }
}
