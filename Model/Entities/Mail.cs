﻿namespace Pickfc.Model.Entities
{
    public class Mail
    {
        public string From { get; set; } = string.Empty; 
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
