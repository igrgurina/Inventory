﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using NHibernate Fluent Mapping template.
// Code is generated on: 4.2.2018. 16:43:01
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Collections;

namespace Inventory
{
    /// <summary>
    /// There are no comments for UserMap in the schema.
    /// </summary>
    public partial class UserMap : ClassMap<User>
    {
        /// <summary>
        /// There are no comments for UserMap constructor in the schema.
        /// </summary>
        public UserMap()
        {
              Schema(@"main");
              Table(@"Users");
              LazyLoad();
              Id(x => x.Id)
                .Column("UserId")
                .CustomType("Int32")
                .Access.Property()                
                .GeneratedBy.Identity();
              Map(x => x.FirstName)    
                .Column("FirstName")
                .CustomType("String")
                .Access.Property()
                .Generated.Never();
              Map(x => x.LastName)    
                .Column("LastName")
                .CustomType("String")
                .Access.Property()
                .Generated.Never();
              Map(x => x.DateHired)    
                .Column("DateHired")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never();
              Map(x => x.DateFired)    
                .Column("DateFired")
                .CustomType("DateTime")
                .Access.Property()
                .Generated.Never();
              Map(x => x.Active)    
                .Column("IsActive")
                .CustomType("Boolean")
                .Access.Property()
                .Generated.Never();
              HasManyToMany<Inventory>(x => x.Equipments)
                .Access.Property()
                .AsBag()
                .Cascade.SaveUpdate()
                .LazyLoad()
                .Generic()
                .Component(c => {
                        c.Map(x => x.DateFrom)    
                            .Column("DateFrom")
                            .CustomType("DateTime")
                            .Access.Property()
                            .Generated.Never();
                        c.Map(x => x.DateTo)    
                            .Column("DateTo")
                            .CustomType("DateTime")
                            .Access.Property()
                            .Generated.Never();
                        c.References<Equipment>(r => r.Equipments, "AssetId");
                        })
                .Table("Inventory")
                .FetchType.Join()
                .ChildKeyColumns.Add("AssetId", mapping => mapping.Name("AssetId")
                                                                     .Nullable())
                .ParentKeyColumns.Add("UserId", mapping => mapping.Name("UserId")
                                                                     .Nullable());
              ExtendMapping();
        }

        #region Partial Methods

        partial void ExtendMapping();

        #endregion
    }

}
