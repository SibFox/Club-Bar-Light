using System.Collections.Generic;
using System.Windows.Controls;
using BarLight.Model;

namespace BarLight.Common
{
    public static class Manager
    {
        public static Frame MainFrame { get; set; }
        public static Frame FrameContainer_First { get; set; }


        public static List<ClientCart> Cart = new List<ClientCart>();
        public static TextBlock CartAmmount { get; set; }
        public static double CartPrice = 0.0;

        public static Button ProfileButton { private get; set; }
        public static void ChangePFButton(string str)
        {
            ProfileButton.Content = str;
            ProfileButton.FontSize = 18;
        }

        public static string CardNum { get; set; }

        public static int ClientID { get; set; }

        public static Establishment ChoosenEstablishment { get; set; }
        public static Button EstablishmentButton { private get; set; }
        public static void ChangeELButton(string str)
        {
            EstablishmentButton.Content = str;
            EstablishmentButton.FontSize = 13.5;
        }
        public static bool IsELButtonNotNull() => EstablishmentButton != null;

        private static BarLightDBEntities _context;

        public static BarLightDBEntities Context
        {
            get {
                if (_context == null)
                    _context = new BarLightDBEntities();
                return _context;
            }
        }
    }
}
