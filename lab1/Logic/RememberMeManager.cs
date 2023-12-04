using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab1.Model;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;

namespace lab1.Logic
{
    public class RememberMeManager
    {
        readonly static string path = System.IO.Directory.GetCurrentDirectory() + "\\auth.rmm";

        public static void SaveLoginData(LoginData data)
        {
            Console.WriteLine(path);
            if (data.AutoLogin == false) data.Password = "";
            var json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
        }
        public static void DeleteAuthFile()
        {
            if(File.Exists(path))
                File.Delete(path);
        }
        public static LoginData ReadLoginData()
        {
            LoginData data = new LoginData();
            data.Login = data.Password = ""; 
            data.LastLogin = DateTime.Now;
            data.AutoLogin = false;
            if(File.Exists(path))
            return JsonSerializer.Deserialize<LoginData>(File.ReadAllText(path).Trim());
            else return data;
        }
    }
}
