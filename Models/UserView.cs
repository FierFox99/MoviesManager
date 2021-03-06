using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoviesManager.Models
{
    public class UserView
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Requis")]
        [Display(Name = "Nom d'usager")]
        [Remote("UserNameAvailable", "Users", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "Ce nom d'usager n'est pas disponible.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Requis")]
        [Display(Name = "Nom")]
        public string FullName { get; set; }

        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        public bool Admin { get; set; }

        [Required(ErrorMessage = "Requis")]
        [Display(Name = "Nouveau mot de passe")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirmation")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "La confirmation ne correspond pas.")]
        public string Confirmation { get; set; }

        public string AvatarId { get; set; }
        private ImageGUIDReference AvatarReference { get; set; }

        [Display(Name = "Avatar")]
        public string AvatarImageData { get; set; }
            
        public void InitAvatarManagement()
        {
            AvatarReference = new ImageGUIDReference(@"/ImagesData/Avatars/", @"no_avatar.png");
            AvatarReference.MaxSize = 512;
            AvatarReference.HasThumbnail = false;
        }
        public UserView()
        {
            InitAvatarManagement();
        }

        public String GetAvatarURL()
        {
            return AvatarReference.GetURL(AvatarId, false);
        }
        public void SaveAvatar()
        {
            AvatarId = AvatarReference.SaveImage(AvatarImageData, AvatarId);
        }
        public void RemoveAvatar()
        {
            AvatarReference.Remove(AvatarId);
        }
        public User ToUser()
        {   // Attention cette méthode retourne une instance de User
            // qui ne sera pas utilisable en tant cible de modification
            // car il ne contiendra pas un object context
            // et l'instruction suivante générera une exception
            // DB.Entry(user).State = EntityState.Modified;
            return new User()
            {
                Id = this.Id,
                AvatarId = this.AvatarId,
                UserName = this.UserName,
                FullName = this.FullName,
                Password = this.Password,
                Admin = this.Admin
            };
        }
        public void CopyToUser(User user)
        { // Utilisez cette fonction pour copier un UserView dans en User
            user.Id = Id;
            user.AvatarId = AvatarId;
            user.UserName = UserName;
            user.FullName = FullName;
            user.Password = Password;
            user.Admin = Admin;
        }
        public void CopyToUserView(UserView user)
        { // Utilisez cette fonction pour copier un UserView dans un autre UserView
            user.Id = Id;
            user.AvatarId = AvatarId;
            user.UserName = UserName;
            user.FullName = FullName;
            user.Password = Password;
            user.Admin = Admin;
        }
    }
    public class LoginView
    {
        [Required(ErrorMessage = "Requis")]
        [Display(Name = "Nom d'usager")]
        [Remote("UserNameExist", "Users", HttpMethod = "POST", ErrorMessage = "Introuvable...")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Requis")]
        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        [Remote("PasswordValid", "Users", HttpMethod = "POST", AdditionalFields = "UserName", ErrorMessage = "Incorrecte...")]
        public string Password { get; set; }

    }


}