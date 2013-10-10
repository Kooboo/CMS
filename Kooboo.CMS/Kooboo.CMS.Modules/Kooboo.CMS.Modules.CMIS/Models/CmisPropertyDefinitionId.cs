#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.Models
{
    /// <summary>
    /// Holds object-type Property Definition ID references.
    /// </summary>
    public static class CmisPropertyDefinitionId
    {
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the ID of the allowed object-type IDs within a parent object (cmis:allowedChildObjectTypeIds).
        /// </summary>
        public static string AllowedChildObjectTypeIds = "cmis:allowedChildObjectTypeIds";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the ID of the base object-type for an object (cmis:baseTypeId).
        /// </summary>
        public static string BaseTypeId = "cmis:baseTypeId";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the opaque token used for optimistic locking and concurrency checking (cmis:changeToken).
        /// </summary>
        public static string ChangeToken = "cmis:changeToken";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the textual comment associated with a given version (cmis:checkinComment).
        /// </summary>
        public static string CheckinComment = "cmis:checkinComment";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the ID of a content stream (cmis:contentStreamId).
        /// </summary>
        public static string ContentStreamId = "cmis:contentStreamId";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the file name of a content stream (cmis:contentStreamFileName).
        /// </summary>
        public static string ContentStreamFileName = "cmis:contentStreamFileName";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the length (in bytes) of the content stream (cmis:contentStreamLength).
        /// </summary>
        public static string ContentStreamLength = "cmis:contentStreamLength";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the MIME type of a Content Stream (cmis:contentStreamMimeType).
        /// </summary>
        public static string ContentStreamMimeType = "cmis:contentStreamMimeType";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the user who created an object (cmis:createdBy).
        /// </summary>
        public static string CreatedBy = "cmis:createdBy";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the Date/Time stamp when an object was created (cmis:creationDate).
        /// </summary>
        public static string CreationDate = "cmis:creationDate";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the boolean flag specifing whether the repository must throw an error at any attempt to
        /// update or delete an object (cmis:isImmutable).
        /// </summary>
        public static string IsImmutable = "cmis:isImmutable";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the boolean flag specifying whether a versionable document object of a version series
        /// (that contains one or more major versions) has the most recent LastModificationDate
        /// (cmis:isLatestMajorVersion).
        /// </summary>
        public static string IsLatestMajorVersion = "cmis:isLatestMajorVersion";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the boolean flag specifying whether a versionable document object of a version series has
        /// the most recent LastModificationDate (cmis:isLatestVersion).
        /// </summary>
        public static string IsLatestVersion = "cmis:isLatestVersion";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the boolean flag specifying whether a versionable document object of a version series
        /// is the major version (cmis:isMajorVersion).
        /// </summary>
        public static string IsMajorVersion = "cmis:isMajorVersion";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the boolean flag specifying whether a versions series is curently checked out
        /// (cmis:isVersionSeriesCheckedOut).
        /// </summary>
        public static string IsVersionSeriesCheckedOut = "cmis:isVersionSeriesCheckedOut";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the Date/Time stamp when an object was last modified (cmis:lastModificationDate).
        /// </summary>
        public static string LastModificationDate = "cmis:lastModificationDate";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the user who last modified an object (cmis:LastModifiedBy).
        /// </summary>
        public static string LastModifiedBy = "cmis:lastModifiedBy";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the name of an object (cmis:name).
        /// </summary>
        public static string Name = "cmis:name";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the ID of an object (cmis:objectId).
        /// </summary>
        public static string ObjectId = "cmis:objectId";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the ID of an object's type (cmis:objectTypeId).
        /// </summary>
        public static string ObjectTypeId = "cmis:objectTypeId";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the ID of an object's parent folder (cmis:parentId).
        /// </summary>
        public static string ParentId = "cmis:parentId";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the fully qualified path to a folder (cmis:path).
        /// </summary>
        public static string Path = "cmis:path";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the user-friendly description of the policy (cmis:policyText).
        /// </summary>
        public static string PolicyText = "cmis:policyText";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the ID of the source object of the relationship (cmis:sourceId).
        /// </summary>
        public static string SourceId = "cmis:sourceId";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the ID of the target object of the relationship (cmis:targetId).
        /// </summary>
        public static string TargetId = "cmis:targetId";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the description (to a user) of the position of an individual object with respect to the
        /// version series, e.g. version 1.0 (cmis:versionLabel).
        /// </summary>
        public static string VersionLabel = "cmis:versionLabel";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the user who has a version series checked out (cmis:versionSeriesCheckedOutBy).
        /// </summary>
        public static string VersionSeriesCheckedOutBy = "cmis:versionSeriesCheckedOutBy";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the ID of a checked out version series (cmis:versionSeriesCheckedOutId).
        /// </summary>
        public static string VerisonSeriesCheckedOutId = "cmis:versionSeriesCheckedOutId";
        /// <summary>
        /// Gets the <see cref="System.String"/> object that corresponds to the Property Definition ID
        /// of the ID of a version series (cmis:versionSeriesId).
        /// </summary>
        public static string VersionSeriesId = "cmis:versionSeriesId";
    }
}
