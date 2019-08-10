using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoteBunny.DAL.Xml.Repositories
{
    public class XmlRepository<T> : IRepository<T> where T : BaseEntity
    {
        private string _filename;
        private List<T> _items;

        public XmlRepository(string filename)
        {
            _filename = filename;
            _items = GetItems();
        }
        public List<T> GetItems()
        {
            if (System.IO.File.Exists(_filename))
            {
                return ObjectSerializer.ObjectSerializer<List<T>>.DeserializeXmlObject(_filename);
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

        public void First(Predicate<T> condition) => _items.FirstOrDefault(x => condition(x));

        public IQueryable<T> GetAll() => _items.AsQueryable();

        public void Save() => ObjectSerializer.ObjectSerializer<List<T>>.SerializeXmlObject(_items, _filename);

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
