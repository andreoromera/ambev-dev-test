﻿namespace Ambev.Dev.Test.Domain.Auth;

public class SignInResponse
{
    public string Token { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}
