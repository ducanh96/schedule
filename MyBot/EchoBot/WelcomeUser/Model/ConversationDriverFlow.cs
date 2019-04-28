using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.WelcomeUser.Model
{
    public class ConversationDriverFlow
    {
        public enum QuestionDriver
        {
            UserName,
            Password,
            None, // Our last action did not involve a question.
        }
        public QuestionDriver LastQuestionAsked { get; set; } = QuestionDriver.None;
    }
}
