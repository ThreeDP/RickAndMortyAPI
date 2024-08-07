using Refit;
using BFF.Models;
using BFF.DTOs;
using System.Collections;

namespace BFF.Repositories;
public interface ICharacterGateway {
    [Get("/")]
    Task<Characters?> GetCharacters(CharacterFilterDTO c);

    [Get("/{id}")]
    Task<Character> GetCharacter(int id);

    // [Get("/{ids}")]
    // Task<Character[]> GetCharactersByIds(int[] ids);

    [Get("/{**ids}")]
    Task<IEnumerable<Character?>> GetCharactersByIds(string ids);
}
