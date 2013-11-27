#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Modules.CMIS.Models;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Cmis
{

    public class CmisWcfClient : ICmisService
    {
        #region .ctor
        string _endpoint;
        string _userName;
        string _password;
        public CmisWcfClient(string endpointUrl, string userName, string password)
        {
            _endpoint = endpointUrl;
            _userName = userName;
            _password = password;
        }
        #endregion
        private cmisExtensionType GetcmisExtensionType()
        {
            return null;
        }
        private Kooboo.CMS.Modules.CMIS.Services.IService GetService()
        {
            if (!Uri.IsWellFormedUriString(_endpoint, UriKind.Absolute))
            {
                throw new ArgumentException(string.Format("\"{0}\" is not a wellformed uri.".Localize(), _endpoint));
            }
            Uri uri = new Uri(_endpoint);
            EndpointAddress address = new EndpointAddress(_endpoint);
            BasicHttpBinding binding = new BasicHttpBinding()
            {
                MessageEncoding = WSMessageEncoding.Mtom,
                TransferMode = TransferMode.Streamed,
            };
            if (uri.Scheme.ToLower() == "https")
            {
                binding.Security = new BasicHttpSecurity()
                {
                    Mode = BasicHttpSecurityMode.TransportWithMessageCredential,
                    Transport = new System.ServiceModel.HttpTransportSecurity()
                    {
                        ClientCredentialType = HttpClientCredentialType.None
                    },
                    Message = new BasicHttpMessageSecurity()
                    {
                        ClientCredentialType = BasicHttpMessageCredentialType.UserName
                    }
                };
            }

            ChannelFactory<Kooboo.CMS.Modules.CMIS.Services.IService> factory = new ChannelFactory<Kooboo.CMS.Modules.CMIS.Services.IService>(binding, address);

            factory.Faulted += factory_Faulted;
            factory.Credentials.UserName.UserName = _userName;
            factory.Credentials.UserName.Password = _password;

            System.Net.ServicePointManager.ServerCertificateValidationCallback =
           ((sender, certificate, chain, sslPolicyErrors) => true);

            return factory.CreateChannel();
        }

        void factory_Faulted(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private T HandleFault<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (System.ServiceModel.Security.MessageSecurityException security)
            {
                throw security.InnerException;
            }
        }

        #region GetRepositories
        public IEnumerable<KeyValuePair<string, string>> GetRepositories()
        {
            return HandleFault(() =>
                 {
                     var service = GetService();
                     getRepositoriesRequest request = new getRepositoriesRequest(GetcmisExtensionType());

                     var response = service.GetRepositories(request);

                     return response.repositories.Select(it => new KeyValuePair<string, string>(it.repositoryId, it.repositoryName)).ToArray();
                 });
        }
        #endregion

        #region GetFolderTrees
        //private TreeNode<KeyValuePair<string, string>> ToTreeNode(PathedCmisObjectContainer container)
        //{
        //    TreeNode<KeyValuePair<string, string>> treeNode = new TreeNode<KeyValuePair<string, string>>();
        //    if (container.ObjectInFolder.Object != null)
        //    {
        //        var id = container.ObjectInFolder.Object.Id;
        //        var name = id;
        //        var nameProperty = container.ObjectInFolder.Object.Properties.Name;
        //        if (nameProperty != null)
        //        {
        //            name = ((CmisPropertyString)nameProperty).Value[0];
        //        }
        //        treeNode.Node = new KeyValuePair<string, string>(id, name);
        //    }
        //    if (container.Children != null)
        //    {
        //        treeNode.Children = container.Children.Select(it => ToTreeNode(it));
        //    }
        //    return treeNode;
        //}
        private TreeNode<KeyValuePair<string, string>> ToTreeNode(cmisObjectInFolderContainerType container)
        {
            TreeNode<KeyValuePair<string, string>> treeNode = new TreeNode<KeyValuePair<string, string>>();
            if (container.objectInFolder.@object != null)
            {
                //var id = container.ObjectInFolder.Object.Id;
                var id = string.Empty;
                var name = string.Empty;
                var idProperty = container.objectInFolder.@object.properties.Items.FirstOrDefault(it => it.localName.Equals("Id", StringComparison.OrdinalIgnoreCase));
                if (idProperty != null)
                {
                    id = name = idProperty.stringValue;
                }
                //var nameProperty = container.ObjectInFolder.Object.Properties.Name;
                var nameProperty = container.objectInFolder.@object.properties.Items.FirstOrDefault(it => it.localName.Equals("Name", StringComparison.OrdinalIgnoreCase));
                if (nameProperty != null)
                {
                    name = nameProperty.stringValue;
                }
                treeNode.Node = new KeyValuePair<string, string>(id, name);
            }
            if (container.children != null)
            {
                treeNode.Children = container.children.Select(it => ToTreeNode(it));
            }
            return treeNode;
        }
        public IEnumerable<TreeNode<KeyValuePair<string, string>>> GetFolderTrees(string reposiotryId)
        {
            return HandleFault(() =>
                {
                    var service = GetService();

                    getRepositoryInfoRequest request = new getRepositoryInfoRequest(reposiotryId, null);

                    var response = service.GetRepositoryInfo(request);

                    var getFolderTreeRequest = new getFolderTreeRequest(reposiotryId, response.repositoryInfo.rootFolderId, null, null, null, null, null, null, GetcmisExtensionType());

                    var folders = service.GetFolderTree(getFolderTreeRequest).objects;



                    List<cmisObjectInFolderContainerType> allFolders = new List<cmisObjectInFolderContainerType>();
                    //ignore the root folder.
                    foreach (var item in folders)
                    {
                        if (item.children != null)
                        {
                            EnumerateFolders(item.children, ref allFolders);
                        }
                    }
                    return allFolders.Select(it => ToTreeNode(it));
                });
        }
        private void EnumerateFolders(IEnumerable<cmisObjectInFolderContainerType> folders, ref  List<cmisObjectInFolderContainerType> allFolders)
        {
            foreach (var item in folders)
            {
                allFolders.Add(item);
                if (item.children != null)
                {
                    EnumerateFolders(item.children, ref allFolders);
                }
            }
        }
        #endregion

        #region AddTextContent
        public string AddTextContent(string repositoryId, string folderId, TextContent textContent, IEnumerable<Category> categories)
        {
            var cmisProperties = Kooboo.CMS.Modules.CMIS.Services.Implementation.ModelHelper.ToCmisPropertiesType(textContent, categories);

            var service = GetService();

            var createDocumentRequest = new createDocumentRequest(repositoryId, cmisProperties, folderId, null, null, null, null, null, GetcmisExtensionType());

            return service.CreateDocument(createDocumentRequest).objectId;
        }
        #endregion

        public string UpdateTextContent(string repositoryId, string folderId, TextContent textContent, IEnumerable<Category> categories)
        {
            return HandleFault(() =>
               {
                   var cmisProperties = Kooboo.CMS.Modules.CMIS.Services.Implementation.ModelHelper.ToCmisPropertiesType(textContent, categories);

                   var service = GetService();

                   var contentIntegrateId = new ContentIntegrateId(repositoryId, folderId, textContent.UUID);


                   var updatePropertiesRequest = new updatePropertiesRequest(repositoryId, contentIntegrateId.Id, null, cmisProperties, GetcmisExtensionType());

                   return service.UpdateProperties(updatePropertiesRequest).objectId;
               });
        }

        public void DeleteTextContent(string repositoryId, string folderId, string contentUUID)
        {
            var contentIntegrateId = new ContentIntegrateId(repositoryId, folderId, contentUUID);

            var service = GetService();

            var deleteObjectRequest = new deleteObjectRequest(repositoryId, contentIntegrateId.Id, true, GetcmisExtensionType());
            service.DeleteObject(deleteObjectRequest);
        }

        #region Page
        public string AddPage(string repositoryId, Sites.Models.Page page)
        {
            var service = GetService();

            return service.AddPage(repositoryId, page.FullName, page);
            //return null;
        }

        public string UpdatePage(string repositoryId, Sites.Models.Page page)
        {
            var service = GetService();

            return service.UpdatePage(repositoryId, page.FullName, page);

            //return null;
        }

        public void DeletePage(string repositoryId, string pageId)
        {
            var service = GetService();

            service.DeletePage(repositoryId, pageId);
        }
        #endregion
    }
}
