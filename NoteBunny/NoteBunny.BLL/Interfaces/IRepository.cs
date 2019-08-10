﻿using NoteBunny.BLL.Models;
using System;
using System.Linq;

namespace NoteBunny.BLL.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        void Add(T record);
        void Update(T record);
        void Delete(T record);
        void Delete(Predicate<T> condition);
        void First(Predicate<T> condition);
        T FindById(string id);
        void Save();
    }
}
