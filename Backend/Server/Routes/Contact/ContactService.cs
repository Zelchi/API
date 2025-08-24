using Microsoft.EntityFrameworkCore;
using Backend.Server.Config;

namespace Backend.Server.Routes.Contact;

public class ContactService(ContactRepository contactRepository)
{
    private readonly ContactRepository _contactRepository = contactRepository;

    public async Task<IEnumerable<ContactEntity>> GetAll(int accountId)
    {
        var contacts = await _contactRepository.GetAllByAccountIdAsync(accountId);
        return contacts.Select(c => new ContactEntity
        {
            Id = c.Id,
            Name = c.Name,
            Phone = c.Phone,
            Email = c.Email,
            AccountId = c.AccountId,
            DeletedAt = c.DeletedAt,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        });
    }

    public async Task<ContactEntity> GetById(int id, int accountId)
    {
        var contact = await _contactRepository.GetByIdAsync(id, accountId);
        if (contact == null)
            throw new Exception($"Contato com ID {id} não encontrado");

        return new ContactEntity
        {
            Id = contact.Id,
            Name = contact.Name,
            Phone = contact.Phone,
            Email = contact.Email,
            AccountId = contact.AccountId,
            DeletedAt = contact.DeletedAt,
            CreatedAt = contact.CreatedAt,
            UpdatedAt = contact.UpdatedAt
        };
    }

    public async Task<ContactEntity> Create(CreateContactDto createContactDto, int accountId)
    {
        var contact = new ContactEntity
        {
            Name = createContactDto.Name,
            Phone = createContactDto.Phone,
            Email = createContactDto.Email,
            AccountId = accountId
        };

        var createdContact = await _contactRepository.CreateAsync(contact);

        return new ContactEntity
        {
            Id = createdContact.Id,
            Name = createdContact.Name,
            Phone = createdContact.Phone,
            Email = createdContact.Email,
            AccountId = createdContact.AccountId,
            DeletedAt = createdContact.DeletedAt,
            CreatedAt = createdContact.CreatedAt,
            UpdatedAt = createdContact.UpdatedAt
        };
    }

    public async Task<ContactEntity> Update(int id, UpdateContactDto updateContactDto, int accountId)
    {
        var contact = await _contactRepository.GetByIdAsync(id, accountId);
        if (contact == null)
            throw new Exception($"Contato com ID {id} não encontrado");

        if (!string.IsNullOrEmpty(updateContactDto.Name)) contact.Name = updateContactDto.Name;
        if (!string.IsNullOrEmpty(updateContactDto.Phone)) contact.Phone = updateContactDto.Phone;
        if (!string.IsNullOrEmpty(updateContactDto.Email)) contact.Email = updateContactDto.Email;

        var updatedContact = await _contactRepository.UpdateAsync(contact);

        return new ContactEntity
        {
            Id = updatedContact.Id,
            Name = updatedContact.Name,
            Phone = updatedContact.Phone,
            Email = updatedContact.Email,
            AccountId = updatedContact.AccountId,
            DeletedAt = updatedContact.DeletedAt,
            CreatedAt = updatedContact.CreatedAt,
            UpdatedAt = updatedContact.UpdatedAt
        };
    }

    public async Task Delete(int id, int accountId)
    {
        var exists = await _contactRepository.ExistsAsync(id, accountId);
        if (!exists)
            throw new Exception($"Contato com ID {id} não encontrado");

        await _contactRepository.DeleteAsync(id, accountId);
    }
}