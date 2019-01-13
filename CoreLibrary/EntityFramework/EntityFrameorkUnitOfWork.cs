#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreLibrary.DataContext;
using CoreLibrary.Exception;
using CoreLibrary.Repositories;
using CoreLibrary.UnitOfWork;
using Microsoft.EntityFrameworkCore;

#endregion

namespace CoreLibrary.EntityFramework
{
    public class EntityFrameorkUnitOfWork : IUnitOfWorkAsync
    {
        #region Private Fields

        private DbContext _context;
        protected readonly IServiceProvider ServiceProvider;
        private DbTransaction _transaction;

        #endregion Private Fields

        #region Constuctor/Dispose

        public EntityFrameorkUnitOfWork(IDataContextAsync dataContext, IServiceProvider serviceProvider)
        {
            _context = (DbContext)dataContext;
            ServiceProvider = serviceProvider;
        }


        public int SaveChanges()
        {
            CheckDisposed();
            return _context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            var saveChangesAsync = SaveChangesAsync();
            _transaction.Commit();
            return await saveChangesAsync;
        }

        public Task<int> SaveChangesAsync()
        {
            CheckDisposed();
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            CheckDisposed();
            return _context.SaveChangesAsync(cancellationToken);
        }

        public IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class
        {
            CheckDisposed();
            var repositoryType = typeof(IRepositoryAsync<TEntity>);
            var repository = (IRepositoryAsync<TEntity>)ServiceProvider.GetService(repositoryType);
            if (repository == null)
            {
                throw new RepositoryNotFoundException(repositoryType.Name,
                    $"Repository {repositoryType.Name} not found in the IOC container. Check if it is registered during startup.");
            }

            ((IRepositoryInjection) repository)?.SetContext(_context);
            return repository;
        }

        public IRepository<TEntity> GetRepository<TEntity> () where TEntity : class
        {
            CheckDisposed();
            var repositoryType = typeof(IRepository<TEntity>);
            var repository = (IRepository<TEntity>)ServiceProvider.GetService(repositoryType);
            if (repository == null)
            {
                throw new RepositoryNotFoundException(repositoryType.Name,
                    $"Repository {repositoryType.Name} not found in the IOC container. Check if it is registered during startup.");
            }

            ((IRepositoryInjection) repository).SetContext(_context);
            return repository;
        }

        protected bool _isDisposed;

        protected void CheckDisposed()
        {
            if (_isDisposed) throw new ObjectDisposedException("The UnitOfWork is already disposed and cannot be used anymore.");
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                        {
                            _context.Database.CurrentTransaction?.Dispose();
                            _context.Database.GetDbConnection().Close();
                        }
                        else
                        {
                            _context.Database.CurrentTransaction?.Dispose();
                            _context.Dispose();
                            _context = null;
                        }
                    }
                }
            }
            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~EntityFrameorkUnitOfWork()
        {
            Dispose(false);
        }


        #endregion Constuctor/Dispose



        #region Unit of Work Transactions

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            if (_context.Database.GetDbConnection().State != ConnectionState.Open)
            {
                _context.Database.GetDbConnection().Open();
            }
            //_context.Database.BeginTransaction();
            _transaction = _context.Database.GetDbConnection().BeginTransaction();
        }

        int IUnitOfWorkAsync.Commit()
        {
            var saveChanges = SaveChanges();
            _transaction.Commit();
            return saveChanges;
        }

        public void Commit()
        {
            // _context.Database.CommitTransaction(); 
            _transaction.Commit(); 
        }

        public void Rollback()
        {
            //_context.Database.RollbackTransaction();
            _transaction.Rollback();
        }

        public void DisposeTransaction()
        {

        }

        #endregion

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters) where TElement : class
            => _context.Set<TElement>().FromSql(sql, parameters).AsEnumerable();

        public IEnumerable<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : class , new()
            => _context.Set<TEntity>().FromSql(commandText, parameters).AsEnumerable();
    }
}