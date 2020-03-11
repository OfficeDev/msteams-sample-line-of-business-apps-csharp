using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;
using Microsoft.Graph;
using Newtonsoft.Json;
using ProfessionalServices.LeaveBot.Helper;
using ProfessionalServices.LeaveBot.Helpers;
using ProfessionalServices.LeaveBot.Models;
using ProfessionalServices.LeaveBot.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProfessionalServices.LeaveBot.Dialogs

{
    /// <summary>

    /// This Dialog enables the user to issue a set of commands against AAD

    /// to do things like list recent email, send an email, and identify the user

    /// This Dialog also makes use of the GetTokenDialog to help the user login

    /// </summary>

    [Serializable]
    public class RootDialog : IDialog<object>

    {
        private const string ProfileKey = "profile";

        private const string EmailKey = "emailId";

        /// <summary>

        /// This is the name of the OAuth Connection Setting that is configured for this bot

        /// </summary>

        public async Task StartAsync(IDialogContext context)

        {
            context.Wait(MessageReceivedAsync);
        }

        /// <summary>

        /// Supports the commands recents, send, me, and signout against the Graph API

        /// </summary>

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)

        {
            var activity = await result as Activity;

            string message = string.Empty;

            if (activity.Text != null)

                message = Microsoft.Bot.Connector.Teams.ActivityExtensions.GetTextWithoutMentions(activity).ToLowerInvariant();

            string userEmailId = string.Empty;

            string emailKey = GetEmailKey(activity);

            if (context.ConversationData.ContainsKey(emailKey))

            {
                userEmailId = context.ConversationData.GetValue<string>(emailKey);
            }
            else

            {
                // Fetch from roaster

                userEmailId = await GetUserEmailId(activity);

                context.ConversationData.SetValue(emailKey, userEmailId);
            }

            string profileKey = GetProfileKey(activity);

            Employee employee;

            if (context.ConversationData.ContainsKey(profileKey))

            {
                employee = context.ConversationData.GetValue<Employee>(profileKey);
            }
            else

            {
                employee = await DocumentDBRepository.GetItemAsync<Employee>(userEmailId);

                if (employee != null)

                    context.ConversationData.SetValue<Employee>(profileKey, employee);
            }

            if (employee == null)

            {
                // If Bot Service does not have a token, send an OAuth card to sign in.

                await SendOAuthCardAsync(context, (Activity)context.Activity);
            }
            else if ((string.IsNullOrEmpty(employee.ManagerEmailId) && string.IsNullOrEmpty(employee.DemoManagerEmailId))

                && (!IsValidActionWithoutManager(activity.Value)))

            {
                await SendSetManagerCard(context, activity, employee, message);
            }
            else if (activity.Value != null)

            {
                await HandleActions(context, activity, employee);
            }
            else

            {
                if (message.ToLowerInvariant().Contains("set manager"))

                {
                    await SendSetManagerCard(context, activity, employee, message);
                }
                else if (message.ToLowerInvariant().Contains("reset"))

                {
                    // Sign the user out from AAD

                    await Signout(userEmailId, context);
                }
                else

                {
                    var reply = activity.CreateReply();

                    bool isManager = await IsManager(employee);

                    reply.Attachments.Add(EchoBot.WelcomeLeaveCard(employee.DisplayName, isManager));

                    try
                    {
                        await context.PostAsync(reply);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        throw;
                    }
                }
            }
        }

        private static string GetEmailKey(IActivity activity)

        {
            return activity.From.Id + EmailKey;
        }

        private static string GetProfileKey(IActivity activity)

        {
            return activity.From.Id + ProfileKey;
        }

        public static async Task<bool> IsManager(Employee employee)

        {
            var allEmployees = await DocumentDBRepository.GetItemsAsync<Employee>(l => l.Type == Employee.TYPE);

            return allEmployees.Any(s => s.ManagerEmailId == employee.EmailId);
        }

        private async Task HandleActions(IDialogContext context, Activity activity, Employee employee)

        {
            string type = string.Empty;

            if (activity.Name == Constants.EditLeave) // Edit request

            {
                var editRequest = JsonConvert.DeserializeObject<EditRequest>(activity.Value.ToString());

                type = editRequest.data.Type;
            }
            else

            {
                var details = JsonConvert.DeserializeObject<InputDetails>(activity.Value.ToString());

                type = details.Type;
            }

            var reply = activity.CreateReply();

            switch (type)

            {
                case Constants.ApplyForOtherLeave:

                case Constants.ApplyForPersonalLeave:

                case Constants.ApplyForSickLeave:

                case Constants.ApplyForVacation:

                    await ApplyForLeave(context, activity, employee, type);

                    break;

                case Constants.Withdraw:

                    await WithdrawLeave(context, activity, employee, type);

                    break;

                case Constants.RejectLeave:

                case Constants.ApproveLeave:

                    await ApproveOrRejectLeaveRequest(context, activity, type, employee);

                    break;

                case Constants.SetManager:

                    await SetEmployeeManager(context, activity, employee);

                    break;

                case Constants.ShowPendingApprovals:

                    await ShowPendingApprovals(context, activity, employee);

                    break;

                case Constants.LeaveRequest:

                    try

                    {
                        reply.Attachments.Add(EchoBot.LeaveRequest());
                    }
                    catch (Exception ex)

                    {
                        ErrorLogService.LogError(ex);
                    }

                    await context.PostAsync(reply);

                    break;

                case Constants.LeaveBalance:

                    reply.Attachments.Add(EchoBot.ViewLeaveBalance(employee));

                    await context.PostAsync(reply);

                    break;

                case Constants.Holidays:

                    reply.Attachments.Add(EchoBot.PublicHolidays());

                    await context.PostAsync(reply);

                    break;

                default:

                    reply = activity.CreateReply("It will redirect to the tab");

                    await context.PostAsync(reply);

                    break;
            }
        }

        private async Task ShowPendingApprovals(IDialogContext context, Activity activity, Employee employee)

        {
            var pendingLeaves = await DocumentDBRepository.GetItemsAsync<LeaveDetails>(l => l.Type == LeaveDetails.TYPE);

            pendingLeaves = pendingLeaves.Where(l => l.ManagerEmailId == employee.EmailId && l.Status == LeaveStatus.Pending);

            if (pendingLeaves.Count() == 0)

            {
                var reply = activity.CreateReply();

                reply.Text = "No pending leaves for approval.";

                await context.PostAsync(reply);
            }
            else

            {
                var reply = activity.CreateReply();

                reply.Text = "Here are all the leaves pending for approval:";

                foreach (var leave in pendingLeaves)

                {
                    var attachment = EchoBot.ManagerViewCard(employee, leave);

                    reply.Attachments.Add(attachment);
                }

                await context.PostAsync(reply);
            }
        }

        private bool IsValidActionWithoutManager(object activityValue)

        {
            if (activityValue == null)

                return true;

            return (activityValue.ToString().Contains(Constants.SetManager)

                || activityValue.ToString().Contains(Constants.ApproveLeave)

                || activityValue.ToString().Contains(Constants.RejectLeave)

                || activityValue.ToString().Contains(Constants.LeaveBalance)

                || activityValue.ToString().Contains(Constants.Holidays));
        }

        private static async Task ApproveOrRejectLeaveRequest(IDialogContext context, Activity activity, string type, Employee employee)

        {
            var managerResponse = JsonConvert.DeserializeObject<ManagerResponse>(activity.Value.ToString());

            var leaveDetails = await DocumentDBRepository.GetItemAsync<LeaveDetails>(managerResponse.LeaveId);

            var appliedByEmployee = await DocumentDBRepository.GetItemAsync<Employee>(leaveDetails.AppliedByEmailId);

            // Check the leave type and reduce in DB.

            leaveDetails.Status = type == Constants.ApproveLeave ? LeaveStatus.Approved : LeaveStatus.Rejected;

            leaveDetails.ManagerComment = managerResponse.ManagerComment;

            await DocumentDBRepository.UpdateItemAsync(leaveDetails.LeaveId, leaveDetails);

            var conunt = EchoBot.GetDayCount(leaveDetails);

            var employeeView = EchoBot.EmployeeViewCard(appliedByEmployee, leaveDetails);

            UpdateMessageInfo managerMessageIds = leaveDetails.UpdateMessageInfo;

            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            var managerCardUpdate = activity.CreateReply();

            var managerView = EchoBot.ManagerViewCard(appliedByEmployee, leaveDetails);

            managerCardUpdate.Attachments.Add(managerView);

            if (!string.IsNullOrEmpty(managerMessageIds.Manager))

            {
                await connector.Conversations.UpdateActivityAsync(managerCardUpdate.Conversation.Id, managerMessageIds.Manager, managerCardUpdate);
            }

            bool isGroup = !string.IsNullOrEmpty(leaveDetails.ChannelId);

            if (!isGroup)

            {
                var messageId = await SendNotification(context, isGroup ? leaveDetails.ChannelId : appliedByEmployee.UserUniqueId, null, employeeView, managerMessageIds.Employee, isGroup);// Update card.

                var msg = $"Your {conunt} days leave has been {leaveDetails.Status.ToString()} by your manager.";

                messageId = await SendNotification(context, isGroup ? leaveDetails.ChannelId : appliedByEmployee.UserUniqueId, msg, null, null, isGroup); // Send message.

                if (string.IsNullOrEmpty(messageId))

                {
                    var reply = activity.CreateReply();

                    reply.Text = $"Failed to notify {appliedByEmployee.DisplayName}. Please try again later.";

                    await context.PostAsync(reply);
                }
            }
            else

            {
                var msg = $" - Your {conunt} days leave has been {leaveDetails.Status.ToString()} by your manager.";

                var messageId = await SendChannelNotification(context, leaveDetails.ChannelId, null, employeeView, appliedByEmployee, managerMessageIds.Employee, leaveDetails.ConversationId, false);// Update card.

                messageId = await SendChannelNotification(context, leaveDetails.ChannelId, msg, null, appliedByEmployee, null, leaveDetails.ConversationId, true); // Send message.

                if (string.IsNullOrEmpty(messageId))

                {
                    var reply = activity.CreateReply();

                    reply.Text = $"Failed to notify {appliedByEmployee.DisplayName}. Please try again later.";

                    await context.PostAsync(reply);
                }
            }
        }

        private async Task SetEmployeeManager(IDialogContext context, Activity activity, Employee employee)

        {
            var details = JsonConvert.DeserializeObject<SetManagerDetails>(activity.Value.ToString());

            await SetEmaployeeManager(context, activity, employee, details.txtManager.ToLower());
        }

        private static async Task SetEmaployeeManager(IDialogContext context, Activity activity, Employee employee, string emailId)

        {
            employee.ManagerEmailId = emailId;

            await UpdateEmployeeInDB(context, employee);

            var reply = activity.CreateReply();

            reply.Text = "Your manager is set successfully. Manger Email Id: " + emailId;

            await context.PostAsync(reply);
        }

        private static async Task WithdrawLeave(IDialogContext context, Activity activity, Employee employee, string leaveCategory)

        {
            var managerId = await GetManagerId(employee);

            if (managerId == null)

            {
                var reply = activity.CreateReply();

                reply.Text = "Unable to fetch your manager details. Please make sure that your manager has installed the Leave App.";

                await context.PostAsync(reply);
            }
            else

            {
                EditLeaveDetails vacationDetails = JsonConvert.DeserializeObject<EditLeaveDetails>(activity.Value.ToString());

                LeaveDetails leaveDetails = await DocumentDBRepository.GetItemAsync<LeaveDetails>(vacationDetails.LeaveId);

                leaveDetails.Status = LeaveStatus.Withdrawn;

                var attachment = EchoBot.ManagerViewCard(employee, leaveDetails);

                // Manger Updates.

                var conversationId = await SendNotification(context, managerId, null, attachment, leaveDetails.UpdateMessageInfo.Manager, false);

                if (!String.IsNullOrEmpty(conversationId))

                {
                    leaveDetails.UpdateMessageInfo.Manager = conversationId;
                }

                if (!String.IsNullOrEmpty(conversationId))

                {
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                    var employeeCardReply = activity.CreateReply();

                    var employeeView = EchoBot.EmployeeViewCard(employee, leaveDetails);

                    employeeCardReply.Attachments.Add(employeeView);

                    if (!string.IsNullOrEmpty(leaveDetails.UpdateMessageInfo.Employee))

                    {
                        await connector.Conversations.UpdateActivityAsync(employeeCardReply.Conversation.Id, leaveDetails.UpdateMessageInfo.Employee, employeeCardReply);
                    }
                    else

                    {
                        var reply = activity.CreateReply();

                        reply.Text = "Your leave request has been successfully withdrawn!";

                        await context.PostAsync(reply);

                        var msgToUpdate = await connector.Conversations.ReplyToActivityAsync(employeeCardReply);
                    }
                }
                else

                {
                    var reply = activity.CreateReply();

                    reply.Text = "Failed to send notification to your manger. Please try again later.";

                    await context.PostAsync(reply);
                }

                // Update DB for all the message Id realted changes

                await DocumentDBRepository.UpdateItemAsync(leaveDetails.LeaveId, leaveDetails);
            }
        }

        private static async Task ApplyForLeave(IDialogContext context, Activity activity, Employee employee, string leaveCategory)

        {
            if (string.IsNullOrEmpty(employee.ManagerEmailId) && string.IsNullOrEmpty(employee.DemoManagerEmailId))

            {
                var reply = activity.CreateReply();

                reply.Text = "Please set your manager and try again.";

                reply.Attachments.Add(EchoBot.SetManagerCard());

                await context.PostAsync(reply);

                return;
            }

            var managerId = await GetManagerId(employee);

            if (managerId == null)

            {
                var reply = activity.CreateReply();

                reply.Text = "Unable to fetch your manager details. Please make sure that your manager has installed the Leave App.";

                await context.PostAsync(reply);
            }
            else

            {
                VacationDetails vacationDetails = null;

                if (activity.Name == Constants.EditLeave) // Edit request

                {
                    var editRequest = JsonConvert.DeserializeObject<EditRequest>(activity.Value.ToString());

                    vacationDetails = editRequest.data;
                }
                else

                    vacationDetails = JsonConvert.DeserializeObject<VacationDetails>(activity.Value.ToString());

                LeaveDetails leaveDetails;

                if (!string.IsNullOrEmpty(vacationDetails.LeaveId))

                {
                    // Edit request

                    leaveDetails = await DocumentDBRepository.GetItemAsync<LeaveDetails>(vacationDetails.LeaveId);
                }
                else

                {
                    leaveDetails = new LeaveDetails();

                    leaveDetails.LeaveId = Guid.NewGuid().ToString();
                }

                leaveDetails.AppliedByEmailId = employee.EmailId;

                leaveDetails.EmployeeComment = vacationDetails.LeaveReason;

                var channelData = context.Activity.GetChannelData<TeamsChannelData>();

                leaveDetails.ChannelId = channelData.Channel?.Id; // Set channel Data if request is coming from a channel.

                if (!string.IsNullOrEmpty(leaveDetails.ChannelId))

                    leaveDetails.ConversationId = activity.Conversation.Id;

                leaveDetails.StartDate = new LeaveDate()

                {
                    Date = DateTime.Parse(vacationDetails.FromDate),

                    Type = (DayType)Enum.Parse(typeof(DayType), vacationDetails.FromDuration)
                };

                leaveDetails.EndDate = new LeaveDate()

                {
                    Date = DateTime.Parse(vacationDetails.ToDate),

                    Type = (DayType)Enum.Parse(typeof(DayType), vacationDetails.ToDuration)
                };

                leaveDetails.LeaveType = (LeaveType)Enum.Parse(typeof(LeaveType), vacationDetails.LeaveType);

                leaveDetails.Status = LeaveStatus.Pending;

                leaveDetails.ManagerEmailId = employee.ManagerEmailId;// Added for easy reporting.

                switch (leaveCategory)

                {
                    case Constants.ApplyForPersonalLeave:

                        leaveDetails.LeaveCategory = LeaveCategory.Personal;

                        break;

                    case Constants.ApplyForSickLeave:

                        leaveDetails.LeaveCategory = LeaveCategory.Sickness;

                        break;

                    case Constants.ApplyForVacation:

                        leaveDetails.LeaveCategory = LeaveCategory.Vacation;

                        break;

                    case Constants.ApplyForOtherLeave:

                    default:

                        leaveDetails.LeaveCategory = LeaveCategory.Other;

                        break;
                }

                if (!string.IsNullOrEmpty(vacationDetails.LeaveId))

                {
                    // Edit request

                    await DocumentDBRepository.UpdateItemAsync(leaveDetails.LeaveId, leaveDetails);
                }
                else

                {
                    await DocumentDBRepository.CreateItemAsync(leaveDetails);
                }

                var attachment = EchoBot.ManagerViewCard(employee, leaveDetails);

                UpdateMessageInfo managerMessageIds = leaveDetails.UpdateMessageInfo;

                // Manger Updates.

                var conversationId = await SendNotification(context, managerId, null, attachment, managerMessageIds.Manager, false);

                if (!string.IsNullOrEmpty(conversationId))

                {
                    managerMessageIds.Manager = conversationId;
                }

                if (!string.IsNullOrEmpty(conversationId))

                {
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                    var employeeCardReply = activity.CreateReply();

                    var employeeView = EchoBot.EmployeeViewCard(employee, leaveDetails);

                    employeeCardReply.Attachments.Add(employeeView);

                    if (!string.IsNullOrEmpty(managerMessageIds.Employee))

                    {
                        // Update existing item.

                        await connector.Conversations.UpdateActivityAsync(employeeCardReply.Conversation.Id, managerMessageIds.Employee, employeeCardReply);
                    }
                    else

                    {
                        var reply = activity.CreateReply();

                        reply.Text = "Your leave request has been successfully submitted to your manager! Please review your details below:";

                        await context.PostAsync(reply);

                        var msgToUpdate = await connector.Conversations.ReplyToActivityAsync(employeeCardReply);

                        managerMessageIds.Employee = msgToUpdate.Id;
                    }
                }
                else

                {
                    var reply = activity.CreateReply();

                    reply.Text = "Failed to send notification to your manger. Please try again later.";

                    await context.PostAsync(reply);
                }

                await DocumentDBRepository.UpdateItemAsync(leaveDetails.LeaveId, leaveDetails);
            }
        }

        private static async Task SendSetManagerCard(IDialogContext context, Activity activity, Employee employee, string message)

        {
            //Ask for manager details.

            var card = EchoBot.SetManagerCard(); // WelcomeLeaveCard(employee.Name.Split(' ').First());

            if (message.Contains("@"))

            {
                var emailId = ExtractEmails(message).FirstOrDefault();

                if (!string.IsNullOrEmpty(emailId))

                {
                    await SetEmaployeeManager(context, activity, employee, emailId);

                    return;
                }
            }

            var msg = context.MakeMessage();

            msg.Text = "Please enter manager email ID for leave approval";

            msg.Attachments.Add(card);

            await context.PostAsync(msg);
        }

        public static List<string> ExtractEmails(string str)

        {
            string RegexPattern = @"\b[A-Z0-9._-]+@[A-Z0-9][A-Z0-9.-]{0,61}[A-Z0-9]\.[A-Z.]{2,6}\b";

            // Find matches

            System.Text.RegularExpressions.MatchCollection matches

                = System.Text.RegularExpressions.Regex.Matches(str, RegexPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            List<string> MatchList = new List<string>(matches.Count);

            // add each match

            int c = 0;

            foreach (System.Text.RegularExpressions.Match match in matches)

            {
                MatchList.Add(match.ToString());

                c++;
            }

            return MatchList;
        }

        private static async Task UpdateEmployeeInDB(IDialogContext context, Employee employee)

        {
            await DocumentDBRepository.UpdateItemAsync(employee.EmailId, employee);

            var profileKey = GetProfileKey(context.Activity);

            context.ConversationData.SetValue(profileKey, employee);
        }

        private static async Task<string> GetManagerId(Employee employee)

        {
            var managerId = employee.ManagerEmailId ?? employee.DemoManagerEmailId;

            var manager = await DocumentDBRepository.GetItemAsync<Employee>(managerId);

            if (manager != null)

                return manager.UserUniqueId;
            else return null;
        }

        private async Task<string> GetUserEmailId(Activity activity)

        {
            // Fetch the members in the current conversation

            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            var members = await connector.Conversations.GetConversationMembersAsync(activity.Conversation.Id);
            return members.Where(m => m.Id == activity.From.Id).First().AsTeamsChannelAccount().UserPrincipalName.ToLower();
        }

        private async Task SendOAuthCardAsync(IDialogContext context, Activity activity)

        {
            var reply = await context.Activity.CreateOAuthReplyAsync(ApplicationSettings.ConnectionName, "In order to use Leave Bot we need your basic details, Please sign in", "Sign In", true).ConfigureAwait(false);

            await context.PostAsync(reply);

            context.Wait(WaitForToken);
        }

        private async Task WaitForToken(IDialogContext context, IAwaitable<object> result)

        {
            var activity = await result as Activity;

            var tokenResponse = activity.ReadTokenResponseContent();

            if (tokenResponse != null)

            {
                // Use the token to do exciting things!

                await AddUserToDatabase(context, tokenResponse);

                context.Wait(MessageReceivedAsync);
            }
            else

            {
                // Get the Activity Message as well as activity.value in case of Auto closing of pop-up

                string input = activity.Type == ActivityTypes.Message ? Microsoft.Bot.Connector.Teams.ActivityExtensions.GetTextWithoutMentions(activity)

                                                                : ((dynamic)(activity.Value)).state.ToString();

                if (!string.IsNullOrEmpty(input))

                {
                    tokenResponse = await context.GetUserTokenAsync(ApplicationSettings.ConnectionName, input.Trim());

                    if (tokenResponse != null)

                    {
                        try

                        {
                            await AddUserToDatabase(context, tokenResponse);
                        }
                        catch (Exception ex)

                        {
                            ErrorLogService.LogError(ex);
                        }

                        context.Wait(MessageReceivedAsync);

                        return;
                    }
                }

                await context.PostAsync($"Hmm. Something went wrong. Please initiate the SignIn again. Try sending help.");

                //await SendOAuthCardAsync(context, activity);

                // await MessageReceivedAsync(context,  result);

                context.Wait(MessageReceivedAsync);
            }
        }

        private static async Task AddUserToDatabase(IDialogContext context, TokenResponse tokenResponse)

        {
            var client = new SimpleGraphClient(tokenResponse.Token);

            User me = null;

            //User manager = null;

            var profilePhotoUrl = string.Empty;

            try

            {
                me = await client.GetMe();

                // manager = await client.GetManager();

                var photo = await client.GetProfilePhoto();

                var fileName = me.Id + "-ProflePhoto.png";

                string imagePath = System.Web.Hosting.HostingEnvironment.MapPath("~/ProfilePhotos/");

                if (!System.IO.Directory.Exists(imagePath))

                    System.IO.Directory.CreateDirectory(imagePath);

                imagePath += fileName;

                using (var fileStream = System.IO.File.Create(imagePath))

                {
                    photo.Seek(0, SeekOrigin.Begin);

                    photo.CopyTo(fileStream);
                }

                profilePhotoUrl = ApplicationSettings.BaseUrl + "/ProfilePhotos/" + fileName;
            }
            catch (Exception ex)

            {
                ErrorLogService.LogError(ex);

                profilePhotoUrl = null;
            }

            ConnectorClient connector = new ConnectorClient(new Uri(context.Activity.ServiceUrl));

            var channelData = context.Activity.GetChannelData<TeamsChannelData>();

            var employee = new Employee()

            {
                Name = me.DisplayName,
                EmailId = me.UserPrincipalName.ToLower(),
                UserUniqueId = context.Activity.From.Id, // For proactive messages

                TenantId = channelData.Tenant.Id,

                DemoManagerEmailId = string.Empty,

                JobTitle = me.JobTitle ?? me.UserPrincipalName,

                LeaveBalance = new LeaveBalance

                {
                    OptionalLeave = 2,

                    PaidLeave = 20,

                    SickLeave = 10
                },

                AzureADId = me.Id,

                PhotoPath = profilePhotoUrl,
            };

            var employeeDoc = await DocumentDBRepository.CreateItemAsync(employee);

            context.ConversationData.SetValue(GetProfileKey(context.Activity), employee);

            var msg = context.MakeMessage();

            var card = EchoBot.SetManagerCard(); // WelcomeLeaveCard(employee.Name.Split(' ').First());

            msg.Attachments.Add(card);

            await context.PostAsync(msg);
        }

        /// <summary>

        /// Signs the user out from AAD

        /// </summary>

        public static async Task Signout(string emailId, IDialogContext context)

        {
            context.ConversationData.Clear();

            await context.SignOutUserAsync(ApplicationSettings.ConnectionName);

            await DocumentDBRepository.DeleteItemAsync(emailId);

            var pendingLeaves = await DocumentDBRepository.GetItemsAsync<LeaveDetails>(l => l.Type == LeaveDetails.TYPE);

            foreach (var leave in pendingLeaves.Where(l => l.AppliedByEmailId == emailId))

            {
                await DocumentDBRepository.DeleteItemAsync(leave.LeaveId);
            }

            await context.PostAsync($"We have cleared everything related to you.");
        }

        public static async Task<string> SendNotification(IDialogContext context, string userOrChannelId, string messageText, Microsoft.Bot.Connector.Attachment attachment, string updateMessageId, bool isChannelMessage)

        {
            var userId = userOrChannelId.Trim();

            var botId = context.Activity.Recipient.Id;

            var botName = context.Activity.Recipient.Name;

            var channelData = context.Activity.GetChannelData<TeamsChannelData>();

            var connectorClient = new ConnectorClient(new Uri(context.Activity.ServiceUrl));

            var parameters = new ConversationParameters

            {
                Bot = new ChannelAccount(botId, botName),

                Members = !isChannelMessage ? new ChannelAccount[] { new ChannelAccount(userId) } : null,

                ChannelData = new TeamsChannelData

                {
                    Tenant = channelData.Tenant,

                    Channel = isChannelMessage ? new ChannelInfo(userId) : null,

                    Notification = new NotificationInfo() { Alert = true }
                },

                IsGroup = isChannelMessage
            };

            try

            {
                var conversationResource = await connectorClient.Conversations.CreateConversationAsync(parameters);

                var replyMessage = Activity.CreateMessageActivity();

                replyMessage.From = new ChannelAccount(botId, botName);

                replyMessage.Conversation = new ConversationAccount(id: conversationResource.Id.ToString());

                replyMessage.ChannelData = new TeamsChannelData() { Notification = new NotificationInfo(true) };

                replyMessage.Text = messageText;

                if (attachment != null)

                    replyMessage.Attachments.Add(attachment);//  EchoBot.ManagerViewCard(employee, leaveDetails));

                if (string.IsNullOrEmpty(updateMessageId))

                {
                    var resourceResponse = await connectorClient.Conversations.SendToConversationAsync(conversationResource.Id, (Activity)replyMessage);

                    return resourceResponse.Id;
                }
                else

                {
                    await connectorClient.Conversations.UpdateActivityAsync(conversationResource.Id, updateMessageId, (Activity)replyMessage);

                    return updateMessageId; // Just return the same Id.
                }
            }
            catch (Exception ex)

            {
                // Handle the error.

                ErrorLogService.LogError(ex);

                var msg = context.MakeMessage();

                msg.Text = ex.Message;

                await context.PostAsync(msg);

                return null;
            }
        }

        private static async Task<string> SendChannelNotification(IDialogContext context, string channelId, string messageText, Microsoft.Bot.Connector.Attachment attachment, Employee employee, string updateMessageId, string channleConversationId, bool addAtMention)

        {
            var connectorClient = new ConnectorClient(new Uri(context.Activity.ServiceUrl));

            try

            {
                var replyMessage = Activity.CreateMessageActivity();

                replyMessage.Conversation = new ConversationAccount(id: channelId);

                replyMessage.ChannelData = new TeamsChannelData() { Notification = new NotificationInfo(true) };

                replyMessage.Text = messageText;

                if (addAtMention)

                {
                    replyMessage.AddMentionToText(new ChannelAccount(employee.UserUniqueId, employee.DisplayName), MentionTextLocation.PrependText);
                }

                if (attachment != null)

                    replyMessage.Attachments.Add(attachment);//  EchoBot.ManagerViewCard(employee, leaveDetails));

                if (string.IsNullOrEmpty(updateMessageId))

                {
                    var resourceResponse = await connectorClient.Conversations.SendToConversationAsync(channleConversationId, (Activity)replyMessage);

                    return resourceResponse.Id;
                }
                else

                {
                    await connectorClient.Conversations.UpdateActivityAsync(channleConversationId, updateMessageId, (Activity)replyMessage);

                    return updateMessageId; // Just return the same Id.
                }
            }
            catch (Exception ex)

            {
                ErrorLogService.LogError(ex);

                // Handle the error.

                var msg = context.MakeMessage();

                msg.Text = ex.Message;

                await context.PostAsync(msg);

                return null;
            }
        }
    }
}