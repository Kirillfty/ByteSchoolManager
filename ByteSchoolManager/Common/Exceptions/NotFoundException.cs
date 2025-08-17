namespace ByteSchoolManager.Common.Exceptions;

public class NotFoundException(string entityType, string entityName) : Exception($"Entity {entityType} with Id[{entityName}] not found");