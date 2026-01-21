using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace MatematikOyunu
{
    public partial class Game7 : BaseGameForm
    {
        Label[] anaKareler = new Label[5];
        Label[] dikeyKareler = new Label[4];
        Label ustKareLabel;
        Label vKareLabel;
        Label bKareLabel;

        // ✅ Yeni doğru cevaplar (uçmadan)
        int dogruY = 3;   // 7 + y = 10
        int dogruZ = 2;   // a - 5 = 4
        int dogruX = 2;   // x * 10 = 20
        int dogruA = 7;   // a - 5 = 4 => a = 9
        int dogruV = 14;  // 7'nin altındaki boş kare
        int dogruB = 12;  // 20 - 8 = 12

        SoundPlayer fail;
        SoundPlayer succes;

        public Game7()
        {
            this.Text = "Matematik Oyunu - Level 7";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            fail = new SoundPlayer(Properties.Resources.fail);
            succes = new SoundPlayer(Properties.Resources.succes);

            OyunKur();
        }

        private void OyunKur()
        {
            // 1) ORTA SATIR: 7 + [y] = 10
            string[] ortaMetin = { "7", "+", "", "=", "10" };
            for (int i = 0; i < 5; i++)
            {
                anaKareler[i] = KareEkle(
                    100 + i * 80, 220, 80, 80,
                    ortaMetin[i],
                    (i == 2),
                    (i == 2) ? "yKare" : ""
                );
            }

            // 7'nin Çevresi (üst +, alt =, altındaki vKare)
            int solSayiX = anaKareler[0].Location.X; // 7'nin X
            int solSayiY = anaKareler[0].Location.Y; // 7'nin Y

            KareEkle(solSayiX, solSayiY - 80, 80, 80, "+");
            Label altKareEsit = KareEkle(solSayiX, solSayiY + 80, 80, 80, "=");
            vKareLabel = KareEkle(altKareEsit.Location.X, altKareEsit.Location.Y + 80, 80, 80, "", true, "vKare");

            // 2) DİKEY KARELER: a - 5 = z
            int baslangicX = 100 + 2 * 80; // 260

            KareEkle(baslangicX - 160, 60, 80, 80, "", true, "aKare");
            KareEkle(baslangicX - 80, 60, 80, 80, "-");

            string[] dikeyMetinler = { "5", "-", "=", "" };
            int[] yKonum = { 60, 140, 300, 380 };

            for (int i = 0; i < 4; i++)
            {
                dikeyKareler[i] = KareEkle(
                    baslangicX, yKonum[i], 80, 80,
                    dikeyMetinler[i],
                    (i == 3),
                    (i == 3) ? "zKare" : ""
                );
            }

            // sağa "=" (dekor)
            KareEkle(baslangicX + 80, 60, 80, 80, "=");

            // 3) Sağ uç (10'un üstü/altı): * ve x, sonuç 20
            int sagSayiX = anaKareler[4].Location.X; // 10'un X
            int sagSayiY = anaKareler[4].Location.Y;

            KareEkle(sagSayiX, sagSayiY - 80, 80, 80, "*");
            ustKareLabel = KareEkle(sagSayiX, sagSayiY - 160, 80, 80, "", true, "ustKare");

            Label altKare1 = KareEkle(sagSayiX, sagSayiY + 80, 80, 80, "=");
            Label sonucKare = KareEkle(altKare1.Location.X, altKare1.Location.Y + 80, 80, 80, "20");

            // ==========================================
            // YENİ KARELER: 20'nin sağına "-" "8" "=" ""  => b = 12
            // ==========================================
            int yeniX = sonucKare.Location.X + 80;
            int yeniY = sonucKare.Location.Y;

            KareEkle(yeniX, yeniY, 80, 80, "-");
            KareEkle(yeniX + 80, yeniY, 80, 80, "8");
            KareEkle(yeniX + 160, yeniY, 80, 80, "=");
            bKareLabel = KareEkle(yeniX + 240, yeniY, 80, 80, "", true, "bKare");

            // 7) ALT SAYI KUTULARI (güncellendi)
            int[] sayilar = { 3, 4, 9, 2, 12, 7, 14 };
            for (int i = 0; i < sayilar.Length; i++)
            {
                Label kutu = KareEkle(40 + i * 90, 500, 80, 60, sayilar[i].ToString());
                kutu.BackColor = Color.LightBlue;
                kutu.MouseDown += LblSayi_MouseDown;
            }
        }

        private Label KareEkle(int x, int y, int w, int h, string text = "", bool surukle = false, string name = "")
        {
            Label lbl = new Label();
            lbl.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Size = new Size(w, h);
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.BackColor = Color.LightGray;
            lbl.Location = new Point(x, y);
            lbl.Text = text;

            if (surukle)
            {
                lbl.Name = name;
                lbl.AllowDrop = true;
                lbl.DragEnter += Lbl_DragEnter;
                lbl.DragDrop += Lbl_DragDrop;
            }

            this.Controls.Add(lbl);
            return lbl;
        }

        private void LblSayi_MouseDown(object sender, MouseEventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null) lbl.DoDragDrop(lbl.Text, DragDropEffects.Move);
        }

        private void Lbl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text)) e.Effect = DragDropEffects.Move;
        }

        private void Lbl_DragDrop(object sender, DragEventArgs e)
        {
            Label hedef = sender as Label;
            string suruklenen = e.Data.GetData(DataFormats.Text).ToString();
            bool dogruMu = false;

            switch (hedef.Name)
            {
                case "yKare": dogruMu = (suruklenen == dogruY.ToString()); break;
                case "zKare": dogruMu = (suruklenen == dogruZ.ToString()); break;
                case "ustKare": dogruMu = (suruklenen == dogruX.ToString()); break;
                case "aKare": dogruMu = (suruklenen == dogruA.ToString()); break;
                case "vKare": dogruMu = (suruklenen == dogruV.ToString()); break;
                case "bKare": dogruMu = (suruklenen == dogruB.ToString()); break;
            }

            if (dogruMu)
            {
                hedef.Text = suruklenen;
                hedef.BackColor = Color.LightGreen;
            }
            else
            {
                fail.Play();
                hedef.BackColor = Color.LightCoral;
            }

            if (anaKareler[2].Text == dogruY.ToString() &&
                dikeyKareler[3].Text == dogruZ.ToString() &&
                ustKareLabel.Text == dogruX.ToString() &&
                vKareLabel.Text == dogruV.ToString() &&
                bKareLabel.Text == dogruB.ToString() &&
                this.Controls["aKare"].Text == dogruA.ToString())
            {
                succes.Play();
                MessageBox.Show("Tebrikler! Seviye Tamamlandı 🎉", "Başarılı");
                Game8 yeniForm = new Game8();
                yeniForm.Show();
                this.Hide();
            }
        }
    }
}
