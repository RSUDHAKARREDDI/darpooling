﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClientGUI
{
    class SearchPage : TabPage
    {
        public SearchPage() : base("Ric")
        {
        }
    }

    public partial class SearchTabControl : UserControl
    {
        public SearchTabControl()
        {
            InitializeComponent();
        }
    }
}