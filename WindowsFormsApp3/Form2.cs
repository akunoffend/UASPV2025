using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace WindowsFormsApp3
{
    public partial class Form2 : Form
    {
        private string selectedImagePath = string.Empty;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Menutup seluruh aplikasi jika Form2 ditutup
        }

        private void button1_Click(object sender, EventArgs e) // Tombol Home - Kembali ke Form 1
        {
            Form1 form1 = new Form1(); // Buat instance Form1
            form1.Show(); // Tampilkan Form1
            this.Close(); // Tutup Form2
        }

        private void button2_Click(object sender, EventArgs e) // Tombol Pilih File
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Image Files|*.jpeg;*.jpg;*.png;*.bmp" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = openFileDialog.FileName;
                    textBox1.Text = selectedImagePath; // Menampilkan nama file di TextBox
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedImagePath))
            {
                MessageBox.Show("Pilih gambar terlebih dahulu!");
                return;
            }

            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "ind", EngineMode.Default))
                {
                    // Proses gambar
                    using (var img = Pix.LoadFromFile(selectedImagePath))
                    {
                        var result = engine.Process(img);
                        textBox2.Text = result.GetText();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                Clipboard.SetText(textBox2.Text); // Salin teks ke clipboard
                MessageBox.Show("Teks berhasil disalin!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Tidak ada teks untuk disalin.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Clear(); // Menghapus teks hasil OCR
            textBox1.Clear(); // Menghapus path file yang dipilih
        }
    }
}
