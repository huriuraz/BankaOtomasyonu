using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _13253065P_
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }


        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();//eğer uygulamadan çıkılırsa tamamen kapatır
        }

        private void hesapAçtırmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHesap frm4 = new FormHesap();
            frm4.MdiParent = this;
            frm4.Show();//menüstrip'ten hesap açtırmaya tıklarsa Formhesap formu gösterilecek
        }

        private void raporİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRapor frm6 = new FormRapor();
            frm6.MdiParent = this;
            frm6.Show();//menüstrip'ten rapor işlemlerine tıklarsa Formrapor formu gösterilecek
        }

        private void hesapKapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHesapKapat frm8 = new FormHesapKapat();
            frm8.MdiParent = this;
            frm8.Show();//menüstrip'ten hesap kapata tıklarsa FormHesapKapat formu gösterilecek
        }

        private void kullanıcıEkleÇıkarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUser frm3 = new FormUser();
            frm3.MdiParent = this;
            frm3.Show();//menüstrip'ten kullanıcı ekle'ye tıklarsa FormUser formu gösterilecek
        }

        private void paraİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBankaIslem frm5 = new FormBankaIslem();
            frm5.MdiParent = this;
            frm5.Show();//menüstrip'ten para işlemlerine tıklarsa Formbankaislem formu gösterilecek
        }
        private void hesapÖzetiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormOzet frm7 = new FormOzet();
            frm7.MdiParent = this;
            frm7.Show();//menüstrip'ten hesap özetine tıklarsa Formozet formu gösterilecek
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
