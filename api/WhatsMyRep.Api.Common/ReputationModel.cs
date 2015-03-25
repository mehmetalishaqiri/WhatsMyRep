using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsMyRep.Api.Shared.Helpers;

namespace WhatsMyRep.Api.Common
{
    public class ReputationModel
    {
        private string _developer;
        

        public string Developer
        {
            get { return !String.IsNullOrEmpty(_developer) ? _developer : "Fairy"; }
            set { _developer = value; }
        }

        public string Email { get; set; }

        public string Gravatar
        {
            get { return String.Format("http://www.gravatar.com/avatar/{0}?s=400&d=mm&r=g", GravatarHelper.HashEmailForGravatar(Email)); }
        }

        public int BugsResolved { get; set; }

        public int CompletedTasks { get; set; }

        public int Total
        {
            get { return CompletedTasks + BugsResolved; }
        }
    }
}
