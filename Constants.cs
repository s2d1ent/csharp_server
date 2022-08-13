namespace AMES
{
    internal class Constants
    {
        public const string NAME = "AMES";
        public const string FULLNAME = "Application Modular Exntensible Server";
        public const string VERSION = "1.0.54";
        public const string DISTRIBUTIVE = "https://github.com/s2d1ent/csharp_server.git";
        public const string LICENSE = "GNU/PGL";
        public const string ALLOW_HTTP_OPTIONS = "GET, POST, DELETE, OPTIONS, TRACE, PUT";  
        public static string PATH_PHP = "";
        public static string PATH_PYTHON = "";
        public static string ROOT = "";
        public static bool PHPFASTCGI = false;
        public static string[] EXTENSIONS = new string[10];
        public static string PATH = "";
        // {
        //     get
        //     {
        //         return _path;
        //     }
        //     set
        //     {
        //         if(_path == null)
        //         {
        //             _path = value;
        //         }
        //         else
        //         {
        //             AMESException exception = new();
        //             throw exception;
        //         }
        //     }
        // }
        public static string PATH_WWW 
        {
            get
            {
                return _path_www;
            }
            set
            {
                if(_path_www == null)
                {
                    _path_www = value;
                }
                else
                {
                    AMESException exception = new();
                    throw exception;
                }
            }
        }
        public static string OS 
        {
            get
            {
                return _os;
            }
            set
            {
                if(_os == null)
                {
                    _os = value;
                }
                else
                {
                    AMESException exception = new();
                    throw exception;
                }
            }
        }

        private static string _path = null;
        private static string _path_www = null;
        private static string _os = null;

    }
}