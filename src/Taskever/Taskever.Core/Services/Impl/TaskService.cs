using System.Collections.Generic;
using System.Linq;
using Abp.Data.Repositories;
using Abp.Modules.Core.Entities;
using Abp.Modules.Core.Services.Impl;
using Taskever.Entities;
using Taskever.Services.Dto;

namespace Taskever.Services.Impl
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<Task> _taskRepository;

        public TaskService(IRepository<Task> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        //[AbpCoreAuthorize(Features = "GetOwnTasks")]
        public virtual IList<TaskDto> GetMyTasks()
        {
            return _taskRepository.Query(q => q.ToList()).MapIList<Task, TaskDto>();
        }

        public virtual TaskDto CreateTask(TaskDto task)
        {
            var taskEntity = task.MapTo<Task>();

            //TODO: Automatically set Tenant and Creator User informations!?
            taskEntity.Tenant = Tenant.Current;
            taskEntity.CreatorUser = new User { Id = 1 }; //TODO: Get User from current principal?

            _taskRepository.Insert(taskEntity);

            return taskEntity.MapTo<TaskDto>();
        }

        public TaskDto UpdateTask(TaskDto task)
        {
            var taskEntity = task.MapTo<Task>();

            //TODO: Automatically set Tenant and Creator User informations!?
            taskEntity.Tenant = Tenant.Current;
            taskEntity.LastModifierUser = new User { Id = 1 }; //TODO: Get User from current principal?

            _taskRepository.Update(taskEntity);
            return taskEntity.MapTo<TaskDto>();
        }

        public virtual void DeleteTask(int taskId)
        {
            _taskRepository.Delete(taskId);
        }
    }
}