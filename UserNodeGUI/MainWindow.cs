﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserNodeGUI
{
    public partial class MainWindow : Form
    {
        UserNodeCore.UserNodeCore core;
        ConnectDialog connectDlg;

        public MainWindow()
        {
            InitializeComponent();
            core = new UserNodeCore.UserNodeCore(new Communication.UserNode("prova"));
            //SearchPanel.Hide();
            //ResultTabControl.Hide();
            core.resultCallback += new UserNodeCore.UserNodeCore.ResultReceiveHandler(onNewResult);
        }

        /// <summary>
        /// Each new Result received (through callback in NodeCore), goes
        /// here, then is processed in an appropriate method, exploiting
        /// overloading to change behaviour depending on Result subclass.
        /// </summary>
        /// <param name="result"></param>
        public void onNewResult(Communication.Result result) {
            if (result.GetType() == typeof(Communication.LoginOkResult))
            {
                SetConnectedView(true);
                connectDlg.Dispose();
            }
        }
                
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connectDlg = new ConnectDialog(core);           
            connectDlg.SetConnectedViewCallback = new ConnectDialog.SetConnectedViewDelegate(this.SetConnectedView);
            connectDlg.ShowDialog();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            /*
             * 1. Build a new search trip request and send it.
             * 2. Create a new tab
             * 3. On receiving the result, populate the tab with trips.
             */
            string source = sourceTextBox.Text;
            string destination = destinationTextBox.Text;
            AddTabPage(source + " - " + destination);
        }

        private void SetConnectedView(bool connected)
        {
            if (connected)
            {
                connectedStatusLabel.Text = "Connected";
                disconnectToolStripMenuItem.Enabled = true;
                connectToolStripMenuItem.Enabled = false;
                newTripToolStripMenuItem.Enabled = true;
                SearchPanel.Show();
                ResultTabControl.Show();
            }
            else
            {
                connectedStatusLabel.Text = "Not connected";
                connectToolStripMenuItem.Enabled = true;
                disconnectToolStripMenuItem.Enabled = false;
                newTripToolStripMenuItem.Enabled = false;
                SearchPanel.Hide();
                ResultTabControl.Hide();
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core.Unjoin();
            SetConnectedView(false);
        }

        private void newTripToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTripDialog dlg = new NewTripDialog(core);
            dlg.ShowDialog();
        }

        private void AddTabPage(string label)
        {
            TabPage page = new TabPage(label);
            ResultTabControl.Controls.Add(page);
            ResultTabControl.SelectTab(page);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            //ResultTabControl.Controls.RemoveAt();
            TabPage p = ResultTabControl.SelectedTab;
            if (p != null)
                p.Dispose();
        }
    }
}