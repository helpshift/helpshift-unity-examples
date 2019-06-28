using System;
using System.Collections.Generic;
using HSMiniJSON;

namespace Helpshift
{
    /// <summary>
    /// Helpshift JSON utility to convert JSON params to model objects.
    /// </summary>
    public static class HelpshiftJSONUtility
    {
        /// <summary>
        /// Gets the HelpshiftUser model from the give JSON string.
        /// This api is to be used in "authenticationFailed" callback for deserializing to HelpshiftUser.
        /// </summary>
        /// <returns>The HelpshiftUser object.</returns>
        /// <param name="serializedJSONHelpshiftUser">Serialized JSON helpshift user.</param>
        public static HelpshiftUser getHelpshiftUser(string serializedJSONHelpshiftUser)
        {
            Dictionary<string, object> serialzedDataMap = (Dictionary<string, object>)Json.Deserialize(serializedJSONHelpshiftUser);

            string identifier = Convert.ToString(serialzedDataMap["identifier"]);
            string email = Convert.ToString(serialzedDataMap["email"]);
            string authToken = Convert.ToString(serialzedDataMap["authToken"]);
            string name = Convert.ToString(serialzedDataMap["name"]);

            HelpshiftUser.Builder builder = new HelpshiftUser.Builder(identifier, email);
            builder.setName(name);
            builder.setAuthToken(authToken);
            return builder.build();
        }

        /// <summary>
        /// Gets the auth failure reason for login from the given JSON string.
        /// This api is to be used in "authenticationFailed" callback for deserializing to HelpshiftAuthFailureReason.
        /// </summary>
        /// <returns>The auth failure reason.</returns>
        /// <param name="serializedJSONAuthFailure">Serialized JSON auth failure.</param>
        public static HelpshiftAuthFailureReason getAuthFailureReason(string serializedJSONAuthFailure)
        {
            Dictionary<string, object> serialzedDataMap = (Dictionary<string, object>)Json.Deserialize(serializedJSONAuthFailure);

            string reason = Convert.ToString(serialzedDataMap["authFailureReason"]);

            HelpshiftAuthFailureReason authFailureReason = HelpshiftAuthFailureReason.INVALID_AUTH_TOKEN;

            if ("0".Equals(reason))
            {
                authFailureReason = HelpshiftAuthFailureReason.AUTH_TOKEN_NOT_PROVIDED;
            }
            else if ("1".Equals(reason))
            {
                authFailureReason = HelpshiftAuthFailureReason.INVALID_AUTH_TOKEN;
            }

            return authFailureReason;
        }
    }
}

