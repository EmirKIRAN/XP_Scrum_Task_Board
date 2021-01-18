using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using task_board.Models.EntityFramework;

namespace task_board.Controllers
{
    public class htmlGener
    {
        public List<tblKart> createCardObject(int id, List<tblKart> model) // Bu fonksiyon ilgili alana(ToDo,In Progress vs.) göre kartların listesini geri döner.
        {
            List<tblKart> kartlar = new List<tblKart>(); // Geri dönecek olan kart listesi oluşturulur.
            foreach (var kart in model) // Liste içinde gezinme
            {
                if (kart.durumID == id) // Eğer kart durumu verilin alanın id'si ile aynı ise listeye eklenir.
                    kartlar.Add(kart); // Örneğin alan TODO ise bunun durumID bilgisi = 1 olur. kart.durumID = 1 olanlar bu listeye eklenir
            }
            return kartlar; // İlgili alana ait kart listesi geriye döndürülür.
        }
        public string getStateColor(string time) // Zamana göre renk belirleme fonksiyonu
        {
            // parametre olarak alınan değer şu şekidedir ==> 7 SAAT vs.
            int gun = Convert.ToInt32(time.Split(' ')[0]); // Burada ilk olarak zaman ifadesinin sayısal kısmı tamsayıya çevrilir.
            if (gun < 3) // Eğer saat 3 ten küçükse renk olarak kırmızı döner
                return "bg-danger";
            else if (gun >= 3 && gun < 7) // Eğer saat 3 ile 7 arasında ise sarı renk geri döner
                return "bg-warning";
            else // Eğer saat değikeni yediden büyük ise yeşil renk dönmüş olur.
                return "bg-success";
        }
    }
}