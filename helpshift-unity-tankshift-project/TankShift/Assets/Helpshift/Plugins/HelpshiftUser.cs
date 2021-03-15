using System;

namespace Helpshift
{
    /// <summary>
    /// Class to define a Helpshift user.
    /// </summary>
    public class HelpshiftUser
    {
        public readonly string identifier;
        public readonly string email;
        public readonly string name;
        public readonly string authToken;

        private HelpshiftUser(string identifier, string email, string name, string authToken)
        {
            this.identifier = identifier;
            this.email = email;
            this.name = name;
            this.authToken = authToken;
        }

        /// <summary>
        /// Builder class for HelpshiftUser model.
        /// </summary>
        public sealed class Builder
        {
            private string identifier = null;
            private string email = null;
            private string name;
            private string authToken;

            public Builder(string identifier, string email)
            {
                this.email = email;
                this.identifier = identifier;
            }

            public Builder setName(string name)
            {
                this.name = name;
                return this;
            }

            public Builder setAuthToken(string authToken)
            {
                this.authToken = authToken;
                return this;
            }

            public HelpshiftUser build()
            {
                return new HelpshiftUser(identifier, email, name, authToken);
            }
        }
    }
}