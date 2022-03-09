# Web-Server C# .Net Core
**Подготовил:** студент 3ИСП Тюменев Виктор<br>
**Код специальности:**  09.02.07<br>
**Специальность:**  СПО «Информационные системы и программирование»<br>
**Год:** 2021 <br>
**Описание проекта:**<br>
Данный проект представляет собой простую реализацию Веб-сервера на платформе C#.Net Core, созданный с целью преобретения нового опыта в разработке ПО и изучения внутреннего устройства Веб-сервера сервера.

<br>

## Таблица 1 - Реализованные технологии сервера

Название   | Реализация   | Версия      | Разрядность системы|
:---       |   :---       |   :---:     |  :---:             |
**Php**        |Implemented                 | 8.0.12   | x86
**Php-cgi**    |Implemented                 | 8.0.12   | x86
**Python**     |Partially implemented       | 3.10.0   | x86
**MySql**      |Implemented                 | 5.7.36   | x86
**Multithreaded processing**  | Implemented |  -       |-
**Domain system**             | Implemented |  -       |-
**Custom alias**              | Implemented |  -       |-
**Multiple site**             | Implemented |  -       |-
**Extern modules**            | Implemented |  -       |-

<br>

## Документация
- [Описание пользовательских типов](\doc\Classes.md) 
- [MySql](\doc\MySql.md)
- [Внешние модули](\doc\Modules.md)
- [API](\doc\Api.md) 
- [Дополнительные материалы](\doc\Additionally.md)

<br><br>
# Структура global-config.json
**Если global-config.json будет отсутствовать, то сервер выдаст ошибку и не запустится**
<br><br>
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
  "MaxWorker": 4,
  "MaxWorkerAsync": 4
}
```
<br>

## Описание конфига
- IPv4 - адресс в формате IPv4, если он не установлен, то по стандарту идет 127.0.0.1
- Listen - порт по которому прослушивает сервер
- ListenUse - обозначает, будет в файл C:\Windows\System32\drivers\etc\hosts записываться порт *(если значение true, то запись будет выглядеть: 127.0.0.1:80 ;если false, то: 127.0.0.1)*
- MultipleSite - обозначает режим работы сервера с сайтами. Если стоит значение true, то в папке /www/ может находиться несколько сайтов *(/www/site, /www/site2/, ...)*, если false, то все файлы сайта должны находиться строго в /www/
- ModuleEnabled - обозначает можно ли использовать внешние модули. Если стоит значение true, то сервер будет обрабатывать функции модулей.
- Modules - массив, сожержит относительные/абсолютные ссылки на модули приложения. Будут работать только функции ***Begin()*** 
- Server - сервер, программная часть
    - Extensions - форматы файлов, поддерживаемы сервером
    - Module - ассив, сожержит относительные/абсолютные ссылки на модули сервера.
- System - описывает систему
    - OS - ОС пользователя
    - Bit - разрядность системы
- Alias - создает собственный алиас по паре key:value, где key название папки в которой у вас расположен сайт, а value название вашего аливаса. Работает только когда ***MultipleSite = true***
- Interpreters - Описывает интерпритаторы используемые сервером
    - php, python - названия интерпритаторов в списке ***Dictionary<string, Interpreter>***     
    - Version - разрядность интерпритатора
    - Name - полное или кратное название языка
    - Path - расположения исполняемого файла интерпритатора
- MySqlPath - пути до служб mysql
- MinWorker - минимальное кол-во работающих ядер, минимальное значение не должно быть меньше 2
- MinWorkerAsync - минимальное число параллельно работающих ядер, минимальное значение не должно быть меньше 2
- MaxWorker - максимальное число работающих ядер, устанавливается пользователем относительно используемого процессора
- MaxWorkerAsync - максимальное число параллельно работающих ядер, устанавливается пользователем относительно используемого процессора
<br><br>


