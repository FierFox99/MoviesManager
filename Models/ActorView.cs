using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoviesManager.Models
{
    public enum SexeType { Masculin, Féminin, Autre }
    public class ActorView
    {
        public int Id { get; set; }

        [Display(Name = "Nom")]
        [Required(ErrorMessage = "Requis")]
        [Remote("ActorNameAvailable", "Users", HttpMethod = "POST", ErrorMessage = "Ce nom d'acteur existe déja.")]
        public string Name { get; set; }

        public int CountryId { get; set; }
        [Display(Name = "Date de naissance")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Genre")]
        public SexeType Sexe { get; set; }

        [Display(Name = "Pays")]
        [Required(ErrorMessage = "Requis")]
        public string CountryName { get; set; }

        [Display(Name = "Photo")]
        public string PhotoImageData { get; set; }

        [Display(Name = "Photo")]
        public string PhotoId { get; set; }
        public List<Casting> Castings { get; set; }
        public Country Country { get; set; }

        private ImageGUIDReference PhotoReference { get; set; }
        public void InitPhotoManagement()
        {
            PhotoReference = new ImageGUIDReference(@"/ImagesData/Actors/", @"NoPhoto.png");
            PhotoReference.MaxSize = 1024;
            PhotoReference.HasThumbnail = true;
        }
        public String GetPhotoURL(bool thumbnail = false)
        {
            return PhotoReference.GetURL(PhotoId, thumbnail);
        }
        public void SavePhoto()
        {
            PhotoId = PhotoReference.SaveImage(PhotoImageData, PhotoId);
        }
        public void RemovePhoto()
        {
            PhotoReference.Remove(PhotoId);
        }

        public ActorView()
        {
            InitPhotoManagement();
        }

        public Actor ToActor()
        {
            return new Actor
            {
                Id = this.Id,
                Name = this.Name,
                Sexe = (int)this.Sexe,
                CountryId = this.CountryId,
                BirthDate = this.BirthDate,
                PhotoId = this.PhotoId
            };
        }

        public void CopyToActor(Actor actor)
        {
            actor.Id = Id;
            actor.Name = Name;
            actor.Sexe = (int)Sexe;
            actor.CountryId = CountryId;
            actor.BirthDate = BirthDate;
            PhotoId = PhotoId;
        }
    }
}