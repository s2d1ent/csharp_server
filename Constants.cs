//     AMES(Application Modular Extensible Server) This is a simple web server which is a tutorial
//     Copyright (C) 2022 Viktor Tyumenev
//
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
//
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <https://www.gnu.org/licenses/>.
//
//      Email: tumenev33@mail.ru
//      Email: vornfrost@gmail.com

namespace AMES
{
    internal class Constants
    {
        public const string NAME = "AMES";
        public const string FULLNAME = "Application Modular Exntensible Server";
        public const string VERSION = "0.0.60";
        public const string DISTRIBUTIVE = "https://github.com/s2d1ent/csharp_server.git";
        public const string LICENSE = "GNU/GPL v3";
        public const string ALLOW_HTTP_OPTIONS = "GET, POST, DELETE, OPTIONS, TRACE, PUT";  
        public const string DEFAULT_WWW_PATH = "/var/www/";
        public static string PATH_PHP = "";
        public static string PATH_PYTHON = "";
        public static string ROOT = "";
        public static bool PHPFASTCGI = false;
        public static string[] EXTENSIONS = new string[10];
        public static string PATH = "";
        public static OperationsSystem OS = OperationsSystem.NONE;

    }
}