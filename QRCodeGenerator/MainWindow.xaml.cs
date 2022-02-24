using Microsoft.Win32;
using QRCoder;
using Svg;
using Svg.Pathing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using static QRCoder.SvgQRCode;

namespace QRCodeGenerator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        
        private QRCodeData currentQRCode;
        private QRCoder.QRCodeGenerator gen = new QRCoder.QRCodeGenerator();
        public event PropertyChangedEventHandler PropertyChanged;

        #region Getter/Setter
        public int SelectedTabIndex { get; set; }

        #region VCard/Contact details  

        private string _vcard_Name = "";
        public string Vcard_Name
        {
            get => _vcard_Name;
            set => SetProp(ref _vcard_Name, value);
        }

        private string _vcard_FirstName = "";
        public string Vcard_FirstName
        {
            get => _vcard_FirstName;
            set => SetProp(ref _vcard_FirstName, value);
        }

        private string _vcard_Organization = "";
        public string Vcard_Organization
        {
            get => _vcard_Organization;
            set => SetProp(ref _vcard_Organization, value);
        }

        private string _vcard_Telephone = "";
        public string Vcard_Telephone
        {
            get => _vcard_Telephone;
            set => SetProp(ref _vcard_Telephone, value);
        }

        private string _vcard_Email = "";
        public string Vcard_Email
        {
            get => _vcard_Email;
            set => SetProp(ref _vcard_Email, value);
        }

        private string _vcard_Cellphone = "";
        public string Vcard_Cellphone
        {
            get => _vcard_Cellphone;
            set => SetProp(ref _vcard_Cellphone, value);
        }

        private string _vcard_Fax = "";
        public string Vcard_Fax
        {
            get => _vcard_Fax;
            set => SetProp(ref _vcard_Fax, value);
        }

        private string _vcard_Street = "";
        public string Vcard_Street
        {
            get => _vcard_Street;
            set => SetProp(ref _vcard_Street, value);
        }

        private string _vcard_City = "";
        public string Vcard_City
        {
            get => _vcard_City;
            set => SetProp(ref _vcard_City, value);
        }

        private string _vcard_Region_State = "";
        public string Vcard_Region_State
        {
            get => _vcard_Region_State;
            set => SetProp(ref _vcard_Region_State, value);
        }

        private string _vcard_Postcode = "";
        public string Vcard_Postcode
        {
            get => _vcard_Postcode;
            set => SetProp(ref _vcard_Postcode, value);
        }

        private string _vcard_Country = "";
        public string Vcard_Country
        {
            get => _vcard_Country;
            set => SetProp(ref _vcard_Country, value);
        }

        private string _vcard_Website = "";
        public string Vcard_Website
        {
            get => _vcard_Website;
            set => SetProp(ref _vcard_Website, value);
        }

        #endregion

        #region SMS

        private string _sms_Number = "Type phone number here...";
        public string Sms_Number
        {
            get => _sms_Number;
            set => SetProp(ref _sms_Number, value);
        }

        #endregion

        #region Mail
        
        private string _mail_to = "Type URL here...";
        public string Mail_To
        {
            get => _mail_to;
            set => SetProp(ref _mail_to, value);
        }

        private string _mail_subject = "Type URL here...";
        public string Mail_Subject
        {
            get => _mail_subject;
            set => SetProp(ref _mail_subject, value);
        }

        private string _mail_message = "Type URL here...";
        public string Mail_Message
        {
            get => _mail_message;
            set => SetProp(ref _mail_message, value);
        }

        #endregion

        #region URL

        private string _url_urltext = "Type URL here...";
        public string URL_urltext 
        {
            get => _url_urltext;
            set => SetProp(ref _url_urltext, value);
        }

        #endregion

        #region plain text

        private string _plainText_Text="Offline QR Code Generator";
        public string PlainText_Text 
        {
            get => _plainText_Text;
            set =>SetProp(ref _plainText_Text, value);
        }

        #endregion

        #region qr image stuff

        private BitmapImage _QRCodeImage = null;
        public BitmapImage QRCodeImage 
        { 
            get => _QRCodeImage;
            set => SetProp(ref _QRCodeImage, value,callGenerateQR:false);
        }

        public int _pixelWidthHeight = 6;
        public int PixelWidthHeight
        {
            get => _pixelWidthHeight;
            set => SetProp(ref _pixelWidthHeight, value);
            
        }

        public int _logoSize = 15;
        public int LogoSize
        {
            get => _logoSize;
            set => SetProp(ref _logoSize, value);

        }

        public int _borderSize = 0;
        public int BorderSize
        {
            get => _borderSize;
            set => SetProp(ref _borderSize, value);

        }


        private QRCoder.QRCodeGenerator.ECCLevel _eCCLvL= QRCoder.QRCodeGenerator.ECCLevel.H;
        public QRCoder.QRCodeGenerator.ECCLevel ECCLvL
        {
            get => _eCCLvL;
            set => SetProp(ref _eCCLvL, value);
            
        }

        public IEnumerable<QRCoder.QRCodeGenerator.ECCLevel> ECCLvLValues
        {
            get
            {
                List<QRCoder.QRCodeGenerator.ECCLevel> enumValues = new List<QRCoder.QRCodeGenerator.ECCLevel>();

                foreach (QRCoder.QRCodeGenerator.ECCLevel enumValue in Enum.GetValues(typeof(QRCoder.QRCodeGenerator.ECCLevel)))
                {
                    enumValues.Add(enumValue);
                }
                return enumValues;
            }
        }

        public enum ComboColor { Black, White, Red, Blue, Green, Yellow, Pink }

        private ComboColor _foreGroundColor = ComboColor.Black;
        public ComboColor ForeGroundColor
        {
            get => _foreGroundColor;
            set => SetProp(ref _foreGroundColor, value);

        }

        private ComboColor _backGroundColor = ComboColor.White;
        public ComboColor BackGroundColor
        {
            get => _backGroundColor;
            set => SetProp(ref _backGroundColor, value);

        }


        public IEnumerable<ComboColor> ComboColors
        {
            get
            {
                List<ComboColor> enumValues = new List<ComboColor>();

                foreach (ComboColor enumValue in Enum.GetValues(typeof(ComboColor)))
                {
                    enumValues.Add(enumValue);
                }
                return enumValues;
            }
        }

        #endregion

        #region wifi

        private PayloadGenerator.WiFi.Authentication _wifiEncryption = PayloadGenerator.WiFi.Authentication.WPA;
        public PayloadGenerator.WiFi.Authentication WEncryption
        {
            get => _wifiEncryption;
            set => SetProp(ref _wifiEncryption, value);

        }
        public IEnumerable<PayloadGenerator.WiFi.Authentication> WEncryptions
        {
            get
            {
                List<PayloadGenerator.WiFi.Authentication> enumValues = new List<PayloadGenerator.WiFi.Authentication>();

                foreach (PayloadGenerator.WiFi.Authentication enumValue in Enum.GetValues(typeof(PayloadGenerator.WiFi.Authentication)))
                {
                    enumValues.Add(enumValue);
                }
                return enumValues;
            }
        }

        private string _wifi_ssid = "";
        public string Wifi_SSID
        {
            get => _wifi_ssid;
            set => SetProp(ref _wifi_ssid, value);
        }

        private string _wifi_password = "";
        public string Wifi_Password
        {
            get => _wifi_password;
            set => SetProp(ref _wifi_password, value);
        }

        #endregion

        #region event

        private string _event_Title = "";
        public string Event_Title
        {
            get => _event_Title;
            set => SetProp(ref _event_Title, value);
        }

        private string _event_Location = "";
        public string Event_Location
        {
            get => _event_Location;
            set => SetProp(ref _event_Location, value);
        }

        private DateTime? _event_StartDate = DateTime.Now;
        public DateTime? Event_StartDate
        {
            get => _event_StartDate;
            set => SetProp(ref _event_StartDate, value);
        }

        private DateTime? _event_EndDate = DateTime.Now;
        public DateTime? Event_EndDate
        {
            get => _event_EndDate;
            set => SetProp(ref _event_EndDate, value);
        }

        #endregion


        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            GenerateQR();
        }

       
        public void GenerateQR()
        {
            if (PixelWidthHeight < 1 || PixelWidthHeight > 500) { PixelWidthHeight = 10; }
            if (LogoSize<5 || LogoSize > 50) { LogoSize=15; }
            if (BorderSize < 0 || BorderSize>20) { BorderSize = 0; }
            QRCodeData qrCodeData= null;

            switch (SelectedTabIndex)
            {
                case 0: // plain text
                    {
                        qrCodeData = gen.CreateQrCode(PlainText_Text, ECCLvL);
                        break;
                    }
                case 1: //URL
                    {
                        string txt = URL_urltext;
                        if (!txt.ToLower().StartsWith("http") && !txt.ToLower().StartsWith("https"))
                        {
                            txt = "http://" + txt;
                        }

                        PayloadGenerator.Url url = new PayloadGenerator.Url(txt);
                        qrCodeData = gen.CreateQrCode(url, ECCLvL);
                        break;
                    }
                case 2: //email
                    {

                        PayloadGenerator.Mail mail = new PayloadGenerator.Mail(Mail_To, Mail_Subject, Mail_Message, PayloadGenerator.Mail.MailEncoding.MAILTO);
                        qrCodeData = gen.CreateQrCode(mail, ECCLvL);
                        break;
                    }
                case 3: //sms
                    {

                        PayloadGenerator.SMS sms = new PayloadGenerator.SMS(Sms_Number);
                        qrCodeData = gen.CreateQrCode(sms, ECCLvL);
                        break;
                    }
                case 4: //wifi
                    {
                        PayloadGenerator.WiFi wifi = new PayloadGenerator.WiFi(Wifi_SSID, Wifi_Password, WEncryption);
                        qrCodeData = gen.CreateQrCode(wifi, ECCLvL);
                        break;
                    }
                case 5: //VCard
                    {
                        PayloadGenerator.ContactData vcard = new PayloadGenerator.ContactData(
                            PayloadGenerator.ContactData.ContactOutputType.VCard3,
                            Vcard_Name,
                            Vcard_FirstName,
                            null,
                            Vcard_Telephone,
                            Vcard_Cellphone,
                            null,
                            Vcard_Email,
                            null,
                            Vcard_Website,
                            Vcard_Street,
                            null,
                            Vcard_City,
                            Vcard_Postcode,
                            Vcard_Country,
                            null,
                            Vcard_Region_State,
                            PayloadGenerator.ContactData.AddressOrder.Default,
                            Vcard_Organization
                            );
                        qrCodeData = gen.CreateQrCode(vcard, ECCLvL);
                        break;
                    }
                case 6: //event
                    {
                        bool allday = false;
                        if (Event_StartDate.HasValue && Event_EndDate.HasValue && Event_StartDate.Value.Ticks == Event_EndDate.Value.Ticks) { allday= true; }
                        PayloadGenerator.CalendarEvent calevent = new PayloadGenerator.CalendarEvent(Event_Title, "", Event_Location, Event_StartDate.Value, Event_EndDate.Value, allday);
                        qrCodeData = gen.CreateQrCode(calevent, ECCLvL);
                        break;
                    }
            }

            QRCode qrimage = new QRCode(qrCodeData);

            Bitmap b = null;
            if (logofile!=null)
            {
                if (logofile.ToLower().EndsWith(".svg"))
                {
                    SvgDocument svgDoc = SvgDocument.Open(logofile);
                    b= svgDoc.Draw();
                }
                else
                {
                    b = (Bitmap)Bitmap.FromFile(logofile);
                }
            }
            QRCodeImage = GetBitmapImage(qrimage.GetGraphic(PixelWidthHeight, GetColor(ForeGroundColor), GetColor(BackGroundColor),b,LogoSize,BorderSize));
            currentQRCode = qrCodeData;
        }



        #region helper methods

        public bool SetProp<T>(ref T property, T value, [CallerMemberName] string propertyName = null, bool callGenerateQR = true)
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
            {
                return false;
            }
            property = value;
            OnPropertyChanged(propertyName);
            if (callGenerateQR) { GenerateQR(); }

            return true;
        }

        public Color GetColor(ComboColor color)
        {
            switch (color)
            {
                case ComboColor.White:
                    {
                        return Color.White;
                    }
                case ComboColor.Black:
                    {
                        return Color.Black;
                    }
                case ComboColor.Red:
                    {
                        return Color.Red;
                    }
                case ComboColor.Blue:
                    {
                        return Color.Blue;
                    }
                case ComboColor.Green:
                    {
                        return Color.Green;
                    }
                case ComboColor.Yellow:
                    {
                        return Color.Yellow;
                    }
                case ComboColor.Pink:
                    {
                        return Color.Pink;
                    }
            }
            return Color.Black;
        }
        public byte[] GetbyteColor(ComboColor color)
        {
            switch (color)
            {
                case ComboColor.White:
                    {
                        return BitConverter.GetBytes(Color.White.ToArgb());
                    }
                case ComboColor.Black:
                    {
                        return BitConverter.GetBytes(Color.Black.ToArgb());
                    }
                case ComboColor.Red:
                    {
                        return BitConverter.GetBytes(Color.Red.ToArgb());
                    }
                case ComboColor.Blue:
                    {
                        return BitConverter.GetBytes(Color.Blue.ToArgb());
                    }
                case ComboColor.Green:
                    {
                        return BitConverter.GetBytes(Color.Green.ToArgb());
                    }
                case ComboColor.Yellow:
                    {
                        return BitConverter.GetBytes(Color.Yellow.ToArgb());
                    }
                case ComboColor.Pink:
                    {
                        return BitConverter.GetBytes(Color.Pink.ToArgb());
                    }
            }
            return new byte[4];
        }

        private Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private Image GetImage(byte[] b)
        {
            using (var ms = new MemoryStream(b))
            {
                return Image.FromStream(ms);
            }
        }


        private BitmapImage GetBitmapImage(Bitmap b)
        {
            MemoryStream ms = new MemoryStream();  
            b.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);  

            var bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.EndInit();

            return bi;
        }
        private BitmapImage GetBitmapImage(byte[] b)
        {
            Image image = GetImage(b);
            using (var ms = new MemoryStream())
            {

                image.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        private SvgPath CreatePath(PointF[] segments)
        {
            SvgPath thePath = new SvgPath();
            thePath.StrokeWidth = (SvgUnit)1.0;
            thePath.ID = Guid.NewGuid().ToString();
            thePath.PathData = new SvgPathSegmentList();

            foreach (PointF point in segments)
            {
                SvgMoveToSegment segment = new SvgMoveToSegment(true, point);
                thePath.PathData.Add(segment);
            }

            return thePath;
        }





        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        #endregion


        private void SaveTo_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "png Image|*.png|Bitmap Image|*.bmp|svg vector|*.svg|Ascii txt|*.txt";
            dialog.Title = "Save Image/Vector";
            bool? result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                FileInfo fi = new FileInfo(dialog.FileName);

                if (fi.Extension.ToLower().Equals(".png"))
                {
                  
                    //PngByteQRCode qrimage = new PngByteQRCode(currentQRCode);
                    //BitmapImage bi = GetBitmapImage(qrimage.GetGraphic(PixelWidthHeight, GetbyteColor(ForeGroundColor), GetbyteColor(BackGroundColor), true));

                    using (FileStream stream = new FileStream(fi.FullName, FileMode.Create))
                    {
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(QRCodeImage));
                        encoder.Save(stream);
                    }
                }

                if (fi.Extension.ToLower().Equals(".bmp"))
                {
                    //BitmapByteQRCode qrimage = new BitmapByteQRCode(currentQRCode);
                    //BitmapImage bi = GetBitmapImage(qrimage.GetGraphic(PixelWidthHeight, GetbyteColor(ForeGroundColor), GetbyteColor(BackGroundColor)));

                    using (FileStream stream = new FileStream(fi.FullName, FileMode.Create))
                    {
                        BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(QRCodeImage));
                        encoder.Save(stream);
                    }
                }


                if (fi.Extension.ToLower().Equals(".svg"))
                {
                    SvgQRCode qrimage = new SvgQRCode(currentQRCode);


                    Bitmap b = null;
                    SvgLogo logo = null;
                    if (logofile != null)
                    {
                        if (logofile.ToLower().EndsWith(".svg"))
                        {
                            SvgDocument svgDoc = SvgDocument.Open(logofile);
                            logo = new SvgLogo(svgDoc.GetXML(), LogoSize, true);
                        }
                        else
                        {
                            b = (Bitmap)Bitmap.FromFile(logofile);
                            logo = new SvgLogo(b, LogoSize, true);
                        }
                    }

                    string svgData = qrimage.GetGraphic(PixelWidthHeight, GetColor(ForeGroundColor), GetColor(BackGroundColor), true,logo:logo);
                    File.WriteAllText(fi.FullName, svgData);
                }

                if (fi.Extension.ToLower().Equals(".txt"))
                {
                    AsciiQRCode qrimage = new AsciiQRCode(currentQRCode);

                    Bitmap b = null;
                    SvgLogo logo = null;
                    if (logofile != null)
                    {
                        b = (Bitmap)Bitmap.FromFile(logofile);
                        logo = new SvgLogo(b, LogoSize, true);
                    }

                    string txtdata = qrimage.GetGraphic(1);
                    File.WriteAllText(fi.FullName, txtdata);
                }

            }
        }

        string logofile;
        private void QRLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Known formats|*.png;*.bmp;*.jpg;*.jpeg;*.svg|png|*.png|bmp|*.bmp|jpeg|*.jpg;*.jpeg|vector svg|*.svg";
            bool? result=dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                logofile = dialog.FileName;
            }
            GenerateQR();
        }

        private void RemoveQRLogo_Click(object sender, RoutedEventArgs e)
        {
            logofile = null;
            GenerateQR();
        }
    }
}
