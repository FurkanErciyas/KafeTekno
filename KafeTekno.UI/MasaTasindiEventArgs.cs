using System;

namespace KafeTekno.UI
{
    public class MasaTasindiEventArgs : EventArgs
    {
        public MasaTasindiEventArgs(int eskiMasaNo, int yeniMasaNo)
        {
            EskiMasaNo = eskiMasaNo;
            YeniMasaNo = yeniMasaNo;
        }
        public int EskiMasaNo { get; }
        public int YeniMasaNo { get; }
    }
}