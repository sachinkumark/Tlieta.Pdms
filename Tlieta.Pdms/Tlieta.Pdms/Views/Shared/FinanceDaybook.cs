﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tlieta.Pdms.DB;
using Telerik.WinControls.UI;
using Tlieta.Pdms.Code;
using Telerik.WinControls.Themes;

namespace Tlieta.Pdms.Views.Shared
{
    public partial class FinanceDaybook : UserControl
    {
        public FinanceDaybook()
        {
            InitializeComponent();
            
            try
            {
                radBillDate.Value = DateTime.Now;
                FromDate.Value = DateTime.Now;
                ToDate.Value = DateTime.Now;

                GridViewSummaryRowItem countAmount = new GridViewSummaryRowItem();
                countAmount.Add(new GridViewSummaryItem()
                {
                    Aggregate = GridAggregateFunction.Sum,
                    FormatString = "Total Debit = {0}",
                    //FieldName = "Debit",
                    Name = "Debit"
                });
                countAmount.Add(new GridViewSummaryItem()
                {                    
                    Aggregate = GridAggregateFunction.Sum,
                    FormatString = "Total Credit = {0}",
                    //FieldName = "Credit",
                    Name = "Credit"
                });


                this.FinanceGrid.SummaryRowsBottom.Add(countAmount);

                FillGrid();
                this.FinanceGrid.Columns["BillingDate"].FormatString = "{0:dd/MMM/yyyy}";
            }
            catch (Exception x)
            {
                FileLogger.LogError(x);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillGrid();
            }
            catch { }
        }

        private void FillGrid()
        {
            DateTime from = GetDateFromControl(FromDate);
            DateTime to = GetDateFromControl(ToDate);

            List<DayBook> billing = new DayBookData().GetDayBook(from, to);

            FinanceGrid.DataSource = billing;
            FinanceGrid.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            FinanceGrid.ClearSelection();

            FinanceGrid.Columns["DayBookId"].IsVisible = false;
            FinanceGrid.Columns["UpdatedOn"].IsVisible = false;
        }

        private DateTime GetDateFromControl(Telerik.WinControls.UI.RadDateTimePicker date)
        {
            try
            {
                return (DateTime)(date.DateTimePickerElement.Value);
            }
            catch { return DateTime.Now; }
        }

        private void chkCredit_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (chkCredit.Checked)
            {
                chkDebit.Checked = false;
            }
        }

        private void chkDebit_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (chkDebit.Checked)
            {
                chkCredit.Checked = false;
            }
        }

        private void FinanceGrid_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                txtBillingId.Text = e.Row.Cells["DayBookId"].Value.ToString();
                radBillDate.Value = Convert.ToDateTime(e.Row.Cells["BillingDate"].Value.ToString());
                int credit = Convert.ToInt32(e.Row.Cells["Credit"].Value.ToString());
                int debit = Convert.ToInt32(e.Row.Cells["Debit"].Value.ToString());
                if (credit == 0)
                {
                    chkDebit.Checked = true;
                    txtAmount.Value = debit;
                }
                else
                {
                    chkCredit.Checked = true;
                    txtAmount.Value = credit;
                }
                txtBillNumber.Text = e.Row.Cells["BillingNumber"].Value.ToString();
                txtNotes.Text = e.Row.Cells["Notes"].Value.ToString();
            }
            catch { }
        }

        private void RefreshTexts()
        {
            txtBillingId.Text = "";
            txtNotes.Text = "";
            txtBillNumber.Text = "";
            txtAmount.Value = 0;
            chkCredit.Checked = false; chkDebit.Checked = false;
            radBillDate.Value = DateTime.Now;
        }

        private void btnAddBilling_Click(object sender, EventArgs e)
        {
            DayBook billing = GetFormData();
            if (billing == null)
            {
                return;
            }
            bool result = new DayBookData().AddDayBook(billing);

            if (result)
            {
                MessageBox.Show("Added Successfully");
                RefreshTexts();
                FillGrid();
            }
            else
            {
                MessageBox.Show("Billing could not be added. Please contact admin.");
                return;
            }
        }

        private void btnUpdateBilling_Click(object sender, EventArgs e)
        {
            string id = txtBillingId.Text.Trim();
            if (id == "")
            {
                MessageBox.Show("Select(double click) a billing to update");
                return;
            }

            DayBook billing = GetFormData();
            if (billing == null)
            {
                return;
            }
            billing.DayBookId = Convert.ToInt32(id);
            bool result = new DayBookData().UpdateDayBook(billing);

            if (result)
            {
                MessageBox.Show("Updated Successfully");
                RefreshTexts();
                FillGrid();
            }
            else
            {
                MessageBox.Show("Billing could not be updated. Please contact admin.");
                return;
            }
        }

        private void btnDeleteBilling_Click(object sender, EventArgs e)
        {
            string id = txtBillingId.Text.Trim();
            if (id == "")
            {
                MessageBox.Show("Select(double click) a billing to update");
                return;
            }

            bool result = new DayBookData().DeleteDayBook(
                                            Convert.ToInt32(id));
            if (result)
            {
                MessageBox.Show("Deleted Successfully");
                FillGrid();
                RefreshTexts();
            }
            else
            {
                MessageBox.Show("Billing could not be deleted. Please contact admin.");
                return;
            }
        }

        private DayBook GetFormData()
        {
            DateTime billingdate = DateTime.Now;
            try
            {
                billingdate = (DateTime)(radBillDate.DateTimePickerElement.Value);
            }
            catch
            {
                MessageBox.Show("Enter valid billing date");
                return null;
            }

            bool IsCredit = chkCredit.Checked;
            bool IsDebit = chkDebit.Checked;
            if (!IsCredit && !IsDebit)
            {
                MessageBox.Show("Select credit/debit");
                return null;
            }
            int debit = 0; int credit = 0; int amount = 0;
            try
            {
                amount = Convert.ToInt32(txtAmount.Value);
            }
            catch
            {
                MessageBox.Show("Enter valid amount (rounded off)");
                return null;
            }
            if (IsCredit)
            {
                credit = amount;
            }
            else
            {
                debit = amount;
            }

            return (new DayBook()
            {
                BillingDate = billingdate,
                BillingNumber = txtBillNumber.Text,
                Credit = credit,
                Debit = debit,
                UpdatedOn = DateTime.Now,
                Notes = txtNotes.Text
            });
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.FinanceGrid.Print(true, this.radPrintDocument1);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            RefreshTexts();
        }
    }
}
