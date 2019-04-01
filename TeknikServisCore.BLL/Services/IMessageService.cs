using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeknikServisCore.Models.Enums;
using Microsoft.AspNetCore.Identity;
using TeknikServisCore.Models.ViewModels;

namespace TeknikServisCore.BLL.Services
{
    public interface IMessageService
    {
        MessageStates MessageStates { get; }

        Task SendAsync(MessageViewModel message, params string[] contacts);


    }
}
