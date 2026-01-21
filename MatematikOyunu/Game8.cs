using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace MatematikOyunu
{
    public partial class Game8 : BaseGameForm
    {
        Label[] anaKareler = new Label[5]; // orta satır [5][+][y][=][7]
        Label[] dikeyKareler = new Label[4]; // üst ve alt kareler
        Label ustKareLabel;
        Label vKareLabel;
        Label bKareLabel;
        Label cKareLabel; // 7'nin sağındaki yeni boş kare (Cevap 5)

        // Doğru Cevaplar
        int dogruY = 2;
        int dogruZ = 4;
        int dogruX = 3;
        int dogruA = 9;
        int dogruV = 14;
        int dogruB = 11;
        int dogruC = 5; // Yeni eklenen boş karenin cevabı

        SoundPlayer fail;
        SoundPlayer succes;

        public Game8()
        {
            this.Text = "Matematik Oyunu - Level 8";
            this.Size = new Size(1000, 650); // Genişlik yeni kareler için artırıldı
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            fail = new SoundPlayer(Properties.Resources.fail);
            succes = new SoundPlayer(Properties.Resources.succes);

            OyunKur();
        }

        private void OyunKur()
        {
            // 1) ORTA SATIR: 5 + [y] = 7
            string[] ortaMetin = { "5", "+", "", "=", "7" };
            for (int i = 0; i < 5; i++)
            {
                anaKareler[i] = KareEkle(100 + i * 80, 220, 80, 80, ortaMetin[i], (i == 2), (i == 2) ? "yKare" : "");
            }

            // ==========================================
            // YENİ KARELER: 7'nin sağına "-" "2" "=" ""
            // ==========================================
            int yediX = anaKareler[4].Location.X;
            int yediY = anaKareler[4].Location.Y;

            KareEkle(yediX + 80, yediY, 80, 80, "-");
            KareEkle(yediX + 160, yediY, 80, 80, "2");
            KareEkle(yediX + 240, yediY, 80, 80, "=");
            cKareLabel = KareEkle(yediX + 320, yediY, 80, 80, "", true, "cKare");

            // 5'in Çevresi
            int besX = anaKareler[0].Location.X;
            int besY = anaKareler[0].Location.Y;
            KareEkle(besX, besY - 80, 80, 80, "+");
            Label altKareEsit = KareEkle(besX, besY + 80, 80, 80, "=");
            vKareLabel = KareEkle(altKareEsit.Location.X, altKareEsit.Location.Y + 80, 80, 80, "", true, "vKare");



            // 2) DİKEY KARELER (Sol taraf ve merkez dikey)
            int baslangicX = 100 + 2 * 80;
            KareEkle(baslangicX - 160, 60, 80, 80, "", true, "aKare");
            KareEkle(baslangicX - 80, 60, 80, 80, "-");

            string[] dikeyMetinler = { "6", "-", "=", "" };
            int[] yKonum = { 60, 140, 300, 380 };
            for (int i = 0; i < 4; i++)
            {
                dikeyKareler[i] = KareEkle(baslangicX, yKonum[i], 80, 80, dikeyMetinler[i], (i == 3), (i == 3) ? "zKare" : "");
            }

            KareEkle(baslangicX + 80, 60, 80, 80, "=");

            // 3) 7'nin ÜSTÜ VE ALTI
            Label yildizKare = KareEkle(yediX, yediY - 80, 80, 80, "*");
            ustKareLabel = KareEkle(yediX, yediY - 160, 80, 80, "", true, "ustKare");
            Label altKare1 = KareEkle(yediX, yediY + 80, 80, 80, "=");
            Label yirmiBirKaresi = KareEkle(altKare1.Location.X, altKare1.Location.Y + 80, 80, 80, "21");

            // 21'in sağına "-" "10" "=" ""
            int yeniX = yirmiBirKaresi.Location.X + 80;
            int yeniY = yirmiBirKaresi.Location.Y;
            KareEkle(yeniX, yeniY, 80, 80, "-");
            KareEkle(yeniX + 80, yeniY, 80, 80, "10");
            KareEkle(yeniX + 160, yeniY, 80, 80, "=");
            bKareLabel = KareEkle(yeniX + 240, yeniY, 80, 80, "", true, "bKare");

            // 7) ALT SAYI KUTULARI (5 eklendi)
            int[] sayilar = { 2, 4, 9, 5, 3, 14, 11 };
            for (int i = 0; i < sayilar.Length; i++)
            {
                Label kutu = KareEkle(40 + i * 90, 520, 80, 60, sayilar[i].ToString());
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

            // KAZANMA KONTROLÜ
            if (anaKareler[2].Text == dogruY.ToString() &&
                dikeyKareler[3].Text == dogruZ.ToString() &&
                ustKareLabel.Text == dogruX.ToString() &&
                vKareLabel.Text == dogruV.ToString() &&
                bKareLabel.Text == dogruB.ToString() &&
                cKareLabel.Text == dogruC.ToString() &&
                this.Controls["aKare"].Text == dogruA.ToString())
            {
                succes.Play();
                MessageBox.Show("Tebrikler! Tüm denklemler çözüldü 🎉", "Başarılı");
                Game9 yeniForm = new Game9();
                yeniForm.Show();
                this.Hide();
            }
        }
    }
}