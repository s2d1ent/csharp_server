# Web-Server C# .Net Core
Этот сервер представляет собой простую реализацию C# .Net Core Веб-сервер, созданный с целью получения нового опыта в разработке программного обеспечения.

Список субъектов:<br>
Сервер как класс программного обеспечения и программное обеспечение в целом - Server <br>
Клиент - Client <br>
Глобальное надстройка - Global<br>
Interpreter - Interpreter
<br><br>

## Таблица 1 - Реализованные технологии сервера

Title   | Realization   | Version       | 
:---    |   :---        |   :---:       |  
Php     |Partially implemented          | 8.0.12 | 
Python  |Unrealized                     | 3.9.8 |
Online Office              | Unrealized  | -    |
Multithreaded processing   | Implemented |  -   |
Domain system   | Unrealized |  -   |
Multiple site   | Unrealized |  -   |
<br><br>

## Список используемых библиотек проекта
``` csharp
using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
```
<br>

## Принцип работы
Данный сервер работает по принципу прослушивания TCP запроса по определенному IPv4 аддресу и порту.
В начале работы сервера инициализируется класс Global, который берет данные из global-config.json и производит основную надстройку сервера. Инициализируются такие аспекты сервера как:
- Interpreter - разные интерпритаторы, которые работают на разных разрядностях Windows OS

- Server - сервера с глобальной настройкой IPv4 в классе Global и выбранным портом(Listen) у сервера. Сервер настраивается на конфиге сервера в папке /servers/server_config.serve.json

- Client - инициализация данного класса происходит при получении запроса по сети, как только TCPListener получает запрос на подключение, происходит добавление запроса в ThreadPool и клиент ожидает своей очереди для обработки запроса

