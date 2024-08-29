﻿using System.ComponentModel.DataAnnotations;

namespace QLThuVien.Business.ViewModels;

public class LoginVm
{
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}