using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JsonDB.Models
{
    public class Lang
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }

        public string ControllerName { get; set; }
      
        public string LangName { get; set; }

        public string oldkey { get; set; }
    }

    public class LangData
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }

    public class Controllerss
    {
        public string ControllergName { get; set; }
        public string OldName { get; set; }

        public string LangName { get; set; }
    }
    public class AllLang
    {

        public string LangName { get; set; }
        public string OldName { get; set; }
    }
}