using System;
using System.Windows.Media.Imaging;

namespace SnowyPeak.KeyIndicatorWpf
{
    class Icons
    {
        private static Icons instance;
        public static Icons Instance
        {
            get
            {
                if (instance == null)
                    instance = new Icons();

                return instance;
            }
        }

        public BitmapImage NoIndicators { get; private set; }
        public BitmapImage Off { get; private set; }
        public BitmapImage Caps { get; private set; }
        public BitmapImage CapsNum { get; private set; }
        public BitmapImage CapsNumScroll { get; private set; }
        public BitmapImage CapsScroll { get; private set; }
        public BitmapImage Num { get; private set; }
        public BitmapImage NumScroll { get; private set; }
        public BitmapImage Scroll { get; private set; }

        private Icons()
        {
            NoIndicators = new BitmapImage(new Uri("pack://application:,,,/Icons/lock-single.ico"));
            Off = new BitmapImage(new Uri("pack://application:,,,/Icons/lock.ico"));
            Caps = new BitmapImage(new Uri("pack://application:,,,/Icons/lock-caps.ico"));
            CapsNum = new BitmapImage(new Uri("pack://application:,,,/Icons/lock-caps-num.ico"));
            CapsNumScroll = new BitmapImage(new Uri("pack://application:,,,/Icons/lock-caps-num-scroll.ico"));
            CapsScroll = new BitmapImage(new Uri("pack://application:,,,/Icons/lock-caps-scroll.ico"));
            Num = new BitmapImage(new Uri("pack://application:,,,/Icons/lock-num.ico"));
            NumScroll = new BitmapImage(new Uri("pack://application:,,,/Icons/lock-num-scroll.ico"));
            Scroll = new BitmapImage(new Uri("pack://application:,,,/Icons/lock-scroll.ico"));
        }
    }
}
