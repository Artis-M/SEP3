using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Tier2.Model;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private IMessageService MessageService;

        public MessageController(IMessageService messageService)
        {
            this.MessageService = messageService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IList<Chatroom>>> getChatRooms()
        {
            try
            {
                IList<Chatroom> chatrooms = await MessageService.getChatrooms();
                return Ok(chatrooms);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("groupId")]
        public async Task<ActionResult<Chatroom>> getMessagesByRoomId([FromBody] ObjectId Id)
        {
            Chatroom chatroom = new Chatroom();
            IList<Chatroom> chatrooms = await MessageService.getChatrooms();
            try
            {
                foreach (var Chatroom in chatrooms)
                {
                    if (Chatroom.ID.Equals(Id))
                    {
                        chatroom = Chatroom;
                    }
                }

                return Ok(chatroom);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        // [HttpPost]
        // public async Task<ActionResult<Message>> sendMessage([FromBody] Message message, [FromBody] int chatRoomId)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }
        //
        //     try
        //     {
        //         Message sentMessage =  await MessageService.sendMessage(message, chatRoomId);
        //         return Created($"/{sentMessage.Id}", sentMessage);
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //         return StatusCode(500, e.Message);
        //     }
        // }
    }
}