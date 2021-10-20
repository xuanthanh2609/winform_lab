﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab04_Bai4.Models;

namespace Lab04_Bai4
{

    public partial class frmQldonhang : Form
    {
        dbGiaoHang dbGiaoHang = new dbGiaoHang();
        public frmQldonhang()
        {
            InitializeComponent();
        }

        private void frmQldonhang_Load(object sender, EventArgs e)
        {
            dateTimeStart.Value = new DateTime(2019, 10, 01);
            dateTimeEnd.Value = new DateTime(2019, 10, 03);

            List<Order> orders = dbGiaoHang.Orders.ToList();
            ListGiaohang(orders);
            txttongcong.Text = "0";
        }

        private void ListGiaohang(List<Order> orders)
        {
            int tong = 0;
            dgvQLdonhang.Rows.Clear();
            List<Order> List = orders.Where(p => p.Invoice.DeliveryDate >= dateTimeStart.Value && p.Invoice.DeliveryDate <= dateTimeEnd.Value).ToList();
            var Listt = from q in List
                        group q by new { q.InvoiceNo, q.Invoice.OrderDate, q.Invoice.DeliveryDate } into p
                        select new { InvoiceNo = p.Key.InvoiceNo, OrderDate = p.Key.OrderDate, p.Key.DeliveryDate, Price = p.Sum(o => o.Price * o.Quantity) };
            foreach (var item in Listt)
            {
                int index = dgvQLdonhang.Rows.Add();

                for (int i = 0; i < dgvQLdonhang.Rows.Count; i++)

                    dgvQLdonhang.Rows[index].Cells[0].Value = i;
                    dgvQLdonhang.Rows[index].Cells[1].Value = item.InvoiceNo;
                    dgvQLdonhang.Rows[index].Cells[2].Value = item.OrderDate.ToString("dd/MM/yyyy");
                    dgvQLdonhang.Rows[index].Cells[3].Value = item.DeliveryDate.Date.ToString("dd/MM/yyyy");
                    dgvQLdonhang.Rows[index].Cells[4].Value = item.Price;
                    tong += Convert.ToInt32(item.Price);  
            }
            txttongcong.Text = tong.ToString();

        }

        private void CBxemtatca_CheckedChanged(object sender, EventArgs e)
        {
            List<Order> orders = dbGiaoHang.Orders.ToList();
            if (CBxemtatca.Checked == true)
            {
                CBxemtatca_CheckStateChanged();
            } 
        }

        private void CBxemtatca_CheckStateChanged()
        {
            var M = dateTimeStart.Value.Month;
            var Y = dateTimeEnd.Value.Year;

            dateTimeStart.Value = new DateTime(Y, M, 01);
            if (M == 1 || M == 3 || M == 5 || M == 7 || M == 8 || M == 10 || M == 12)
            {
                dateTimeEnd.Value = new DateTime(Y, M, 31);
            }
            else if (M == 4 || M == 6 || M == 9 || M == 11)
            {
                dateTimeEnd.Value = new DateTime(Y, M, 30);
            }
            else
            {
                dateTimeEnd.Value = new DateTime(Y, M, 28);
            }
        }

        private void dateTimeStart_ValueChanged(object sender, EventArgs e)
        {
            List<Order> List = dbGiaoHang.Orders.ToList();
            ListGiaohang(List);
        }

    }
}