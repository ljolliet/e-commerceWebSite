﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Projet;

namespace Projet.Controllers
{
    public class EnregistrementsController : Controller
    {
        private Classique_Web_2017Entities db = new Classique_Web_2017Entities();

        // GET: Enregistrements
        public ActionResult Index()
        {
            return View(db.Enregistrement.ToList());
        }
        public ActionResult Extrait(int? id)
        {
            var enregistrement = db.Enregistrement.Single(g => g.Code_Morceau == id);
            return File(enregistrement.Extrait, "mp3");
        }
        public ActionResult listeEnregistrementsFromAlbum(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var enregistrement = (from enr in db.Enregistrement
                                  join comp in db.Composition_Disque on enr.Code_Morceau equals comp.Code_Morceau
                                  join dis in db.Disque on comp.Code_Disque equals dis.Code_Disque
                                  join alb in db.Album on dis.Code_Album equals alb.Code_Album
                          where alb.Code_Album == id
                          select enr).Distinct();
            var data = db.Album.Single(g => g.Code_Album == id);
            ViewBag.Titre_Album = data.Titre_Album;
            ViewBag.Code = id; 
            return View(enregistrement);
        }
    }
}