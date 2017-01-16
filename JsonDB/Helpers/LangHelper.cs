using JsonDB.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace JsonDB.Helpers
{
    public class LangHelper
    {
     

        public static List<LangData> GetLangStrings(string path)
        {
            List<LangData> alljson = new List<LangData>();
            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                alljson = JsonConvert.DeserializeObject<List<LangData>>(content);

            }
            else
            {
                File.Create(path).Close();
                File.WriteAllText(path, "[]");
                GetLangStrings(path);

            }
            return alljson;
        }

        public static bool CreateNewKey(string key,string value,string controllername)
        {
            if (CheckKey(key, controllername))
            {
                return false;
            }
           
            var parentfolder = HttpContext.Current.Server.MapPath("~/Lang/");
            System.IO.DirectoryInfo di = new DirectoryInfo(parentfolder);
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                var lang = dir.Name;
                var filenametowrite=""; 
                if(!File.Exists(HttpContext.Current.Server.MapPath("~/Lang/" + lang + "/" + controllername + ".json")))
                {
                    FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("~/Lang/" + lang + "/" + controllername + ".json"));
                    file.Create().Close();
                    filenametowrite = HttpContext.Current.Server.MapPath("~/Lang/" + lang + "/" + controllername + ".json");
                }
                else
                {
                    filenametowrite = HttpContext.Current.Server.MapPath("~/Lang/" + lang + "/" + controllername + ".json");
                }

                List<LangData> _data = GetLangStrings(filenametowrite);
                if (Utils.IsAny<LangData>(_data))
                {
                    _data.Add(new LangData()
                    {
                        Key = key,
                        Value = value,
                    });
                }
                else
                {
                    _data = new List<LangData>();
                    _data.Add(new LangData()
                    {
                        Key = key,
                        Value = value,
                    });
                }
                string json = JsonConvert.SerializeObject(_data.ToArray(), Formatting.Indented);
                FileInfo file2 = new FileInfo(filenametowrite);
                if (file2.Exists)
                {
                    System.IO.File.WriteAllText(filenametowrite, json);

                }
            }
            return true;
          
        }

        public static bool CheckKey(string key, string conrollername)
        {
            var parentfolder = HttpContext.Current.Server.MapPath("~/Lang/");
            System.IO.DirectoryInfo di = new DirectoryInfo(parentfolder);
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                var lang = dir.Name;
                JArray json;
                string filename = "";
                if(!File.Exists( HttpContext.Current.Server.MapPath("~/Lang/" + lang + "/" + conrollername + ".json"))){
                    FileInfo file2 = new FileInfo(HttpContext.Current.Server.MapPath("~/Lang/" + lang + "/" + conrollername + ".json"));
                    file2.Create().Close();
                    filename = HttpContext.Current.Server.MapPath("~/Lang/" + lang + "/" + conrollername + ".json");
                }
                else
                {
                  filename=  HttpContext.Current.Server.MapPath("~/Lang/" + lang + "/" + conrollername + ".json");
                }
                  string content = File.ReadAllText(filename);
                 json = (JArray)JsonConvert.DeserializeObject(content);
                
                if (json != null)
                {
                    var s = json.ToObject<List<LangData>>();

                    foreach (var item in s)
                    {
                        if (item.Key == key)
                        {
                            return true;
                        }
                    }
                }
                
            }
        
            return false;
        }

      
      
        public static void Delete(string Controllername, string key)
        {
            var parentfolder = HttpContext.Current.Server.MapPath("~/Lang/");
            System.IO.DirectoryInfo di = new DirectoryInfo(parentfolder);
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                var filename = dir.Name; ;
                var filenametowrite = HttpContext.Current.Server.MapPath("~/Lang/" + filename + "/" + Controllername + ".json");
                List<LangData> _data = GetLangStrings(filenametowrite);
                if (Utils.IsAny<LangData>(_data))
                {
                    foreach (var item in _data.ToList<LangData>())
                    {
                        if (item.Key == key)
                        {
                            _data.Remove(item);
                        }
                    }
                    string json = JsonConvert.SerializeObject(_data.ToArray(), Formatting.Indented);
                    FileInfo file2 = new FileInfo(filenametowrite);
                    if (file2.Exists)
                    {
                        System.IO.File.WriteAllText(filenametowrite, json);

                    }
                }
            }
        }
        public List<string> getAllLang(string path)
        {

            try
            {
                return Directory.GetDirectories(path, "*.*",SearchOption.TopDirectoryOnly).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<string>();
            }
           
        }

        public List<string> getAllController(string path)
        {
            try
            {
                return Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<string>();
            }
        }

        public static string getOneKeyText(string lang,string controller, string key)
        {
            
          
                string content = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Lang/" + lang + "/" + controller + ".json"));
             JArray json = (JArray)JsonConvert.DeserializeObject(content);
         
            var s = json.ToObject<List<LangData>>();
            foreach(var item in s)
            {
                if (item.Key == key)
                {
                    return item.Value;
                }
            }
           return "";
        }

        public static void Update(string langName, string controllerName, string key, string value)
        {
            var filenametowrite = HttpContext.Current.Server.MapPath("~/Lang/" + langName + "/" + controllerName + ".json");
            List<LangData> _data = GetLangStrings(filenametowrite);
            foreach(var item in _data)
            {
                if (item.Key == key)
                {
                    item.Key = key;
                    item.Value = value;
                }
            }
            string json = JsonConvert.SerializeObject(_data.ToArray(), Formatting.Indented);
            FileInfo file2 = new FileInfo(filenametowrite);
            if (file2.Exists)
            {
                System.IO.File.WriteAllText(filenametowrite, json);

            }

        }
    }
    public static class Utils
    {
        public static List<LangData> CurrentLoadFile;
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }

        public static string Value(string key)
        {
            if (Utils.IsAny<LangData>(CurrentLoadFile))
            {
                foreach (var item in CurrentLoadFile)
                {
                    if (item.Key == key)
                    {
                        return item.Value;
                    }
                }
            }
            return "";
        }
    }
}