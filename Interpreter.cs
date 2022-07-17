using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings;
using System.Threading.Tasks;

namespace AMES
{
    internal class Interpreter
    {
        public byte[] GetFile(string path)
        {
            return File.ReadAllBytes(path);
        }
 
        public static HttpCodes Delete(string path)
        {
            HttpCodes result = HttpCodes.NONE;
            bool has = false;
            bool file = false;

            try
            {
                if(!File.Exists(path))
                {
                    result = HttpCodes.NotFound;
                }
                else
                {
                    has = true;
                    file = true;
                }

                if(!Directory.Exists(path) && !has)
                {
                    result = HttpCodes.NotFound;
                }
                else
                {
                    has = true;
                }

                if(!has)
                {
                    return result;
                }
                else
                {
                    if(file)
                    {
                        File.Delete(path);
                    }
                    else
                    {
                        Directory.Delete(path, true);
                    }
                    result = HttpCodes.NoContent; 
                }
            }
            catch
            {
                return HttpCodes.InternalServer;                
            }

            return result;
        }
    }
}
