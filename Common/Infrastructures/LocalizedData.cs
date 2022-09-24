using Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Infrastructures {
  public static class EntityConventions {
    public static readonly JsonSerializerSettings Settings;

    static EntityConventions() {
      Settings = new JsonSerializerSettings {
        NullValueHandling = NullValueHandling.Ignore
      };
    }
  }

  public class LocalizedData:Dictionary<string,string> {

    public LocalizedData() {
    //  foreach (var lang in LanguagesModel.GetLanguages())
    //    this.Add(lang,"");
    }    
    public LocalizedData(params string[] prams) {
      var languages = LanguagesModel.GetLanguages();
      for (int i = 0; i < languages.Count; i++) {
        this.Add(languages[i], prams[i]);
      }
    }

    public LocalizedData(string jsonObj) {
      var obj= JsonConvert.DeserializeObject<Dictionary<string,string>>(jsonObj);

      LanguagesModel.GetLanguages().ToList().ForEach(k => {
        try {
          if (obj[k] != null)
            this.Add(k, obj[k]);
        } catch (Exception) {
          this.Add(k, "");
        }

      });
      //obj.Keys.ToList().ForEach(k => {
      //  this.Add(k, obj[k]);
      //});
    }
    public override string ToString() {
      try { return JsonConvert.SerializeObject(this); } catch (Exception) { return "{'en':'','ar':''}"; }

    }

       
    }

  public static class LanguagesModel {
    static List<string> Languages = new List<string>() {"en","ar"};

    public static List<string> GetLanguages() {
      return Languages;
    }
    public static void AddLanguage(string language) {
      Languages.Add(language);
    }
    public static void AddLanguages(string[] languages) {
      Languages.AddRange(languages);
    }

  } 

}
