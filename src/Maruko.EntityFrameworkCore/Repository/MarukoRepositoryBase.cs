﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Maruko.Domain.Entities.Auditing;
using Maruko.Domain.Repositories;
using Maruko.Domain.UnitOfWork;

namespace Maruko.EntityFrameworkCore.Repository
{
    /// <summary>
    /// 仓储的抽象实现
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    public abstract class MarukoRepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : FullAuditedEntity<TPrimaryKey>
    {
        public abstract IQueryable<TEntity> GetAll(bool isMaster = false);

        public IUnitOfWork UnitOfWork { get; set; }

        public virtual List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        public virtual TEntity FirstOrDefault(TPrimaryKey id)
        {
            return GetAll().FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().SingleOrDefault(predicate);
        }

        public abstract TEntity Insert(TEntity entity);

        public abstract TEntity Update(TEntity entity);

        public abstract TEntity UpdateColumn(TEntity entity, Func<TEntity, string[]> funcColums);



        public abstract void Delete(TPrimaryKey id);

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var source in GetAll().Where(predicate).ToList())
            {
                Delete(source);
            }
        }

        public abstract void Delete(TEntity entity);

        public virtual int Count()
        {
            return GetAll().Count();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).Count();
        }

        public virtual List<TEntity> GetAllList(Func<TEntity, bool> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }


        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}