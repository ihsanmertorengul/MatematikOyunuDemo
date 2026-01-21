using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace MatematikOyunu
{
    public partial class Game2 : BaseGameForm
    {
        Label[] anaKareler = new Label[5]; // orta satır [5][+][y][=][7]
        Label[] dikeyKareler = new Label[4]; // üst ve alt kareler
        Label[] sayiKutulari = new Label[6]; // alt sayı kutuları
        int dogruY = 2;
        int dogruZ = 4;
        SoundPlayer fail;
        SoundPlayer succes;

        public Game2()
        {
            this.Text = "Matematik Oyunu - Level 2";
            this.Size = new Size(600, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            fail = new SoundPlayer(Properties.Resources.fail);
            succes = new SoundPlayer(Properties.Resources.succes);

            OyunKur();
        }

        private void OyunKur()
        {
            // Orta satır: [5] [+] [y] [=] [7]
            string[] ortaMetin = { "5", "+", "", "=", "7" };
            for (int i = 0; i < 5; i++)
            {
                Label lbl = new Label();
                lbl.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.Size = new Size(80, 80);
                lbl.BorderStyle = BorderStyle.FixedSingle;
                lbl.BackColor = Color.LightGray;
                lbl.Location = new Point(100 + i * 80, 220);
                lbl.Text = ortaMetin[i];

                if (i == 2) // y boş
                {
                    lbl.Name = "yKare";
                    lbl.AllowDrop = true;
                    lbl.DragEnter += Lbl_DragEnter;
                    lbl.DragDrop += Lbl_DragDrop;
                }

                anaKareler[i] = lbl;
                this.Controls.Add(lbl);
            }

            // Dikey kareler: üstte [6], [-], altta [=], [z boş]
            string[] dikeyMetinler = { "6", "-", "=", "" };
            int[] yKonum = { 60, 140, 300, 380 };
            for (int i = 0; i < 4; i++)
            {
                Label lbl = new Label();
                lbl.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.Size = new Size(80, 80);
                lbl.BorderStyle = BorderStyle.FixedSingle;
                lbl.BackColor = Color.LightGray;
                lbl.Location = new Point(100 + 2 * 80, yKonum[i]);
                lbl.Text = dikeyMetinler[i];

                if (i == 3) // z boş
                {
                    lbl.Name = "zKare";
                    lbl.AllowDrop = true;
                    lbl.DragEnter += Lbl_DragEnter;
                    lbl.DragDrop += Lbl_DragDrop;
                }

                dikeyKareler[i] = lbl;
                this.Controls.Add(lbl);
            }

            // Alt sayı kutuları manuel belirlenmiş
            int[] sayilar = { 2, 4, 1, 5, 7, 6 };
            for (int i = 0; i < sayilar.Length; i++)
            {
                Label lblSayi = new Label();
                lblSayi.Font = new Font("Segoe UI", 16, FontStyle.Bold);
                lblSayi.TextAlign = ContentAlignment.MiddleCenter;
                lblSayi.Size = new Size(80, 60);
                lblSayi.Location = new Point(40 + i * 90, 500);
                lblSayi.BorderStyle = BorderStyle.FixedSingle;
                lblSayi.BackColor = Color.LightBlue;
                lblSayi.Text = sayilar[i].ToString();

                lblSayi.MouseDown += LblSayi_MouseDown;
                sayiKutulari[i] = lblSayi;
                this.Controls.Add(lblSayi);
            }
        }

        private void LblSayi_MouseDown(object sender, MouseEventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null)
                lbl.DoDragDrop(lbl.Text, DragDropEffects.Move);
        }

        private void Lbl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Move;
        }

        private void Lbl_DragDrop(object sender, DragEventArgs e)
        {
            Label hedef = sender as Label;
            string suruklenen = e.Data.GetData(DataFormats.Text).ToString();

            if (hedef.Name == "yKare")
            {
                if (suruklenen == dogruY.ToString())
                {
                    hedef.Text = suruklenen;
                    hedef.BackColor = Color.LightGreen;
                }
                else
                {
                    fail.Play();
                    hedef.BackColor = Color.LightCoral;
                }
            }
            else if (hedef.Name == "zKare")
            {
                if (suruklenen == dogruZ.ToString())
                {
                    hedef.Text = suruklenen;
                    hedef.BackColor = Color.LightGreen;
                }
                else
                {
                    fail.Play();
                    hedef.BackColor = Color.LightCoral;
                }
            }

            if (anaKareler[2].Text == dogruY.ToString() && dikeyKareler[3].Text == dogruZ.ToString())
            {
                succes.Play();

                MessageBox.Show("Tebrikler! Her iki cevap da doğru 🎉", "Başarılı");

                // ✅ Yeni formu aç
                Game3  yeniForm = new Game3();
                yeniForm.Show();

                // ❌ Bu formu kapat
                this.Hide();
            }
        }

    }
}
