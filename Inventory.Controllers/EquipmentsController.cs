﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory;
using Inventory.Core;
using Inventory.Model;
using Inventory.SQLiteDAL;
using NHibernate;

namespace Inventory.Controllers
{
    public class EquipmentsController
    {
        private IUnitOfWorkFactory uowFactory;
        private IEquipmentRepository repository;
        private ICategoryRepository categoryRepository;
        public ISession context => NHibernateSessionProvider.GetSession(); // can we make this per form session?

        public EquipmentsController()
        {
            //var context = NHibernateSessionProvider.GetSession();
            var _session = context;
            this.uowFactory = new NHibernateUnitOfWorkFactory(context);
            this.repository = EquipmentRepository.GetInstance(_session);
            this.categoryRepository = CategoryRepository.GetInstance(_session);
        }

        public EquipmentsController(IUnitOfWorkFactory uowFactory, IEquipmentRepository repository, ICategoryRepository categoryRepository)
        {
            this.uowFactory = uowFactory;
            this.repository = repository;
            this.categoryRepository = categoryRepository;
        }

        public List<Equipment> Index()
        {
            var query = repository.GetAll();
            return query.ToList();
        }

        public void Create(Equipment entity)
        {
            using (IUnitOfWork uow = uowFactory.Create())
            {
                repository.Add(entity);
                uow.Save();
            }
        }

        internal void Index(IShowEquipmentListView inForm, MainFormController mainController)
        {
            inForm.Display(mainController, repository.GetAll().ToList());
        }

        public Equipment Details(int Id)
        {
            return repository.GetByKey(Id);
        }

        public void Edit(Equipment entity)
        {
            using (IUnitOfWork uow = uowFactory.Create())
            {
                Equipment original = repository.GetByKey(entity.Id);
                original.Id = entity.Id;
                original.Active = entity.Active;
                original.DateAcquired = entity.DateAcquired;
                original.DateDisposed = entity.DateDisposed;
                uow.Save();
            }
        }

        internal void Create(IAddNewEquipmentView inForm)
        {
            if (inForm.Display(categoryRepository.GetAll().ToList()))
            {
                try
                {
                    this.Create(EquipmentFactory.CreateEquipment(inForm.EquipmentName, inForm.EquipmentCategory, inForm.DateAcquired));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }

        public void Delete(int Id)
        {
            using (IUnitOfWork uow = uowFactory.Create())
            {
                repository.Remove(repository.GetByKey(Id));
                uow.Save();
            }
        }

        private static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(IQueryable<TSource> source, System.Linq.Expressions.Expression<Func<TSource, TKey>> keySelector, bool ascending)
        {

            return ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
        }
    }
}

