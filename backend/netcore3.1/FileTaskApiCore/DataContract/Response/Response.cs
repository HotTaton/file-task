using System.Collections.Generic;
using System.Linq;

namespace FileTaskApiCore.DataContract.Response
{
    /// <summary>
    /// Инкапсулирует ответ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        /// <summary>
        /// Сущность, связанная с ответом сервиса
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// Сообщения
        /// </summary>
        public IList<Message> Messages { get; set; } = new List<Message>();

        public bool HasErrors { get => Messages.Any(message => message.Type == MessageType.Error); }

        /// <summary>
        /// Добавляет ошибку в список сообщений
        /// </summary>
        /// <param name="text">Текст ошибки</param>
        public void AddError(string text) => Messages.Add(new Message(text, MessageType.Error));
    }
}
