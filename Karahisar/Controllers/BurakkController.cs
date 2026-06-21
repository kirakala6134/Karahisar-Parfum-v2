using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Karahisar.Models;


namespace Karahisar.Controllers
{
    public class BurakkController : Controller
    {
        //karahisardbEntities19 db=new karahisardbEntities19();
        karahisardbEntitiesev1 db =new karahisardbEntitiesev1();
        // GET: Burakk
        public void urunsayisibul()
        {
            if (Session["id"] != null)
            {
                int id = Convert.ToInt32(Session["id"].ToString());
                int sayi = db.SepetTablo.Count(alanlar => alanlar.musteriid == id);
                Session["urunsayisi"] = sayi;
            }
            else
            {
                Session["urunsayisi"] = 0;
            }
        }
        public ActionResult Index()
        {
            urunsayisibul();
            var deger = db.UrunTablo.SqlQuery("select * from UrunTablo where uruntrendler='Trend'");
            return View(deger);
        }
        public ActionResult Urunler()
        {
            urunsayisibul();
            var deger=db.UrunTablo.ToList();
            return View(deger);
        }
        [HttpGet]
        public ActionResult CanliAramaPartial(string kelime)
        {
            //AI içerir
            if (string.IsNullOrEmpty(kelime)) return new EmptyResult();

            using (karahisardbEntitiesev1 db = new karahisardbEntitiesev1())
            {
                var products = db.UrunTablo
                    .Where(p => p.urunadi.ToLower().Contains(kelime.ToLower()))
                    .Take(5)
                    .ToList();

                return PartialView("_AramaSonuclariPartial", products);
            }
        }
        [HttpGet]
        public ActionResult MiniSepet()
        {
            //AI içerir
            string musteriAd = Session["kullanici"] as string;
            if (string.IsNullOrEmpty(musteriAd)) return new EmptyResult();

            using (karahisardbEntitiesev1 db = new karahisardbEntitiesev1())
            {
                var musteri = db.MusteriTablo.FirstOrDefault(a => a.musteriadi == musteriAd);
                if (musteri == null) return new EmptyResult();
                var sepetUrunleri = db.SepetTablo
                    .Where(s => s.musteriid == musteri.musteriid)
                    .Join(db.UrunTablo,
                          sepet => sepet.urunid,
                          urun => urun.urunid,
                          (sepet, urun) => urun)
                    .ToList();
                return PartialView("MiniSepet", sepetUrunleri);
            }
        }
        public ActionResult Amouage()
        {
            urunsayisibul();
            var deger = db.UrunTablo.SqlQuery("select * from UrunTablo where urunmarkaadi='Amouage'");
            return View(deger);
        }
        public ActionResult Dior()
        {
            urunsayisibul();
            var deger = db.UrunTablo.SqlQuery("select * from UrunTablo where urunmarkaadi='Dior'");
            return View(deger);
        }
        public ActionResult Creed()
        {
            urunsayisibul();
            var deger = db.UrunTablo.SqlQuery("select * from UrunTablo where urunmarkaadi='Creed'");
            return View(deger);
        }
        public ActionResult Chanel()
        {
            urunsayisibul();
            var deger = db.UrunTablo.SqlQuery("select * from UrunTablo where urunmarkaadi='Chanel'");
            return View(deger);
        }
        public ActionResult Carolina()
        {
            urunsayisibul();
            var deger = db.UrunTablo.SqlQuery("select * from UrunTablo where urunmarkaadi='Carolina Herrera'");
            return View(deger);
        }
        public ActionResult Erkek()
        {
            urunsayisibul();
            var deger = db.UrunTablo.SqlQuery("select * from UrunTablo where uruncinsiyet='Erkek'");
            return View(deger);
        }
        public ActionResult Kadın()
        {
            urunsayisibul();
            var deger = db.UrunTablo.SqlQuery("select * from UrunTablo where uruncinsiyet='Kadın'");
            return View(deger);
        }
        public ActionResult Unisex()
        {
            urunsayisibul();
            var deger = db.UrunTablo.SqlQuery("select * from UrunTablo where uruncinsiyet='Unisex'");
            return View(deger);
        }
        public ActionResult Coksatanlar()
        {
            urunsayisibul();
            var deger = db.UrunTablo.SqlQuery("select * from UrunTablo where uruncoksatanlar='Coksatan'");
            return View(deger);
        }
        public ActionResult Yeniparfumler()
        {
            urunsayisibul();
            var deger = db.UrunTablo.SqlQuery("select * from UrunTablo where urunyeniparfum='Yeni'");
            return View(deger);
        }
        public ActionResult Urunozel(int gelenid)
        {
            urunsayisibul();
            var deger = db.UrunTablo.SqlQuery("select * from UrunTablo where urunid="+Convert.ToString(gelenid));
            return View(deger);
        }
        public ActionResult iletisim()
        {
            urunsayisibul();
            return View();
        }
        public ActionResult hakkimizda()
        {
            urunsayisibul();
            return View();
        }
        public ActionResult Girisyap()
        {
            urunsayisibul();
            var deger = db.UrunTablo.SqlQuery("select * from MusteriTablo");
            return View(deger);
        }
        [HttpPost]
        public ActionResult Girisyap(FormCollection veriler)
        {
                MusteriTablo musteri = new MusteriTablo();
                musteri.musteriadi = veriler["kullanici_adi"];
                musteri.musteriemail = veriler["kullanici_email"];
                musteri.musteriparola = veriler["kullanici_parola"];
                musteri.musteriparolatekrar = veriler["kullanici_parola_tekrar"];
                musteri.musteritel = veriler["kullanici_telno"];
                musteri.musteriadres = veriler["kullanici_adres"];
                musteri.musterisoyadi = veriler["kullanici_soyadi"];
                var deger = db.MusteriTablo.FirstOrDefault(a => a.musteriadi == musteri.musteriadi);
                if (deger == null)
                {
                    if (veriler["kullanici_parola"] != veriler["kullanici_parola_tekrar"])
                    {
                        ViewBag.mesaj = "Girdiğiniz Parolalar birbirleri ile uyuşmuyorlar!";
                        return View();
                    }
                    else
                    {
                        db.MusteriTablo.Add(musteri);
                        db.SaveChanges();
                        Session["kullanici"] = musteri.musteriadi;
                        Session["id"] = musteri.musteriid;
                        return RedirectToAction("index");
                    }
                }
                else
                {
                    ViewBag.mesaj = "Bu Kullanıcı Adı Önceden Alındı";
                    return View();
                }
            
        }
        public ActionResult cikisyap()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult giris(string hata2)
        {
            urunsayisibul();
            ViewBag.hata2 = hata2;
            return View();
        }
        [HttpPost]
        public ActionResult giris(FormCollection fc)
        {
            MusteriTablo uye = new MusteriTablo();
            uye.musteriadi = fc["kullanici_adi"];
            uye.musteriparola = fc["kullanici_parola"];
            uye.musteriparolatekrar = fc["kullanici_parola_tekrar"];
            var deger = db.MusteriTablo.FirstOrDefault(alanlar => alanlar.musteriadi == uye.musteriadi);
            if (deger == null)
            {
                ViewBag.hata2 = "böyle bir kullanıcı yoktur";
                return View();
            }
            else
            {
                var deger2 = db.MusteriTablo.FirstOrDefault(alanlar => alanlar.musteriadi == uye.musteriadi && alanlar.musteriparola==uye.musteriparola);
                if(deger2 == null)
                {
                    ViewBag.hata2 = "şifre yanlış";
                    return View();
                }
                else
                {
                    var deger3=db.MusteriTablo.Where(alanlar=>alanlar.musteriadi==uye.musteriadi).ToList();
                    Session["kullanici"]=uye.musteriadi;
                    Session["id"]=deger3.First().musteriid;
                    Session.Timeout = 1;
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        public ActionResult sepeteekle(int id)
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("giris", "Burakk", new { hata2 = "sepete ürün eklemek için oturum açın" });
            }
            else
            {
                SepetTablo sp = new SepetTablo();
                sp.musteriid = Convert.ToInt16(Session["id"]);
                sp.urunid = Convert.ToInt16(id);
                if (db.SepetTablo.Any(alanlar => alanlar.musteriid == sp.musteriid && alanlar.urunid == sp.urunid))
                {
                    return RedirectToAction("index");
                }
                else
                {
                    db.SepetTablo.Add(sp);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            
        }
        public ActionResult sepet()
        {
            urunsayisibul();
            int uid = Convert.ToInt32(Session["id"]);
            var deger=db.SepetTablo.Include("UrunTablo").Where(alanlar => alanlar.musteriid==uid).ToList();
            return View(deger);
        }
        public ActionResult adminana()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("index");
            }
            var degerler=db.UrunTablo.ToList();
            return View(degerler);
        }
        public ActionResult admingiris()
        {

            return View();
        }
        [HttpPost]
        public ActionResult admingiris(FormCollection veri)
        {
            admin adm=new admin();
            adm.admin1 = veri["adminkad"];
            adm.adminsifre = veri["adminsifre"];
            var bilgi=db.admin.FirstOrDefault(alanlar=> alanlar.admin1==adm.admin1);
            if (bilgi == null)
            {
                ViewBag.hata3 = "böyle bir kullanıcı yoktur";
                return View();
            }
            else
            {
                var bilgi2 = db.admin.FirstOrDefault( alanlar =>alanlar.admin1==adm.admin1&&alanlar.adminsifre ==adm.adminsifre);
                if (bilgi2 == null)
                {
                    ViewBag.hata3 = "şifre yanlış";
                    return View();
                }
                else
                {
                    
                    Session["Admin"] = adm.admin1;
                    return RedirectToAction("adminana");
                    
                }
            }
            return View();
        }
        public ActionResult adminsil(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("index");
            }
            else
            {
                var urun = db.UrunTablo.Find(id);
                db.UrunTablo.Remove(urun);
                db.SaveChanges();
                return RedirectToAction("adminana");
            }
        }
        public ActionResult MiniSepetPartial()
        {
            
            string musteriAd = Session["kullanici"] as string;
            if (string.IsNullOrEmpty(musteriAd)) return new EmptyResult();

            using (karahisardbEntitiesev1 db = new karahisardbEntitiesev1())
            {
                var musteri = db.MusteriTablo.FirstOrDefault(a => a.musteriadi == musteriAd);
                if (musteri == null) return new EmptyResult();
                var sepetUrunleri = db.SepetTablo
                                      .Where(s => s.musteriid == musteri.musteriid)
                                      .Select(s => s.UrunTablo)
                                      .ToList();

                return PartialView("MiniSepetPartial", sepetUrunleri);
            }
        }
        public ActionResult sepettencikar(int id)
        {
            int uyeid = Convert.ToInt32(Session["id"].ToString());
            var deger=db.SepetTablo.FirstOrDefault(alanlar=>alanlar.urunid==id&&alanlar.musteriid==uyeid);
            db.SepetTablo.Remove(deger);
            db.SaveChanges();
            return RedirectToAction("sepet");
        }
        public ActionResult adminguncelle(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("index");
            }
            var deger=db.UrunTablo.Where(alanlar=>alanlar.urunid==id).ToList();
            return View(deger);
        }
        [HttpPost]
        public ActionResult adminguncelle(FormCollection fc)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("index");
            }
            int id = Convert.ToInt16(fc["urunid"]);
            UrunTablo u = db.UrunTablo.Find(id);
            u.urunmarkaadi = fc["urunmarkaadi"];
            u.urunadi = fc["urunadi"];
            u.urunkucukaciklama = fc["urunkucukaciklama"];
            u.urunfiyat = Convert.ToDecimal(fc["urunfiyat"]);
            u.urunnota = fc["urunnota"];
            u.urunaciklama = fc["urunaciklama"];
            u.urunresim = fc["urunresim"];
            db.SaveChanges();
            return RedirectToAction("adminana");
        }
    }
}