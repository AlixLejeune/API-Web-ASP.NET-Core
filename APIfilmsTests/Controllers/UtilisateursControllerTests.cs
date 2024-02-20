using Microsoft.VisualStudio.TestTools.UnitTesting;
using API_Web_ASP.NET_Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Web_ASP.NET_Core.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

namespace API_Web_ASP.NET_Core.Controllers.Tests
{
    [TestClass()]
    public class UtilisateursControllerTests
    {
        static FilmRatingsDBContext context = new FilmRatingsDBContext();
        UtilisateursController controller = new UtilisateursController(context);

        [TestMethod()]
        public void UtilisateursControllerTest()
        {
            UtilisateursController testCtrl = new UtilisateursController(context);
            Assert.IsNotNull(testCtrl);
        }

        [TestMethod()]
        public void GetUtilisateursTest()
        {
            CollectionAssert.AreEquivalent(context.Utilisateurs.ToList(), controller.GetUtilisateurs().Result.Value.ToList());
        }

        [TestMethod()]
        public void GetUtilisateurByIdTest()
        {
            Assert.AreEqual(context.Utilisateurs.Where(c => c.UtilisateurId == 1).FirstOrDefault(),controller.GetUtilisateurById(1).Result.Value);
        }

        [TestMethod()]
        public void GetUtilisateurByIdFail()
        {
            Assert.IsInstanceOfType(controller.GetUtilisateurById(-2).Result.Result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void GetUtilisateurByEmailTest()
        {
            Assert.AreEqual(context.Utilisateurs.FirstOrDefault(c => c.Mail == "rrichings1@naver.com"), controller.GetUtilisateurByEmail("rrichings1@naver.com").Result.Value);

        }

        [TestMethod()]
        public void PutUtilisateurTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        [ExpectedException(typeof(AggregateException))]
        public void PostUtilisateurFailContrainteDB()
        {
            Utilisateur userAtester = new Utilisateur()
            {
                Nom = "MACHIN",
                Prenom = "Luc"
            };
            var resultat = controller.PostUtilisateur(userAtester).Result.Result;
        }

        [TestMethod()]
        public void PostUtilisateurFailContrainteModel()
        {
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            Utilisateur userAtester = new Utilisateur()
            {
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "1",
                Mail = "machin" + chiffre + "@gmail.com",
                Pwd = "1",
                Rue = "",
                CodePostal = "1",
                Ville = "",
                Pays = "",
                Latitude = null,
                Longitude = null
            };
            string PhoneRegex = @"^0[0-9]{9}$";
            Regex regex = new Regex(PhoneRegex);
            if (!regex.IsMatch(userAtester.Mobile))
            {
                controller.ModelState.AddModelError("Mobile", "Le n° de mobile doit contenir 10 chiffres"); //On met le même message que dans la classe Utilisateur.
            }
            Assert.IsInstanceOfType(controller.PostUtilisateur(userAtester).Result.Result,typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Postutilisateur_ModelValidated_CreationOK()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            // Le mail doit être unique donc 2 possibilités :
            // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
            // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE de l’API ou remove du DbSet.
            Utilisateur userAtester = new Utilisateur()
            {
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "machin" + chiffre + "@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            // Act
            var result = controller.PostUtilisateur(userAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout
            // Assert
            Utilisateur? userRecupere = context.Utilisateurs.Where(u => u.Mail.ToUpper() ==
            userAtester.Mail.ToUpper()).FirstOrDefault(); // On récupère l'utilisateur créé directement dans la BD grace à son mail unique
            // On ne connait pas l'ID de l’utilisateur envoyé car numéro automatique.
            // Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 users
            userAtester.UtilisateurId = userRecupere.UtilisateurId;
            Assert.AreEqual(userRecupere, userAtester, "Utilisateurs pas identiques");
            var delete = controller.DeleteUtilisateur(context.Utilisateurs.FirstOrDefault(c => c.Mail == userAtester.Mail).UtilisateurId).Result;
        }

        [TestMethod()]
        public void DeleteUtilisateurTest()
        {
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            Utilisateur userAtester = new Utilisateur()
            {
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "machin" + chiffre + "@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            context.Add(userAtester);
            context.SaveChanges();
            int newId = context.Utilisateurs.First(c => c.Equals(userAtester)).UtilisateurId;
            var resultat = controller.DeleteUtilisateur(newId).Result;
            Assert.IsInstanceOfType(resultat, typeof(NoContentResult));
            Assert.IsNull(context.Utilisateurs.FirstOrDefault(c => c.UtilisateurId == newId));
        }
    }
}