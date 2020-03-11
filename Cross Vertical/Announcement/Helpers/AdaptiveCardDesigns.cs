using AdaptiveCards;
using CrossVertical.Announcement.Helper;
using CrossVertical.Announcement.Models;
using CrossVertical.Announcement.Repository;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskModule;

namespace CrossVertical.Announcement.Helpers
{
    public class AdaptiveCardDesigns
    {
        public static Attachment GetWelcomeScreen(bool isChannelCard)
        {
            var card = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveContainer()
                    {
                        Items=new List<AdaptiveElement>()
                        {
                            new AdaptiveImage()
                            {
                                Url=new System.Uri(ApplicationSettings.BaseUrl + "/Resources/Announce-Header.png")
                            },
                            new AdaptiveTextBlock()
                            {
                                Weight=AdaptiveTextWeight.Bolder,
                                Text="Reach people right where they collaborate. "

                            },
                            new AdaptiveTextBlock()
                            {
                                IsSubtle=true,
                                Text="Get the message out to employees using Microsoft Teams. Send announcements to a set of employees, stores, roles or locations in one or more channels or individually.\nUsing this app, you can:",
                                Wrap=true
                            },
                            new AdaptiveTextBlock()
                            {
                                Size=AdaptiveTextSize.Small,
                                IsSubtle=true,
                                Wrap=true,
                                Text="* Collaborate and communicate with large employee groups\n* Target announcements via 1:1 chats for select employees\n* Post in Channels to encourage discussion and feedback\n* Deliver announcements to desktop, web clients or mobile clients of Microsoft Teams  – wherever users are\n* Track and report employee engagement on what you post\n* Track and report employee’s “read receipt if requested "
                            },
                            new AdaptiveTextBlock()
                            {
                                Text= isChannelCard ? "Note: This application works only in personal scope." : "Take your pick to get started:",
                                IsSubtle=true,

                            }
                        }
                    }
                }
            };
            if (isChannelCard)
            {
                card.Actions.Add(new AdaptiveOpenUrlAction()
                {
                    Id = "chatinpersonal",
                    Title = "Go to Personal App",
                    Url = new System.Uri($"https://teams.microsoft.com/l/chat/0/0?users=28:{ApplicationSettings.AppId}")
                });

            }
            else
                card.Actions = new List<AdaptiveAction>()
                {
                    new AdaptiveSubmitAction()
                    {
                        Id = "createmessage",
                        Title = "📢 Create Message",
                        Data = new AdaptiveCardValue<ActionDetails>()
                        { Data = new ActionDetails() { ActionType = Constants.CreateOrEditAnnouncement } }
                    },
                        new AdaptiveSubmitAction()
                        {
                            Id="showdrafts",
                            Title="⏱️ View Drafts & Scheduled",
                            Data = new ActionDetails() { ActionType = Constants.ShowAllDrafts}
                        },
                        new AdaptiveOpenUrlAction()
                        {
                            Id="viewall",
                            Title="📄 View All", // Take to Tab
                            Url = new System.Uri(Constants.HistoryTabDeeplink)
                        },
                        //,
                        //new AdaptiveSubmitAction()
                        //{
                        //    Id="viewstatistics",
                        //    Title="👁‍🗨 View Statistics"
                        //},
                        new AdaptiveSubmitAction()
                        {
                            Id = "adminpanel",
                            Title = "⚙️ Admin Panel",
                            Data = new ActionDetails() { ActionType = Constants.Configure }
                        }
                };

            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
        }

        public static async Task<Attachment> GetEditAnnouncementCard(string announcementId, string tenantId)
        {
            var campaign = await Cache.Announcements.GetItemAsync(announcementId);

            var tenant = await Cache.Tenants.GetItemAsync(tenantId);

            var groups = new List<Group>();
            foreach (var groupID in tenant.Groups)
            {
                groups.Add(await Cache.Groups.GetItemAsync(groupID));
            }

            var teams = new List<Team>();
            foreach (var teamID in tenant.Teams)
            {
                teams.Add(await Cache.Teams.GetItemAsync(teamID));
            }

            return campaign.GetCreateNewCard(groups, teams, true).ToAttachment();
        }

        public static async Task<Attachment> GetEditAnnouncementCardForTab(string announcementId, string tenantId)
        {
            var card = await GetEditAnnouncementCard(announcementId, tenantId);
            return SetSubmitAction(card, new AnnouncementActionDetails()
            {
                ActionType = Constants.EditAnnouncementFromTab,
                Id = announcementId
            });
        }

        private static Attachment SetSubmitAction(Attachment card, ActionDetails action)
        {
            var campaign = card.Content as AdaptiveCard;
            var submitAction = campaign.Actions.FirstOrDefault();
            if (submitAction != null && submitAction.Title == "✔️ Preview")// TODO: change this
            {
                var acknowledgeAction = submitAction as AdaptiveSubmitAction;
                acknowledgeAction.Data = action;
            }
            return card;
        }

        public static async Task<Attachment> GetTemplateCard(string announcementId, string tenantId)
        {
            var card = await GetEditAnnouncementCard(announcementId, tenantId);

            return SetSubmitAction(card, new ActionDetails() { ActionType = Constants.CreateOrEditAnnouncement });
        }

        public static async Task<Attachment> GetPreviewAnnouncementCard(string announcementId)
        {
            var campaign = await Cache.Announcements.GetItemAsync(announcementId);

            campaign.ShowAllDetailsButton = false;
            var card = campaign.GetPreviewCard().ToAttachment();
            campaign.ShowAllDetailsButton = true;
            return card;
        }

        public static async Task<Attachment> GetCreateNewAnnouncementCard(string tenantId)
        {
            var tenant = await Cache.Tenants.GetItemAsync(tenantId);

            var groups = new List<Group>();
            foreach (var groupID in tenant.Groups)
            {
                groups.Add(await Cache.Groups.GetItemAsync(groupID));
            }

            var teams = new List<Team>();
            foreach (var teamID in tenant.Teams)
            {
                teams.Add(await Cache.Teams.GetItemAsync(teamID));
            }
            return new Campaign().GetCreateNewCard(groups, teams, false).ToAttachment();
        }

        public static async Task<Attachment> GetScheduleConfirmationCard(string announcementId)
        {
            var campaign = await Cache.Announcements.GetItemAsync(announcementId);
            var date = campaign.Schedule.ScheduledTime.ToString("MM/dd/yyyy");
            var time = campaign.Schedule.ScheduledTime.ToString("HH:mm");

            var card = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveContainer()
                                        {
                                            Items=new List<AdaptiveElement>()
                                            {
                                                new AdaptiveTextBlock()
                                                {
                                                    Text="Please select schedule for your anouncement:"
                                                },
                                                new AdaptiveDateInput()
                                                {
                                                    Id = "Date",
                                                    Value = date
                                                },
                                                new AdaptiveTimeInput()
                                                {
                                                    Id = "Time",
                                                    Value = time
                                                }
                                            }
                                        }
                },
                Actions = new List<AdaptiveAction>()
                          {
                            new AdaptiveSubmitAction()
                            {
                                Id = "sendNow",
                                Title = "Send Now",
                                Data = new AnnouncementActionDetails()
                                {
                                    ActionType = Constants.SendAnnouncement,
                                    Id = announcementId
                                }
                            },
                            new AdaptiveSubmitAction()
                            {
                                Id= "schedule",
                                Title="Schedule",
                                Data = new AnnouncementActionDetails()
                                {
                                    Id = announcementId,
                                    ActionType = Constants.ScheduleAnnouncement
                                }
                            }
                          }
            };

            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
        }

        public static Attachment GetScheduleConfirmationCard(string announcementId, string date, string time, bool allowEdit)
        {
            var Card = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveContainer()
                    {
                        Items=new List<AdaptiveElement>()
                        {
                            new AdaptiveTextBlock()
                            {
                                Weight=AdaptiveTextWeight.Bolder,
                                Text="Please select your action:"
                            }
                        }
                    }
                },
                Actions = new List<AdaptiveAction>()
                          {
                            new AdaptiveSubmitAction()
                            {
                                Id = "sendNow",
                                Title = "Send Now",
                                Data = new AnnouncementActionDetails()
                                {
                                    ActionType = Constants.SendAnnouncement ,
                                    Id = announcementId
                                }
                            },
                            new AdaptiveShowCardAction()
                            {
                                Id = "sendLater",
                                Title="Send Later",
                                Card=new AdaptiveCard()
                                {
                                    Body=new List<AdaptiveElement>()
                                    {
                                        new AdaptiveContainer()
                                        {
                                            Items=new List<AdaptiveElement>()
                                            {
                                                new AdaptiveTextBlock()
                                                {
                                                    Text="Schedule your anouncement here"
                                                },
                                                new AdaptiveDateInput()
                                                {
                                                    Id = "Date",
                                                    Placeholder="Select Date",
                                                    Value = date
                                                },
                                                new AdaptiveTimeInput()
                                                {
                                                    Id = "Time",
                                                    Placeholder="Select time",
                                                    Value = time
                                                }
                                            }
                                        }
                                    },
                                    Actions=new List<AdaptiveAction>()
                                    {
                                      new AdaptiveSubmitAction()
                                      {
                                          Id= "schedule",
                                          Title="Schedule",
                                          Data = new AnnouncementActionDetails(){ Id = announcementId,  ActionType = Constants.ScheduleAnnouncement}
                                      }
                                    }
                                }
                              }

                          }
            };

            if (allowEdit)
            {
                Card.Actions.Add(
                new AdaptiveSubmitAction()
                {
                    Id = "editAnnouncement",
                    Title = "Edit",
                    Data = new AdaptiveCardValue<AnnouncementActionDetails>()
                    {
                        Data = new AnnouncementActionDetails()
                        {
                            ActionType = Constants.ShowEditAnnouncementTaskModule,
                            Id = announcementId
                        }
                    }
                });
            }

            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = Card
            };
        }

        public static Attachment GetUpdateMessageCard(string message)
        {
            var Card = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveContainer()
                    {
                        Items=new List<AdaptiveElement>()
                        {
                            new AdaptiveTextBlock()
                            {
                                Weight=AdaptiveTextWeight.Bolder,
                                Text=message,
                                Wrap = true
                            }
                        }
                    }
                }
            };

            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = Card
            };
        }

        public static Attachment GetCardWithAcknowledgementDetails(Attachment campaignAttachment, string id, string userId, string groupId)
        {
            var campaign = campaignAttachment.Content as AdaptiveCard;
            var action = campaign.Actions.FirstOrDefault(a => a.Title == Constants.Acknowledge);
            if (action != null)
            {
                var acknowledgeAction = action as AdaptiveSubmitAction;
                acknowledgeAction.Data = new AnnouncementAcknowledgeActionDetails()
                {
                    ActionType = Constants.Acknowledge,
                    Id = id,
                    GroupId = groupId,
                    UserId = userId
                };
            }
            return campaignAttachment;
        }

        public static Attachment GetCardToUpdatePreviewCard(Attachment campaignAttachment, string message)
        {

            var campaign = campaignAttachment.Content as AdaptiveCard;

            campaign.Body.Add(
                new AdaptiveContainer()
                {
                    Items = new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock()
                                    {
                                        Text= message,
                                        Wrap=true,
                                        HorizontalAlignment=AdaptiveHorizontalAlignment.Left,
                                        Spacing=AdaptiveSpacing.None,
                                        Weight=AdaptiveTextWeight.Bolder,
                                        Color= AdaptiveTextColor.Attention,
                                        MaxLines=1
                                    }
                                }
                });
            return campaignAttachment;
        }


        public static Attachment GetCardWithoutAcknowledgementAction(Attachment campaignAttachment)
        {
            var campaign = campaignAttachment.Content as AdaptiveCard;
            RemoveAction(campaign, Constants.Acknowledge);
            RemoveAction(campaign, Constants.ContactSender);
            return campaignAttachment;
        }

        private static void RemoveAction(AdaptiveCard campaign, string actionName)
        {
            var action = campaign.Actions.FirstOrDefault(a => a.Title == actionName);
            if (action != null)
            {
                campaign.Actions.Remove(action);
            }
        }

        internal static Attachment GetAnnouncementBasicDetails(Campaign campaign)
        {
            var basicDetailsCard = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
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
                                             new AdaptiveImage(){
                                                 Id = "profileImage",
                                                 Url =  Uri.IsWellFormedUriString(campaign.Author?.ProfilePhoto,UriKind.Absolute)?
                                                 new Uri(campaign.Author?.ProfilePhoto) : null,
                                                 Size =AdaptiveImageSize.Medium,Style=AdaptiveImageStyle.Person }

                                         }
                                    },
                                    new AdaptiveColumn()
                                    {
                                         Width=AdaptiveColumnWidth.Auto,
                                         Items=new List<AdaptiveElement>()
                                         {
                                             new AdaptiveTextBlock(){
                                                 Text = campaign.Title,
                                                 Weight =AdaptiveTextWeight.Bolder,Wrap=true},
                                             new AdaptiveTextBlock(){
                                                 Text =  "Author: " + campaign.Author?.Name,
                                                 Size = AdaptiveTextSize.Default,Spacing=AdaptiveSpacing.None,IsSubtle=true,Wrap=true
                                             },
                                              new AdaptiveTextBlock(){
                                                 Text =  $"Created Date: {campaign.CreatedTime.ToShortDateString()}",
                                                 Weight= AdaptiveTextWeight.Lighter,
                                                 Size = AdaptiveTextSize.Default,Wrap=true}
                                         }
                                    }
                                }
                            }
                },
            };
            return basicDetailsCard.ToAttachment();
        }
    }
}