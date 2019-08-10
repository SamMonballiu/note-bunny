using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBunny.BLL.Repositories
{
    public class TagRepository
    {
        private IRepository<Tag> _tagRepo;
        public TagRepository(IRepository<Tag> tagRepo)
        {
            _tagRepo = tagRepo;
        }
        public void AddTagsFromString(string tagString)
        {
            if (String.IsNullOrWhiteSpace(tagString))
            {
                throw new InvalidOperationException("No valid tags supplied");
            }

            var existingTags = _tagRepo.GetAll();
            var newTagCandidates = tagString.Split(',').Select(t => t.Trim());

            foreach (var tag in newTagCandidates)
            {
                if (!existingTags.Any(x => x.Name.ToLower() == tag.ToLower()))
                {
                    var newTag = new Tag(tag);
                    _tagRepo.Add(newTag);
                }
            }
            _tagRepo.Save();
        }

        public List<Tag> GetTagsFromString(string tagString)
        {
            var results = new List<Tag>();

            var tagSearchTerms = tagString.Split(',');
            
            foreach (var tag in tagSearchTerms)
            {
                var foundTag = FindStringByName(tag);
                results.Add((foundTag == null) ? PersistNewTag(tag) : foundTag);
            }

            return results;
        }

        public Tag FindStringByName(string name)
        {
            return _tagRepo.First(x => x.Name.ToLower() == name.Trim().ToLower());
        }

        public Tag PersistNewTag(string name)
        {
            var newTag = new Tag(name.Trim());
            _tagRepo.Add(newTag);
            _tagRepo.Save();
            return newTag;
        }

        public List<Tag> GetTagsFromIds(List<string> ids)
        {
            return _tagRepo.GetAll()
                .Where(x => ids.Contains(x.Id))
                .ToList();
        }

        public List<string> GetTagIdsFromNames(List<string> names)
        {
            return _tagRepo.GetAll()
                .Where(x => names.Contains(x.Name))
                .Select(x => x.Id)
                .ToList();
        }

        public List<string> GetTagIdsFromNames(string names)
        {
            return _tagRepo.GetAll()
                .Where(x => names.Split(',').Select(p => p.Trim()).Contains(x.Name))
                .Select(x => x.Id)
                .ToList();
        }
    }
}
