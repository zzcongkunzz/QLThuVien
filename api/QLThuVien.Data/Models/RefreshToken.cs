﻿using QLThuVien.Data.Models;

namespace QLThuVien.Business.Models;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public string Token { get; set; }
    public string JwtId { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime DateExpire { get; set; }
    
    
}