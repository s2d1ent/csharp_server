# Web-Server C# .Net Core
Этот сервер представляет собой простую реализацию C# .Net Core Веб-сервер, созданный с целью получения нового опыта в разработке программного обеспечения

## Таблица 1 - Реализованные технологии сервера

Title   | Realization   | Version       | 
:---    |   :---        |   :---:       |  
Php     |Partially implemented          | 8.0.12 | 
Python  |Unrealized                     | 3.9.8 |
Online Office              | Unrealized  | -    |
Multithreaded processing   | Implemented |  -   |
Domain system   | Implemented |  -   |
Multiple site   | Implemented |  -   |
<br><br>

## Список используемых библиотек проекта
``` csharp
using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Newtonsoft.Json;
```
<br>

# Принцип работы(в кратце)
Данный сервер работает по принципу прослушивания TCP запроса по определенному IPv4 аддресу и порту.
В начале работы сервера инициализируется класс Global, который берет данные из global-config.json и производит основную настройку сервера инициализируя следующие классы:

- Interpreter - класс, обозначает интерпритаторы которые использует сервер на Windows OS

- Server - класс, обозначает программную часть сервера, которая ведет прослушивание с помощью TCPListener по паре IPv4:port(127.0.0.1:80)

- Client - класс, обозначает подключенного пользователя. Данный класс инициализируется в процессе прослушивания в классе Server

**Если global-config.json будет отсутствовать, то сервер выдаст ошибку и не запустится**
<br><br>
# Структура global-config.json
``` json
{
  "IPv4": "your_ip", 
  "Listen": 80, 
  "Server": {
    "Path": "www", 
    "Extensions": [ 
      ".php",
      ".html",
      ".htm",
      ".xhtml"
    ],
    "Framework_py": null 
  },
  "System": { 
    "OS": "Windows", 
    "Bit": "x64", 
    "Processor": "", 
    "RAM": ""
  },
  "Interpreters": {
    "phpx64": {
      "Error": false,
      "Error_message": null,
      "Version": "x64",
      "Name": "php",
      "Path": "includes/php/win64/php.exe",
      "Reponse": null
    },
    "phpx86": {
      "Error": false,
      "Error_message": null,
      "Version": "x86",
      "Name": "php",
      "Path": "includes/php/win86/php.exe",
      "Reponse": null
    },
    "pyx64": {
      "Error": false,
      "Error_message": null,
      "Version": "x64",
      "Name": "py",
      "Path": "includes/python/win64/python.exe",
      "Reponse": null
    }
  },
  "PoolMin_worker": "2",
  "PoolMin_async": "2",
  "PoolMax_worker": "4",
  "PoolMax_async": "4"
}
```
## Описание структуры
- IPv4 - адресс в формате IPv4, если он не установлен, то по стандарту идет 127.0.0.1
- Listen - порт по которому прослушивает сервер
- Server - сервер, программная часть
    - Path - директория с сайтами, относительно проекта
    - Extensions - форматы файлов, поддерживаемы сервером
    - Framework_py - фреймворк для Python, на котором работает сайт
- System - описывает систему
    - OS - ОС пользователя
    - Bit - разрядность системы
    - Processor - процессор пользователя
    - Ram - кол-во оперативной памяти пользователя
- Interpreters - Описывает интерпритаторы используемые сервером
    - phpx64, phpx86 и тд - названия интерпритаторов в списке Dictionary<string, Interpreter>
    - Version - разрядность интерпритатора
    - Name - полное или кратное название языка
    - Path - расположения исполняемого файла интерпритатора
- PoolMin_worker - минимальное кол-во работающих ядер, минимальное значение не должно быть меньше 2
- PoolMin_async - минимальное число параллельно работающих ядер, минимальное значение не должно быть меньше 2
- PoolMax_worker - максимальное число работающих ядер, устанавливается пользователем относительно используемого процессора
- PoolMax_async - максимальное число параллельно работающих ядер, устанавливается пользователем относительно используемого процессора
