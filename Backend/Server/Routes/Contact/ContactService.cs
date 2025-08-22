using Microsoft.EntityFrameworkCore;
using Backend.Server.Config;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Backend.Server.Routes.Contact;

public class ContactService(Database context)
{
    private readonly Database Context = context;

    public async Task<IEnumerable<ContactEntity>> GetAllContactsAsync()
    {
        var contacts = await Context.Contacts
            .Where(c => c.DeletedAt == DateTime.MinValue)
            .Select(c => new ContactEntity
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                DeletedAt = c.DeletedAt,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync();

        return contacts;
    }

    public async Task<ContactEntity> GetContactByIdAsync(int id)
    {
        var contact = await Context.Contacts
            .Where(c => c.DeletedAt == DateTime.MinValue)
            .Select(c => new ContactEntity
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                DeletedAt = c.DeletedAt,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (contact == null)
            throw new KeyNotFoundException($"Contato com ID {id} não encontrado");

        return contact;
    }

    public async Task<ContactEntity> CreateContactAsync(CreateContactDto createContactDto)
    {
        var contact = new ContactEntity
        {
            Name = createContactDto.Name,
            Phone = createContactDto.Phone,
            Email = createContactDto.Email,
            DeletedAt = DateTime.MinValue,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Contacts.Add(contact);
        await Context.SaveChangesAsync();

        return new ContactEntity
        {
            Id = contact.Id,
            Name = contact.Name,
            Phone = contact.Phone,
            Email = contact.Email,
            DeletedAt = contact.DeletedAt,
            CreatedAt = contact.CreatedAt,
            UpdatedAt = contact.UpdatedAt
        };
    }

    public async Task<ContactEntity> UpdateContactAsync(int id, UpdateContactDto updateContactDto)
    {
        var contact = await Context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == DateTime.MinValue) ??
            throw new KeyNotFoundException($"Contato com ID {id} não encontrado");

        if (!string.IsNullOrEmpty(updateContactDto.Name)) contact.Name = updateContactDto.Name;
        if (!string.IsNullOrEmpty(updateContactDto.Phone)) contact.Phone = updateContactDto.Phone;
        if (!string.IsNullOrEmpty(updateContactDto.Email)) contact.Email = updateContactDto.Email;

        contact.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync();

        return new ContactEntity
        {
            Id = contact.Id,
            Name = contact.Name,
            Phone = contact.Phone,
            Email = contact.Email,
            DeletedAt = contact.DeletedAt,
            CreatedAt = contact.CreatedAt,
            UpdatedAt = contact.UpdatedAt
        };
    }

    public async Task<bool> DeleteContactAsync(int id)
    {
        var contact = await Context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == DateTime.MinValue);

        if (contact == null) return false;

        contact.DeletedAt = DateTime.UtcNow;
        contact.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync();

        return true;
    }
}