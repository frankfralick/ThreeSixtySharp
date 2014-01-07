using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using ThreeSixtySharp.Objects;

namespace ThreeSixtySharp
{
    public class Field
    {
        const string BaseUrl = "https://manage.velasystems.com";
        private string Username { get; set; }
        private string Password { get; set; }


        /// <summary>
        /// Constructor for ThreeSixtySharpBase.
        /// </summary>
        /// <param name="username">User's username.</param>
        /// <param name="password">User's password.</param>
        public Field(string username, string password)
        {
            Username = username;
            Password = password;
        }


        /// <summary>
        /// Generic execute method for use by other methods returning a ThreeSixtySharp.Objects.XXXXXX instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public T Execute<T>(RestRequest request) where T : new()
        {   
            var client = new RestClient();
            client.BaseUrl = BaseUrl;
            var response = client.Execute<T>(request);
            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            return response.Data;
        }


        /// <summary>
        /// Execute method for RestRequest.
        /// </summary>
        /// <param name="request"></param>
        public void Execute(RestRequest request) 
        {
            var client = new RestClient();
            client.BaseUrl = BaseUrl;
            var response = client.Execute(request);
            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
        }


        /// <summary>
        /// Generic execute method that returns a Task rather that result.Data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<T> ExecuteAsync<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = BaseUrl;
            var task_completion_source = new TaskCompletionSource<T>();
            client.ExecuteAsync<T>(request, response =>
            {
                task_completion_source.SetResult(response.Data);
            });
            return task_completion_source.Task;
        }


        /// <summary>
        /// This is the login method.  It returns a "ticket" that must be supplied with subsequent requests.
        /// </summary>
        /// <returns>Returns instance of ThreeSixtySharp.Objects.AuthTicket.</returns>
        public AuthTicket GetTicket()
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "api/login";
            request.RootElement = "";
            request.AddParameter("username", Username, ParameterType.GetOrPost);
            request.AddParameter("password", Password, ParameterType.GetOrPost);

            return Execute<AuthTicket>(request);
        }


        /// <summary>
        /// Expires a ticket for access to a specific project.
        /// </summary>
        /// <param name="ticket">ThreeSixtySharp.Objects.AuthTicket instance for current user.</param>
        /// <param name="project">ThreeSixtySharp.Objects.Project instance to return ticket for.</param>
        public void ReturnTicketOneProject(AuthTicket ticket, Project project)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "api/logout";
            request.AddParameter("ticket", ticket);
            request.AddParameter("project_id", project.Project_ID);

            Execute(request);
        }


        /// <summary>
        /// Iterates through list of projects current user has access to and 
        /// expires the ticket for each one.  
        /// </summary>
        /// <param name="ticket">ThreeSixtySharp.Objects.AuthTicket instance for current user.</param>
        public void ReturnTicketAllProjects(AuthTicket ticket)
        {
            List<Project> projects = GetProjects(ticket);
            foreach (Project project in projects)
            {
                var request = new RestRequest(Method.POST);
                request.Resource = "api/logout";
                request.AddParameter("ticket", ticket);
                request.AddParameter("project_id", project.Project_ID);

                Execute(request);
            }
        }


        /// <summary>
        /// Get a list of Project objects that the owner of the ticket has access to.
        /// </summary>
        /// <param name="ticket">ThreeSixtySharp.Objects.AuthTicket instance for current user.</param>
        /// <returns>List of ThreeSixtySharp.Objects.Project instances that the owner of the 
        /// AuthTicket has access to.</returns>
        public List<Project> GetProjects(AuthTicket ticket)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "api/projects";
            request.AddParameter("ticket", ticket.Ticket);
            
            return Execute<List<Project>>(request);
        }


        /// <summary>
        /// Returns a list of all ThreeSixtySharp.Objects.File objects for a specified Project, 
        /// and optionally for just a specified Document_Path.
        /// </summary>
        /// <param name="ticket">ThreeSixtySharp.Objects.AuthTicket instance for current user.</param>
        /// <param name="project">ThreeSixtySharp.Objects.Project instance to return files from.</param>
        /// <param name="path">Optional ThreeSixtySharp.Objects.Document_Path instance that filters returned File instances
        /// to just those located at the Document_Path.Path location.</param>
        /// <returns>List of ThreeSixtySharp.Object.File instances.</returns>
        public List<ThreeSixtySharp.Objects.File> GetAllFiles(AuthTicket ticket, Project project, Document_Path path = null)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "api/library/all_files";
            if (path != null)
            { 
                request.AddParameter("directory", path.Path);
            }
            
            request.AddParameter("ticket", ticket.Ticket);
            request.AddParameter("project_id", project.Project_ID);
            return Execute<List<ThreeSixtySharp.Objects.File>>(request);
        }

        /// <summary>
        /// Asynchronous method for returning a list of all ThreeSixtySharp.Objects.File objects for a specified Project, 
        /// and optionally for just a specified Document_Path.
        /// </summary>
        /// <param name="ticket">ThreeSixtySharp.Objects.AuthTicket instance for current user.</param>
        /// <param name="project">ThreeSixtySharp.Objects.Project instance to return files from.</param>
        /// <param name="path">Optional ThreeSixtySharp.Objects.Document_Path instance that filters returned File instances
        /// to just those located at the Document_Path.Path location.</param>
        /// <returns>List of ThreeSixtySharp.Object.File instances.</returns>
        public Task<List<ThreeSixtySharp.Objects.File>> GetAllFilesAsync(AuthTicket ticket, Project project, Document_Path path = null)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "api/library/all_files";
            if (path != null)
            {
                request.AddParameter("directory", path.Path);
            }

            request.AddParameter("ticket", ticket.Ticket);
            request.AddParameter("project_id", project.Project_ID);
            return ExecuteAsync<List<ThreeSixtySharp.Objects.File>>(request);
        }


        /// <summary>
        /// Publish a file that is new to the project.
        /// </summary>
        /// <param name="ticket">ThreeSixtySharp.Objects.AuthTicket instance for current user.</param>
        /// <param name="project">ThreeSixtySharp.Objects.Project instance to return files from.</param>
        /// <param name="doc_path"></param>
        /// <param name="origin_full_filename"></param>
        /// <param name="document_id"></param>
        /// <param name="tags"></param>
        /// <param name="caption"></param>
        /// <param name="allow_replace"></param>
        /// <returns></returns>
        public ThreeSixtySharp.Objects.File PublishNew(AuthTicket ticket,
                                    Project project,
                                    Document_Path doc_path,
                                    string origin_full_filename,
                                    List<string> tags = null,
                                    string caption = null)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "api/library/publish?replace=0";
            request.AddParameter("ticket", ticket.Ticket);
            request.AddParameter("project_id", project.Project_ID);
            request.AddParameter("directory", doc_path.Path);
            request.AddParameter("filename", System.IO.Path.GetFileName(origin_full_filename));
            request.AddFile("Filedata", origin_full_filename);

            if (tags != null)
            {
                request.AddParameter("tags", string.Join(",", tags));
            }
            if (caption != null)
            {
                request.AddParameter("caption", caption);
            }

            return Execute<ThreeSixtySharp.Objects.File>(request);
        }


        /// <summary>
        /// Publish a revision to a file that will be a new revision to an existing document.
        /// </summary>
        /// <param name="ticket">ThreeSixtySharp.Objects.AuthTicket instance for current user.</param>
        /// <param name="project">ThreeSixtySharp.Objects.Project instance to return files from.</param>
        /// <param name="doc_path"></param>
        /// <param name="origin_full_filename"></param>
        /// <param name="document_id">The ThreeSixtySharp.Objects.File.Document_Id parameter of the file to revise.</param>
        /// <param name="tags"></param>
        /// <param name="caption"></param>
        /// <param name="allow_replace"></param>
        /// <returns></returns>
        /// *****This method is not working, possibly issue with the Field API.  There is a support ticket being worked on.
        public ThreeSixtySharp.Objects.File PublishRevision(AuthTicket ticket,
                                    Project project,
                                    Document_Path doc_path,
                                    string origin_full_filename,
                                    string document_id,
                                    List<string> tags = null,
                                    string caption = null)
        {
            var request = new RestRequest(Method.POST);
            
            request.Resource = "api/library/publish?replace=0";
            //I've tried these two:
            //request.Resource = "api/library/publish?replace=1";
            //request.Resource = "api/library/publish";

            request.AddParameter("ticket", ticket.Ticket);
            request.AddParameter("project_id", project.Project_ID);
            request.AddParameter("directory", doc_path.Path);
            request.AddParameter("filename", System.IO.Path.GetFileName(origin_full_filename));
            //I've tried these as well:
            //request.AddParameter("filename", "ProjectJSON_2.txt");
            //request.AddParameter("filename", "ProjectJSON_3.txt");

            request.AddParameter("document_id", document_id);
            request.AddFile("Filedata", origin_full_filename);

            if (tags != null)
            {
                request.AddParameter("tags", string.Join(",", tags));
            }

            if (caption != null)
            {
                request.AddParameter("caption", caption);
            }

            return Execute<ThreeSixtySharp.Objects.File>(request);
        }


        /// <summary>
        /// Publish a file that will become the new base revision of a previously existing file.  Beware.
        /// </summary>
        /// <param name="ticket">ThreeSixtySharp.Objects.AuthTicket instance for current user.</param>
        /// <param name="project">ThreeSixtySharp.Objects.Project instance to return files from.</param>
        /// <param name="doc_path"></param>
        /// <param name="origin_full_filename"></param>
        /// <param name="document_id">The ThreeSixtySharp.Objects.File.Document_Id parameter of the file to revise.</param>
        /// <param name="tags"></param>
        /// <param name="caption"></param>
        /// <param name="allow_replace"></param>
        /// <returns></returns>
        public ThreeSixtySharp.Objects.File PublishBaseRevision(AuthTicket ticket,
                                    Project project,
                                    Document_Path doc_path,
                                    string origin_full_filename,
                                    string document_id,
                                    List<string> tags = null,
                                    string caption = null)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "api/library/publish?replace=1";
            request.AddParameter("ticket", ticket.Ticket);
            request.AddParameter("project_id", project.Project_ID);
            request.AddParameter("directory", doc_path.Path);
            request.AddParameter("filename", System.IO.Path.GetFileName(origin_full_filename));
            request.AddFile("Filedata", origin_full_filename);

            if (tags != null)
            {
                request.AddParameter("tags", string.Join(", ", tags));
            }
            if (caption != null)
            {
                request.AddParameter("caption", caption);
            }

            return Execute<ThreeSixtySharp.Objects.File>(request);
        }


        /// <summary>
        /// Awaitable upload method for use with async calling methods.  
        /// </summary>
        /// <param name="ticket">ThreeSixtySharp.Objects.AuthTicket instance for current user.</param>
        /// <param name="project">ThreeSixtySharp.Objects.Project instance to return files from.</param>
        /// <param name="doc_path"></param>
        /// <param name="origin_full_filename"></param>
        /// <param name="document_id"></param>
        /// <param name="tags"></param>
        /// <param name="caption"></param>
        /// <param name="allow_replace"></param>
        /// <returns></returns>
        public Task<ThreeSixtySharp.Objects.File> PublishAsync(AuthTicket ticket,
                                    Project project,
                                    Document_Path doc_path,
                                    string origin_full_filename,
                                    string document_id = null,
                                    List<string> tags = null,
                                    string caption = null,
                                    bool allow_replace = false)
        {

            //TODO This method to be split out into PublishNewAsync, PublishRevisionAsync, and PublishBaseRevisionAsync.
            var request = new RestRequest(Method.POST);
            int replace = 0;
            if (allow_replace == true)
            {
                replace = 1;
            }
            request.Resource = string.Format("api/library/publish?replace={0}", replace);
            request.AddParameter("ticket", ticket.Ticket);
            request.AddParameter("project_id", project.Project_ID);
            request.AddParameter("directory", doc_path.Path);
            request.AddParameter("filename", System.IO.Path.GetFileName(origin_full_filename));
            request.AddFile("Filedata", origin_full_filename);

            if (document_id != null)
            {
                request.AddParameter("document_id", document_id);
            }
            if (tags != null)
            {
                request.AddParameter("tags", string.Join(",", tags));
            }
            if (caption != null)
            {
                request.AddParameter("caption", caption);
            }

            return ExecuteAsync<ThreeSixtySharp.Objects.File>(request);
        }


        /// <summary>
        /// Delete a specific revision of a file.
        /// </summary>
        /// <param name="ticket">ThreeSixtySharp.Objects.AuthTicket instance for current user.</param>
        /// <param name="project">ThreeSixtySharp.Objects.Project instance to return files from.</param>
        /// <param name="doc">ThreeSixtySharp.Objects.File instance to delete.</param>
        /// <param name="revision">Specify which revision of the file to delete.  Revisions start at 0 and increment up.</param>
        public void DeleteRevision(AuthTicket ticket, Project project, ThreeSixtySharp.Objects.File doc, int revision)
        {   
            var request = new RestRequest(Method.POST);

            request.Resource = "api/library/delete";
            request.AddParameter("ticket", ticket.Ticket);
            request.AddParameter("project_id", project.Project_ID);
            request.AddParameter("id", doc.Document_Id);
            request.AddParameter("rev", revision);

            Execute(request);
        }


        /// <summary>
        /// Delete all revisions of a given file.
        /// </summary>
        /// <param name="ticket">ThreeSixtySharp.Objects.AuthTicket instance for current user.</param>
        /// <param name="project">ThreeSixtySharp.Objects.Project instance to return files from.</param>
        /// <param name="doc">ThreeSixtySharp.Objects.File instance to delete.</param>
        public void DeleteAllRevisions(AuthTicket ticket, Project project, ThreeSixtySharp.Objects.File doc)
        {
            var request = new RestRequest(Method.POST);

            request.Resource = "api/library/delete";
            request.AddParameter("ticket", ticket.Ticket);
            request.AddParameter("project_id", project.Project_ID);
            request.AddParameter("id", doc.Document_Id);
            request.AddParameter("rev", "nil");

            Execute(request);
        }


        public ThreeSixtySharp.Objects.File GetFileMetadata(AuthTicket ticket, Project project, string document_id, int revision_number)
        {
            var request = new RestRequest(Method.POST);

            request.Resource = "api/library/file/{id}/{type}/{rev}";
            request.AddParameter("ticket", ticket.Ticket);
            request.AddParameter("project_id", project.Project_ID);
            request.AddParameter("id", document_id, ParameterType.UrlSegment);
            request.AddParameter("type", "metadata", ParameterType.UrlSegment);
            request.AddParameter("rev", revision_number, ParameterType.UrlSegment);
            request.RootElement = "document";

            ThreeSixtySharp.Objects.File metaDataFile = Execute<ThreeSixtySharp.Objects.File>(request);
            if (metaDataFile.Tags != null)
            {
                //This is a janky short term fix.  BIM 360 Field returns Tags as an 
                //array of strings and a custom deserializer needs to be made to parse this
                //into this list of strings.  This should be ok for now.
                List<string> parsedTags = metaDataFile.Tags[0].Split(',').ToList();
                metaDataFile.Tags = parsedTags;
            }

            return metaDataFile;
        }


        public List<ThreeSixtySharp.Objects.File> GetFileMetadataAllRevisions(AuthTicket ticket, Project project, string document_id)
        {
            var request = new RestRequest(Method.POST);

            request.Resource = "api/library/file/{id}/{type}/{rev}";
            request.AddParameter("ticket", ticket.Ticket);
            request.AddParameter("project_id", project.Project_ID);
            request.AddParameter("id", document_id, ParameterType.UrlSegment);
            request.AddParameter("type", "metadata", ParameterType.UrlSegment);
            request.RootElement = "document";

            return Execute<List<ThreeSixtySharp.Objects.File>>(request);
        }


        public void DownloadFile(AuthTicket ticket, Project project, string document_id, int revision_number)
        {
            var request = new RestRequest(Method.POST);

            request.Resource = "api/library/file/{id}/{type}/{rev}";
            request.AddParameter("ticket", ticket.Ticket);
            request.AddParameter("project_id", project.Project_ID);
            request.AddParameter("id", document_id, ParameterType.UrlSegment);
            request.AddParameter("type", "original", ParameterType.UrlSegment);
            request.AddParameter("rev", revision_number);

            var client = new RestClient();
            client.BaseUrl = BaseUrl;
            client.ExecuteAsync(request, response =>
                {
                    if (response.ErrorException != null)
                    {
                        throw response.ErrorException;
                    }
                    else
                    {
                        byte[] file_bytes = response.RawBytes;
                        //in progress
                    }
                });
        }


        public List<Area> GetAreas(AuthTicket ticket, Project project, DateTime? maxDate = null)
        {
            var request = new RestRequest(Method.POST);

            request.Resource = "api/areas";
            request.AddParameter("ticket", ticket.Ticket);
            request.AddParameter("project_id", project.Project_ID);

            if (maxDate != null)
            {
                request.AddParameter("max_date", maxDate.ToString()); //Not tested
            }

            //request.RootElement = "";

            //return Execute<List<Area>>(request);
            return Execute<List<Area>>(request);
        }


        public List<Issue> GetIssues(AuthTicket ticket, Project project)
        {
            var request = new RestRequest(Method.POST);

            request.Resource = "api/get_issues";
            request.AddParameter("ticket", ticket.Ticket);
            request.AddParameter("project_id", project.Project_ID);


            return Execute<List<Issue>>(request);
        }

    }
}






























