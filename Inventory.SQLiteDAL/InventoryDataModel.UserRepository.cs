﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using the template for generating Repositories and a Unit of Work for NHibernate model.
// Code is generated on: 4.2.2018. 16:43:01
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------
using System;
using System.Linq;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Linq;

namespace Inventory
{
    public partial class UserRepository : NHibernateRepository<User>, IUserRepository
    {
        public UserRepository(ISession session) : base(session)
        {
        }

        public virtual ICollection<User> GetAll()
        {
            return session.CreateQuery(string.Format("from User")).List<User>();
        }

        public virtual User GetByKey(int _Id)
        {
            return session.Get<User>(_Id);
        }
    }
}
