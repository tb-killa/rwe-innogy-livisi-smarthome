using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.Messages;

public interface IMessagePersistence : IService
{
	void Create(Message message);

	Message Get(Guid messageId);

	void UpdateState(Guid messageId, MessageState newState);

	List<Message> GetAll(MessageClass messageClass, string messageType);

	List<Message> GetAll();

	void Delete(Guid messageId);

	List<MessageInfo> CreateBackup();

	void RestoreFromBackup(List<MessageInfo> messageInfos);

	List<Message> GetParamValue(MessageParameterKey param, string value);

	bool ContainsMessage(Guid id);

	bool ContainsMessage(Func<Message, bool> predicate);
}
