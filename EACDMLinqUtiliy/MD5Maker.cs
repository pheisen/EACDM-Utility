using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QTIUtility;
using System.Security.Cryptography;

namespace EACDMLinqUtiliy
{
    public partial class MD5Maker : Form
    {
        public MD5Maker()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            tbHash.Text = Utilities.Md5HashUtilityUTF8(textBox1.Text.Trim());
            string first_encode = Utilities.Md5HashUtility(textBox1.Text.Trim());
            //this.tbSHA1.Text = Utilities.SHAHashUtilityUTF8(textBox1.Text.Trim());
            this.tbSHA1.Text = Utilities.Md5HashUtilityUTF8(first_encode);
            byte[] b = Encoding.Unicode.GetBytes(textBox1.Text.Trim());
            this.tbb64md5.Text = Utilities.Md5HashUtilityUTF8(Convert.ToBase64String(b, Base64FormattingOptions.None));
        }
        //public static bool ValidatePassword(string password, string correctHash)
        //{
        //    // Extract the parameters from the hash
        //    char[] delimiter = { ':' };
        //    string[] split = correctHash.Split(delimiter);
        //    int iterations = Int32.Parse(split[ITERATION_INDEX]);
        //    byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
        //    byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

        //    byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
        //    return SlowEquals(hash, testHash);
        //}
        private byte[] merge(byte[] source1,byte[] source2) {
        Byte[] buffer = new Byte[source1.Length + source2.Length];
            System.Buffer.BlockCopy(source1, 0, buffer, 0, source1.Length);
            System.Buffer.BlockCopy(source2, 0, buffer, source1.Length, source2.Length);
            return buffer;
        }

        private void button2_Click(object sender, EventArgs e)
        {
           string password = tbPlaintext.Text.Trim();
           byte[] pwd = UTF8Encoding.UTF8.GetBytes(password);
           string sha512 = tbSHAHash.Text.Trim();
           //lbResult.Text = PasswordHash.ValidatePassword(password, sha512).ToString();
            char[] delimiter = { ':' };
            string[] split = sha512.Split(delimiter);
            int iterations = Int32.Parse(split[2]);
            byte[] salt = Convert.FromBase64String(split[3]);
            byte[] hash = Convert.FromBase64String(split[4]);
            HMACSHA512 hmc = new HMACSHA512(salt);
            byte[] ii = new byte[0];
            hash = hmc.ComputeHash(merge(salt,pwd));
            for (int i = 1; i < iterations; i++)
            {
                
              //  ii[0] = (byte) i;
                hash = hmc.ComputeHash(merge(salt,hash));
            }
            lbResult.Text = Convert.ToBase64String(hash);
        }
    }
}
