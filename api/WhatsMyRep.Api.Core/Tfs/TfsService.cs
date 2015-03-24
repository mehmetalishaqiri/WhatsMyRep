/*   
    The MIT License (MIT)
    
    Copyright (C) 2011 Mehmetali Shaqiri (mehmetalishaqiri@gmail.com)
 
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE. 
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Microsoft.SqlServer.Server;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using WhatsMyRep.Api.Common;

namespace WhatsMyRep.Api.Core.Tfs
{
    public class TfsService : ITfsService
    {
        /// <summary>
        /// TFS Team Project Collection Url
        /// </summary>
        private readonly string _tfsCollection = ConfigurationManager.AppSettings["TfsCollection"];

        private readonly string _projectName = ConfigurationManager.AppSettings["ProjectName"];


        public IEnumerable<WorkItemModel> GetWorkItems()
        {
            var tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(_tfsCollection));

            tfs.EnsureAuthenticated();

            var workItemStore = tfs.GetService<WorkItemStore>();

            var workItems = new List<WorkItemModel>();


            var query = String.Format(@"SELECT [System.Id], [System.WorkItemType]," +
                                  " [System.State], [System.AssignedTo], [System.Title]" +
                                  " FROM WorkItems" +
                                  " WHERE [System.TeamProject] = '{0}'", _projectName);

            var items = workItemStore.Query(query).Cast<WorkItem>().Select(wi => new
            {
                wi.Id,
                Type = wi.Type.Name,
                wi.Title,
                wi.State,
                wi.Fields
            }).ToList();

            foreach (var item in items)
            {
                string assignedTo = String.Empty, createdBy = String.Empty;
                DateTime createdOn;

                foreach (Field field in item.Fields)
                {
                    switch (field.Name)
                    {
                        case "Assigned To":
                            assignedTo = field.Value.ToString();
                            break;
                        case "Created By":
                            createdBy = field.Value.ToString();
                            break;
                    }
                }
                workItems.Add(new WorkItemModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    State = item.State,
                    Type = item.Type,
                    AssignedTo = assignedTo,                    
                    CreatedBy = createdBy
                });
            }

            return workItems;
        }
        
    }
}
