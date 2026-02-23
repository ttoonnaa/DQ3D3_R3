using System;
using System.Collections.Generic;

namespace SrLib
{
    public class SrException : Exception
    {
        public SrException(string message) : base(message)
        {
        }
        public SrException(string message, Exception e) : base(message, e)
        {
        }
        
        /// <summary>
        /// ネストされたメッセージを取得する
        /// </summary>
        public List<string> GetNestedMessages()
        {
            var messages = new List<string>();
            messages.Add(Message);
            var inner = InnerException;
            while (inner != null)
            {
                messages.Add(inner.Message);
                inner = inner.InnerException;
            }

            return messages;
        }
    }
}
