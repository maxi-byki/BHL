namespace BHL_url_server.DTOs;

public record LinkDTO(
    int RegisterPositionId,
    string DomainAddress,
    DateTime InsertDate,
    DateTime? DeleteDate
    );