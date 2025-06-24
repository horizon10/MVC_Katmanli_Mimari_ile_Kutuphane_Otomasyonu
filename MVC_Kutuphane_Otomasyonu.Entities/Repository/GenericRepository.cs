using MVC_Kutuphane_Otomasyonu.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Kutuphane_Otomasyonu.Entities.Repository
{
    public class GenericRepository<TContext, TEntity> : IGenericRepository<TContext, TEntity>
        where TContext : DbContext, new()
        where TEntity : class, new()
    {
        public void Delete(TContext context, Expression<Func<TEntity,bool>>filter)
        {
            var model = context.Set<TEntity>().FirstOrDefault(filter);
            context.Set<TEntity>().Remove(model);
        }

        public List<TEntity> GetAll(TContext context, Expression<Func<TEntity, bool>> filter = null, params string[] tbl)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();

            // Include işlemleri
            foreach (var item in tbl)
            {
                query = query.Include(item);
            }

            // Filter varsa en son uygula
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.ToList();
        }


        public TEntity GetByFilter(TContext context, Expression<Func<TEntity, bool>> filter,params string[] tbl)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();
            foreach (var item in tbl)
            {
                query = query.Include(item);
            }
            return query.FirstOrDefault(filter);
            //return tbl==null ? context.Set<TEntity>().FirstOrDefault(filter) : context.Set<TEntity>().Include(tbl).FirstOrDefault(filter);
        }

        public TEntity GetById(TContext context, int? id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public void InsertorUpdate(TContext context, TEntity entity)
        {
            context.Set<TEntity>().AddOrUpdate(entity);
        }

        public void Save(TContext context)
        {
            context.SaveChanges();
        }

        
    }
}
