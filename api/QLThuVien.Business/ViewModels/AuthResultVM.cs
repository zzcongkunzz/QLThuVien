﻿namespace QLThuVien.Business.ViewModels;

public class AuthResultVm
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    public UserVm? UserInformation { get; set; }
}