using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace MatematikOyunu
{
    public partial class Game9 : BaseGameForm
    {
        Label[] anaKareler = new Label[5];
        Label[] dikeyKareler = new Label[4];
        Label ustKareLabel;
        Label vKareLabel;
        Label bKareLabel;
        Label cKareLabel;
        Label dKareLabel;

        // ✅ Yeni doğru cevaplar (uçmadan)
        int dogruY = 2;   // 7 + y = 9
        int dogruX = 2;   // 9 * x = 18
        int dogruC = 5;   // 9 - 4 = 5
        int dogruD = 6;   // x * 3 = 6
        int dogruB = 11;  // 18 - 7 = 11

        int dogruA = 7;   // a - 5 = 3
        int dogruZ = 3;   // a - 5 = z
        int dogruV = 14;  // sol alttaki vKare

        SoundPlayer fail;
        SoundPlayer succes;

        public Game9()
        {
            this.Text = "Matematik Oyunu - Level 9";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            fail = new SoundPlayer(Properties.Resources.fail);
            succes = new SoundPlayer(Properties.Resources.succes);

            OyunKur();
        }

        private void OyunKur()
        {
            // 1) ORTA SATIR (ANA): 7 + [y] = 9
            string[] ortaMetin = { "7", "+", "", "=", "9" };
            for (int i = 0; i < 5; i++)
            {
                anaKareler[i] = KareEkle(
                    100 + i * 80, 220, 80, 80,
                    ortaMetin[i],
                    (i == 2),
                    (i == 2) ? "yKare" : ""
                );
            }

            int sagSayiX = anaKareler[4].Location.X; // "9" karesi
            int sagSayiY = anaKareler[4].Location.Y;

            // 2) 9'UN SAĞI: 9 - 4 = 5 (cKare)
            KareEkle(sagSayiX + 80, sagSayiY, 80, 80, "-");
            Label sabitDortAlt = KareEkle(sagSayiX + 160, sagSayiY, 80, 80, "4");
            KareEkle(sagSayiX + 240, sagSayiY, 80, 80, "=");
            cKareLabel = KareEkle(sagSayiX + 320, sagSayiY, 80, 80, "", true, "cKare"); // 5

            // 3) 9'UN ÜSTÜ: 9 * x = 18 (dikey bağlantı)
            KareEkle(sagSayiX, sagSayiY - 80, 80, 80, "*");
            ustKareLabel = KareEkle(sagSayiX, sagSayiY - 160, 80, 80, "", true, "ustKare"); // x = 2

            // 4) x'İN SAĞI: x * 3 = 6 (dKare)
            KareEkle(sagSayiX + 80, sagSayiY - 160, 80, 80, "*");
            Label sabitUcUst = KareEkle(sagSayiX + 160, sagSayiY - 160, 80, 80, "3");
            KareEkle(sagSayiX + 240, sagSayiY - 160, 80, 80, "=");
            dKareLabel = KareEkle(sagSayiX + 320, sagSayiY - 160, 80, 80, "", true, "dKare"); // 6

            // 5) 9'UN ALTI: 9 = 18 (x=2 olduğundan) — burada sonuç sabit
            Label altEsitKare = KareEkle(sagSayiX, sagSayiY + 80, 80, 80, "=");
            Label onSekizKaresi = KareEkle(sagSayiX, sagSayiY + 160, 80, 80, "18");

            // 6) 18'İN SAĞI: 18 - 7 = 11 (bKare)
            KareEkle(onSekizKaresi.Location.X + 80, onSekizKaresi.Location.Y, 80, 80, "-");
            Label sabitYedi = KareEkle(onSekizKaresi.Location.X + 160, onSekizKaresi.Location.Y, 80, 80, "7");
            KareEkle(onSekizKaresi.Location.X + 240, onSekizKaresi.Location.Y, 80, 80, "=");
            bKareLabel = KareEkle(onSekizKaresi.Location.X + 320, onSekizKaresi.Location.Y, 80, 80, "", true, "bKare"); // 11

            // ==========================================
            // DİKEY BAĞLANTILAR (+ ve =)
            // ==========================================
            // Üstteki 3 ile alttaki 4 arasına "+"
            KareEkle(sabitUcUst.Location.X, sagSayiY - 80, 80, 80, "+");

            // Üstteki d (6) ile alttaki c (5) arasına "+"
            KareEkle(dKareLabel.Location.X, sagSayiY - 80, 80, 80, "+");

            // 7'nin üzerine "="
            KareEkle(sabitYedi.Location.X, sagSayiY + 80, 80, 80, "=");

            // b'nin üzerine "="
            KareEkle(bKareLabel.Location.X, sagSayiY + 80, 80, 80, "=");

            // ==========================================
            // SOL TARAF (vKare) + ORTA DİKEY BLOK (a - 5 = z)
            // ==========================================
            int solSayiX = anaKareler[0].Location.X; // "7" karesi
            int solSayiY = anaKareler[0].Location.Y;

            KareEkle(solSayiX, solSayiY - 80, 80, 80, "+");
            Label solEsit = KareEkle(solSayiX, solSayiY + 80, 80, 80, "=");
            vKareLabel = KareEkle(solEsit.Location.X, solEsit.Location.Y + 80, 80, 80, "", true, "vKare"); // 12

            // Merkezi dikey sıra: a - 5 = z
            int dikeyX = 100 + 2 * 80; // 260
            KareEkle(dikeyX - 160, 60, 80, 80, "", true, "aKare"); // 8
            KareEkle(dikeyX - 80, 60, 80, 80, "-");

            string[] dikeyMetinler = { "5", "-", "=", "" };
            int[] yDikey = { 60, 140, 300, 380 };
            for (int i = 0; i < 4; i++)
            {
                dikeyKareler[i] = KareEkle(
                    dikeyX, yDikey[i], 80, 80,
                    dikeyMetinler[i],
                    (i == 3),
                    (i == 3) ? "zKare" : ""
                );
            }
            KareEkle(dikeyX + 80, 60, 80, 80, "=");

            // ==========================================
            // ŞIKLAR (güncellendi)
            // Doğrular: y=2, z=3, x=2, a=8, v=12, b=11, c=5, d=6
            // ==========================================
            int[] sayilar = { 2, 3, 5, 12, 11, 7, 6, 14 }; // 10 dikkat dağıtıcı
            for (int i = 0; i < sayilar.Length; i++)
            {
                Label kutu = KareEkle(40 + i * 90, 550, 80, 60, sayilar[i].ToString());
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
                case "cKare": dogruMu = (suruklenen == dogruC.ToString()); break;
                case "dKare": dogruMu = (suruklenen == dogruD.ToString()); break;
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
                cKareLabel.Text == dogruC.ToString() &&
                dKareLabel.Text == dogruD.ToString() &&
                this.Controls["aKare"].Text == dogruA.ToString())
            {
                succes.Play();
                MessageBox.Show("Tebrikler! Tüm bulmaca çözüldü 🎉", "Başarılı");
                Game10 yeniForm = new Game10();
                yeniForm.Show();
                this.Hide();
            }
        }
    }
}
