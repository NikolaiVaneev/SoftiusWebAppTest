using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using SoftiusWebAppTest.Models;
using SoftiusWebAppTest.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace SoftiusWebAppTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IToastNotification _toastNotification;

        public HomeController(ILogger<HomeController> logger, IToastNotification toastNotification)
        {
            _logger = logger;
            _toastNotification = toastNotification;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public string Calc(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                _toastNotification.AddErrorToastMessage("Пустое входное значение");
                return "-1";
            }
            string[] lines = input.Split(
                new string[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None);

            if (lines.Length < 2)
            {
                _toastNotification.AddWarningToastMessage("Входное значение менее двух строк");
                return "-1";
            }

            if (int.TryParse(lines[0], out int studentsCount))
            {
                var Data = new InputDataConverter(studentsCount, lines[1]);
                if (Data.Successful && Data.Students.Count > 0)
                {
                    // HACK : Добавляем сообщение первому пользователю
                    Data.Students[0].IncomingMessages.Add(new Message());

                    foreach (var student in Data.Students)
                    {
                        student.SendOutMessages(Data.Students);
                    }
                    return GetStudentsStatistic(Data.Students);
                }
                else
                {
                    _toastNotification.AddWarningToastMessage(Data.ErrorDescription);
                    return "-1";
                }    
                
            }
            else
            {
                _toastNotification.AddWarningToastMessage("Не верное количество студентов");
                return "-1";
            }
        }


        private string GetStudentsStatistic(List<Student> students)
        {
            StringBuilder result = new();

            var allOutgoingMessages = students.Where(s => s.OutgoingMessages.Count > 0).SelectMany(s => s.OutgoingMessages);

            if (allOutgoingMessages.Count() == 0)
            {
                _toastNotification.AddWarningToastMessage("Рассылка не была произведена. Проверьте входные параметры");
                return "-1";
            }


            if (allOutgoingMessages.Count() < students.Count - 1)
            {
                _toastNotification.AddWarningToastMessage("Не все студенты были уведомлены. Проверьте входные параметры");
                return "-1";
            }

                result.AppendLine(allOutgoingMessages.Count().ToString());
            foreach (var message in allOutgoingMessages)
            {
                result.AppendLine($"{message.From.Id} {message.To.Id}");
            }
            _toastNotification.AddSuccessToastMessage("Успешно!");
            return result.ToString();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
