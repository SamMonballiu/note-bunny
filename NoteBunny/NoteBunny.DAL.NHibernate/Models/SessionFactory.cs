using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NoteBunny.BLL.Models;
using NoteBunny.DAL.NHibernate.Mappings;
using System;

namespace NoteBunny.DAL.NHibernate.Models
{
    public class SessionFactory
    {
        private static volatile ISessionFactory _sessionFactory;
        private static object syncRoot = new object();

        public static ISessionFactory Factory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    InitializeSessionFactory();
                }

                return _sessionFactory;
            }
        }

        private static void InitializeSessionFactory()
        {


            _sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(@"Server=DESKTOP-7LUT7NB\SQLEXPRESS;Initial Catalog=NoteBunnyDb;Integrated Security=true")
                    .ShowSql()
                    )
                .Mappings(m => m.FluentMappings
                    .AddFromAssemblyOf<NoteMapping>()
                    .AddFromAssemblyOf<TagMapping>()
                    )
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg))
                .BuildSessionFactory();
        }

        public static ISession OpenSession() => Factory.OpenSession();
    }
}
