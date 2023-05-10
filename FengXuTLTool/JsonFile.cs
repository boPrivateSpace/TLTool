using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FengXuTLTool
{
    class JsonFile
    {

        public static void SetJsonFile(FileModel fileModel)
        {
            string strSrc = JsonConvert.SerializeObject(fileModel);//将json装换为string
            string path = Directory.GetCurrentDirectory() + "\\Config.json";

            File.WriteAllText(path, strSrc, System.Text.Encoding.UTF8);//将内容写进json文件
        }

        public static void SetLoginJsonFile(FileModel fileModel)
        {
            string strSrc = JsonConvert.SerializeObject(fileModel);//将json装换为string
            string path = "FengXuTLTool.libs.Login.json";
            File.WriteAllText(path, strSrc, System.Text.Encoding.UTF8);//将内容写进json文件
        }

        public static FileModel ReadJsonConfigFile()
        {
            string folder = Directory.GetCurrentDirectory(); //获取应用程序的当前工作目录。 
            string path = folder + "\\Config.json";
            var jsonObject = ReadJsonFile(path);
            if (jsonObject == null)
            {
                return null;
            }

            FileModel fileModel = new FileModel();
            fileModel.FileName = jsonObject["FileName"].ToString();
            fileModel.FilePath = jsonObject["FilePath"].ToString();
            fileModel.Ip = jsonObject["Ip"].ToString();
            fileModel.Pwd = jsonObject["Pwd"].ToString();
            fileModel.UserName = jsonObject["UserName"].ToString();
            fileModel.LoginCode = jsonObject["LoginCode"].ToString();

            return fileModel;
        }

        public static List<LoginCode> ReadJsonLoginFile()
        {
            string path = "FengXuTLTool.libs.Login.json";

            var keyValuePairs = ReadJsonFile(path);
            List<LoginCode> root = JsonConvert.DeserializeObject<List<LoginCode>>(keyValuePairs["data"].ToString());

            return root;
        }

        public static JObject ReadJsonFile(string path)
        {
            string folder = Directory.GetCurrentDirectory(); //获取应用程序的当前工作目录。 
            if (!File.Exists(path))
            {
                return null;
            }

            StreamReader file = File.OpenText(path);
            JsonTextReader reader = new JsonTextReader(file);
            return (JObject)JToken.ReadFrom(reader);
        }

    }
}
