using System.Collections.Generic;

namespace api.ApiResponses
{
    /// <summary>
    /// Note: This messages will be store on Redis
    /// </summary>
    public class ApiInvalidCallsMessage
    {
        public static string InvalidRequestMessages(int ResponseCode)
        {
            Dictionary<int, string> DictionaryMessage = new Dictionary<int, string>();

            DictionaryMessage.Add(1, "Your request is invalid.");

            return DictionaryMessage[ResponseCode];
        }

        public static string ServerErrorMessage(int ResponseCode)
        {
            Dictionary<int, string> DictionaryMessage = new Dictionary<int, string>();

            DictionaryMessage.Add(1, "We couldn't proceed your request. Please wait a few minutes before you try again");

            return DictionaryMessage[ResponseCode];
        }
    }
}
