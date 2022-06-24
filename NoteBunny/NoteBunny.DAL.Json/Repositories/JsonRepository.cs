using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NoteBunny.DAL.Json.Repositories
{
    public class JsonRepository<T> : IRepository<T> where T: BaseEntity
    {
        private string _filename;
        private List<T> _items;

        public JsonRepository(string filename)
        {
            _filename = filename;
            _items = GetItems();
        }

        private List<T> GetItems()
        {
            try
            {
                if (System.IO.File.Exists(_filename))
                {
                    return JsonConvert.DeserializeObject<List<T>>(System.IO.File.ReadAllText(_filename));
                }
            }
            catch (Exception)
            {
                return new List<T>();
            }

            return new List<T>();
        }
        public void Add(T record) => _items.Add(record);
        public bool Delete(T record)
        {
            if (_items.Contains(record))
            {
                _items.Remove(record);
                return true;
            }
            return false;
        }

        public bool Delete(Predicate<T> condition)
        {
            var count = _items.Count;
            _items.RemoveAll(x => condition(x));
            return count == _items.Count;
        }

        public T FindById(string id) => _items.FirstOrDefault(x => x.Id == id);

        public T First(Predicate<T> condition) => _items.FirstOrDefault(x => condition(x));

        public IQueryable<T> GetAll()
        {
            _items = GetItems();
            return _items.AsQueryable();
        }

        public void Save()
        {
            using (StreamWriter file = File.CreateText(_filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
                serializer.Serialize(file, _items);
            }
        }

        public void Update(T record)
        {
            var existingItem = FindById(record.Id);
            try
            {
                var index = _items.IndexOf(existingItem);
                _items[index] = record;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
