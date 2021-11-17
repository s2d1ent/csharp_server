# Web-Server C# .Net Core
**Подготовил:** студент 3ИСП Тюменев Виктор<br>
**Код специальности:**  09.02.07<br>
**Факультет:**  Прикладная информатика<br>
**Год:** 2021 <br>
**Описание проекта:**<br>
Данный проект представляет собой простую реализацию Веб-сервера на платформе C#.Net Core, созданный с целью преобретения нового опыта в разработке ПО и изучения внутреннего, программного устройства сервера.

<br>

## Таблица 1 - Реализованные технологии сервера

Название   | Реализация   | Версия      | Разрядность системы|
:---       |   :---       |   :---:     |  :---:             |
Php        |Partially implemented       | 8.0.12             | x86/x64
Python     |Partially implemented       | 3.9.8              | x86/x64
MySql      |Unrealized                  | 5.7.36/8.0.27      | x86/x64
Django     |Unrealized                  | -                  | -
Onlly Office               | Unrealized  |  -   |-
Multithreaded processing   | Implemented |  -   |-
Domain system              | Implemented |  -   |-
Custom alias               | Implemented |  -   |-
Multiple site              | Implemented |  -   |-

<br>

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
  "IPv4": "127.0.0.1",
  "Listen": 80,
  "Server": {
    "Path": "www",
    "Extensions": [
      ".py",
      ".php",
      ".html",
      ".htm",
      ".xhtml"
    ],
    "Framework_py": null
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
    "phpx64": {
      "Version": "x64",
      "Name": "php",
      "Path": "includes/php/win64/php.exe"
    },
    "phpx86": {
      "Version": "x86",
      "Name": "php",
      "Path": "includes/php/win86/php.exe"
    },
    "pyx64": {
      "Version": "x64",
      "Name": "py",
      "Path": "includes/python/win64/python.exe"
    }
  },
  "PoolMin_worker": "2",
  "PoolMin_async": "2",
  "PoolMax_worker": "2",
  "PoolMax_async": "2"
}
```
<br>

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
- Alias - создает собственный алиас по паре key:value, где key название папки в которой у вас расположен сайт, а value название вашего аливаса
- Interpreters - Описывает интерпритаторы используемые сервером
    - phpx64, phpx86 и тд - названия интерпритаторов в списке Dictionary<string, Interpreter>     
    - Version - разрядность интерпритатора
    - Name - полное или кратное название языка
    - Path - расположения исполняемого файла интерпритатора
- PoolMin_worker - минимальное кол-во работающих ядер, минимальное значение не должно быть меньше 2
- PoolMin_async - минимальное число параллельно работающих ядер, минимальное значение не должно быть меньше 2
- PoolMax_worker - максимальное число работающих ядер, устанавливается пользователем относительно используемого процессора
- PoolMax_async - максимальное число параллельно работающих ядер, устанавливается пользователем относительно используемого процессора
<br><br>
# Класс Global
Название | Тип | Тип данных | Описание |
:---     |:--- |:--- | :---     |
Global()   |Конструктор | - | Инициализирует объект класса Global
IPv4 | Поле | string | Принимает IPv4 из global-config.json
Ipv4 | Класс | Object <br> IPAddress | Принимает значение строки IPv4 и парсит ее, при отсутствии ставит 127.0.0.1
Listen | Поле | int | Порт по которому прослушивается сайт
Server | Класс | Object <br> Server| Класс сервера, который занимается хостингом сайтов
System | Список |Object <br>Dictionary<string, string> | Принимает значения параметров системы
Alias | Список |Object <br>Dictionary<string, string> | Обозначает алиасы введенные пользователем
Interpreters | Список |Object <br>Dictionary<string, Interpreter> | Принимает значения и информацию об интерпритаторах которые использует проект
PoolMin_worker | Поле | int | Принимает значение минимального количества одновременно работающих потоков
PoolMin_async | Поле | int | Принимает значение минимального количества асинхронно работающих потоков
PoolMax_worker | Поле | int | Принимает значение минимального количества одновременно работающих потоков
PoolMax_async | Поле | int | Принимает значение минимального количества асинхронно работающих потоков
ThreadPoolMin_worker | Поле | int | Минимальное количество одновременно работающих потоков 
ThreadPoolMin_async| Поле | int | Максимальное количество асинхронно работающих потоков
ThreadPoolMax_worker| Поле | int | Максимальное количество одновременно работающих потоков
ThreadPoolMax_async| Поле | int | Максимальное количество асинхронно работающих потоков
GetServer() | Функция | void | Передает значения параметров из конфига в Server 
SerializeConfig() | Функция | void | Серилизует класс Global в global-config.json
GetSystem() | Функция | void | Получает информацию системы
GetInfo() | Функция | void | Выводи информацию о сервере
<br><br>
# Класс Server
Название | Тип | Тип данных | Описание |
:---     |:--- |:--- | :---     |
Server() | Конструктор | - | Инициализирует объект класса Server
Server(int port) | Конструктор | - | Инициализирует объект класса Server
Server(string ip, int port) | Конструктор | - | Инициализирует объект класса Server
Ipv4 | Класс | Object <br>IPAddress | Принимает значение строки IPv4 и парсит ее, при отсутствии ставит 127.0.0.1 
Listen | Поле | int | Порт по которому прослушивается сайт
Listener | Поле | Object <br>TCPListener | Объект класса TCPListener, который занимается прослушиванием по связке Ip:Listen(127.0.0.1:80)
Active | Поле | bool | Обозначает активен ли ты сейчас сервер true если работает
global | Класс | Object <br>Global | Обозначает объект класса Global, из которого могут браться параметры
Domains | Список | List<string> | Список перечисляющий сущечтвующие на сервере домены
Path | Поле | string | Показывает локальный путь до папки с сайтами
Extensions | Поле | Object <br>Array[string] | Обозначает допустимые расширения на сервере(py, php, html...)
Framework_py | Поле | string | Показывает путь до папки с использующимся фреймворком для сайтов на языке Python
registry | Поле | string | Хранит в себе значения доменов в файле hosts
Start() | Функция | void | Запускает сервер
StartAsync() | Функция | void | Асинхронная функция запуска сервера
Stop() | Функция | void | Выключает сервер
GetInfo() | Функция | string | Выводит информацию об объекте класса Server
GetStatus() | Функция | string | Выводит информацию о статусе сервера, выводит поле Active
OpenDocumentation() | Функция | void | -
ClientThread(object client) | Функция | void | Инициализирует класс Client в ThreedPool
GetDomains() | Функция | void | Получает домены из папки с путем Global.Path или Global.Alias и записывает в Domains
DomainsRegister() | Функция | void | Регистрирует домены в файле hosts
DomainsClear() | Функция | void | Очищает файл hosts по завершению работы сервера
<br><br>
# Класс Client
Название | Тип | Тип данных | Описание |
:---     |:--- |:--- | :---     |
Client(TcpClient c, Server s) | Конструктор | - | Выполняет основную работы обработки запроса клиента
~Client() | Дисктруктор | - | Очищает ОЗУ после завершения работы клиента
GetSheet(string link, string file) | Функция | void  | Возвращает ответ на запрос клиента
AnyFile(string address) | Функция | string | Запускает обработку файлов Python, Php и возвращает отввет в виде html разметки
GetContentType(string link) | Функция | string | Возвращает тип контента файла
GetFormat(string link) | Функция | string | Возвращает расширение файла
SendError(int code)| Функция | void | Возвращает ошибку
SendError(string message, int code) | Функция | void | Возвращает ошибку
<br><br>
# Класс Interpreter
Название | Тип | Тип данных | Описание |
:---     |:--- |:--- | :---     |
Version| Поле | string | Обозначает разрядность интерпритатора
Name| Поле | string | Обозначает имя интерпритатора
Path | Поле | string | Обозначает путь до донтерпритатора Пример: /includes/php/win64/php.exe
PerformPhp(string php, string file) | Функция | string | Возвращает результат работы интерпритатора

