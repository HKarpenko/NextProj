﻿namespace Notification.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string message);
    }
}