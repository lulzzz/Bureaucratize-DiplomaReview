/*
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

using System;
using System.IO;
using Akka.Actor;
using Bureaucratize.FileStorage.Contracts;
using Bureaucratize.FileStorage.Contracts.Commands.Base;
using Bureaucratize.ImageProcessing.Contracts.RemotingMessages;
using Bureaucratize.ImageProcessing.Core.Commands;
using Bureaucratize.ImageProcessing.Infrastructure;
using Bureaucratize.ImageProcessing.Infrastructure.CommandHandlers;
using Bureaucratize.ImageProcessing.Infrastructure.QueryHandlers;
using Bureaucratize.ImageProcessing.Infrastructure.ResourceCommandHandlers;
using Bureaucratize.ImageProcessing.Infrastructure.ResourceQueryHandlers;
using Bureaucratize.Templating.Core.Infrastructure.Commands;
using Bureaucratize.Templating.Core.Template;
using Bureaucratize.Templating.Infrastructure;
using Bureaucratize.Templating.Infrastructure.CommandHandlers;
using Bureaucratize.Templating.Infrastructure.QueryHandlers;
using Bureaucratize.Templating.Infrastructure.ResourceCommandHandlers;

namespace Bureaucratize.Console.Host
{
    public class Program
    {
        private static readonly IImageProcessingPersistenceConfiguration ImageProcessingConfig = new ImageProcessingPersistenceConfiguration();
        private static readonly ITemplatingPersistenceConfiguration TemplatingConfig = new TemplatingPersistenceConfiguration();

        static void Main(string[] args)
        {
            var templateId = CreateTemplateDefinition();

            var createDocument = new CreateDocumentToProcess
            {
                Id = Guid.NewGuid(),
                RequesterIdentifier = Guid.NewGuid(),
                TemplateDefinitionIdentifier = templateId
            };
                
            var createDocumentResult = new CreateDocumentToProcessHandler(ImageProcessingConfig).Handle(createDocument);

            var addBitmapToDocument = new AddBitmapForDocumentToProcess
            {
                DocumentId = createDocument.Id,
                OrderedBitmap = new OrderedBitmapToSave
                {
                    FileData = File.ReadAllBytes(
                        @"C:\\Users\\micha_000\\Desktop\\pekao_template_enhanced_filled_3.jpg"),
                    Order = 1,
                    FileLabel = "Test123",
                    FileType = BitmapFiletype.Jpg
                }
            };

            var getDocumentHandler = new GetDocumentToProcessHandler(ImageProcessingConfig,
                new GetDocumentToProcessResourcesHandler(ImageProcessingConfig),
                new GetTemplateDefinitionByIdHandler(TemplatingConfig));

            var addBitmapToDocumentResult = new AddBitmapForDocumentToProcessHandler
                    (getDocumentHandler, new SavePageBitmapForDocumentToProcessHandler(ImageProcessingConfig))
                .Handle(addBitmapToDocument);

            var system = ActorSystem.Create("producer");

            IActorRef serverRef = system.ActorSelection("akka.tcp://api@localhost:8081/user/image-processing")
                .ResolveOne(TimeSpan.FromMilliseconds(500)).Result;

            serverRef.Ask<ProcessDocumentOfIdRequest>(new ProcessDocumentOfIdRequest(createDocument.Id,
                Guid.NewGuid()));

            System.Console.ReadKey();
        }

        public static Guid CreateTemplateDefinition()
        {
            var templateDefinition = new CreateTemplateDefinitionHandler(TemplatingConfig)
                .Handle(new CreateTemplateDefinition
                {
                    TemplateCreatorId = Guid.NewGuid(),
                    TemplateName = "TEST1"
                }).Result;

            var pageDefinitionResult = new AddTemplateDefinitionPageHandler(TemplatingConfig,
                    new SaveBitmapForTemplatePageCanvasHandler(TemplatingConfig))
                .Handle(new AddTemplateDefinitionPage
                    {
                        TemplateId = templateDefinition.Id,
                        PageNumber = 1,
                        ReferenceCanvas = new SaveBitmapCommand
                        {
                            FileData = File.ReadAllBytes(
                                @"C:\\Users\\micha_000\\Desktop\\pekao_template_enhanced_twice-bigger.jpg"),
                            FileLabel = "Test123 Canvas",
                            FileType = BitmapFiletype.Jpg,
                        }
                });

            var pageDefinition = pageDefinitionResult.Result.DefinedPages[1] as TemplatePageDefinition;

            var areaAddResult = new AddTemplatePageAreasHandler(TemplatingConfig)
                .Handle(new AddTemplatePageAreas
                {
                    TemplatePageId = pageDefinition.Id,
                    Areas = new []
                    {
                        new AddTemplatePageArea
                        {
                            AreaName = "Wnioskodawca 1 - Imie",
                            DimensionX = 732,
                            DimensionY = 1018,
                            DimensionWidth = 400,
                            DimensionHeight = 60,
                            ExpectedData = TemplatePartExpectedDataType.Letters,
                            TemplatePageId = pageDefinition.Id,
                            AreaParts = new []
                            {
                                new AddTemplatePageAreaPart
                                {
                                    DimensionX = 732,
                                    DimensionY = 1018,
                                    DimensionWidth = 26,
                                    DimensionHeight = 60,
                                    OrderInArea = 1
                                },
                                new AddTemplatePageAreaPart
                                {
                                    DimensionX = 762,
                                    DimensionY = 1018,
                                    DimensionWidth = 26,
                                    DimensionHeight = 60,
                                    OrderInArea = 2
                                },
                                new AddTemplatePageAreaPart
                                {
                                    DimensionX = 792,
                                    DimensionY = 1018,
                                    DimensionWidth = 26,
                                    DimensionHeight = 60,
                                    OrderInArea = 3
                                },
                                new AddTemplatePageAreaPart
                                {
                                    DimensionX = 822,
                                    DimensionY = 1018,
                                    DimensionWidth = 26,
                                    DimensionHeight = 60,
                                    OrderInArea = 4
                                },
                                new AddTemplatePageAreaPart
                                {
                                    DimensionX = 852,
                                    DimensionY = 1018,
                                    DimensionWidth = 26,
                                    DimensionHeight = 60,
                                    OrderInArea = 5
                                },
                                new AddTemplatePageAreaPart
                                {
                                    DimensionX = 882,
                                    DimensionY = 1018,
                                    DimensionWidth = 26,
                                    DimensionHeight = 60,
                                    OrderInArea = 6
                                },
                                new AddTemplatePageAreaPart
                                {
                                    DimensionX = 912,
                                    DimensionY = 1018,
                                    DimensionWidth = 26,
                                    DimensionHeight = 60,
                                    OrderInArea = 7
                                },
                                new AddTemplatePageAreaPart
                                {
                                    DimensionX = 942,
                                    DimensionY = 1018,
                                    DimensionWidth = 26,
                                    DimensionHeight = 60,
                                    OrderInArea = 8
                                },
                                new AddTemplatePageAreaPart
                                {
                                    DimensionX = 972,
                                    DimensionY = 1018,
                                    DimensionWidth = 26,
                                    DimensionHeight = 60,
                                    OrderInArea = 9
                                },
                                new AddTemplatePageAreaPart
                                {
                                    DimensionX = 1002,
                                    DimensionY = 1018,
                                    DimensionWidth = 26,
                                    DimensionHeight = 60,
                                    OrderInArea = 10
                                }
                            }
                        }
                    }
                });

            //pageDefinition.DefineArea(areaOfInterest);

            //templateDefinition.AddPageDefinition(pageDefinition);

            return templateDefinition.Id;
        }
    }
}
