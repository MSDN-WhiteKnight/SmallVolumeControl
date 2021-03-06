﻿/*
Small Volume Control
Author: MSDN.WhiteKnight
License: WTFPL
Requirements: Windows Vista, .NET 4.5 (or newer)
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace SmallVolumeControl
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public class Form1 : Form
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 117);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
        }

        #endregion


        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public const int WM_HOTKEY = 0x312;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            base.OnLoad(e);
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == WM_HOTKEY)
                {
                    if ((int)m.WParam == 3) { this.Close(); return; }
                    float f = Audio.Volume;
                    switch ((int)m.WParam)
                    {
                        case 1: f -= Conf.Instance.Increment; break;
                        case 2: f += Conf.Instance.Increment; break;
                    }
                    if (f < 0.0f) f = 0.0f;
                    if (f > 1.0f) f = 1.0f;
                    Audio.Volume = f;
                    m.Result = (IntPtr)0;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Small Volume Control");
            }

            base.WndProc(ref m);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "volume.xml");

            try
            {
                Conf conf;
                if (System.IO.File.Exists(path)) conf = Conf.Load(path);
                else { conf = new Conf(); conf.Save(path); }
                Conf.Instance = conf;

                bool res = RegisterHotKey(this.Handle, 1, 0, conf.DownKeyCode);
                if (res == false) MessageBox.Show("Error: RegisterHotKey failed", "Small Volume Control");

                res = RegisterHotKey(this.Handle, 2, 0, conf.UpKeyCode);
                if (res == false) MessageBox.Show("Error: RegisterHotKey failed", "Small Volume Control");
                this.Visible = false;

                res = RegisterHotKey(this.Handle, 3, 0, conf.ExitKeyCode);
                if (res == false) MessageBox.Show("Error: RegisterHotKey failed", "Small Volume Control");
                this.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Small Volume Control");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                UnregisterHotKey(this.Handle, 1);
                UnregisterHotKey(this.Handle, 2);
                UnregisterHotKey(this.Handle, 3);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Small Volume Control");
            }
        }
    }

    [Serializable]
    public class Conf
    {
        public string DeviceID { get; set; }
        public float Increment { get; set; }
        public uint DownKeyCode { get; set; }
        public uint UpKeyCode { get; set; }
        public uint ExitKeyCode { get; set; }

        public static Conf Instance { get; set; }

        public Conf()
        {
            Increment = 0.1f;
            DownKeyCode = (uint)Keys.F10;
            UpKeyCode = (uint)Keys.F11;
            ExitKeyCode = (uint)Keys.F9;
            DeviceID = "";
            //Example: {0.0.0.00000000}.{954b1750-0438-45f0-87ed-a4c1167eb4f2}
        }

        public void Save(string file)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Conf));
            var obj = new Conf();

            using (XmlWriter writer = XmlWriter.Create(file))
            {
                xs.Serialize(writer, obj);
            }
        }

        public static Conf Load(string file)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Conf));
            using (XmlReader reader = XmlReader.Create(file))
            {
                return (Conf)xs.Deserialize(reader);
            }
        }
    }


    [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioEndpointVolume
    {
        int f(); int g(); int h(); int i(); // Unused
        int SetMasterVolumeLevelScalar(float fLevel, System.Guid pguidEventContext);
        int j();
        int GetMasterVolumeLevelScalar(out float pfLevel);
        int k(); int l(); int m(); int n();
        int SetMute([MarshalAs(UnmanagedType.Bool)] bool bMute, System.Guid pguidEventContext);
        int GetMute(out bool pbMute);
    }
    [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMDevice
    {
        int Activate(ref System.Guid id, int clsCtx, int activationParams, out IAudioEndpointVolume aev);
    }
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMDeviceEnumerator
    {
        int f(); // Unused
        int GetDefaultAudioEndpoint(int dataFlow, int role, out IMMDevice endpoint);
        int GetDevice(string pwstrId, out IMMDevice endpoint);
    }
    [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    class MMDeviceEnumeratorComObject { }

    public class Audio
    {
        static IAudioEndpointVolume Vol()
        {
            IMMDeviceEnumerator enumerator = new MMDeviceEnumeratorComObject() as IMMDeviceEnumerator;
            IMMDevice dev = null;
            Marshal.ThrowExceptionForHR(enumerator.GetDevice(Conf.Instance.DeviceID, out dev));
            IAudioEndpointVolume epv = null;
            System.Guid epvid = typeof(IAudioEndpointVolume).GUID;
            Marshal.ThrowExceptionForHR(dev.Activate(ref epvid, /*CLSCTX_ALL*/ 23, 0, out epv));
            return epv;
        }
        public static float Volume
        {
            get { float v = -1; Marshal.ThrowExceptionForHR(Vol().GetMasterVolumeLevelScalar(out v)); return v; }
            set { Marshal.ThrowExceptionForHR(Vol().SetMasterVolumeLevelScalar(value, System.Guid.Empty)); }
        }
        public static bool Mute
        {
            get { bool mute; Marshal.ThrowExceptionForHR(Vol().GetMute(out mute)); return mute; }
            set { Marshal.ThrowExceptionForHR(Vol().SetMute(value, System.Guid.Empty)); }
        }
    }

}
