﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace Inventory
{
    public class UserRepository : NHibernateRepository<User>, IUserRepository
    {
        private static UserRepository _instance;

        private UserRepository(ISession session) : base(session)
        {
        }

        public virtual ICollection<User> GetAll()
        {
            return session.CreateQuery("from User").List<User>();
        }

        public ICollection<User> GetAllActive()
        {
            return session.Query<User>().Where(u => u.Active).ToList();
        }

        public virtual User GetByKey(int _Id)
        {
            return session.Get<User>(_Id);
        }

        public static UserRepository GetInstance(ISession session)
        {
            return _instance ?? (_instance = new UserRepository(session));
        }

        public virtual ICollection<Equipment> GetAllCurrentEquipment(int _userId)
        {
            var _user = session.Get<User>(_userId);

            var _eq = _user.Equipments.AsQueryable()
                .Where(c => c.DateTo == null)
                .Fetch(c => c.Equipments).Select(c => c.Equipments);

            return _eq.ToList();
        }

        public virtual ICollection<Equipment> GetAllEquipmentFromTo(int _userId, DateTime _from, DateTime _to)
        {
            var _user = session.Get<User>(_userId);

            var _eq = _user.Equipments.AsQueryable()
                .Where(c => c.DateTo <= _to && c.DateFrom > _from)
                .Fetch(c => c.Equipments).Select(c => c.Equipments);

            return _eq.ToList();
        }
    }
}