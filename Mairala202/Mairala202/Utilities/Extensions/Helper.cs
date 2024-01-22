namespace Mairala202.Utilities.Extensions
{
    public static class Helper
    {
        public static bool CheckFileSize(this IFormFile file,int Mb)
        {
            if (file.Length*1024*1024>Mb)
            {
                return false;
            }
            return true;
        }
        public static bool CheckFileType(this IFormFile file,string type)
        {
            if (file.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }
        public static async Task<string> CreateFileAsync(this IFormFile file, string root,params string[] folders)
        {
            string path = root;
            string filename = Guid.NewGuid().ToString()+file.FileName;
            foreach (var item in folders)
            {
                path = Path.Combine(path, item);
            }
            path = Path.Combine(path, filename);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                 await file.CopyToAsync(fs);
            }
            return filename;

        }
        public static void DeleteFile(this string file, string root, params string[] folders)
        {
            string path = root;
            
            foreach (var item in folders)
            {
                path = Path.Combine(path, item);
            }
            path = Path.Combine(path, file);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

    }
}
