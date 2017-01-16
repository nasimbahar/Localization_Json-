using JsonDB.Helpers;
using JsonDB.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JsonDB.Controllers
{
    public class AllLangController : Controller
    {
        string path = "";
        LangHelper langheper;
        public AllLangController()
        {
            langheper = new LangHelper();

        }

        public ActionResult HowtoUse()
        {
            return View();
        }
        public ActionResult Index()
        {
          
            this.path = Server.MapPath("~/Lang");
            if (!Directory.Exists(this.path))
            {
                Directory.CreateDirectory(this.path);
            }

            var allLang = langheper.getAllLang(this.path);
            List<string> allpath = new List<string>();
            foreach (var onelang in allLang)
            {
                allpath.Add(onelang.Replace(Server.MapPath("~/Lang/"), ""));
            }

            ViewBag.allpath = allpath;
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(AllLang obj)
        {
            if (ModelState.IsValid)
            {
                var foldername = obj.LangName;
                this.path = Server.MapPath("~/Lang/" + foldername);
                if (!Directory.Exists(this.path))
                {
                    Directory.CreateDirectory(this.path);
                    ViewBag.message = "New Language create successfully";
                }
                else
                {
                    ViewBag.message = "This Language Is Already Exists";
                }
            }
            return View();

        }

        public ActionResult Edit(string id)
        {
            AllLang obj = new AllLang { LangName = id, OldName = id };

            return View(obj);
        }
        [HttpPost]
        public ActionResult Edit(AllLang obj)
        {
            if (ModelState.IsValid)
            {
                var foldername = obj.OldName;
                var newfoldername = obj.LangName;
                var oldfile = Server.MapPath("~/Lang/" + foldername);
                var newfolder = Server.MapPath("~/Lang/" + newfoldername);
                if (Directory.Exists(oldfile))
                {
                    Directory.Move(oldfile, newfolder);
                }


                if (Directory.Exists(oldfile))
                {
                    Directory.Delete(oldfile);
                }
                ViewBag.message = "The Language Name is rename successfully";
            }
            return View();

        }

        public ActionResult Delete(string id)
        {
            AllLang obj = new AllLang { LangName = id, OldName = id };

            return View(obj);
        }

        [HttpPost]
        public ActionResult Delete(AllLang obj)
        {
            if (ModelState.IsValid)
            {

                var newfoldername = obj.LangName;

                var newfolder = Server.MapPath("~/Lang/");



                if (Directory.Exists(newfolder))
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(newfolder);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        if (dir.Name == obj.OldName)
                        {
                            dir.Delete(true);
                        }
                    }
                }
                ViewBag.message = "The Language is deleted successfully";
            }
            return View();

        }

        public ActionResult Details(string id)
        {
            ViewBag.lang = id;
            this.path = Server.MapPath("~/Lang/" + id);
            if (!Directory.Exists(this.path))
            {
                Directory.CreateDirectory(this.path);
            }

            var allLang = langheper.getAllController(this.path);
            List<string> allpath = new List<string>();
            foreach (var onelang in allLang)
            {
                string split = onelang.Replace(Server.MapPath("~/Lang/" + id + "/"), "");
                allpath.Add(split.Replace(".json", ""));
            }

            ViewBag.Controller = allpath;
            return View();

        }
        public ActionResult CreatenewController(string lang)
        {
            var obj = new Controllerss { LangName = lang };
            ViewBag.lang = lang;
            return View(obj);
        }
        [HttpPost]
        public ActionResult CreatenewController(Controllerss obj)
        {
            ViewBag.lang = obj.LangName;
            if (ModelState.IsValid)
            {

                var parentfolder = Server.MapPath("~/Lang/");
                System.IO.DirectoryInfo di = new DirectoryInfo(parentfolder);


                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    var filename = dir.Name; ;
                    this.path = Server.MapPath("~/Lang/" + filename + "/" + obj.ControllergName + ".json");
                    FileInfo file2 = new FileInfo(this.path);
                    if (!file2.Exists)
                    {
                        file2.Create().Close();

                    }
                }
                ViewBag.message = "New Language file create successfully";
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditController(string id,string lang)
        {
            Controllerss obj = new Controllerss { ControllergName = id, OldName = id ,LangName=lang};
            ViewBag.lang = lang;

            return View(obj);


        }
        [HttpPost]
        public ActionResult EditController(Controllerss obj)
        {
            ViewBag.lang = obj.LangName;
            if (ModelState.IsValid)
            {
              
                var parentfolder = Server.MapPath("~/Lang/");
                System.IO.DirectoryInfo di = new DirectoryInfo(parentfolder);


                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    var filename = dir.Name;
                    var oldfilpath = Server.MapPath("~/Lang/" + filename + "/" + obj.OldName + ".json");
                    var newpath = Server.MapPath("~/Lang/" + filename + "/" + obj.ControllergName + ".json");
                    FileInfo file2 = new FileInfo(oldfilpath);
                    if (file2.Exists)
                    {
                        System.IO.File.Move(oldfilpath, newpath);

                    }
                }
                ViewBag.message = "Language file Reneme successfully";

            } 
            return View();
        }

        public ActionResult DeleteLangfile(string id,string lang)
        {
            ViewBag.lang = lang;
            Controllerss obj = new Controllerss {ControllergName = id, OldName = id ,LangName=lang};

            return View(obj);
        }
        [HttpPost]
        public ActionResult DeleteLangfile(Controllerss obj)
        {
            ViewBag.lang = obj.LangName;
            if (ModelState.IsValid)
            {

                var parentfolder = Server.MapPath("~/Lang/");
                System.IO.DirectoryInfo di = new DirectoryInfo(parentfolder);


                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    var filename = dir.Name;
                    var filpath = Server.MapPath("~/Lang/" + filename + "/" + obj.ControllergName + ".json");
                   
                    FileInfo file2 = new FileInfo(filpath);
                    if (file2.Exists)
                    {
                        System.IO.File.Delete(filpath);

                    }
                }
                ViewBag.message = "The Language file is deleted successfully";
            }
            return View();
        }

     

       public ActionResult DetailsLangfile(string id,string lang)
        {
            ViewBag.ControllerName = id;
            ViewBag.lang = lang;
            this.path = Server.MapPath("~/Lang/" + lang + "/" + id + ".json");
            List<LangData> list = LangHelper.GetLangStrings(this.path);
            if (!Utils.IsAny<LangData>(list))
            {
                list = new List<LangData>();
                list.Add(new LangData { Key = "No Date", Value = " No Date is Inserted" });
            }
            return View(list);
        }
       
        public ActionResult CreateNewKey(string controllername,string lang)
        {
            Lang obj = new Lang { ControllerName = controllername,LangName=lang };
            ViewBag.Controllername = controllername;
            ViewBag.lang = lang;
            return View(obj);
        }
        [HttpPost]
        public ActionResult CreateNewKey(Lang obj)
        {
           
            ViewBag.Controllername = obj.ControllerName;
            ViewBag.lang = obj.LangName;
            if (ModelState.IsValid)
            {
               if( LangHelper.CreateNewKey(obj.Key, obj.Value, obj.ControllerName))
                {
                    ViewBag.message = "New Key Created successfully";
                }
                else
                {
                    ViewBag.message = "This key is already exists ";
                }
            }
            return View(obj);
        }

        public ActionResult EditLangText(string lang,string controllername,string key) {
            ViewBag.Controllername = controllername;
            ViewBag.lang = lang;
            var value = LangHelper.getOneKeyText(lang, controllername, key);
            Lang obj = new Lang { LangName = lang, Key = key, ControllerName = controllername,Value=value };
            return View(obj);
        }
        [HttpPost]
        public ActionResult EditLangText(Lang obj)
        {
            ViewBag.Controllername = obj.ControllerName;
            ViewBag.lang = obj.LangName;
            if (ModelState.IsValid)
            {
                LangHelper.Update(obj.LangName, obj.ControllerName, obj.Key, obj.Value);
                ViewBag.message = "Text Updated Successfully";

            }
            return View(obj);

        }

        public ActionResult DeleteLangText(string lang,string controllername,string key)
        {
            ViewBag.Controllername = controllername;
            ViewBag.lang = lang;
            var value = LangHelper.getOneKeyText(lang, controllername, key);
            Lang obj = new Lang { ControllerName = controllername, Key = key,Value=value,LangName=lang };
            return View(obj);
        }
        [HttpPost]
        public ActionResult DeleteLangText(Lang obj)
        {
            ViewBag.Controllername = obj.ControllerName;
            ViewBag.lang = obj.LangName;
            LangHelper.Delete(obj.ControllerName, obj.Key);
                ViewBag.message = "This Key is Deleted successfully";
           
            return View(obj);
        }

        
    }
}