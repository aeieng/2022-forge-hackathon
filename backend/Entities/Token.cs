﻿namespace Backend.Entities;

public record Token(string AccessToken, DateTime ExpiresAt);