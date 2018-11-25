/*
* Copyright (c) 2014 All Rights Reserved by the SDL Group.
* 
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
* 
*     http://www.apache.org/licenses/LICENSE-2.0
* 
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Management.Automation;
using Trisoft.ISHRemote.Objects;
using Trisoft.ISHRemote.Objects.Public;
using Trisoft.ISHRemote.Exceptions;
using System.Collections.Generic;

namespace Trisoft.ISHRemote.Cmdlets.DocumentObj
{
    public abstract class DocumentObjCmdlet : TrisoftCmdlet
    {
        public Enumerations.ISHType[] ISHType
        {
            get { return new Enumerations.ISHType[] { Enumerations.ISHType.ISHIllustration, Enumerations.ISHType.ISHLibrary, Enumerations.ISHType.ISHMasterDoc, Enumerations.ISHType.ISHModule, Enumerations.ISHType.ISHTemplate }; }
        }

        /// <summary>
        /// Wrap incoming objects as PSObjects and extend with PSNoteProperties for every IshField value entry
        /// </summary>
        /// <param name="ishSession">Incoming ishSession allows future tuning or simple disablement</param>
        /// <param name="ishObjects">Object to wrap and return as PSObject</param>
        /// <returns>Wrapped PSObjects</returns>
        internal List<PSObject> WrapAsPSObjectAndAddNoteProperties(IshSession ishSession, List<IshObject> ishObjects)
        {
            List<PSObject> psObjects = new List<PSObject>();
            foreach (var ishObject in ishObjects)
            {
                PSObject psObject = PSObject.AsPSObject(ishObject);  // returning a PSObject object that inherits from ishObject
                foreach (IshField ishField in ishObject.IshFields.Fields())
                {
                    psObject.Properties.Add(ishSession.NameHelper.GetPSNoteProperty(ISHType, (IshMetadataField)ishField));
                }
                psObjects.Add(psObject);
            }
            return psObjects;
        }

        /// <summary>
        /// Wrap incoming objects as PSObjects and extend with PSNoteProperties for every IshField value entry
        /// </summary>
        /// <param name="ishSession">Incoming ishSession allows future tuning or simple disablement</param>
        /// <param name="ishObject">Single Object to wrap and return as PSObject</param>
        /// <returns>Wrapped PSObjects</returns>
        internal List<PSObject> WrapAsPSObjectAndAddNoteProperties(IshSession ishSession, IshObject ishObject)
        {
            List<IshObject> ishObjects = new List<IshObject>();
            ishObjects.Add(ishObject);
            return WrapAsPSObjectAndAddNoteProperties(ishSession, ishObjects);
        }
    }
}
