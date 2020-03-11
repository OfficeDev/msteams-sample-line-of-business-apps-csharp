using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams.Models;
using Newtonsoft.Json.Linq;
using ProfessionalServices.LeaveBot.Dialogs;
using ProfessionalServices.LeaveBot.Helpers;
using ProfessionalServices.LeaveBot.Models;
using ProfessionalServices.LeaveBot.Repository;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ProfessionalServices.LeaveBot.Controllers

{
    [BotAuthentication]
    public class MessagesController : ApiController

    {
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)

        {
            if (activity != null && activity.Type == ActivityTypes.Message)

            {
                try
                {
                    await Conversation.SendAsync(activity, () => new RootDialog());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
            }
            else if (activity.Type == ActivityTypes.Invoke)

            {
                if (activity.Name == "signin/verifyState")

                {
                    await Conversation.SendAsync(activity, () => new RootDialog());
                }
                else

                {
                    return await HandleInvokeMessages(activity);
                }
            }
            else

            {
                await HandleSystemMessage(activity);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private async Task<HttpResponseMessage> HandleInvokeMessages(Activity activity)

        {
            var activityValue = activity.Value.ToString();

            if (activity.Name == "task/fetch")

            {
                var action = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskModule.TaskModuleActionData<EditLeaveDetails>>(activityValue);

                var leaveDetails = await DocumentDBRepository.GetItemAsync<LeaveDetails>(action.Data.Data.LeaveId);

                // TODO: Convert this to helpers once available.

                JObject taskEnvelope = new JObject();

                JObject taskObj = new JObject();

                JObject taskInfo = new JObject();

                taskObj["type"] = "continue";

                taskObj["value"] = taskInfo;

                taskInfo["card"] = JObject.FromObject(EchoBot.LeaveRequest(leaveDetails));

                taskInfo["title"] = "Edit Leave";

                taskInfo["height"] = 500;

                taskInfo["width"] = 600;

                taskEnvelope["task"] = taskObj;

                return Request.CreateResponse(HttpStatusCode.OK, taskEnvelope);
            }
            else if (activity.Name == "task/submit")

            {
                activity.Name = Constants.EditLeave;

                await Conversation.SendAsync(activity, () => new RootDialog());
            }

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        private async Task<Activity> HandleSystemMessage(Activity message)

        {
            if (message.Type == ActivityTypes.DeleteUserData)

            {
                // Implement user deletion here

                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)

            {
                // Handle conversation state changes, like members being added and removed

                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info

                // Not available in all channels

                for (int i = 0; i < message.MembersAdded.Count; i++)

                {
                    if (message.MembersAdded[i].Id == message.Recipient.Id)

                    {
                        // Bot is added. Let's send welcome message.

                        message.Text = "hi";

                        await Conversation.SendAsync(message, () => new RootDialog());

                        break;
                    }
                    else

                    {
                        try

                        {
                            var userId = message.MembersAdded[i].Id;

                            var channelData = message.GetChannelData<TeamsChannelData>();

                            var connectorClient = new ConnectorClient(new Uri(message.ServiceUrl));

                            var parameters = new ConversationParameters

                            {
                                Members = new ChannelAccount[] { new ChannelAccount(userId) },

                                ChannelData = new TeamsChannelData

                                {
                                    Tenant = channelData.Tenant,

                                    Notification = new NotificationInfo() { Alert = true }
                                }
                            };

                            var conversationResource = await connectorClient.Conversations.CreateConversationAsync(parameters);

                            var replyMessage = Activity.CreateMessageActivity();

                            replyMessage.ChannelData = new TeamsChannelData() { Notification = new NotificationInfo(true) };

                            replyMessage.Conversation = new ConversationAccount(id: conversationResource.Id.ToString());

                            var name = message.MembersAdded[i].Name;

                            if (name != null)

                            {
                                name = name.Split(' ').First();
                            }

                            replyMessage.Attachments.Add(EchoBot.WelcomeLeaveCard(name, false));

                            await connectorClient.Conversations.SendToConversationAsync((Activity)replyMessage);
                        }
                        catch (Exception ex)

                        {
                            ErrorLogService.LogError(ex);
                        }
                    }
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)

            {
                // Handle add/remove from contact lists

                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)

            {
                // Handle knowing tha the user is typing
            }

            return null;
        }
    }
}