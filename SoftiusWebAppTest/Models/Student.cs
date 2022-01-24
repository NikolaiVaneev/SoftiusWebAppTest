using System.Collections.Generic;

namespace SoftiusWebAppTest.Models
{
    public class Student
    {
        public Student(int id, int maxMessagesPerDay)
        {
            Id = id;
            MaxMessagesPerDay = maxMessagesPerDay;
            OutgoingMessages = new();
            IncomingMessages = new();
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// Максимум сообщений в сутки
        /// </summary>
        public int MaxMessagesPerDay { get; }
        /// <summary>
        /// Исходящие сообщения
        /// </summary>
        public List<Message> OutgoingMessages { get; private set; }
        /// <summary>
        /// Входящие сообщения
        /// </summary>
        public List<Message> IncomingMessages { get; private set; }

        /// <summary>
        /// Разослать сообщения
        /// </summary>
        /// <param name="students"></param>
        public void SendOutMessages(List<Student> students)
        {
            foreach (var student in students)
            {
                if (student.Equals(this))
                    continue;

                if (this.OutgoingMessages.Count == MaxMessagesPerDay)
                {
                    return;
                }

                if (student.IncomingMessages.Count == 0 && this.IncomingMessages.Count > 0)
                {
                    Message message = new()
                    {
                        From = this,
                        To = student
                    };

                    student.IncomingMessages.Add(message);
                    this.OutgoingMessages.Add(message);
                }
            }
        }
    }
}
