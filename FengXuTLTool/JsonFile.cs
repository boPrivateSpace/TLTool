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







        public static FileModel ReadJsonFile()
        {
            string folder = Directory.GetCurrentDirectory(); //获取应用程序的当前工作目录。 
            string path = folder + "\\Config.json";
            FileModel fileModel = new FileModel();
            if (!File.Exists(path))
            {
                return null;
            }

            StreamReader file = File.OpenText(path);
            JsonTextReader reader = new JsonTextReader(file);
            JObject jsonObject = (JObject)JToken.ReadFrom(reader);
            if (jsonObject == null)
            {
                return null;
            }
            fileModel.FileName = jsonObject["FileName"].ToString();
            fileModel.FilePath = jsonObject["FilePath"].ToString();
            fileModel.Ip = jsonObject["Ip"].ToString();
            fileModel.Pwd = jsonObject["Pwd"].ToString();
            fileModel.UserName = jsonObject["UserName"].ToString();

            return fileModel;
        }

    }
}
