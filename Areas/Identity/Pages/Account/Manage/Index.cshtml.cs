// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rocastone.Models;
using System.ComponentModel.DataAnnotations;

namespace Rocastone.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Display(Name = "Usuario")]
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {

            [Phone]
            [Display(Name = "Phone number")]
            public string Telefono { get; set; }
            [Display(Name = "Nombre")]
            public string Nombre { get; set; }
            [Display(Name = "Apellido")]
            public string Apellido { get; set; }
            [Display(Name = "Teléfono")]
            public byte[] ProfilePicture { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var telefono = user.Telefono;
            var nombre = user.Nombre;
            var apellido = user.Apellido;
            var profilePicture = user.ProfilePicture;

            Username = userName;

            Input = new InputModel
            {
                Telefono = telefono,
                Nombre = nombre,
                Apellido = apellido,
                ProfilePicture = profilePicture
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            var nombre = user.Nombre;
            var apellido = user.Apellido;
            var telefono = user.Telefono;




            if (Input.Nombre != nombre)
            {
                user.Nombre = Input.Nombre;
                await _userManager.UpdateAsync(user);
            }

            if (Input.Apellido != apellido)
            {
                user.Apellido = Input.Apellido;
                await _userManager.UpdateAsync(user); ;
            }

            if (Input.Telefono != telefono)
            {
                user.Telefono = Input.Telefono;
                await _userManager.UpdateAsync(user); ;
            }

            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    user.ProfilePicture = dataStream.ToArray();
                }
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
