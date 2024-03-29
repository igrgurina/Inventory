﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Inventory.Core;
using Inventory.Model;
using Inventory.SQLiteDAL;
using NHibernate;

namespace Inventory.Controllers
{
    public class EquipmentsController
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IEquipmentRepository repository;
        private readonly IUnitOfWorkFactory uowFactory;

        public EquipmentsController()
        {
            //var context = NHibernateSessionProvider.GetSession();
            var _session = context;
            uowFactory = new NHibernateUnitOfWorkFactory(context);
            repository = EquipmentRepository.GetInstance(_session);
            categoryRepository = CategoryRepository.GetInstance(_session);
        }

        public EquipmentsController(IUnitOfWorkFactory uowFactory, IEquipmentRepository repository,
            ICategoryRepository categoryRepository)
        {
            this.uowFactory = uowFactory;
            this.repository = repository;
            this.categoryRepository = categoryRepository;
        }

        public ISession context => NHibernateSessionProvider.GetSession(); // can we make this per form session?

        private List<Equipment> Index()
        {
            return repository.GetAll().ToList();
        }

        private void Create(Equipment entity)
        {
            using (var uow = uowFactory.Create())
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
            using (var uow = uowFactory.Create())
            {
                var original = repository.GetByKey(entity.Id);
                original.Id = entity.Id;
                original.Active = entity.Active;
                original.DateAcquired = entity.DateAcquired;
                original.DateDisposed = entity.DateDisposed;
                uow.Save();
            }
        }

        public void Create(IAddNewEquipmentView inForm, IUserRepository userRepository)
        {
            if (inForm.Display(categoryRepository.GetAll().ToList()))
                try
                {
                    using (var uow = uowFactory.Create())
                    {
                        userRepository = UserRepository.GetInstance(context);
                        // make sure that administrator user gets created and "assigned"
                        var user = userRepository.GetAll()
                            .FirstOrDefault(u => u.LastName == Constants.ADMINISTRATOR_LASTNAME);

                        if (user == null)
                        {
                            user = UserFactory.CreateDefaultUser(Constants.ADMINISTRATOR_LASTNAME);

                            userRepository.Add(user);
                            //uow.Save();
                        }

                        var entity = EquipmentFactory.CreateEquipment(inForm.EquipmentName, inForm.EquipmentCategory,
                            inForm.DateAcquired);
                        var inventory = InventoryFactory.CreateInventory(entity, user, entity.DateAcquired);
                        entity.Users.Add(inventory);

                        repository.Add(entity);
                        uow.Save();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
        }

        public void Delete(int Id)
        {
            using (var uow = uowFactory.Create())
            {
                repository.Remove(repository.GetByKey(Id));
                uow.Save();
            }
        }

        private static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector, bool ascending)
        {
            return ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
        }

        public void Dispose(IDisposeEquipmentView frm)
        {
            if (frm.Display(Index()))
            {
                var entity = frm.SelectedEquipment;
                entity.Active = false; // dispose
                entity.DateDisposed = frm.DateDisposed;

                Edit(entity);
            }
        }
    }
}