namespace EchoBot.PromptUsers
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
        public StatusInvoiceQuestion LastStatusInvoiceQuestion { get; set; } = StatusInvoiceQuestion.NONE;
    }
    public enum DriverQuestions
    {
        NONE,
        TRANSITION,
        
        USERNAME,
        PASSWORD,

        INVOICE_CHOOSE,
        INVOICE_NOW, // MAP ĐẾN PHẦN 
        INVOICE_PAST,

        #region chuc nag dang nhap hoac reset password
        CHOOSE_LOGIN_RESET,
        LOGIN,
        CHANGE_PASSWORD,
        #endregion

        // phan click detail Customer
        #region funtional for Invoice_now
        CHOOSE_FUNCTIONAL_CUSTOMER,

        // items -> 
        CHOOSE_STATUS_INVOICE
        
        #endregion
    }

    public enum StatusInvoiceQuestion
    {
        NONE,
        //USERNAME,
        //PASSWORD,
        //PASSWORD_NEW
        INPUT_INVOICE_CODE
    }
}
