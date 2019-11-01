using NHibernate;
using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using NoteBunny.DAL.NHibernate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBunny.DAL.NHibernate.Repositories
{
    public class NHibRepository<T> : IRepository<T> where T : BaseEntity
    {
        private List<T> _items;
        protected ISession _session = null;

        public NHibRepository()
        {
            _session = SessionFactory.OpenSession();
            _items = GetAll().ToList();
        }

        public void Add(T record)
        {
            _items.Add(record);
        }

        public bool Delete(T record)
        {
            return _items.Remove(record);
        }

        public bool Delete(Predicate<T> condition)
        {
            throw new NotImplementedException();
        }

        public T FindById(string id)
        {
            return _items.First(x => x.Id == id);
        }

        public T First(Predicate<T> condition)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll()
        {
            IQueryable<T> items;
            //var session = SessionFactory.OpenSession();
            _session.BeginTransaction();
            items = _session.Query<T>();
            return items.AsQueryable();


            using (var dsession = SessionFactory.OpenSession())
            {
                using (var transaction = _session.BeginTransaction())
                {
                    items = _session.Query<T>();
                    //_items = items.ToList();
                }
            }

            return items.AsQueryable();
        }

        public void Save()
        {
            using (var transaction = _session.BeginTransaction())
            {
                foreach (var item in _items)
                {
                    _session.SaveOrUpdate(item);
                }
                transaction.Commit();
            }
        }

        public void Update(T record)
        {
            throw new NotImplementedException();
        }
    }
}
