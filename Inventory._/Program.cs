﻿using System;
using System.Windows.Forms;
using Inventory.Controllers;
using Inventory.Forms;
using Inventory.SQLiteDAL;

namespace Inventory
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var _formsFactory = new WindowFormsFactory();

            var context = NHibernateSessionProvider.GetSession();
            var uowFactory = new NHibernateUnitOfWorkFactory(context);

            var mainCtrl = new MainFormController(_formsFactory, uowFactory, UserRepository.GetInstance(context),
                EquipmentRepository.GetInstance(context), CategoryRepository.GetInstance(context));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMainWindow(mainCtrl));
        }
    }
}