#region License

// /*
// Transformalize - Replicate, Transform, and Denormalize Your Data...
// Copyright (C) 2013 Dale Newman
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// */

#endregion

using System.Configuration;

namespace Transformalize.Configuration {
    public class ActionConfigurationElement : ConfigurationElement {

        private const string MODE = "mode";
        private const string FILE = "file";
        private const string ACTION = "action";
        private const string CONNECTION = "connection";
        private const string METHOD = "method";
        private const string URL = "url";
        private const string MODES = "modes";
        private const string FROM = "from";
        private const string TO = "to";
        private const string CC = "cc";
        private const string BCC = "bcc";
        private const string SUBJECT = "subject";
        private const string PASSWORD = "password";
        private const string USERNAME = "username";
        private const string PORT = "port";
        private const string HOST = "host";
        private const string ARGUMENTS = "arguments";
        private const string ENABLE_SSL = "enable-ssl";
        private const string HTML = "html";

        [ConfigurationProperty(ACTION, IsRequired = true)]
        public string Action {
            get { return this[ACTION] as string; }
            set { this[ACTION] = value; }
        }

        [ConfigurationProperty(FILE, IsRequired = false)]
        public string File {
            get { return this[FILE] as string; }
            set { this[FILE] = value; }
        }

        [ConfigurationProperty(MODE, IsRequired = false, DefaultValue = "")]
        public string Mode {
            get { return this[MODE] as string; }
            set { this[MODE] = value; }
        }

        [ConfigurationProperty(CONNECTION, IsRequired = false, DefaultValue = "")]
        public string Connection {
            get { return this[CONNECTION] as string; }
            set { this[CONNECTION] = value; }
        }

        [ConfigurationProperty(METHOD, IsRequired = false, DefaultValue = "get")]
        public string Method {
            get { return this[METHOD] as string; }
            set { this[METHOD] = value; }
        }

        [ConfigurationProperty(URL, IsRequired = false)]
        public string Url {
            get { return this[URL] as string; }
            set { this[URL] = value; }
        }

        [ConfigurationProperty(FROM, IsRequired = false)]
        public string From {
            get { return this[FROM] as string; }
            set { this[FROM] = value; }
        }

        [ConfigurationProperty(TO, IsRequired = false)]
        public string To {
            get { return this[TO] as string; }
            set { this[TO] = value; }
        }

        [ConfigurationProperty(CC, IsRequired = false)]
        public string Cc {
            get { return this[CC] as string; }
            set { this[CC] = value; }
        }

        [ConfigurationProperty(BCC, IsRequired = false)]
        public string Bcc {
            get { return this[BCC] as string; }
            set { this[BCC] = value; }
        }

        [ConfigurationProperty(ARGUMENTS, IsRequired = false)]
        public string Arguments {
            get { return this[ARGUMENTS] as string; }
            set { this[ARGUMENTS] = value; }
        }

        [ConfigurationProperty(ENABLE_SSL, IsRequired = false, DefaultValue = false)]
        public bool EnableSsl {
            get { return (bool) this[ENABLE_SSL]; }
            set { this[ENABLE_SSL] = value; }
        }

        [ConfigurationProperty(HTML, IsRequired = false, DefaultValue = true)]
        public bool Html {
            get { return (bool) this[HTML]; }
            set { this[HTML] = value; }
        }

        [ConfigurationProperty(USERNAME, IsRequired = false, DefaultValue = "")]
        public string Username {
            get { return this[USERNAME] as string; }
            set { this[USERNAME] = value; }
        }

        [ConfigurationProperty(PASSWORD, IsRequired = false, DefaultValue = "")]
        public string Password {
            get { return this[PASSWORD] as string; }
            set { this[PASSWORD] = value; }
        }

        [ConfigurationProperty(HOST, IsRequired = false, DefaultValue = "")]
        public string Host {
            get { return this[HOST] as string; }
            set { this[Host] = value; }
        }

        [ConfigurationProperty(PORT, IsRequired = false, DefaultValue = 25)]
        public int Port {
            get { return (int) this[PORT]; }
            set { this[PORT] = value; }
        }

        [ConfigurationProperty(SUBJECT, IsRequired = false, DefaultValue = "")]
        public string Subject {
            get { return this[SUBJECT] as string; }
            set { this[SUBJECT] = value; }
        }

        [ConfigurationProperty(MODES)]
        public ModeElementCollection Modes {
            get { return this[MODES] as ModeElementCollection; }
        }

        public override bool IsReadOnly() {
            return false;
        }
    }
}