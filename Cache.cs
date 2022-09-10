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

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace AMES
{
    // Must do smart cache with auto update data when file in directory was update
    internal class Cache
    {
        private Dictionary<string, int> _counter;
        public Dictionary<string, byte[]> Data;
        public Cache()
        {
            _counter = new Dictionary<string, int>();
            Data = new Dictionary<string, byte[]>();
        }

        private void RemoveCounter(string name)
        {
            _counter.Remove(name);
        }

        private void AddCounter(string name)
        {
            if(_counter.ContainsKey(name))
            {
                _counter[name]++;
            }
            else
            {
                _counter.Add(name, 0);
            }
        }

        public void Add(string name)
        {
            byte[] page = new byte[0];
            try
            {
                if(_counter.ContainsKey(name))
                {
                    if(_counter[name] >= 10 && !Data.ContainsKey(name))
                    {
                        page = File.ReadAllBytes(name);
                        Data.Add(name, page);
                    }
                    else
                    {
                        _counter[name]++;
                    }
                }
                else
                {
                    AddCounter(name);
                }
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                return;
            }
        }

        public void Update()
        {
            foreach(KeyValuePair<string, byte[]> elem in Data)
            {
                byte[] newValue = File.ReadAllBytes(elem.Key);
                if(newValue != elem.Value)
                {
                    Data[elem.Key] = newValue;
                    Console.WriteLine(newValue);
                    Console.WriteLine(elem.Value);
                }
            }
        }

        public byte[] GetPage(string name)
        {
            try
            {
                return Data[name];
            }
            catch
            {
                return new byte[0];
            }
        }
    }
}
