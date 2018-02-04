﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inventory.Core;

namespace Inventory.Forms
{
    public partial class frmAddUser : Form, IAddNewUserView
    {
        public frmAddUser()
        {
            InitializeComponent();
        }

        private void frmAddUser_Load(object sender, EventArgs e)
        {

        }

        public bool Display()
        {
            return this.ShowDialog() == DialogResult.OK;
        }

        public string FirstName => txtFirstName.Text;
        public string LastName => txtLastName.Text;
        public DateTime DateHired => dateHired.Value;
    }
}