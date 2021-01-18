using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace task_board.Controllers
{
    public class cardAttr
    {
        private int kelimeHesapla(string aciklama) // Kelime bulma fonksiyonu
        {
            return aciklama.Split(' ').Length; // Bu kısımda kelime sayısını boşluk ile ayırarak buluruz.
            // Geriye hesaplanması için kelime sayısını döndürürüz.
        }
        private int saatHesapla(int kelimeSayisi) // Tahmini süreyi hesaplayan fonksiyon
        {
            double saat = Math.Round(kelimeSayisi * 0.22 - 0.2); // Bu kısımda saat değişkeni lineer regresyon kullanılarak hesaplanmıştır.
            // y = ax+b olur ve kelimeSayisi*a(0.22) - 0.2(b) ile hesaplama işlemi gerçekleşir
            return Convert.ToInt32(saat); // Geriye string ifadede yazılmak üzere saat değeri döner.
        }
        public string tahminiSureHesapla(string aciklama) // kart nesnesinin tahminiSure özelliğie kaydedilecek değeri geri döner
        {
            return saatHesapla(kelimeHesapla(aciklama)) + " SAAT";
        }
    }
}