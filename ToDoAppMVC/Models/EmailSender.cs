using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoAppMVC.Models
{
    public class EmailSender : IEmailSender
    {
        public System.Threading.Tasks.Task SendEmailAsync(string email, string subject, string message)
        {
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
