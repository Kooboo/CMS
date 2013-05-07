#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.eCommerce.EventBus;
using Kooboo.CMS.eCommerce.Models.Catalog;
using Kooboo.CMS.eCommerce.Persistence.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Services.Catalog
{
    [Dependency(typeof(IImageTypeService))]
    public class ImageTypeService : IImageTypeService
    {
        #region Properties
        public IImageTypeProvider ImageTypeProvider
        {
            get;
            private set;
        }
        public IEventPublisher EventPublisher
        {
            get;
            private set;
        }
        #endregion

        #region .ctor
        public ImageTypeService(IImageTypeProvider imageTypeProvider, IEventPublisher eventPublisher)
        {
            this.ImageTypeProvider = imageTypeProvider;
            this.EventPublisher = eventPublisher;
        }
        #endregion

        #region Methods
        public void Add(ImageType imageType)
        {
            if (imageType == null)
            {
                throw new ArgumentNullException("imageType");
            }
            this.EventPublisher.PreAdd(imageType);

            ImageTypeProvider.Add(imageType);

            this.EventPublisher.Added(imageType);
        }

        public void Update(ImageType newImageType, ImageType oldImageType)
        {
            if (newImageType == null)
            {
                throw new ArgumentNullException("newImageType");
            }
            if (oldImageType == null)
            {
                throw new ArgumentNullException("oldImageType");
            }
            this.EventPublisher.PreAdd(newImageType);

            ImageTypeProvider.Update(newImageType, oldImageType);

            this.EventPublisher.Added(newImageType);
        }

        public void Delete(ImageType imageType)
        {
            if (imageType == null)
            {
                throw new ArgumentNullException("imageType");
            }
            this.EventPublisher.PreAdd(imageType);

            ImageTypeProvider.Remove(imageType);

            this.EventPublisher.Added(imageType);
        }
        #endregion
    }
}
