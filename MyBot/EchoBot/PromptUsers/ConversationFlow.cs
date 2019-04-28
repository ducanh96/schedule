﻿namespace EchoBot.PromptUsers
{
    public class ConversationFlow
    {
        // Identifies the last question asked.
        public enum Question
        {
            SignIn,
            Name,
            Age,
            Date,
            None, // Our last action did not involve a question.
        }

        // The last question asked.
        public Question LastQuestionAsked { get; set; } = Question.None;
        public DriverQuestions LastDriverQuestion { get; set; } = DriverQuestions.NONE;
    }
    public enum DriverQuestions
    {
        NONE,
        TRANSITION,
        USERNAME,
        PASSWORD
    }

}