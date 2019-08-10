using System;
using System.Collections.Generic;
using System.Linq;
using NoteBunny.BLL.Models;

namespace NoteBunny.BLL.Interfaces
{
    public interface ITagRepository
    {
        void Add(Tag record);
        void AddTagsFromString(string tagString);
        bool Delete(Predicate<Tag> condition);
        bool Delete(Tag record);
        Tag FindById(string id);
        Tag FindStringByName(string name);
        Tag First(Predicate<Tag> condition);
        IQueryable<Tag> GetAll();
        List<string> GetTagIdsFromNames(List<string> names);
        List<string> GetTagIdsFromNames(string names);
        List<Tag> GetTagsFromIds(List<string> ids);
        List<Tag> GetTagsFromString(string tagString);
        Tag PersistNewTag(string name);
        void Save();
        void Update(Tag record);
    }
}