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