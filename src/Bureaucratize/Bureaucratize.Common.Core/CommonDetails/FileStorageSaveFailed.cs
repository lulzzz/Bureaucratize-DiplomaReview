﻿/*
   Copyright (c) 2018 Michał Wilczyński

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using Bureaucratize.FileStorage.Contracts.Commands.Base;

namespace Bureaucratize.Common.Core.CommonDetails
{
    public class FileStorageSaveFailed : IResultDetails
    {
        public IFileStorageCommand FailedCommand { get; }
        public string DetailsMessageKey => nameof(FileStorageSaveFailed);
        

        public FileStorageSaveFailed(IFileStorageCommand failedCommand)
        {
            FailedCommand = failedCommand;
        }

        public string GetDetails()
        {
            return $"Failed to save file for {FailedCommand.GetType().Name}";
        }
    }
}