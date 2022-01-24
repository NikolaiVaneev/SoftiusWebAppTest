using SoftiusWebAppTest.Models;
using System.Collections.Generic;

namespace SoftiusWebAppTest.Service
{
    public class InputDataConverter
    {
        /// <summary>
        /// Извлечение и проверка на корректность входных параметров
        /// </summary>
        /// <param name="studentCount">Общее количество студентов</param>
        /// <param name="studentsMaxMessage">Максимальное количесвто сообщений</param>
        public InputDataConverter(int studentCount, string studentsMaxMessage)
        {
            _studentCount = studentCount;
            _studentsMaxMessage = studentsMaxMessage;
            ExtractData();
        }

        private readonly int _studentCount;
        private readonly string _studentsMaxMessage;

        /// <summary>
        /// Успешность извлечения данных
        /// </summary>
        public bool Successful { get; private set; }
        /// <summary>
        /// Список студентов
        /// </summary>
        public List<Student> Students { get; private set; }
        /// <summary>
        /// Описание возникшей проблемы
        /// </summary>
        public string ErrorDescription { get; private set; }
        private void ExtractData()
        {
            Students = new();
            // Удаляем лишние пробелы.
            string inputStr = System.Text.RegularExpressions.Regex.Replace(_studentsMaxMessage, @"\s+", " ");

            string[] maxMessageArr = inputStr.Trim().Split(" ");

            if (maxMessageArr.Length < _studentCount)
            {
                this.Successful = false;
                ErrorDescription = "Массив максимальных сообщений меньше, чем общее количество студентов";
                return;
            }

            for (int i = 0; i < _studentCount; i++)
            {
                bool convertResult = int.TryParse(maxMessageArr[i], out int maxMessagePerDay);
                if (!convertResult)
                {
                    this.Successful = false;
                    ErrorDescription = "Не корректные значения в массиве максимальных сообщений";
                    return;
                }
                else
                {
                    this.Students.Add(new Student(i + 1, maxMessagePerDay));
                }
            }

            if (Students.Count != _studentCount)
            {
                this.Successful = false;
                ErrorDescription = "Количество студентов не соответствует количеству элементов в массиве";
                return;
            }
            this.Successful = true;
        }
    }
}
