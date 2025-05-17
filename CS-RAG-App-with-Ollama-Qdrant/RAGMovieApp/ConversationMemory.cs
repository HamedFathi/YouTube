using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGMovieApp
{
    public class ConversationMemory
    {
        private readonly List<string> _messages = new();
        private readonly int _maxHistoryItems;

        public ConversationMemory(int maxHistoryItems = 5)
        {
            _maxHistoryItems = maxHistoryItems;
        }

        public void AddMessage(string message)
        {
            _messages.Add(message);

            if (_messages.Count > _maxHistoryItems * 2)
            {
                _messages.RemoveRange(0, 2);
            }
        }

        public IReadOnlyList<string> GetMessages()
        {
            return _messages.AsReadOnly();
        }
    }
}
