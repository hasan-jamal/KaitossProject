﻿using Kaitoss.Web.Data;
using Kaitoss.Web.DataAccess.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Kaitoss.Web.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly KaitossProjectContext _db;
        internal DbSet<T> dbSet;
        public Repository(KaitossProjectContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        // IncludeProp - "Category, CoverType"
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter =null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach(var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp); 
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDeafult(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }
            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
           dbSet.RemoveRange(entity);
        }
    }
}
