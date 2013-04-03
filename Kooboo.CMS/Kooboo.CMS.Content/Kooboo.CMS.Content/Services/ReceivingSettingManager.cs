using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Content.Query;
namespace Kooboo.CMS.Content.Services
{
    public class ReceivingSettingManager : ManagerBase<ReceivingSetting, IReceivingSettingProvider>
    {
        #region ReceivingSetting
        public override ReceivingSetting Get(Repository repository, string name)
        {
            return GetProvider().Get(new ReceivingSetting { Repository = repository, Name = name });
        }
        public override void Add(Repository repository, ReceivingSetting item)
        {
            item.Name = string.Join("-", item.SendingRepository, item.SendingFolder, item.ReceivingFolder);
            base.Add(repository, item);
        }
        #endregion
        #region Process

        public virtual void ReceiveContent(Repository repository, TextContent originalContent, ContentAction action)
        {
            //bool processed = false;
            var allReceivers = All(repository, "").Select(o => Get(repository, o.Name));
            foreach (var receivingSetting in allReceivers)
            {
                if (CheckSetting(originalContent, receivingSetting, action))
                {
                    ProcessMessage(repository, originalContent, receivingSetting.ReceivingFolder, receivingSetting.KeepStatus, action);

                    //processed = true;
                }
            }
        }

        public virtual void ProcessMessage(Repository repository, TextContent originalContent, string receivingFolder, bool keepStatus, ContentAction action)
        {
            var targetFolder = new TextFolder(repository, receivingFolder).AsActual();

            if ((ContentAction.Add & action) == action && (originalContent.Published.HasValue && originalContent.Published.Value == true))
            {
                var content = targetFolder.CreateQuery().WhereEquals("UUID", originalContent.UUID).FirstOrDefault();
                if (content == null)
                {
                    Services.ServiceFactory.TextContentManager.Copy(originalContent, targetFolder, keepStatus, true, null);
                }
                else
                {
                    UpdateAction(repository, originalContent, targetFolder, keepStatus);
                }
            }
            else if ((ContentAction.Update & action) == action)
            {
                UpdateAction(repository, originalContent, targetFolder, keepStatus);
            }
            else if ((ContentAction.Delete & action) == action)
            {
                DeleteAction(repository, originalContent, targetFolder);
            }

        }

        public virtual void DeleteAction(Repository repository, TextContent originalContent, TextFolder targetFolder)
        {
            foreach (var drivedContent in BroadcastingContentHelper.GetContentsByOriginalUUID(targetFolder, originalContent.UUID))
            {
                if (drivedContent.IsLocalized != null && drivedContent.IsLocalized.Value == false)
                {
                    Services.ServiceFactory.TextContentManager.Delete(repository, targetFolder, drivedContent.UUID);
                }
            }
        }

        public virtual void UpdateAction(Repository repository, TextContent originalContent, TextFolder targetFolder, bool keepStatus)
        {
            var contents = BroadcastingContentHelper.GetContentsByOriginalUUID(targetFolder, originalContent.UUID).ToArray();
            var categoriesOfOriginalContent = GetAllCategories(originalContent).ToArray();
            if (originalContent.Published.HasValue && originalContent.Published.Value == true && contents.Length == 0)
            {
                Services.ServiceFactory.TextContentManager.Copy(originalContent, targetFolder, keepStatus, true, null);
            }
            else
            {
                foreach (var drivedContent in contents)
                {
                    if (drivedContent.IsLocalized != null && drivedContent.IsLocalized.Value == false)
                    {
                        var values = BroadcastingContentHelper.ExcludeBasicFields(originalContent);
                        if (keepStatus)
                        {
                            if (originalContent.Published.HasValue)
                            {
                                values["Published"] = originalContent.Published.Value.ToString();
                            }
                        }
                        var categoriesOfDrivedContent = GetAllCategories(drivedContent);
                        Services.ServiceFactory.TextContentManager.Update(repository, targetFolder, drivedContent.UUID, values, null, DateTime.UtcNow, categoriesOfOriginalContent, categoriesOfDrivedContent);
                    }
                }
            }
        }
        private IEnumerable<TextContent> GetAllCategories(TextContent textContent)
        {
            var categories = Providers.DefaultProviderFactory.GetProvider<ITextContentProvider>().QueryCategories(textContent);

            return categories.Select(it => new TextContent(textContent.Repository, "", it.CategoryFolder) { UUID = it.CategoryUUID });
        }
        private bool CheckSetting(ContentBase content, ReceivingSetting receivingSetting, ContentAction action)
        {
            if (receivingSetting.ReceivingFolder == null && receivingSetting.ReceivingFolder.Length == 0)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(receivingSetting.SendingRepository) && string.Compare(receivingSetting.SendingRepository, content.Repository, true) != 0)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(receivingSetting.SendingFolder) && string.Compare(receivingSetting.SendingFolder, content.FolderName, true) != 0)
            {
                return false;
            }
            //if (receivingSetting.Published.HasValue && content.Published != receivingSetting.Published.Value)
            //{
            //    return false;
            //}

            //if ((receivingSetting.AcceptAction & action) != action)
            //{
            //    return false;
            //}

            return true;
        }
        #endregion

    }
}
