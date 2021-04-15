using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Wallets.DataStorage
{
    public class FileDataStorage<TObject> where TObject: class, IStorable
    {
        private static readonly string BaseFolder = Path.Combine(Environment.GetFolderPath
            (Environment.SpecialFolder.ApplicationData),"BudgetsStorage", typeof(TObject).Name);

        public FileDataStorage()
        {
            if (!Directory.Exists(BaseFolder))
            {
                Directory.CreateDirectory(BaseFolder);
            }
        }


        public async Task AddOrUpdateAsync(TObject obj)
        {
            string stringObject = JsonConvert.SerializeObject(obj);

            string filePath = Path.Combine(BaseFolder, obj.Guid.ToString(format:"N"));

            using (StreamWriter sw = new StreamWriter(filePath, false))//false to overwrite the file every time
            {
                await sw.WriteAsync(stringObject);
            } 

        }

        public void Delete(TObject obj)
        {
            string filePath = Path.Combine(BaseFolder, obj.Guid.ToString(format: "N"));

                File.Delete(filePath);
        }



        public async Task<TObject> GetAsync(Guid guid)
        {
            string stringObject = null;

            string filePath = Path.Combine(BaseFolder, guid.ToString(format:"N"));
            if (!File.Exists(filePath))
            {
                return null;
            }

            using (StreamReader sr = new StreamReader(filePath))
            {
                stringObject = await sr.ReadToEndAsync();
            }

            
            return JsonConvert.DeserializeObject<TObject>(stringObject);
        }


        public async Task<List<TObject>> GetAllAsync()
        {
            var res = new List<TObject>();


            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                string stringObject = null;


                using (StreamReader sr = new StreamReader(file))
                {
                    stringObject = await sr.ReadToEndAsync();
                }

                res.Add(JsonConvert.DeserializeObject<TObject>(stringObject));
            }

            return res;
        }



    }
}
