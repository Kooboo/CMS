using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kooboo.CMS.Account.Models
{
    [DataContract]
    public class Setting : IPersistable
    {
        /// <summary>
        /// 设置密码强度的正则表达式
        /// </summary>
        [DataMember]
        public string PasswordStrength { get; set; }

        /// <summary>
        /// 密码不符合规则的提示消息
        /// </summary>
        [DataMember]
        public string PasswordInvalidMessage { get; set; }

        /// <summary>
        /// 是否启用锁定用户功能。（密码错误FailedPasswordAttemptCount后锁定用户）
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable lockout]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool EnableLockout { get; set; }
        /// <summary>
        /// 密码可重试次数
        /// </summary>
        [DataMember]
        public int FailedPasswordAttemptCount { get; set; }

        /// <summary>
        /// 锁定多长时间后解锁
        /// </summary>
        [DataMember]
        public int MinutesToUnlock { get; set; }


        #region IPersistable

        private bool isDummy = true;
        public bool IsDummy
        {
            get { return isDummy; }
            set { isDummy = value; }
        }

        public void Init(IPersistable source)
        {
            isDummy = false;
        }

        public void OnSaved()
        {
            isDummy = false;
        }

        public void OnSaving()
        {

        }

        #endregion
    }
}
