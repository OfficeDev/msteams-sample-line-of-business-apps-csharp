using AdaptiveCards;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using ProfessionalServices.LeaveBot.Helper;
using ProfessionalServices.LeaveBot.Helpers;
using ProfessionalServices.LeaveBot.Models;
using System;
using System.Collections.Generic;
using TaskModule;

namespace ProfessionalServices.LeaveBot
{
    public class EchoBot
    {
        public static Attachment WelcomeLeaveCard(string userName, bool isManager)
        {
            var WelcomeCard = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveContainer
                    {
                        Items=new List<AdaptiveElement>()
                        {
                            new AdaptiveImage()
                            {
                                Url = new Uri(ApplicationSettings.BaseUrl + "/Resources/welcomebanner.png")
                            },
                            new AdaptiveTextBlock()
                            {
                                Text=$"Hey {userName}! Here is what I can do for you",
                                Size=AdaptiveTextSize.Large,
                                Wrap=true
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
                                             new AdaptiveImage(){Url=new Uri(ApplicationSettings.BaseUrl + "/Resources/Leaverequest.png"),Size=AdaptiveImageSize.Small,Style=AdaptiveImageStyle.Default, SelectAction=new AdaptiveSubmitAction(){ DataJson=@"{'Type':'" + Constants.LeaveRequest+"'}", Title="Leave Request"},HorizontalAlignment=AdaptiveHorizontalAlignment.Center, Spacing=AdaptiveSpacing.None }
                                         }
                                    },

                                    new AdaptiveColumn()
                                    {
                                         Width=AdaptiveColumnWidth.Auto,
                                         Items=new List<AdaptiveElement>()
                                         {
                                             new AdaptiveTextBlock(){Text="Leave Request",Color=AdaptiveTextColor.Accent,Size=AdaptiveTextSize.Medium, Spacing=AdaptiveSpacing.None, HorizontalAlignment=AdaptiveHorizontalAlignment.Center}
                                         },
                                           SelectAction = new AdaptiveSubmitAction()
                                         {
                                             DataJson=@"{'Type':'" + Constants.LeaveRequest+"'}", Title="Leave Request"
                                         }
                                    }
                                }
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
                                             new AdaptiveImage(){Url=new Uri(ApplicationSettings.BaseUrl + "/Resources/-g-Leave balance-01.png"),Size=AdaptiveImageSize.Small,Style=AdaptiveImageStyle.Default, SelectAction=new AdaptiveSubmitAction(){ DataJson= @"{'Type':'" + Constants.LeaveBalance+"'}",   Title= "Leave Balance"},HorizontalAlignment=AdaptiveHorizontalAlignment.Center,Spacing=AdaptiveSpacing.None}
                                         }
                                    },

                                    new AdaptiveColumn()
                                    {
                                         Width=AdaptiveColumnWidth.Auto,
                                         Items=new List<AdaptiveElement>()
                                         {
                                             new AdaptiveTextBlock(){Text="Leave Balance",Color=AdaptiveTextColor.Accent,Size=AdaptiveTextSize.Medium,HorizontalAlignment=AdaptiveHorizontalAlignment.Center,Spacing=AdaptiveSpacing.None }
                                         },
                                           SelectAction = new AdaptiveSubmitAction()
                                         {
                                             DataJson=@"{'Type':'" + Constants.LeaveBalance+"'}", Title="Leave Balance"
                                         }
                                    }
                                }
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
                                             new AdaptiveImage(){Url=new Uri(ApplicationSettings.BaseUrl + "/Resources/-g-Public holidays-01.png"),Size=AdaptiveImageSize.Small,Style=AdaptiveImageStyle.Default, SelectAction=new AdaptiveSubmitAction(){ DataJson=@"{'Type':'" + Constants.Holidays+"'}", Title="Holidays"},HorizontalAlignment=AdaptiveHorizontalAlignment.Center,Spacing=AdaptiveSpacing.None}
                                         }
                                    },

                                    new AdaptiveColumn()
                                    {
                                         Width=AdaptiveColumnWidth.Auto,
                                         Items=new List<AdaptiveElement>()
                                         {
                                             new AdaptiveTextBlock(){Text="Public Holidays",Color=AdaptiveTextColor.Accent,Size=AdaptiveTextSize.Medium,HorizontalAlignment=AdaptiveHorizontalAlignment.Center,Spacing=AdaptiveSpacing.None }
                                         },
                                         SelectAction = new AdaptiveSubmitAction()
                                         {
                                             DataJson=@"{'Type':'" + Constants.Holidays+"'}", Title="Holidays"
                                         }
                                    }
                                }
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
                                             new AdaptiveImage(){Url=new Uri(ApplicationSettings.BaseUrl + "/Resources/Help-01.png"),Size=AdaptiveImageSize.Small,Style=AdaptiveImageStyle.Default, SelectAction=new AdaptiveOpenUrlAction(){ Url=new Uri(DeeplinkHelper.HelpDeeplink), Title = "Help"},HorizontalAlignment=AdaptiveHorizontalAlignment.Center,Spacing=AdaptiveSpacing.None}
                                         }
                                    },

                                    new AdaptiveColumn()
                                    {
                                         Width=AdaptiveColumnWidth.Auto,
                                         Items=new List<AdaptiveElement>()
                                         {
                                             new AdaptiveTextBlock(){Text="Help",Color=AdaptiveTextColor.Accent,Size=AdaptiveTextSize.Medium,HorizontalAlignment=AdaptiveHorizontalAlignment.Center,Spacing=AdaptiveSpacing.None}
                                         },
                                           SelectAction = new AdaptiveOpenUrlAction()
                                         {
                                             Url=new Uri(DeeplinkHelper.HelpDeeplink),
                                             Title = "Help"
                                         }
                                    }
                                }
                            }
                        }
                    }
             }
            };

            if (isManager)
                (WelcomeCard.Body[0] as AdaptiveContainer).Items.Insert(3,
                    new AdaptiveColumnSet()
                    {
                        Columns = new List<AdaptiveColumn>()
                                {
                                    new AdaptiveColumn()
                                    {
                                         Width=AdaptiveColumnWidth.Auto,
                                         Items=new List<AdaptiveElement>()
                                         {
                                             new AdaptiveImage(){Url=new Uri(ApplicationSettings.BaseUrl + "/Resources/pendingapprovals.png"),Size=AdaptiveImageSize.Small,Style=AdaptiveImageStyle.Default, SelectAction=new AdaptiveSubmitAction(){ DataJson= @"{'Type':'" + Constants.ShowPendingApprovals+"'}", Title = "Pending Approvals"},HorizontalAlignment=AdaptiveHorizontalAlignment.Center,Spacing=AdaptiveSpacing.None }
                                         }
                                    },

                                    new AdaptiveColumn()
                                    {
                                         Width=AdaptiveColumnWidth.Auto,
                                         Items=new List<AdaptiveElement>()
                                         {
                                             new AdaptiveTextBlock(){Text="Pending Approvals",Color=AdaptiveTextColor.Accent,Size=AdaptiveTextSize.Medium,HorizontalAlignment=AdaptiveHorizontalAlignment.Center,Spacing=AdaptiveSpacing.None}
                                         },
                                           SelectAction = new AdaptiveSubmitAction()
                                         {
                                             DataJson=@"{'Type':'" + Constants.ShowPendingApprovals+"'}", Title="Pending Approvals"
                                         }
                                    }
                                }
                    }
                    );

            var acard = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = WelcomeCard
            };
            return acard;
        }

        public static Attachment SetManagerCard()
        {
            var Card = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                          {
                              new AdaptiveTextBlock(){Text="Enter Manager Email Id:",Id="Setmgr"},
                              new AdaptiveTextInput(){Id="txtManager", IsMultiline=false, Style = AdaptiveTextInputStyle.Email, IsRequired=true, Placeholder="Manager Email Id"}
                          },
                Actions = new List<AdaptiveAction>()
                          {
                              new AdaptiveSubmitAction()
                              {
                                  Title="Set Manager",
                                  DataJson= @"{'Type':'" + Constants.SetManager+"'}",
                                  Id="SetmgrAct"
                              }
                          }
            };

            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = Card
            };
        }

        public static Attachment LeaveRequest(LeaveDetails leaveDetails = null)
        {
            var durations = new List<AdaptiveChoice>();
            durations.Add(new AdaptiveChoice() { Title = "Full Day", Value = DayType.FullDay.ToString() });
            durations.Add(new AdaptiveChoice() { Title = "Half Day", Value = DayType.HalfDay.ToString() });

            var paidLeave = new AdaptiveChoice() { Title = "Paid Leave", Value = LeaveType.PaidLeave.ToString() };
            var sickLeave = new AdaptiveChoice() { Title = "Sick Leave", Value = LeaveType.SickLeave.ToString() };
            var optionalLeave = new AdaptiveChoice() { Title = "Optional Leave", Value = LeaveType.OptionalLeave.ToString() };
            var carriedOverLeave = new AdaptiveChoice() { Title = "Carried over from last year", Value = LeaveType.CarriedLeave.ToString() };

            var maternityLeave = new AdaptiveChoice() { Title = "Maternity Leave", Value = LeaveType.MaternityLeave.ToString() };
            var paternityLeave = new AdaptiveChoice() { Title = "Paternity Leave", Value = LeaveType.PaternityLeave.ToString() };
            var caregiverLeave = new AdaptiveChoice() { Title = "Caregiver Leave", Value = LeaveType.Caregiver.ToString() };

            var LeaveRequest = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveContainer
                    {
                        Items=new List<AdaptiveElement>()
                        {
                             new AdaptiveColumnSet()
                    {
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock(){Text="From", Weight=AdaptiveTextWeight.Lighter,Size=AdaptiveTextSize.Medium,Wrap=true,Id="MyText1" },
                                    new AdaptiveDateInput(){Id="FromDate",Placeholder="From Date", Value = leaveDetails?.StartDate.Date.ToUniversalTime().ToString("u") }
                                },
                                Id="MyCol2"
                            },
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Items=new List<AdaptiveElement>()
                                {
                                   new AdaptiveTextBlock(){Text="Duration", Weight=AdaptiveTextWeight.Lighter,Size=AdaptiveTextSize.Medium,Wrap=true, Id="MyTxt2" },
                                    new AdaptiveChoiceSetInput(){Id="FromDuration",  Choices=new List<AdaptiveChoice>(durations), IsMultiSelect=false,Style=AdaptiveChoiceInputStyle.Compact,
                                        Value =leaveDetails!=null? leaveDetails.StartDate.Type.ToString() : DayType.FullDay.ToString() }
                                },
                                Id="MyCol3"
                            }
                        },
                        Id="MyCol1"
                    },
                        }
                    },
                    new AdaptiveContainer
                    {
                        Items=new List<AdaptiveElement>()
                        {
                             new AdaptiveColumnSet()
                    {
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock(){Text="To", Weight=AdaptiveTextWeight.Lighter,Size=AdaptiveTextSize.Medium,Wrap=true,Id="MyTxt3" },
                                    new AdaptiveDateInput(){Id="ToDate",Placeholder="To Date", Value = leaveDetails?.EndDate.Date.ToUniversalTime().ToString("u")}
                                },
                                Id="MyCol4"
                            },
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Items=new List<AdaptiveElement>()
                                {
                                   new AdaptiveTextBlock(){Text="Duration", Weight=AdaptiveTextWeight.Lighter,Size=AdaptiveTextSize.Medium,Wrap=true,Id="MyTxt4" },
                                    new AdaptiveChoiceSetInput(){Id="ToDuration", Choices=new List<AdaptiveChoice>(durations), IsMultiSelect=false, Style=AdaptiveChoiceInputStyle.Compact,
                                        Value =leaveDetails!=null? leaveDetails.EndDate.Type.ToString() : DayType.FullDay.ToString()}
                                },
                                Id="MyCol5"
                            }
                        },
                        Id="MyCol6"
                    },
                        }
                    },
                    new AdaptiveContainer()
                    {
                        Items=new List<AdaptiveElement>()
                        {
                            new AdaptiveTextBlock(){Text="Please specify a reason for your leave", Color=AdaptiveTextColor.Accent, Weight=AdaptiveTextWeight.Lighter, Size=AdaptiveTextSize.Default,Id="MyTxt6"}
                        }
                    }
                },
                Actions = new List<AdaptiveAction>()
                {
                    new AdaptiveShowCardAction()
                    {
                        Title="Vacation",
                        Id="NewShow",
                         Card=new AdaptiveCard()
                       {
                          Body=new List<AdaptiveElement>()
                          {
                              new AdaptiveContainer
                                {
                                    Items=new List<AdaptiveElement>()
                                    {
                                            new AdaptiveColumnSet()
                                {
                                    Columns=new List<AdaptiveColumn>()
                                    {
                                        new AdaptiveColumn()
                                        {
                                            Spacing = AdaptiveSpacing.None,
                                            Width="auto",
                                            Items=new List<AdaptiveElement>()
                                            {
                                                new AdaptiveImage() { Url = new Uri(ApplicationSettings.BaseUrl + "/Resources/Vacation-01.png"),Id="NewImgId" }
                                            },
                                            Id="MyCol8"
                                        },
                                        new AdaptiveColumn()
                                        {
                                            Spacing = AdaptiveSpacing.Small,
                                            Items=new List<AdaptiveElement>()
                                            {
                                                new AdaptiveTextBlock(){Text="Yay! Have a great vacation!",Id="MyTxt7"}
                                            },
                                            Id="MyCol9"
                                        }
                                    },
                                    Id="MyCol7"
                                },
                                    }
                                }
                             ,
                              new AdaptiveChoiceSetInput(){Id="LeaveTypeVacation", Choices=new List<AdaptiveChoice>{paidLeave, optionalLeave, carriedOverLeave  } , IsMultiSelect=false, Style=AdaptiveChoiceInputStyle.Compact,
                                  Value  = leaveDetails?.LeaveCategory== LeaveCategory.Vacation?leaveDetails.LeaveType.ToString():"" , IsRequired=true},
                              new AdaptiveTextInput(){Id="LeaveReasonVacation", IsMultiline=true,MaxLength=300, IsRequired=true, Placeholder="Comments (Optional)",
                              Value = leaveDetails?.LeaveCategory== LeaveCategory.Vacation?leaveDetails.EmployeeComment:""}
                          },
                          Actions=new List<AdaptiveAction>()
                          {
                              new AdaptiveSubmitAction()
                              {
                                  Title="Submit",
                                  DataJson= @"{'Type':'" + Constants.ApplyForVacation+"' , 'LeaveId':'" + leaveDetails?.LeaveId +"' }",
                                  Id="ActId"
                              }
                          }
                       }
                    },
                     new AdaptiveShowCardAction()
                    {
                        Title="Sickness",
                        Id="Show2",
                         Card=new AdaptiveCard()
                       {
                          Body=new List<AdaptiveElement>()
                          {
                              new AdaptiveContainer
                                {
                                    Items=new List<AdaptiveElement>()
                                    {
                                            new AdaptiveColumnSet()
                                {
                                    Columns=new List<AdaptiveColumn>()
                                    {
                                        new AdaptiveColumn()
                                        {
                                            Spacing = AdaptiveSpacing.None,
                                            Width="auto",
                                            Items=new List<AdaptiveElement>()
                                            {
                                                new AdaptiveImage() { Url = new Uri(ApplicationSettings.BaseUrl + "/Resources/HeartIcon.png"),Id="MyImg2" }
                                            },
                                            Id="MyCol11"
                                        },
                                        new AdaptiveColumn()
                                        {
                                            Spacing = AdaptiveSpacing.Small,
                                            Items=new List<AdaptiveElement>()
                                            {
                                                new AdaptiveTextBlock(){Text="Get well soon!",Id="MyTxt9"}
                                            },
                                            Id="MyCol12"
                                        }
                                    },
                                    Id="MyCol10"
                                },
                                    }
                                }
                               ,
                              new AdaptiveChoiceSetInput(){Id="LeaveTypeSickness", Choices=new List<AdaptiveChoice>(){ sickLeave }, IsMultiSelect=false, Style=AdaptiveChoiceInputStyle.Compact,
                                  Value  = leaveDetails?.LeaveCategory== LeaveCategory.Sickness?leaveDetails.LeaveType.ToString():"" , IsRequired=true},
                              new AdaptiveTextInput(){Id="LeaveReasonSickness", IsMultiline=true,MaxLength=300, IsRequired=true, Placeholder="Comments (Optional)",
                               Value = leaveDetails?.LeaveCategory== LeaveCategory.Sickness?leaveDetails.EmployeeComment:""}
                          },
                          Actions=new List<AdaptiveAction>()
                          {
                              new AdaptiveSubmitAction()
                              {
                                  Title="Submit",
                                  DataJson= @"{'Type':'" + Constants.ApplyForSickLeave+"' , 'LeaveId':'" + leaveDetails?.LeaveId +"' }",
                                  Id="ActId2"
                              }
                          }
                       }
                    },
                     new AdaptiveShowCardAction()
                    {
                        Title="Personal",
                        Id="Show3",
                         Card=new AdaptiveCard()
                       {
                          Body=new List<AdaptiveElement>()
                          {
                              new AdaptiveContainer
                                {
                                    Items=new List<AdaptiveElement>()
                                    {
                                            new AdaptiveColumnSet()
                                {
                                    Columns=new List<AdaptiveColumn>()
                                    {
                                        new AdaptiveColumn()
                                        {
                                            Spacing = AdaptiveSpacing.None,
                                            Width="auto",
                                            Items=new List<AdaptiveElement>()
                                            {
                                                new AdaptiveImage() { Url = new Uri(ApplicationSettings.BaseUrl + "/Resources/Like.png"), Id="MyImg5" }
                                            },
                                            Id="MyCol14",
                                        },
                                        new AdaptiveColumn()
                                        {
                                            Spacing = AdaptiveSpacing.Small,
                                            Items=new List<AdaptiveElement>()
                                            {
                                                    new AdaptiveTextBlock(){Text="Go ahead",Id="MyImage"}
                                            },
                                            Id="MyCol15"
                                        }
                                    },
                                    Id="MyCol13"
                                },
                                    }
                                },
                              new AdaptiveChoiceSetInput(){Id="LeaveTypePersonal", Choices=new List<AdaptiveChoice>() { paidLeave, optionalLeave, carriedOverLeave }, IsMultiSelect=false, Style=AdaptiveChoiceInputStyle.Compact,
                                  Value =leaveDetails?.LeaveCategory== LeaveCategory.Personal?leaveDetails.LeaveType.ToString():"", IsRequired=true},
                              new AdaptiveTextInput(){Id="LeaveReasonPersonal", IsMultiline=true,MaxLength=300, IsRequired=true, Placeholder="Comments (Optional)",
                                  Value = leaveDetails?.LeaveCategory== LeaveCategory.Personal?leaveDetails.EmployeeComment:""}
                          },
                          Actions=new List<AdaptiveAction>()
                          {
                              new AdaptiveSubmitAction()
                              {
                                  Title="Submit",
                                  DataJson=   @"{'Type':'" + Constants.ApplyForPersonalLeave+"' , 'LeaveId':'" + leaveDetails?.LeaveId +"' }",
                                  Id="ActionSub"
                              }
                          }
                       }
                    },
                     new AdaptiveShowCardAction()
                    {
                        Title="Other",
                        Id="Show4",
                        Card=new AdaptiveCard()
                       {
                          Body=new List<AdaptiveElement>()
                          {
                              new AdaptiveTextInput(){Id="LeaveReasonOther", IsMultiline=true,MaxLength=300, IsRequired=true, Placeholder="Comments (Optional)",
                              Value = leaveDetails?.LeaveCategory== LeaveCategory.Other?leaveDetails.EmployeeComment:""},
                              new AdaptiveChoiceSetInput(){Id="LeaveTypeOther", Choices=new List<AdaptiveChoice>() { optionalLeave, maternityLeave, paternityLeave, caregiverLeave }, IsMultiSelect=false, Style=AdaptiveChoiceInputStyle.Compact,
                                  Value =leaveDetails?.LeaveCategory== LeaveCategory.Other?leaveDetails.LeaveType.ToString():"", IsRequired=true},
                          },
                          Actions=new List<AdaptiveAction>()
                          {
                              new AdaptiveSubmitAction()
                              {
                                  Title="Submit",
                                  DataJson= @"{'Type':'" + Constants.ApplyForOtherLeave+"' , 'LeaveId':'" + leaveDetails?.LeaveId +"' }",
                                  Id="Subact2"
                              }
                          }
                       }
                    }
                },
            };
            if (leaveDetails != null)
            {
                // Provide Withdraw option
            }
            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = LeaveRequest
            };
        }

        public static Attachment ManagerViewCard(Employee employee, LeaveDetails leaveDetails)
        {
            double dayCount = GetDayCount(leaveDetails);

            dayCount = Math.Round(dayCount, 1);

            var startDay = leaveDetails.StartDate.Date.ToString("dddd");
            var endDay = leaveDetails.EndDate.Date.ToString("dddd");

            var startDate = leaveDetails.StartDate.Date.ToString("MMM d");
            var endDate = leaveDetails.EndDate.Date.ToString("MMM d");

            var leaveType = GetDisplayText(leaveDetails.LeaveType);
            Uri photoUri = null;
            if (employee.PhotoPath != null)
                photoUri = new Uri(employee.PhotoPath);

            string leaveMessage = string.Empty;

            switch (leaveDetails.Status)
            {
                case LeaveStatus.Pending:
                    leaveMessage = $"{employee.DisplayName} has requested for {leaveType} for {dayCount} days";
                    break;

                case LeaveStatus.Rejected:
                case LeaveStatus.Approved:
                    leaveMessage = $"{employee.DisplayName}'s {dayCount} days {leaveType} is {leaveDetails.Status.ToString()}";
                    break;

                case LeaveStatus.Withdrawn:
                    leaveMessage = $"{employee.DisplayName} has withdrawn this leave.";
                    break;

                default:
                    break;
            }

            var card3 = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveContainer
                    {
                        Items=new List<AdaptiveElement>()
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
                                    new AdaptiveImage(){Size=AdaptiveImageSize.Large,Url=photoUri,
                                        Style =AdaptiveImageStyle.Person, Id="ImgPerson1"}
                                },
                                Id="Column1"
                            },
                            new AdaptiveColumn()
                            {
                                Spacing=AdaptiveSpacing.Large,
                                Width=AdaptiveColumnWidth.Stretch,
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock(){
                                    Text =leaveMessage,
                                    Size=AdaptiveTextSize.Medium,Wrap=true, Id="MyTxt"},

                                     new AdaptiveTextBlock(){Text=$"{startDay}   {endDay}", Size=AdaptiveTextSize.Default,Wrap=true, Id="Mytxt2"},
                                     new AdaptiveTextBlock(){Text=$"{startDate}   - {endDate}, {leaveDetails.EndDate.Date.Year}",Size=AdaptiveTextSize.Default,Wrap=true,Id="Mytxt3"},
                                     new AdaptiveTextBlock(){Text=$"Reason: {leaveType}",Weight=AdaptiveTextWeight.Bolder,Size=AdaptiveTextSize.Medium,Wrap=true,Id="Mytxt4"},
                                    new AdaptiveTextBlock(){Text=leaveDetails.EmployeeComment,HorizontalAlignment=AdaptiveHorizontalAlignment.Left,Wrap=true,Id="Mytxt5" }
                                },
                                Id="Column2",
                            }
                        },
                        Id="MyColSet1"
                    },
                        }
                    }
                },
                Actions = new List<AdaptiveAction>()
                {
                    new AdaptiveShowCardAction()
                    {
                        Title="Approve",
                        Id="Mgrcmt",
                         Card=new AdaptiveCard()
                       {
                          Body=new List<AdaptiveElement>()
                          {
                              new AdaptiveTextInput(){Id="ManagerCommentApprove", IsMultiline=true,MaxLength=300, IsRequired=true, Placeholder="Comments (Optional)"}
                          },
                          Actions=new List<AdaptiveAction>()
                          {
                              new AdaptiveSubmitAction()
                              {
                                  Title="Approve",
                                  DataJson= @"{'Type':'" + Constants.ApproveLeave+"', 'LeaveId':'" + leaveDetails.LeaveId+"'}",
                                  Id="Mgrsubmit"
                              }
                          }
                       }
                    },
                    new AdaptiveShowCardAction()
                    {
                        Title="Reject",
                        Id="Showreject",
                         Card=new AdaptiveCard()
                       {
                          Body=new List<AdaptiveElement>()
                          {
                              new AdaptiveTextInput(){Id="ManagerCommentReject", IsMultiline=true,MaxLength=300, IsRequired=true, Placeholder="Write a reason (Optional)"}
                          },
                          Actions=new List<AdaptiveAction>()
                          {
                              new AdaptiveSubmitAction()
                              {
                                  Title="Reject",
                                  DataJson= @"{'Type':'" + Constants.RejectLeave+"', 'LeaveId':'" + leaveDetails.LeaveId +"'}",
                                  Id="Mgrnewsubmit"
                              }
                          }
                       }
                    }
                },
            };
            if (leaveDetails.Status == LeaveStatus.Withdrawn || leaveDetails.Status == LeaveStatus.Approved || leaveDetails.Status == LeaveStatus.Rejected)
                card3.Actions = null;

            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card3
            };
        }

        public static Attachment EmployeeViewCard(Employee employee, LeaveDetails leaveDetails)
        {
            double dayCount = GetDayCount(leaveDetails);

            dayCount = Math.Round(dayCount, 1);

            var startDay = leaveDetails.StartDate.Date.ToString("dddd");
            var endDay = leaveDetails.EndDate.Date.ToString("dddd");

            var startDate = leaveDetails.StartDate.Date.ToString("MMM d");
            var endDate = leaveDetails.EndDate.Date.ToString("MMM d, yyyy");

            var leaveType = GetShortDisplayText(leaveDetails.LeaveType);
            Uri photoUri = null;

            if (employee.PhotoPath != null)
                photoUri = new Uri(employee.PhotoPath);

            var statusColor = AdaptiveTextColor.Warning;
            switch (leaveDetails.Status)
            {
                case LeaveStatus.Pending:
                    statusColor = AdaptiveTextColor.Warning;
                    break;

                case LeaveStatus.Approved:
                    statusColor = AdaptiveTextColor.Good;
                    break;

                case LeaveStatus.Rejected:
                    statusColor = AdaptiveTextColor.Attention;
                    break;

                case LeaveStatus.Withdrawn:
                    statusColor = AdaptiveTextColor.Accent;
                    break;

                default:
                    break;
            }

            var container = new AdaptiveContainer
            {
                Items = new List<AdaptiveElement>()
                        {
                             new AdaptiveColumnSet()
                             {
                                 Columns = new List<AdaptiveColumn>()
                                 {
                                     new AdaptiveColumn()
                                     {
                                         Spacing=AdaptiveSpacing.Large,
                                         Width=AdaptiveColumnWidth.Stretch,
                                         Items = new List<AdaptiveElement>()
                                         {
                                             new AdaptiveTextBlock(){Text=$"{startDate}   - {endDate}",Size=AdaptiveTextSize.Default,Wrap=true,Id="TxtDT"},
                                             new AdaptiveTextBlock(){Text=$"{dayCount} days OOF",Weight=AdaptiveTextWeight.Bolder,Size=AdaptiveTextSize.Medium,Wrap=true,Id="TxtEt"},
                                         },
                                         Id="Mynewcolu"
                                     }
                                 },
                                 Id="ColNew"
                             },

                             new AdaptiveColumnSet()
                             {
                                 Columns = new List<AdaptiveColumn>()
                                 {
                                     new AdaptiveColumn()
                                     {
                                         Width=AdaptiveColumnWidth.Stretch,
                                         Items = new List<AdaptiveElement>()
                                         {
                                             new AdaptiveTextBlock(){Text=$"{leaveType} : ", Weight=AdaptiveTextWeight.Bolder, Color = AdaptiveTextColor.Accent, Size=AdaptiveTextSize.Medium, Spacing = AdaptiveSpacing.Medium, Wrap=true,Id="Textu1"},
                                         },
                                         Id="Colu1"
                                     },
                                     new AdaptiveColumn()
                                     {
                                         Width=AdaptiveColumnWidth.Stretch,
                                         Items = new List<AdaptiveElement>()
                                         {
                                             new AdaptiveTextBlock(){Text=leaveDetails.EmployeeComment,HorizontalAlignment=AdaptiveHorizontalAlignment.Left,Wrap=true,Id="Textu2" }
                                         },
                                         Id="Colu2"
                                     }
                                 },
                                 Id="Colu3"
                             }
                             ,
                             new AdaptiveColumnSet()
                             {
                                 Columns = new List<AdaptiveColumn>()
                                 {
                                     new AdaptiveColumn()
                                     {
                                         Width=AdaptiveColumnWidth.Stretch,
                                         Items = new List<AdaptiveElement>()
                                         {
                                             new AdaptiveTextBlock(){Text=$"Approval Status:",Weight=AdaptiveTextWeight.Bolder, Size=AdaptiveTextSize.Medium,Wrap=true,Id="textu3"},
                                         },
                                         Id="Colu4"
                                     },
                                     new AdaptiveColumn()
                                     {
                                         Width=AdaptiveColumnWidth.Stretch,
                                         Items = new List<AdaptiveElement>()
                                         {
                                             new AdaptiveTextBlock(){Text=leaveDetails.Status.ToString(),
                                                 Color = statusColor,
                                                 HorizontalAlignment =AdaptiveHorizontalAlignment.Left,Wrap=true,Id="textu4" }
                                         },
                                          Id="Colu5"
                                     }
                                 },
                                 Id="Colu6"
                             }
                        }
            };

            var card3 = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    container
                },
                Actions = new List<AdaptiveAction>()
                {
                    new AdaptiveSubmitAction()
                    {
                        Title = "Edit",
                        Data = new AdaptiveCardValue<EditLeaveDetails>() { Data =  new EditLeaveDetails() { Type = Constants.EditLeave, LeaveId = leaveDetails.LeaveId }  },
                        Id="SubAct"
                    },
                    new AdaptiveSubmitAction()
                    {
                        Title = "Withdraw",
                        DataJson = @"{'Type':'" + Constants.Withdraw+"' , 'LeaveId':'" + leaveDetails?.LeaveId +"' }",
                        Id="SubAct2"
                    }
                },
            };

            if (!string.IsNullOrEmpty(leaveDetails.ManagerComment))
            {
                container.Items.Add(
                     new AdaptiveColumnSet()
                     {
                         Columns = new List<AdaptiveColumn>()
                                 {
                                     new AdaptiveColumn()
                                     {
                                         Width=AdaptiveColumnWidth.Stretch,
                                         Items = new List<AdaptiveElement>()
                                         {
                                             new AdaptiveTextBlock(){Text=$"Manager Comment:",Weight=AdaptiveTextWeight.Bolder, Size=AdaptiveTextSize.Medium,Wrap=true,Id="ttu1"},
                                         },
                                         Id="collu1"
                                     },
                                     new AdaptiveColumn()
                                     {
                                         Width=AdaptiveColumnWidth.Stretch,
                                         Items = new List<AdaptiveElement>()
                                         {
                                             new AdaptiveTextBlock(){Text=leaveDetails.ManagerComment.ToString(),
                                                 Color = statusColor,
                                                 HorizontalAlignment =AdaptiveHorizontalAlignment.Left,Wrap=true,Id="ttu2" }
                                         },
                                         Id="Collu2"
                                     }
                                 },
                         Id="Cool"
                     }
                     
                    );
            }

            if (leaveDetails.Status == LeaveStatus.Withdrawn)
                card3.Actions = null;

            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card3
            };
        }

        public static double GetDayCount(LeaveDetails leaveDetails)
        {
            var dayCount = (leaveDetails.EndDate.Date - leaveDetails.StartDate.Date).TotalDays + 1;
            if (leaveDetails.EndDate.Type == DayType.HalfDay)
                dayCount -= 0.5;
            if (leaveDetails.StartDate.Type == DayType.HalfDay)
                dayCount -= 0.5;
            return dayCount;
        }

        private static string GetDisplayText(LeaveType leaveDetails)
        {
            switch (leaveDetails)
            {
                case LeaveType.PaidLeave:
                    return "Paid Leave";

                case LeaveType.SickLeave:
                    return "Sick Leave";

                case LeaveType.OptionalLeave:
                    return "Optional Leave";

                case LeaveType.CarriedLeave:
                    return "Carried Leave";

                case LeaveType.MaternityLeave:
                    return "Maternity Leave";

                case LeaveType.PaternityLeave:
                    return "Paternity Leave";

                case LeaveType.Caregiver:
                    return "Caregiver Leave";

                default:
                    break;
            }
            return leaveDetails.ToString();
        }

        private static string GetShortDisplayText(LeaveType leaveDetails)
        {
            switch (leaveDetails)
            {
                case LeaveType.PaidLeave:
                    return "PL";

                case LeaveType.SickLeave:
                    return "SL";

                case LeaveType.OptionalLeave:
                    return "OL";

                case LeaveType.CarriedLeave:
                    return "CL";

                case LeaveType.MaternityLeave:
                    return "ML";

                case LeaveType.PaternityLeave:
                    return "PT";

                case LeaveType.Caregiver:
                    return "CG";

                default:
                    break;
            }
            return leaveDetails.ToString();
        }

        public static Attachment ViewLeaveBalance(Employee employee)
        {
            var LeaveBalanceCard = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveContainer
                    {
                        Items=new List<AdaptiveElement>()
                        {
                            new AdaptiveTextBlock()
                            {
                                Text="Here's your balance ",
                                Size=AdaptiveTextSize.Large,
                                Id="TxtBalance"
                            },

                             new AdaptiveColumnSet()
                    {
                                 Spacing=AdaptiveSpacing.ExtraLarge,
                                 Id="Blccolu",
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock(){Id="Blctxt",Text="Type", Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true, Spacing=AdaptiveSpacing.Padding,Color=AdaptiveTextColor.Accent, Weight=AdaptiveTextWeight.Lighter}
                                },
                                Id="Blccol"
                            },
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                     new AdaptiveTextBlock(){Id="Blctxt2",Text="Remaining", Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true,  Spacing=AdaptiveSpacing.Padding, Color=AdaptiveTextColor.Accent, Weight=AdaptiveTextWeight.Lighter}
                                },
                                Id="Blccol2"
                            }
                        },
                    },
                             new AdaptiveColumnSet()
                    {
                                 Separator=true,
                                 Spacing=AdaptiveSpacing.ExtraLarge,
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock(){Id="Blctxt3",Text="Paid leaves", Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true, Spacing=AdaptiveSpacing.Padding, Weight=AdaptiveTextWeight.Default}
                                },
                                Id="Blccol4"
                            },
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                     new AdaptiveTextBlock(){Id="Blctxt4",Text=employee.LeaveBalance.PaidLeave.ToString(), Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true,  Spacing=AdaptiveSpacing.Padding, Color=AdaptiveTextColor.Accent}
                                },
                                Id="Blccol5"
                            }
                        },
                        Id="Blccol3"
                    },
                             new AdaptiveColumnSet()
                    {
                                 Separator=true,
                                 Spacing=AdaptiveSpacing.ExtraLarge,
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                     new AdaptiveTextBlock(){Id="Blctxt5",Text="Sick leaves", Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true, Spacing=AdaptiveSpacing.Padding, Weight=AdaptiveTextWeight.Default}
                                },
                                Id="Blccol7"
                            },
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                     new AdaptiveTextBlock(){Id="Blctxt6",Text=employee.LeaveBalance.SickLeave.ToString(), Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true,  Spacing=AdaptiveSpacing.Padding, Color=AdaptiveTextColor.Accent}
                                },
                                Id="Blccol8"
                            }
                        },
                        Id="blccol6"
                    },
                             new AdaptiveColumnSet()
                    {
                                 Separator=true,
                                 Spacing=AdaptiveSpacing.ExtraLarge,
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                      new AdaptiveTextBlock(){Id="Blctxt7",Text="Carried over from last year Recommended to utilise for vacations", Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true, Spacing=AdaptiveSpacing.Padding, Weight=AdaptiveTextWeight.Default}
                                },
                                Id="Blccol9"
                           },
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                     new AdaptiveTextBlock(){Id="Blctxt8",Text=employee.LeaveBalance.OptionalLeave.ToString(), Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true,  Spacing=AdaptiveSpacing.Padding, Color=AdaptiveTextColor.Accent},
                                },
                                Id="Blccol10"
                            }
                        },
                        Id="Blccol11"
                    }
                        }
                    }
                },
                Actions = new List<AdaptiveAction>()
                {
                    new AdaptiveOpenUrlAction()
                    {
                        Title="View Details",
                        Url = new Uri(DeeplinkHelper.GetLeaveBoardDeeplink(employee.EmailId)),
                        Id="Blcact"
                    }
                },
            };
            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = LeaveBalanceCard
            };
        }

        public static Attachment PublicHolidays()
        {
            DateTime nextholiday = DateTime.Now;
            string title = string.Empty;
            var month = DateTime.Now.Month;
            List<PublicHoliday> displayList = new List<PublicHoliday>();
            foreach (var item in PublicHolidaysList.HolidayList)
            {
                if (item.Date.Date > DateTime.Now.Date)
                {
                    displayList.Add(item);
                }
                if (displayList.Count == 2)
                    break;
            }

            var PublicHolidaysCard = new AdaptiveCard()
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveContainer
                    {
                        Items=new List<AdaptiveElement>()
                        {
                            new AdaptiveTextBlock()
                            {
                                Text="Here is the list of upcoming public holidays",
                                Size=AdaptiveTextSize.Large,
                                Id="HoliTxt"
                            },

                             new AdaptiveColumnSet()
                    {
                                 Spacing=AdaptiveSpacing.ExtraLarge,

                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock(){Id="Holitxt1",Text="Date", Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true, Spacing=AdaptiveSpacing.Padding,Color=AdaptiveTextColor.Accent, Weight=AdaptiveTextWeight.Lighter}
                                },
                                Id="Holicol1"
                            },
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                     new AdaptiveTextBlock(){Id="Holitxt2",Text="Day", Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true,  Spacing=AdaptiveSpacing.Padding, Color=AdaptiveTextColor.Accent, Weight=AdaptiveTextWeight.Lighter}
                                },
                                Id="Holicol2"
                            },
                             new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                     new AdaptiveTextBlock(){Id="Holitxt3",Text="Event", Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true,  Spacing=AdaptiveSpacing.Padding, Color=AdaptiveTextColor.Accent, Weight=AdaptiveTextWeight.Lighter}
                                },
                                Id="Holicol3"
                            }
                        },
                        Id="Holicol4"
                    },
                             new AdaptiveColumnSet()
                    {
                                 Separator=true,
                                 Spacing=AdaptiveSpacing.ExtraLarge,
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock(){Id="Holitxt4",Text=displayList[0].Date.ToShortDateString(), Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true, Spacing=AdaptiveSpacing.Padding, Weight=AdaptiveTextWeight.Bolder}
                                },
                                Id="Holicol5"
                            },
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                     new AdaptiveTextBlock(){Id="Holitxt5",Text=displayList[0].Date.DayOfWeek.ToString(), Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true,  Spacing=AdaptiveSpacing.Padding, Weight=AdaptiveTextWeight.Default}
                                },
                                Id="Holicol6"
                            },
                             new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                     new AdaptiveTextBlock(){Id="Holitxt6",Text=displayList[0].Title, Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true,  Spacing=AdaptiveSpacing.Padding, Weight=AdaptiveTextWeight.Default}
                                },
                                Id="Holicol7"
                            }
                        },
                        Id="Holico22"
                    },
                             new AdaptiveColumnSet()
                    {
                                 Separator=true,
                                 Spacing=AdaptiveSpacing.ExtraLarge,
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                     new AdaptiveTextBlock(){Id="Holitxt7",Text=displayList[1].Date.ToShortDateString(), Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true, Spacing=AdaptiveSpacing.Padding, Weight=AdaptiveTextWeight.Bolder}
                                },
                                Id="Holicol8"
                            },
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                     new AdaptiveTextBlock(){Id="Holitxt8",Text=displayList[1].Date.DayOfWeek.ToString(), Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true,  Spacing=AdaptiveSpacing.Padding, Weight=AdaptiveTextWeight.Default}
                                },
                                Id="Holicol9"
                            },
                            new AdaptiveColumn()
                            {
                                Width="50",
                                Spacing=AdaptiveSpacing.ExtraLarge,
                                Items=new List<AdaptiveElement>()
                                {
                                     new AdaptiveTextBlock(){Id="Holitxt9",Text=displayList[1].Title, Size=AdaptiveTextSize.Medium,Wrap=true, Separator=true,  Spacing=AdaptiveSpacing.Padding, Weight=AdaptiveTextWeight.Default}
                                },
                                Id="Holicol10"
                            }
                        },
                        Id="Holicol11"
                    },
                       }
                    }
                },
                Actions = new List<AdaptiveAction>()
                {
                    new AdaptiveOpenUrlAction()
                    {
                        Title="View Details",
                        Url = new Uri(DeeplinkHelper.PublicHolidaysDeeplink),
                        Id="Holiacti"
                    }
                },
            };
            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = PublicHolidaysCard
            };
        }
    }

    public class Constants
    {
        public const string LeaveRequest = "Make a leave request";
        public const string ShowPendingApprovals = "ShowPendingApprovals";

        public const string ApplyForVacation = "ApplyForVacation";
        public const string ApplyForSickLeave = "ApplyForSickLeave";
        public const string ApplyForPersonalLeave = "ApplyForPersonalLeave";
        public const string ApplyForOtherLeave = "ApplyForOtherLeave";

        public const string ApproveLeave = "ApproveLeave";
        public const string RejectLeave = "RejectLeave";
        public const string SetManager = "SetManager";

        public const string LeaveBalance = "View Leave Balance";
        public const string Holidays = "View List of Public Holidays";

        public const string EditLeave = "EditLeave";
        public const string Withdraw = "Withdraw";
    }

    public class InputDetails
    {
        public string Type { get; set; }
    }

    public class SetManagerDetails : InputDetails
    {
        public string txtManager { get; set; }
    }

    public class MessageIds
    {
        public string Employee { get; set; }
        public string Manager { get; set; }
    }

    public class VacationDetails : InputDetails
    {
        public string FromDate { get; set; }
        public string FromDuration { get; set; }
        public string ToDate { get; set; }
        public string ToDuration { get; set; }
        public string LeaveType { get; set; }

        #region Properties to fix issue with multiple Ids for Leave Type
        [JsonProperty("LeaveTypeVacation")]
        private string LeaveTypeVacation { set { LeaveType = value; } }

        [JsonProperty("LeaveTypeSickness")]
        private string LeaveTypeSickness { set { LeaveType = value; } }

        [JsonProperty("LeaveTypePersonal")]
        private string LeaveTypePersonal { set { LeaveType = value; } }

        [JsonProperty("LeaveTypeOther")]
        private string LeaveTypeOther { set { LeaveType = value; } }

        #endregion

        public string LeaveReason { get; set; }

        #region Properties to fix issue with multiple Ids for Leave Reason
        [JsonProperty("LeaveReasonVacation")]
        private string LeaveReasonVacation { set { LeaveReason = value; } }

        [JsonProperty("LeaveReasonSickness")]
        private string LeaveReasonSickness { set { LeaveReason = value; } }

        [JsonProperty("LeaveReasonPersonal")]
        private string LeaveReasonPersonal { set { LeaveReason = value; } }

        [JsonProperty("LeaveReasonOther")]
        private string LeaveReasonOther { set { LeaveReason = value; } }

        #endregion

        public string LeaveId { get; set; }
    }

    public class EditRequest
    {
        public VacationDetails data { get; set; }
    }

    public class ManagerResponse : InputDetails
    {
        public string LeaveId { get; set; }
        public string ManagerComment { get; set; }
        #region Properties to fix issue with multiple Ids for Manager comments
        [JsonProperty("ManagerCommentApprove")]
        private string ManagerCommentApprove { set { ManagerComment = value; } }

        [JsonProperty("ManagerCommentReject")]
        private string ManagerCommentReject { set { ManagerComment = value; } }

        #endregion
    }

    public class EditLeaveDetails : InputDetails
    {
        public string LeaveId { get; set; }
    }
}