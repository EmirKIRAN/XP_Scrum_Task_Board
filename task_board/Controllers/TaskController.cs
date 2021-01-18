using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using task_board.Models.EntityFramework;
using task_board.ViewModels;

namespace task_board.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        taskBoardEntities db = new taskBoardEntities();
        public ActionResult Index() // Kartların ana sayfada görüntülenmesini sağlar.
        {
            var model = db.tblKart.ToList(); // Veri tabanındaki tüm kartlar List tipinde bir veri yapısına aktarılır.
            return View(model); // Liste döndürülmek üzere cshtml sayfasına gönderilir.
        }
        public ActionResult Yeni()
        {
            var model = new CardFormViewModel() { 
                Personeller = db.tblPersonel.ToList(), // dropbox öğesine aktarılmak üzere personellerin listesi List biçiminde verilir.
                Projeler = db.tblProje.ToList() // dropbox öğesine aktarılmak üzere projelerin isimleri List biçiminde verilir.
            };
            return View("Yeni",model);
        }
        // Kayıt işlemlerinde sıradışı olaylar için token kullanımı
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(tblKart kart)
        {
            if(!ModelState.IsValid)
            {
                var model = new CardFormViewModel(){
                    Personeller = db.tblPersonel.ToList(), // dropbox öğesine aktarılmak üzere personellerin listesi List biçiminde verilir.
                    Projeler = db.tblProje.ToList(),
                    Kart = kart
                };
                return View("Yeni",model);
            }
            var kartVarMi = db.tblKart.Find(kart.kartID); // Kullanıcının gönderdiği kartın veritabanında olup olmadığına bakmak için nesne oluşturulur.
            // Eğer nesne boş ise kullanıcının gönderiği kart veri tabanında yoktur ve eklenir.
            cardAttr timeCalc = new cardAttr(); // Tahmini zaman hesaplamak için çağrılır.
            if(kartVarMi==null)
            {
                kart.tarih = DateTime.Now; // Kartın tarihine kartı oluşturma zamanı atanır.
                kart.durumID = 1; // Durum ID olarak ilk başta 1 olarak atılır. TODO kısmına yerleştirilir.
                kart.tahminiSure = timeCalc.tahminiSureHesapla(kart.aciklama); // Tahmini zaman değeri hesaplanıp kart nesnesine aktarılır.
                // Burada kart.tahminiSure ozelliğine değer atanacak onun için fonksiyon yazılacak.
                // Daha düzgün tahmin için model geliştirilmelidir.
                db.tblKart.Add(kart);
            }
            else
            {
                // İlgili değişkenlerin ana nesneye aktarılması
                kartVarMi.aciklama = kart.aciklama;
                kartVarMi.notlar = kart.notlar;
                kartVarMi.personelID = kart.personelID;
                kartVarMi.projeID = kart.projeID;
                kartVarMi.işTakibi = kart.işTakibi;
                kartVarMi.gerceklesenSure = kart.gerceklesenSure;
                kartVarMi.tahminiSure = timeCalc.tahminiSureHesapla(kart.aciklama);
                // İlgili değişiklikler veri tabanına kaydedilir. Güncelleme işlemi gerçekleşir.
            }
            db.SaveChanges(); // Değişiklikler veritabanına kaydedilir.
            return RedirectToAction("Index"); // Kullanıcı Index sayfasına gönderilerek kartları görüntülemesi sağlanır.
        }
        public ActionResult Detay(int id) // KART bilgilerinin gösterilmesi
        {
            var model = db.tblKart.Find(id); // İlgili karta ait veritabanından bilgiler getirilir.
            if (model == null) // Eğer id bilgisine göre bir kayıt yoksa hata basılır.
                return HttpNotFound(); // Hata sayfasına yönlendirilir.
            return View("Detay",model); // Şayet böyle bir kayıt varsa kullanıcı bu sayfaya yönlendirilir.
        }
        public ActionResult Sil(int id) // Kart silme işlemi
        {
            var silinecekKart = db.tblKart.Find(id); // Silinecek karta ait bilgiler veri tabanından getirilir.
            if (silinecekKart == null) // Eğer bu id bilgisine ait bir kayıt veri tabanında yoksa hata verilecektir.
                return HttpNotFound(); // Kullanıcı hata sayfasına yönlendirilir.
            db.tblKart.Remove(silinecekKart); // Eğer ilgili id bilgisine sahip kart varsa veri tabanından silinir.
            db.SaveChanges(); // Veri tabanı güncellemesi yapılır.
            return RedirectToAction("Index"); // Kullanıcı tekrardan ana sayfaya yönlendirilir.
        }
        public void KartDurumGuncelle(int id,int stateID) // Bu kısım kartlart sürüklendiği zaman durum bilgilerinin güncellenmesini sağlar
        {
            // Fornksiyon iki adet parametre alır. İlki kartın id bilgisini içerin id parametresidir
            // İkinci parametre ise sürüklenen alanın id bilgisini içerir ( TODO = 1, In Progress = 2 gibi.)
            var secilenKart = db.tblKart.Find(id); // gelen parametredeki karta ait veri tabanındaki ilgili nesneyi buluyoruz.
            if (secilenKart == null) // Eğer veri tabanında gelen parametredi id bilgisine göre herhangi bir kayıt yoksa fonksiyon boş döner
                return;
            secilenKart.durumID = stateID; // Eğer kayıt bulunduysa kartın durum bilgisi gelen ikinci parametredeki id bilgisi ile düzenlenir.
            db.SaveChanges(); // Değişiklikler veri tabanına kaydedilir.
        }
    }
}