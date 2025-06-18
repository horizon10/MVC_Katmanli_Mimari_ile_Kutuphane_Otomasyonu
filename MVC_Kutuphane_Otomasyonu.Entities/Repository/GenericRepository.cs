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

        public List<TEntity> GetAll(TContext context, Expression<Func<TEntity, bool>> filter = null, string tbl = null)
        {
            return filter == null ? tbl == null ? context.Set<TEntity>().ToList() : context.Set<TEntity>().Include(tbl).ToList()
               :tbl==null ?  context.Set<TEntity>().Where(filter).ToList(): context.Set<TEntity>().Include(tbl).Where(filter).ToList();
        }

        public TEntity GetByFilter(TContext context, Expression<Func<TEntity, bool>> filter,string tbl=null)
        {
            return tbl==null ? context.Set<TEntity>().FirstOrDefault(filter) : context.Set<TEntity>().Include(tbl).FirstOrDefault(filter);
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
