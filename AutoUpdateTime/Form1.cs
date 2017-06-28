using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateTime {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);

        private DateTime getOnlineDate(String server) {
            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(server);
            var response = myHttpWebRequest.GetResponse();
            string todaysDates = response.Headers["date"];
            return DateTime.ParseExact(todaysDates,
                                       "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                       CultureInfo.InvariantCulture.DateTimeFormat,
                                       DateTimeStyles.AssumeUniversal);
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string server = txtServerAdd.Text;

            if (String.IsNullOrEmpty(server))
            {
                MessageBox.Show("Vui lòng nhập vào địa chỉ server");

            }
            try
            {
                var date_online = getOnlineDate(server);
                var date_converted = date_online.ToUniversalTime();

                SYSTEMTIME st = new SYSTEMTIME {
                    wYear = (short)date_converted.Year,
                    wMonth = (short)date_converted.Month,
                    wDay = (short)date_converted.Day,
                    wHour = (short)(date_converted.Hour),
                    wMinute = (short)date_converted.Minute,
                    wSecond = (short)date_converted.Second,
                    wMilliseconds = (short)date_converted.Millisecond
                };

                SetSystemTime(ref st);

                txtUpdatedTime.Text = date_online.ToLongTimeString() + " - " + date_online.ToLongDateString();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Server đã bị ngỏm hoặc có lỗi xảy ra.\nVui lòng nhập server khác.");
                return;
            }
            
        }
    }
}
