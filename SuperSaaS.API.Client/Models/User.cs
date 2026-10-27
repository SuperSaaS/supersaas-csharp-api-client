using System;

namespace SuperSaaS.API.Models
{
    /// <summary>A user of the account.</summary>
    public class User : BaseModel
    {
        /// <summary>Address of the user.</summary>
        public string address { get; set; }

        /// <summary>Country of the user (two-letter ISO code).</summary>
        public string country { get; set; }

        /// <summary>When the user was created.</summary>
        public DateTime created_on { get; set; }

        /// <summary>Credit balance of the user.</summary>
        public string credit { get; set; }

        /// <summary>Email address of the user.</summary>
        public string email { get; set; }

        /// <summary>Custom field 1.</summary>
        public string field_1 { get; set; }

        /// <summary>Custom field 2.</summary>
        public string field_2 { get; set; }

        /// <summary>Your own foreign key for the user.</summary>
        public string fk { get; set; }

        /// <summary>Full name of the user.</summary>
        public string full_name { get; set; }

        /// <summary>The id of the user.</summary>
        public int id { get; set; }

        /// <summary>Mobile phone number of the user.</summary>
        public string mobile { get; set; }

        /// <summary>Login name of the user (often the email address).</summary>
        public string name { get; set; }

        /// <summary>Phone number of the user.</summary>
        public string phone { get; set; }

        /// <summary>Role of the user; see <see cref="Api.Users.Roles"/>.</summary>
        public int role { get; set; }

        /// <summary>Custom field visible to superusers only.</summary>
        public string super_field { get; set; }

        /// <summary>The form attached to the user, if requested.</summary>
        public Form form { get; set; }

        /// <summary>Creates an empty user.</summary>
        public User()
        {
        }
    }
}
