using AdaptiveCards;
using AdaptiveCards.Rendering.Html;
using CrossVertical.Announcement.Helper;
using CrossVertical.Announcement.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskModule;

namespace CrossVertical.Announcement.Models
{
    public partial class Campaign
    {
        public override AdaptiveCard GetCreateNewCard(List<Group> groups, List<Team> teams, bool isEditCard)
        {
            // Update this code to add groups & teams.
            var employeGroups = new List<AdaptiveChoice>();
            foreach (var group in groups)
            {
                employeGroups.Add(new AdaptiveChoice() { Title = group.Name, Value = group.Id });
            }

            var channels = new List<AdaptiveChoice>();
            foreach (var team in teams)
            {
                foreach (var channel in team.Channels)
                {
                    channels.Add(new AdaptiveChoice() { Title = $"{team.Name} > {channel.Name}", Value = $"{team.Id};{channel.Id}" });
                }
            }

            string groupRecipients = null;

            if (Recipients.Groups != null && Recipients.Groups.Count != 0)
            {
                groupRecipients = string.Join(",", Recipients.Groups.Select(g => g.GroupId));
            }

            string channelRecipients = null;
            if (Recipients.Channels != null & Recipients.Channels.Count != 0)
            {
                channelRecipients = string.Join(",", Recipients.Channels.Select(g => g.TeamId + ";" + g.Channel.Id));
            }

            var messageTypeChoices = new List<AdaptiveChoice>();
            messageTypeChoices.Add(new AdaptiveChoice() { Title = "❕ Important", Value = "Important" });
            messageTypeChoices.Add(new AdaptiveChoice() { Title = "❗ Emergency", Value = "Emergency" });
            messageTypeChoices.Add(new AdaptiveChoice() { Title = "📄 Information", Value = "Information" });
            var newCampaignCard = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {

                    new AdaptiveContainer()
                    {
                         Items=new List<AdaptiveElement>()
                         {
                           new AdaptiveImage()
                            {
                                Url=new System.Uri(ApplicationSettings.BaseUrl + "/Resources/CreateMessageHeader.png")
                            },
                            new AdaptiveTextBlock()
                            {
                                Text = "This announcement will be sent from the Contoso Broadcast app handle. Fields marked with (*) are mandatory while composing.",
                                Size = AdaptiveTextSize.Small,
                                Color = AdaptiveTextColor.Accent,
                                Spacing = AdaptiveSpacing.Small,
                                Weight = AdaptiveTextWeight.Bolder,
                                IsSubtle = true,
                                Wrap = true,

                            },
                            new AdaptiveTextBlock()
                            {
                                Separator=true,

                                Spacing=AdaptiveSpacing.Large,
                                Size=AdaptiveTextSize.Large,
                                Color=AdaptiveTextColor.Accent,
                                Text="Choose Audience"

                            },
                            new AdaptiveColumnSet()
                            {
                                Columns=new List<AdaptiveColumn>()
                                {
                                    new AdaptiveColumn()
                                    {
                                        Items=new List<AdaptiveElement>()
                                        {
                                           new AdaptiveTextBlock()
                                           {
                                               Text="Choose group(s) of people",
                                               HorizontalAlignment=AdaptiveHorizontalAlignment.Left,
                                           }
                                        },
                                        Width="50",
                                    },
                                    new AdaptiveColumn()
                                    {
                                        Items=new List<AdaptiveElement>()
                                        {
                                            new AdaptiveChoiceSetInput()
                                            {
                                               Id="groups",
                                               Spacing=AdaptiveSpacing.None,

                                               Choices=new List<AdaptiveChoice>(employeGroups),
                                               IsMultiSelect=true,
                                               Style=AdaptiveChoiceInputStyle.Compact,
                                               Value = isEditCard? groupRecipients:""
                                            },
                                        },
                                        Width="50"
                                    }
                                }
                            },
                            new AdaptiveColumnSet()
                            {
                                Columns=new List<AdaptiveColumn>()
                                {
                                    new AdaptiveColumn()
                                    {
                                        Items=new List<AdaptiveElement>()
                                        {
                                           new AdaptiveTextBlock()
                                           {
                                               Text="Choose channel(s) in Teams",
                                               HorizontalAlignment=AdaptiveHorizontalAlignment.Left,
                                           }
                                        },
                                        Width="50",
                                    },
                                    new AdaptiveColumn()
                                    {
                                        Items=new List<AdaptiveElement>()
                                        {
                                            new AdaptiveChoiceSetInput()
                                            {
                                               Id="channels",
                                               Spacing=AdaptiveSpacing.None,
                                               Value = isEditCard? channelRecipients:"",
                                               Choices =new List<AdaptiveChoice>(channels),
                                               IsMultiSelect=true,
                                               Style=AdaptiveChoiceInputStyle.Compact

                                            },
                                        },
                                        Width="50"
                                    }
                                }
                            },
                            new AdaptiveContainer()
                            {
                                Spacing=AdaptiveSpacing.Large,
                                Separator=true,
                                 Items=new List<AdaptiveElement>()
                                 {
                                     new AdaptiveTextBlock()
                                     {
                                         Size=AdaptiveTextSize.Large,
                                         Color=AdaptiveTextColor.Accent,
                                         Text="Compose Message"
                                     },
                                     new AdaptiveColumnSet()
                                     {
                                         Columns=new List<AdaptiveColumn>()
                                         {
                                             new AdaptiveColumn()
                                             {
                                                 Items=new List<AdaptiveElement>()
                                                 {
                                                     new AdaptiveTextBlock()
                                                     {
                                                         Text="Title*",
                                                         HorizontalAlignment=AdaptiveHorizontalAlignment.Left,
                                                     }
                                                 },
                                                 Width="20"
                                             },
                                             new AdaptiveColumn()
                                             {
                                                 Items=new List<AdaptiveElement>()
                                                 {

                                                     new AdaptiveTextInput()
                                                     {
                                                        Id="title",
                                                        Placeholder="eg: Giving Campaign 2018 is here",
                                                        Value = isEditCard? Title: ""

                                                     }
                                                 },
                                              Width="85"
                                             }

                                         }


                                     },
                                     new AdaptiveColumnSet()
                                     {
                                         Columns=new List<AdaptiveColumn>()
                                         {
                                             new AdaptiveColumn()
                                             {
                                                Items=new List<AdaptiveElement>()
                                                 {
                                                    new AdaptiveTextBlock()
                                                    {
                                                        Text="Sub-Title",
                                                        HorizontalAlignment=AdaptiveHorizontalAlignment.Left,
                                                    }
                                                 },
                                                Width="20"

                                             },
                                             new AdaptiveColumn()
                                             {
                                                 Items=new List<AdaptiveElement>()
                                                 {
                                                     new AdaptiveTextInput()
                                                     {
                                                         Id="subTitle",
                                                         Placeholder="eg: Have you contributed to the mission?",
                                                         Value = isEditCard? SubTitle: ""
                                                     }
                                                 },
                                                 Width="85"
                                             }
                                         }
                                     },
                                     new AdaptiveColumnSet()
                                     {
                                         Columns=new List<AdaptiveColumn>()
                                         {
                                             new AdaptiveColumn()
                                             {
                                                Items=new List<AdaptiveElement>()
                                                 {
                                                    new AdaptiveTextBlock()
                                                    {
                                                        Text="Image*",
                                                        HorizontalAlignment=AdaptiveHorizontalAlignment.Left,
                                                    }
                                                 },
                                                Width="20"
                                             },
                                             new AdaptiveColumn()
                                             {
                                                 Items=new List<AdaptiveElement>()
                                                 {
                                                     new AdaptiveTextInput()
                                                     {
                                                         Id="image",
                                                         Placeholder="eg: https:// URL to image of exact size 530px X 95px",
                                                         Value =  isEditCard? ImageUrl: ""
                                                     }
                                                 },
                                                  Width="85"
                                             }
                                         }
                                     },
                                     new AdaptiveColumnSet()
                                     {
                                         Columns=new List<AdaptiveColumn>()
                                         {
                                             new AdaptiveColumn()
                                             {
                                                Items=new List<AdaptiveElement>()
                                                 {
                                                    new AdaptiveTextBlock()
                                                    {
                                                        Text="Author alias*",
                                                        HorizontalAlignment=AdaptiveHorizontalAlignment.Left,
                                                    }
                                                 },
                                                Width="20"
                                             },
                                             new AdaptiveColumn()
                                             {
                                                 Items=new List<AdaptiveElement>()
                                                 {
                                                     new AdaptiveTextInput()
                                                     {
                                                         Id="authorAlias",
                                                         Placeholder="eg: author@yourorg.com",
                                                         Value =  isEditCard? Author?.EmailId: ""
                                                     }
                                                 },
                                                  Width="85"
                                             }
                                         }
                                     },
                                     new AdaptiveColumnSet()
                                     {
                                         Columns=new List<AdaptiveColumn>()
                                         {
                                             new AdaptiveColumn()
                                             {
                                                Items=new List<AdaptiveElement>()
                                                 {
                                                    new AdaptiveTextBlock()
                                                    {
                                                        Text="Preview*",
                                                        HorizontalAlignment=AdaptiveHorizontalAlignment.Left,
                                                    }
                                                 },
                                                Width="20"

                                             },
                                             new AdaptiveColumn()
                                             {
                                                 Items=new List<AdaptiveElement>()
                                                 {
                                                     new AdaptiveTextInput()
                                                     {
                                                         Id="preview",
                                                         Placeholder="eg: The 2018 Employee Giving Campaign is officially underway! Our incredibly generous culture of employee giving is unique to Contoso, and has a long history going back to our founder and his family’s core belief and value in philanthropy. Individually and collectively, we can have an incredible impact no matter how we choose to give. We are all very fortunate and 2018 has been a good year for the company which we are all participating in. Having us live in a community with a strong social safety net benefits us all so lets reflect our participation in this year's success with participation in Give.",
                                                         IsMultiline=true,
                                                         Value =  isEditCard? Preview: ""
                                                     }
                                                 },
                                                  Width="85"
                                             }
                                         }
                                     },
                                      new AdaptiveColumnSet()
                                     {
                                         Columns=new List<AdaptiveColumn>()
                                         {
                                             new AdaptiveColumn()
                                             {
                                                Items=new List<AdaptiveElement>()
                                                 {
                                                    new AdaptiveTextBlock()
                                                    {
                                                        Text="Body*",
                                                        HorizontalAlignment=AdaptiveHorizontalAlignment.Left,
                                                    }
                                                 },
                                                Width="20"

                                             },
                                             new AdaptiveColumn()
                                             {
                                                 Items=new List<AdaptiveElement>()
                                                 {
                                                     new AdaptiveTextInput()
                                                     {
                                                         Id="body",
                                                         Placeholder="eg: I hope you will take advantage of some of the fun and impactful opportunities that our giving team has put together and I’d like to thank our VPALs John Doe and Jason Natie for all the hard work they've put into planning these events for our team. To find out more about these opportunities, look for details in Give 2018 > General channel.",
                                                         IsMultiline=true,
                                                         Value =  isEditCard? Body: ""
                                                     }
                                                 },
                                                  Width="85"
                                             }
                                         }
                                     },
                                 }


                            }
                         }
                    },
                    new AdaptiveContainer()
                    {
                        Spacing=AdaptiveSpacing.Large,
                        Separator=true,
                        Items=new List<AdaptiveElement>()
                        {
                            new AdaptiveTextBlock()
                            {
                                Size=AdaptiveTextSize.Large,
                                Color=AdaptiveTextColor.Accent,
                                Text="Choose Message Properties"
                            },

                        },

                    },
                    new AdaptiveContainer()
                    {
                        Spacing=AdaptiveSpacing.Medium,
                        Items=new List<AdaptiveElement>()
                        {
                            new AdaptiveColumnSet()
                            {
                                Columns=new List<AdaptiveColumn>()
                                {
                                    new AdaptiveColumn()
                                    {

                                        Items=new List<AdaptiveElement>()
                                        {
                                            new AdaptiveToggleInput()
                                            {
                                                Id="acknowledge",
                                                Title="Request acknowledgement",
                                                Value= isEditCard? IsAcknowledgementRequested.ToString(): "false",
                                            }
                                        }
                                    },
                                    new AdaptiveColumn()
                                    {
                                        Items=new List<AdaptiveElement>()
                                        {
                                            new AdaptiveToggleInput()
                                            {
                                                Id="allowContactIns",
                                                Title="Allow Contact-ins",
                                                Value=isEditCard? IsContactAllowed.ToString(): "false",
                                            }
                                        },
                                        Width="stretch"
                                    }

                                }
                            },
                            new AdaptiveColumnSet()
                            {
                                Columns=new List<AdaptiveColumn>()
                                {
                                    new AdaptiveColumn()
                                    {
                                        Items=new List<AdaptiveElement>()
                                        {
                                            new AdaptiveTextBlock()
                                            {
                                                HorizontalAlignment=AdaptiveHorizontalAlignment.Left,
                                                Text="Choose Message Sensitivity",
                                            }
                                        },
                                        Width="50"
                                    },
                                    new AdaptiveColumn()
                                    {
                                        Items=new List<AdaptiveElement>()
                                        {
                                            new AdaptiveChoiceSetInput()
                                            {
                                               Id="messageType",
                                               Spacing=AdaptiveSpacing.None,
                                               Value= isEditCard?  Sensitivity.ToString() : "Important",
                                               Choices=new List<AdaptiveChoice>(messageTypeChoices),
                                               IsMultiSelect=false,
                                               Style=AdaptiveChoiceInputStyle.Compact,
                                            },
                                        },
                                        Width="50"
                                    }
                                }
                            }
                        }
                    }
                },
                Actions = new List<AdaptiveAction>()
                {
                    new AdaptiveSubmitAction()
                    {
                        Title="✔️ Preview",
                        Data = isEditCard?
                        new AnnouncementActionDetails(){  ActionType = Constants.CreateOrEditAnnouncement, Id = Id }
                        :new ActionDetails(){  ActionType = Constants.CreateOrEditAnnouncement }
                    },
                    new AdaptiveSubmitAction()
                    {
                        Title="❌ Cancel",
                        Data = new ActionDetails(){  ActionType = Constants.Cancel}
                    }
                }
            };

            return newCampaignCard;
        }

        public override AdaptiveCard GetPreviewCard()
        {
            string broadcastType = " ❗ EMERGENCY BROADCAST";
            AdaptiveTextColor broadcaseColor = AdaptiveTextColor.Attention;
            switch (Sensitivity)
            {
                case MessageSensitivity.Information:
                    broadcastType = " 📄 INFORMATION BROADCAST";
                    broadcaseColor = AdaptiveTextColor.Good;
                    break;
                case MessageSensitivity.Important:
                    broadcastType = " ❕ IMPORTANT BROADCAST";
                    broadcaseColor = AdaptiveTextColor.Warning;
                    break;
                default:
                    break;
            }
            AdaptiveCardRenderer renderer = new AdaptiveCardRenderer();
            var previewCard = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveContainer()
                    {
                        Items=new List<AdaptiveElement>()
                        {
                            new AdaptiveTextBlock()
                            {
                                Id = "sensitivity",
                                Text=broadcastType,
                                Wrap=true,
                                HorizontalAlignment=AdaptiveHorizontalAlignment.Left,
                                Spacing=AdaptiveSpacing.None,
                                Weight=AdaptiveTextWeight.Bolder,
                                Color=broadcaseColor,
                                MaxLines=1
                            }
                        }
                    },
                    new AdaptiveContainer()
                    {
                    },
                    new AdaptiveTextBlock()
                    {
                        Id = "title",
                        Text= Title, //  $"Giving Campaign 2018 is here",
                        Size=AdaptiveTextSize.ExtraLarge,
                        Weight=AdaptiveTextWeight.Bolder,
                        Wrap = true

                    },

                    new AdaptiveTextBlock()
                    {
                        Id = "subTitle",
                        Text=SubTitle, //  $"Have you contributed to Contoso's mission this year?",
                        Size=AdaptiveTextSize.Medium,
                        Spacing=AdaptiveSpacing.None,
                        Wrap = true,
                        IsSubtle=true
                    },
                    new AdaptiveImage()
                            {
                                Id = "bannerImage",
                                Url = Uri.IsWellFormedUriString(ImageUrl,UriKind.Absolute)? new Uri(ImageUrl) : null, //"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSG-vkjeuIlD-up_-VHCKgcREhFGp27lDErFkveBLQBoPZOHwMbjw"),
                                Size=AdaptiveImageSize.Stretch
                            },
                    new AdaptiveColumnSet()
                            {
                                Columns=new List<AdaptiveColumn>()
                                {
                                    new AdaptiveColumn()
                                    {
                                         Width=AdaptiveColumnWidth.Auto,
                                         Items=new List<AdaptiveElement>()
                                         {
                                             // Need to fetch this from Graph API.
                                             new AdaptiveImage(){Id = "profileImage", Url=  Uri.IsWellFormedUriString(Author?.ProfilePhoto,UriKind.Absolute)? new Uri(Author?.ProfilePhoto) : null, Size=AdaptiveImageSize.Small,Style=AdaptiveImageStyle.Person } //   new Uri("https://pbs.twimg.com/profile_images/3647943215/d7f12830b3c17a5a9e4afcc370e3a37e_400x400.jpeg"),Size=AdaptiveImageSize.Small,Style=AdaptiveImageStyle.Person 
                                         }
                                    },
                                    new AdaptiveColumn()
                                    {
                                         Width=AdaptiveColumnWidth.Auto,
                                         Items=new List<AdaptiveElement>()
                                         {
                                             new AdaptiveTextBlock(){
                                                 Id = "authorName",
                                                 Text = Author?.Name, // "SERENA RIBEIRO",
                                                 Weight =AdaptiveTextWeight.Bolder,Wrap=true},
                                             new AdaptiveTextBlock(){
                                                 Id = "authorRole",
                                                 Text = Author?.Role, //"Chief of Staff, Contoso Management",
                                                 Size =AdaptiveTextSize.Small,Spacing=AdaptiveSpacing.None,IsSubtle=true,Wrap=true}
                                         }
                                    }
                                }
                            },
                    new AdaptiveContainer()
                    {
                        Items=new List<AdaptiveElement>()
                        {
                            new AdaptiveTextBlock()
                            {
                                Id = "mainText",
                                Text=  ShowAllDetailsButton ? Preview : Body , // "The 2018 Employee Giving Campaign is officially underway!  Our incredibly generous culture of employee giving is unique to Contoso, and has a long history going back to our founder and his family’s core belief and value in philanthropy.   Individually and collectively, we can have an incredible impact no matter how we choose to give.  We are all very fortunate and 2018 has been a good year for the company which we are all participating in.  Having us live in a community with a strong social safety net benefits us all so lets reflect our participation in this year's success with participation in Give.",
                                Wrap=true
                            }
                        }
                    }
                },
            };
            try
            {
                // Render the card
                RenderedAdaptiveCard renderedCard = renderer.RenderCard(previewCard);

                // Get the output HTML 
                HtmlTag html = renderedCard.Html;
                              // (Optional) Check for any renderer warnings
                // This includes things like an unknown element type found in the card
                // Or the card exceeded the maxmimum number of supported actions, etc
                IList<AdaptiveWarning> warnings = renderedCard.Warnings;
            }
            catch (AdaptiveException ex)
            {
                throw ex;
                // Failed rendering
            }

            AdaptiveElement adaptiveElement = previewCard.Body.FirstOrDefault(i => i.Id == "bannerImage");
            if(!Uri.IsWellFormedUriString(ImageUrl, UriKind.Absolute) && adaptiveElement != null )
            {
                previewCard.Body.Remove(adaptiveElement);
            }

            previewCard.Actions = new List<AdaptiveAction>();
            if (ShowAllDetailsButton)
            {
                previewCard.Actions.Add(new AdaptiveSubmitAction()
                {
                    Id = "moreDetails",
                    Title = "More Details",
                    Data = new AdaptiveCardValue<ActionDetails>() { Data = new AnnouncementActionDetails() { ActionType = Constants.ShowMoreDetails, Id = Id } }
                });

                if (IsAcknowledgementRequested)
                {
                    previewCard.Actions.Add(new AdaptiveSubmitAction()
                    {
                        Id = "acknowledge",
                        Title = Constants.Acknowledge,
                        Data = new AnnouncementActionDetails() { ActionType = Constants.Acknowledge, Id = Id }
                    });
                }
                if (IsContactAllowed)
                {
                    previewCard.Actions.Add(new AdaptiveOpenUrlAction()
                    {
                        Id = "contactSender",
                        Title = Constants.ContactSender,
                        Url = new Uri($"https://teams.microsoft.com/l/chat/0/0?users={OwnerId}")
                    });
                }
            }

            return previewCard;
        }
    }
}