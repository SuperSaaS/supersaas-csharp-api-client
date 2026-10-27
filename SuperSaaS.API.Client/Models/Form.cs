using System;

namespace SuperSaaS.API.Models
{
    /// <summary>A form filled in by a user, based on a template form (super form).</summary>
    public class Form : BaseModel
    {
        /// <summary>The form contents.</summary>
        public Object content { get; set; }

        /// <summary>When the form was created.</summary>
        public DateTime created_on { get; set; }

        /// <summary>Whether the form has been deleted.</summary>
        public bool deleted { get; set; }

        /// <summary>The id of the form.</summary>
        public int id { get; set; }

        /// <summary>The id of the appointment the form is attached to.</summary>
        public int reservation_process_id { get; set; }

        /// <summary>The id of the template form (super form).</summary>
        public int super_form_id { get; set; }

        /// <summary>When the form was last updated.</summary>
        public DateTime updated_on { get; set; }

        /// <summary>The id of the user who filled in the form.</summary>
        public int user_id { get; set; }

        /// <summary>Creates an empty form.</summary>
        public Form()
        {
        }
    }
}
