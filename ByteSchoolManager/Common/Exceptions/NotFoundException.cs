namespace ByteSchoolManager.Common.Exceptions;

public class NotFoundException(string entityName, string entityId) : Exception($"Entity {entityName} with Id[{entityId}] not found");