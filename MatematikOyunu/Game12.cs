using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace MatematikOyunu
{
    public partial class Game12 : BaseGameForm
    {
        Label[] anaKareler = new Label[5];
        Label[] dikeyKareler = new Label[4];
        Label ustKareLabel;
        Label vKareLabel;
        Label bKareLabel;
        Label cKareLabel;
        Label dKareLabel;
        Label eKareLabel;
        Label fKareLabel;
        Label gKareLabel; // Cevabı 4 olan karenin altındaki boş kare (Cevap 6)

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
        int dogruF = 12;
        int dogruG = 6; // Yeni kare cevabı

        SoundPlayer fail;
        SoundPlayer succes;

        public Game12()
        {
            this.Text = "Matematik Oyunu - Level 12";
            // Dikey genişleme için yükseklik 1100 yapıldı
            this.Size = new Size(1200, 1100);
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

            // 2) 7'NİN SAĞI: 7 - 2 = 5
            KareEkle(yediX + 80, yediY, 80, 80, "-");
            KareEkle(yediX + 160, yediY, 80, 80, "2");
            KareEkle(yediX + 240, yediY, 80, 80, "=");
            cKareLabel = KareEkle(yediX + 320, yediY, 80, 80, "", true, "cKare");

            // 3) 7'NİN ÜSTÜ: 3 * 2 = 6
            KareEkle(yediX, yediY - 80, 80, 80, "*");
            ustKareLabel = KareEkle(yediX, yediY - 160, 80, 80, "", true, "ustKare");
            KareEkle(yediX + 80, yediY - 160, 80, 80, "*");
            KareEkle(yediX + 160, yediY - 160, 80, 80, "2");
            KareEkle(yediX + 240, yediY - 160, 80, 80, "=");
            dKareLabel = KareEkle(yediX + 320, yediY - 160, 80, 80, "", true, "dKare");

            // 4) 7'NİN ALTI: 21 - 10 = 11
            KareEkle(yediX, yediY + 80, 80, 80, "=");
            Label yirmiBirKaresi = KareEkle(yediX, yediY + 160, 80, 80, "21");
            KareEkle(yirmiBirKaresi.Location.X + 80, yirmiBirKaresi.Location.Y, 80, 80, "-");
            KareEkle(yirmiBirKaresi.Location.X + 160, yirmiBirKaresi.Location.Y, 80, 80, "10");
            KareEkle(yirmiBirKaresi.Location.X + 240, yirmiBirKaresi.Location.Y, 80, 80, "=");
            bKareLabel = KareEkle(yirmiBirKaresi.Location.X + 320, yirmiBirKaresi.Location.Y, 80, 80, "", true, "bKare");

            // 21'in ALTI: 21 - 12 = 9
            KareEkle(yirmiBirKaresi.Location.X, yirmiBirKaresi.Location.Y + 80, 80, 80, "-");
            fKareLabel = KareEkle(yirmiBirKaresi.Location.X, yirmiBirKaresi.Location.Y + 160, 80, 80, "", true, "fKare");
            KareEkle(yirmiBirKaresi.Location.X, yirmiBirKaresi.Location.Y + 240, 80, 80, "=");
            KareEkle(yirmiBirKaresi.Location.X, yirmiBirKaresi.Location.Y + 320, 80, 80, "9");

            // 5) SOL TARAF VE 14'ün ALTI: 14 / 2 = 7
            int besX = anaKareler[0].Location.X;
            int besY = anaKareler[0].Location.Y;
            KareEkle(besX, besY - 80, 80, 80, "+");
            KareEkle(besX, besY + 80, 80, 80, "=");
            vKareLabel = KareEkle(besX, besY + 160, 80, 80, "", true, "vKare");

            KareEkle(vKareLabel.Location.X, vKareLabel.Location.Y + 80, 80, 80, "/");
            KareEkle(vKareLabel.Location.X, vKareLabel.Location.Y + 160, 80, 80, "2"); //sağ x ekle
            KareEkle(vKareLabel.Location.X + 80, vKareLabel.Location.Y + 160 , 80, 80, "*");
            KareEkle(vKareLabel.Location.X, vKareLabel.Location.Y + 240, 80, 80, "=");
            eKareLabel = KareEkle(vKareLabel.Location.X, vKareLabel.Location.Y + 320, 80, 80, "", true, "eKare");


            // 6) MERKEZİ DİKEY SIRA (zKare: 4)
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

            // ==========================================
            // YENİ ENTEGRASYON: zKare(4) altına 4 + 6 = 10
            // ==========================================
            int dikeyAltX = dikeyKareler[3].Location.X;
            int dikeyAltY = dikeyKareler[3].Location.Y;
            KareEkle(dikeyAltX, dikeyAltY + 80, 80, 80, "+");
            gKareLabel = KareEkle(dikeyAltX, dikeyAltY + 160, 80, 80, "", true, "gKare"); // Cevap 6
            KareEkle(dikeyAltX, dikeyAltY + 240, 80, 80, "=");
            KareEkle(dikeyAltX, dikeyAltY + 320, 80, 80, "10");

            KareEkle(gKareLabel.Location.X + 80, gKareLabel.Location.Y, 80, 80, "=");

            // 7) ŞIKLAR (Y koordinatı 950'ye indirildi)
            int[] sayilar = { 2, 4, 9, 5, 3, 14, 11, 6, 7, 12 };
            for (int i = 0; i < sayilar.Length; i++)
            {
                Label kutu = KareEkle(40 + i * 105, 950, 95, 60, sayilar[i].ToString());
                kutu.BackColor = Color.LightBlue;
                kutu.MouseDown += LblSayi_MouseDown;
            }

            //en sağ + ve = kutuları
            KareEkle(dKareLabel.Location.X, yediY - 80, 80, 80, "+");
            KareEkle(bKareLabel.Location.X, yediY + 80, 80, 80, "=");
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
                case "fKare": dogruMu = (suruklenen == dogruF.ToString()); break;
                case "gKare": dogruMu = (suruklenen == dogruG.ToString()); break;
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
                fKareLabel.Text == dogruF.ToString() && gKareLabel.Text == dogruG.ToString() &&
                this.Controls["aKare"].Text == dogruA.ToString())
            {
                succes.Play();
                MessageBox.Show("Tebrikler! Seviye 12 Tamamlandı 🎉", "Harika!");

                GameTimer.Stop();

                ScoreManager.AddScore(GameTimer.GetTime());

                FinishScreen finis = new FinishScreen();
                finis.Show();

                // ❌ Bu formu kapat
                this.Hide();
            }
        }
    }
}