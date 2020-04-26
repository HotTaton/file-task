namespace FileTaskApiCore.DataContract.Response
{
    /// <summary>
    /// Сообщение
    /// </summary>
    public struct Message
    {
        public Message(string text, MessageType messageType)
        {
            Type = messageType;
            Text = text;
        }

        /// <summary>
        /// Тип сообщения
        /// </summary>
        public MessageType Type { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; }
    }
}
