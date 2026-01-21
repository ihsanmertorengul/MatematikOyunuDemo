using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace MatematikOyunu
{
    public partial class Game10 : BaseGameForm
    {
        Label[] anaKareler = new Label[5];
        Label[] dikeyKareler = new Label[4];
        Label ustKareLabel;
        Label vKareLabel;
        Label bKareLabel;
        Label cKareLabel;
        Label dKareLabel;
        Label eKareLabel;

        // Doğru Cevaplar
        int dogruY = 2;
        int dogruZ = 4;
        int dogruX = 3;
        int dogruA = 9;
        int dogruV = 14;
        int dogruB = 11;
        int dogruC = 5;
        int dogruD = 6;
        int dogruE = 7;

        SoundPlayer fail;
        SoundPlayer succes;

        public Game10()
        {
            this.Text = "Matematik Oyunu - Level 10";
            // Yükseklik, şıkların yeni konumuna göre 900 olarak optimize edildi
            this.Size = new Size(1100, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            fail = new SoundPlayer(Properties.Resources.fail);
            succes = new SoundPlayer(Properties.Resources.succes);

            OyunKur();
        }

        private void OyunKur()
        {
            // 1) ORTA SATIR: 5 + [y:2] = 7
            string[] ortaMetin = { "5", "+", "", "=", "7" };
            for (int i = 0; i < 5; i++)
            {
                anaKareler[i] = KareEkle(100 + i * 80, 220, 80, 80, ortaMetin[i], (i == 2), (i == 2) ? "yKare" : "");
            }

            int yediX = anaKareler[4].Location.X;
            int yediY = anaKareler[4].Location.Y;

            // 2) 7'NİN SAĞI: 7 - 2 = 5 (cKare)
            KareEkle(yediX + 80, yediY, 80, 80, "-");
            Label sabitIkiAlt = KareEkle(yediX + 160, yediY, 80, 80, "2");
            KareEkle(yediX + 240, yediY, 80, 80, "=");
            cKareLabel = KareEkle(yediX + 320, yediY, 80, 80, "", true, "cKare");

            // 3) 7'NİN ÜSTÜ VE SAĞI: 3 * 2 = 6
            KareEkle(yediX, yediY - 80, 80, 80, "*");
            ustKareLabel = KareEkle(yediX, yediY - 160, 80, 80, "", true, "ustKare");
            KareEkle(yediX + 80, yediY - 160, 80, 80, "*");
            Label sabitIkiUst = KareEkle(yediX + 160, yediY - 160, 80, 80, "2");
            KareEkle(yediX + 240, yediY - 160, 80, 80, "=");
            dKareLabel = KareEkle(yediX + 320, yediY - 160, 80, 80, "", true, "dKare");

            // 4) 7'NİN ALTI VE SAĞI: 21 - 10 = 11
            KareEkle(yediX, yediY + 80, 80, 80, "=");
            Label yirmiBirKaresi = KareEkle(yediX, yediY + 160, 80, 80, "21");
            KareEkle(yirmiBirKaresi.Location.X + 80, yirmiBirKaresi.Location.Y, 80, 80, "-");
            Label sabitOn = KareEkle(yirmiBirKaresi.Location.X + 160, yirmiBirKaresi.Location.Y, 80, 80, "10");
            KareEkle(yirmiBirKaresi.Location.X + 240, yirmiBirKaresi.Location.Y, 80, 80, "=");
            bKareLabel = KareEkle(yirmiBirKaresi.Location.X + 320, yirmiBirKaresi.Location.Y, 80, 80, "", true, "bKare");

            // 5) SOL TARAF VE 14'ün ALTI: 14 / 2 = 7
            int besX = anaKareler[0].Location.X;
            int besY = anaKareler[0].Location.Y;
            KareEkle(besX, besY - 80, 80, 80, "+");
            KareEkle(besX, besY + 80, 80, 80, "=");
            vKareLabel = KareEkle(besX, besY + 160, 80, 80, "", true, "vKare");

            int onDortX = vKareLabel.Location.X;
            int onDortY = vKareLabel.Location.Y;
            KareEkle(onDortX, onDortY + 80, 80, 80, "/");
            KareEkle(onDortX, onDortY + 160, 80, 80, "2");
            KareEkle(onDortX, onDortY + 240, 80, 80, "=");
            eKareLabel = KareEkle(onDortX, onDortY + 320, 80, 80, "", true, "eKare");

            // 6) DİKEY BAĞLANTILAR
            KareEkle(sabitIkiUst.Location.X, yediY - 80, 80, 80, "+");
            KareEkle(dKareLabel.Location.X, yediY - 80, 80, 80, "+");
            KareEkle(sabitOn.Location.X, yediY + 80, 80, 80, "=");
            KareEkle(bKareLabel.Location.X, yediY + 80, 80, 80, "=");

            // Merkezi dikey sıra
            int dikeyX = 100 + 2 * 80;
            KareEkle(dikeyX - 160, 60, 80, 80, "", true, "aKare");
            KareEkle(dikeyX - 80, 60, 80, 80, "-");
            string[] dikeyMetinler = { "6", "-", "=", "" };
            int[] yDikey = { 60, 140, 300, 380 };
            for (int i = 0; i < 4; i++)
            {
                dikeyKareler[i] = KareEkle(dikeyX, yDikey[i], 80, 80, dikeyMetinler[i], (i == 3), (i == 3) ? "zKare" : "");
            }
            KareEkle(dikeyX + 80, 60, 80, 80, "=");

            // 7) ALT SAYI KUTULARI - eKare'nin altına "yarım kare" boşlukla yerleştirildi
            // eKare Y: 660'da bitiyor, bu yüzden şıkları 740 - 750 civarına çektik.
            int[] sayilar = { 2, 4, 9, 5, 3, 14, 11, 6, 7 };
            for (int i = 0; i < sayilar.Length; i++)
            {
                Label kutu = KareEkle(40 + i * 105, 800, 90, 60, sayilar[i].ToString());
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
                case "eKare": dogruMu = (suruklenen == dogruE.ToString()); break;
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

            if (anaKareler[2].Text == dogruY.ToString() && dikeyKareler[3].Text == dogruZ.ToString() &&
                ustKareLabel.Text == dogruX.ToString() && vKareLabel.Text == dogruV.ToString() &&
                bKareLabel.Text == dogruB.ToString() && cKareLabel.Text == dogruC.ToString() &&
                dKareLabel.Text == dogruD.ToString() && eKareLabel.Text == dogruE.ToString() &&
                this.Controls["aKare"].Text == dogruA.ToString())
            {
                succes.Play();
                MessageBox.Show("Tebrikler! Seviye 10 Başarıyla Tamamlandı 🎉", "Harika!");
                Game11 yeniForm = new Game11();
                yeniForm.Show();
                this.Hide();
            }
        }
    }
}