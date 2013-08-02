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
    public class ThreeSixtySharpBase
    {
        const string BaseUrl = "https://manage.velasystems.com";
        private string Username { get; set; }
        private string Password { get; set; }

        /// <summary>
        /// Constructor for ThreeSixtySharpBase.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public ThreeSixtySharpBase(string username, string password)
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
        /// <returns></returns>
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
        /// Get a list of Project objects that the owner of the ticket has access to.
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public List<Project> GetProjects(AuthTicket ticket)
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "api/projects";
            request.AddParameter("ticket", ticket.Ticket);
            return Execute<List<Project>>(request);
        }


        public List<ThreeSixtySharp.Objects.File> GetAllFiles(AuthTicket ticket, Project project, Document_Path path = null)
        {
            var request = new RestRequest(Method.GET);

            if (path == null)
            {
                request.Resource = "api/library/all_files";
            }

            else
            {
                request.Resource = "api/library/all_files/:{path.Path}";
                request.AddParameter("directory", path.Path, ParameterType.UrlSegment);
            }

            request.AddParameter("ticket", ticket.Ticket);
            request.AddParameter("project_id", project.Project_ID);
            return Execute<List<ThreeSixtySharp.Objects.File>>(request);
        }

        /// <summary>
        /// Publish a file.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="project"></param>
        /// <param name="doc_path"></param>
        /// <param name="origin_full_filename"></param>
        /// <param name="document_id"></param>
        /// <param name="tags"></param>
        /// <param name="caption"></param>
        /// <param name="allow_replace"></param>
        /// <returns></returns>
        public ThreeSixtySharp.Objects.File Publish(AuthTicket ticket,
                                    Project project,
                                    Document_Path doc_path,
                                    string origin_full_filename,
                                    string document_id = null,
                                    List<string> tags = null,
                                    string caption = null,
                                    bool allow_replace = false)
        {
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

            return Execute<ThreeSixtySharp.Objects.File>(request);
        }

        /// <summary>
        /// Awaitable upload method for use with async calling methods.  
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="project"></param>
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
        /// Method for deleting a file.  Hopefully this is the only method that deletes files.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="project"></param>
        /// <param name="doc"></param>
        /// <param name="revision"></param>
        public void DeleteFile(AuthTicket ticket, Project project, ThreeSixtySharp.Objects.File doc, int revision)
        {   
            var request = new RestRequest(Method.POST);

            request.Resource = "api/library/delete";
            request.AddParameter("ticket", ticket);
            request.AddParameter("project_id", project.Project_ID);
            request.AddParameter("id", doc.Document_Id);
            request.AddParameter("rev", revision);

            Execute(request);
        }
    }
}







