# Web-Server C# .Net Core
**Подготовил:** студент 3ИСП Тюменев Виктор<br>
**Код специальности:**  09.02.07<br>
**Специальность:**  СПО «Информационные системы и программирование»<br>
**Кафедра:**  Прикладная информатика<br>
**Год:** 2021 <br>
**Описание проекта:**<br>
Данный проект представляет собой простую реализацию Веб-сервера на платформе C#.Net Core, созданный с целью преобретения нового опыта в разработке ПО и изучения внутреннего устройства Веб-сервера сервера.

<br>

## Таблица 1 - Реализованные технологии сервера

Название   | Реализация   | Версия      | Разрядность системы|
:---       |   :---       |   :---:     |  :---:             |
Php        |Implemented                 | 8.0.12   | x86
Php-cgi    |Implemented                 | 8.0.12   | x86
Python     |Partially implemented       | 3.10.0   | x86
MySql      |Implemented                 | 5.7.36   | x86
Multithreaded processing  | Implemented |  -       |-
Domain system             | Implemented |  -       |-
Custom alias              | Implemented |  -       |-
Multiple site             | Implemented |  -       |-
Extern modules            | Implemented |  -       |-

<br>

## Дополнительная информация
**MySql =>**
**user:** root@127.0.0.1
**password:**<br>
[Описание пользовательских типов данных]("\tree\main\Classes.md") 
<br><br>
## Список используемых библиотек проекта
``` csharp
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;
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
  "Ipv4": "127.0.0.1",
  "Listen": 80,
  "ListenUse": false,
  "MultipleSite": true,
  "ModuleEnabled": true,
  "Server": {
    "Extensions": [
      ".py",
      ".php",
      ".html",
      ".htm",
      ".xhtml"
    ],
    "Modules":{
      "Dlls":[
        "/modules/csharp_server_dll.dll",
        "/modules/dlls.dll"
      ]
    }
  },
  "System": {
    "OS": "Microsoft Windows NT 10.0.18363.0",
    "Bit": "x64",
    "Processor": "",
    "RAM": ""
  },
  "Alias": {
    "python": "kidalovo"
  },
  "Interpreters": {
    "php": {
      "Version": "x86",
      "Name": "php",
      "Path": "includes/php/php.exe",
      "Type": "int"
    },
    "phpcgi": {
      "Version": "x86",
      "Name": "php",
      "Path": "includes/php/php-cgi.exe",
      "Type": "cgi"
    },
    "python": {
      "Version": "x86",
      "Name": "py",
      "Path": "includes/python/win86/python.exe",
      "Type": "int"
    }
  },
  "MySqlPath": [
    "includes/mysql/bin/mysqld.exe",
    "includes/mysql/bin/mysqladmin.exe",
    "includes/mysql/bin/mysql.exe"
  ],
  "MinWorker": 2,
  "MinWorkerAsync": 2,
  "ManWorker": 4,
  "ManWorkerAsync": 4
}
```
<br>

## Описание структуры
- IPv4 - адресс в формате IPv4, если он не установлен, то по стандарту идет 127.0.0.1
- Listen - порт по которому прослушивает сервер
- Server - сервер, программная часть
    - Path - директория с сайтами, относительно проекта
    - Extensions - форматы файлов, поддерживаемы сервером
    - Django - указывает, используется ли фреймворк Django в вашем проекте true означает что используется
- System - описывает систему
    - OS - ОС пользователя
    - Bit - разрядность системы
    - Processor - процессор пользователя
    - Ram - кол-во оперативной памяти пользователя
- Alias - создает собственный алиас по паре key:value, где key название папки в которой у вас расположен сайт, а value название вашего аливаса
- Interpreters - Описывает интерпритаторы используемые сервером
    - php, python - названия интерпритаторов в списке Dictionary<string, Interpreter>     
    - Version - разрядность интерпритатора
    - Name - полное или кратное название языка
    - Path - расположения исполняемого файла интерпритатора
- MySql_path - пути до служб mysql
- PoolMin_worker - минимальное кол-во работающих ядер, минимальное значение не должно быть меньше 2
- PoolMin_async - минимальное число параллельно работающих ядер, минимальное значение не должно быть меньше 2
- PoolMax_worker - максимальное число работающих ядер, устанавливается пользователем относительно используемого процессора
- PoolMax_async - максимальное число параллельно работающих ядер, устанавливается пользователем относительно используемого процессора
<br><br>


